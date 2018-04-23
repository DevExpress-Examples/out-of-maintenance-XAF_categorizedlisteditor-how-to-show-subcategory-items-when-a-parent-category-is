Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.TreeListEditors.Win
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base.General
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl

Namespace WinSample.Module.Win
	Public Class MyCategorizedListEditor
		Inherits CategorizedListEditor
		Public Sub New(ByVal info As DictionaryNode)
			MyBase.New(info)
		End Sub
		Protected Overrides Function CreateControlsCore() As Object
			Dim result As Object = MyBase.CreateControlsCore()
			AddHandler CategoriesListView.SelectionChanged, AddressOf CategoriesListView_SelectionChanged
			Return result
		End Function
		Private Sub CategoriesListView_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
			 UpdateGridViewFilter()
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
			MyBase.Dispose()
		End Sub
	End Class
End Namespace