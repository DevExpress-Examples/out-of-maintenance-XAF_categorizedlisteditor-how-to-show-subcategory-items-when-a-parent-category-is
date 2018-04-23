Imports Microsoft.VisualBasic
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl

Namespace WinSample.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal session As Session, ByVal currentDBVersion As Version)
			MyBase.New(session, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			Dim category As Category = CreateCategory("Parent", Nothing)
			CreateCategory("Child1", category)
			CreateCategory("Child2", category)
		End Sub
		Private Function CreateCategory(ByVal name As String, ByVal parent As HCategory) As Category
			Dim category As Category = Session.FindObject(Of Category)(New BinaryOperator("Name", name))
			If category Is Nothing Then
				category = New Category(Session)
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
			Dim item As Item = Session.FindObject(Of Item)(New BinaryOperator("Name", realName))
			If item Is Nothing Then
				item = New Item(Session)
				item.Name = realName
				item.Category = category
				item.Save()
			End If
		End Sub
	End Class
End Namespace
