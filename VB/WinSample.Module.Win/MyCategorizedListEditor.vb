Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.TreeListEditors.Win
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base.General
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Win.Controls
Imports DevExpress.ExpressApp.Utils

Namespace WinSample.Module.Win
	Public Class MyCategorizedListEditor
		Inherits CategorizedListEditor
		Public Sub New(ByVal info As DictionaryNode)
			MyBase.New(info)
		End Sub
		Protected Overrides Function CreateControlsCore() As Object
			Dim result As Object = MyBase.CreateControlsCore()
			AddHandler CategoriesListView.SelectionChanged, AddressOf CategoriesListView_SelectionChanged
			Dim objectTreeList As ObjectTreeList = TryCast(CategoriesListView.Editor.Control, ObjectTreeList)
			If objectTreeList IsNot Nothing Then
				AddHandler objectTreeList.NodesReloading, AddressOf objectTreeList_NodesReloading
				AddHandler objectTreeList.NodesReloaded, AddressOf objectTreeList_NodesReloaded
			End If
			AddHandler locker.LockedChanged, AddressOf locker_LockedChanged
			Return result
		End Function
		Private locker As New Locker()
		Private Sub objectTreeList_NodesReloaded(ByVal sender As Object, ByVal e As EventArgs)
			locker.Unlock()
		End Sub

		Private Sub objectTreeList_NodesReloading(ByVal sender As Object, ByVal e As EventArgs)
			locker.Lock()
		End Sub

		Private Sub CategoriesListView_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
			If (Not locker.Locked) Then
				UpdateGridViewFilter()
			Else
				locker.Call("UpdateGridViewFilter")
			End If
		End Sub

		Private Sub locker_LockedChanged(ByVal sender As Object, ByVal e As LockedChangedEventArgs)
			If (Not e.Locked) AndAlso e.PendingCalls.Contains("UpdateGridViewFilter") Then
				UpdateGridViewFilter()
			End If

		End Sub
		Private Sub UpdateGridViewFilter()
			If CategoriesListView.CurrentObject IsNot Nothing Then
				Dim categories As New List(Of Guid)()
				Dim currentCategory As BaseObject = CType(CategoriesListView.CurrentObject, BaseObject)
				categories.Add(currentCategory.Oid)
				GetCategories(CType(CategoriesListView.CurrentObject, ITreeNode), categories)
				Me.ItemsDataSource.Criteria(CategoryPropertyName) = New InOperator("Category.Oid", categories)
				RefreshColumns()
			End If
		End Sub
		Private Sub GetCategories(ByVal current As ITreeNode, ByVal childrenCategoryIDs As IList(Of Guid))
			For Each child As ITreeNode In current.Children
				Dim categorizedItem As BaseObject = TryCast(child, BaseObject)
				If categorizedItem IsNot Nothing Then
					childrenCategoryIDs.Add(categorizedItem.Oid)
					GetCategories(child, childrenCategoryIDs)
				End If
			Next child
		End Sub
		Public Overrides Sub Dispose()
			RemoveHandler CategoriesListView.CurrentObjectChanged, AddressOf CategoriesListView_SelectionChanged
			RemoveHandler locker.LockedChanged, AddressOf locker_LockedChanged
			MyBase.Dispose()
		End Sub
	End Class
End Namespace
