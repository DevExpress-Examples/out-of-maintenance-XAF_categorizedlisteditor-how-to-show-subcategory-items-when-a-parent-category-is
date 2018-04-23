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
Imports DevExpress.ExpressApp.Model
Imports System.Collections
Imports DevExpress.ExpressApp.Editors

Namespace WinSample.Module.Win
	<ListEditor(GetType(ICategorizedItem))> _
	Public Class MyCategorizedListEditor
		Inherits CategorizedListEditor
		Public Sub New(ByVal info As IModelListView)
			MyBase.New(info)
		End Sub
		Protected Overrides Sub OnControlsCreated()
			MyBase.OnControlsCreated()
			AddHandler CategoriesListView.SelectionChanged, AddressOf CategoriesListView_SelectionChanged
			Dim objectTreeList As ObjectTreeList = TryCast(CategoriesListView.Editor.Control, ObjectTreeList)
			If objectTreeList IsNot Nothing Then
				AddHandler objectTreeList.NodesReloading, AddressOf objectTreeList_NodesReloading
				AddHandler objectTreeList.NodesReloaded, AddressOf objectTreeList_NodesReloaded
			End If
			AddHandler locker.LockedChanged, AddressOf locker_LockedChanged
		End Sub
		Private locker As New Locker()
		Private Sub objectTreeList_NodesReloaded(ByVal sender As Object, ByVal e As EventArgs)
			locker.Unlock()
		End Sub

		Private Sub objectTreeList_NodesReloading(ByVal sender As Object, ByVal e As EventArgs)
			locker.Lock()
		End Sub

		Private Sub CategoriesListView_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
			UpdateGridViewFilter()
		End Sub

		Private Sub locker_LockedChanged(ByVal sender As Object, ByVal e As LockedChangedEventArgs)
			If (Not e.Locked) AndAlso e.PendingCalls.Contains("UpdateGridViewFilter") Then
				UpdateGridViewFilter()
			End If
		End Sub
		Private Sub UpdateGridViewFilter()
			locker.Call("UpdateGridViewFilter")
			If (Not locker.Locked) Then
				If CategoriesListView.CurrentObject IsNot Nothing Then
					Dim categoryKeys As New ArrayList()
					Dim currentCategory As ITreeNode = CType(CategoriesListView.CurrentObject, ITreeNode)
					categoryKeys.Add(GetCategoryKey(currentCategory))
					AddChildrenKeys(currentCategory, categoryKeys)
					Dim categoryKeyPropertyName As String = String.Format("{0}.{1}", CategoryPropertyName, CategoriesListView.ObjectTypeInfo.KeyMember.Name)
					Me.ItemsDataSource.Criteria(CategoryPropertyName) = New InOperator(categoryKeyPropertyName, categoryKeys)
				End If
			End If
		End Sub
		Private Sub AddChildrenKeys(ByVal current As ITreeNode, ByVal categoryKeys As IList)
			For Each child As ITreeNode In current.Children
				categoryKeys.Add(GetCategoryKey(child))
				AddChildrenKeys(child, categoryKeys)
			Next child
		End Sub
		Private Function GetCategoryKey(ByVal category As Object) As Object
			Return CategoriesListView.ObjectSpace.GetKeyValue(category)
		End Function
		Public Overrides Sub Dispose()
			RemoveHandler CategoriesListView.CurrentObjectChanged, AddressOf CategoriesListView_SelectionChanged
			RemoveHandler locker.LockedChanged, AddressOf locker_LockedChanged
			MyBase.Dispose()
		End Sub
	End Class
End Namespace
