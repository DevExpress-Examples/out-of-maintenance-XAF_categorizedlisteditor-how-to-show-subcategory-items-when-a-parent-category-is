Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl

Namespace WinSample.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal objectSpace As DevExpress.ExpressApp.IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Dim category As Category = CreateCategory("Parent", Nothing)
			CreateCategory("Child1", category)
			CreateCategory("Child2", category)
		End Sub
		Private Function CreateCategory(ByVal name As String, ByVal parent As HCategory) As Category
			Dim category As Category = ObjectSpace.FindObject(Of Category)(New BinaryOperator("Name", name))
			If category Is Nothing Then
				category = ObjectSpace.CreateObject(Of Category)()
				category.Name = name
				category.Parent = parent
				category.Save()
				CreateCategorizedItem("Item1", category)
				CreateCategorizedItem("Item2", category)
			End If
			Return category
		End Function
		Private Sub CreateCategorizedItem(ByVal name As String, ByVal category As Category)
			Dim realName As String = name & " - " & category.Name
			Dim item As Item = ObjectSpace.FindObject(Of Item)(New BinaryOperator("Name", realName))
			If item Is Nothing Then
				item = ObjectSpace.CreateObject(Of Item)()
				item.Name = realName
				item.Category = category
				item.Save()
			End If
		End Sub
	End Class
End Namespace
