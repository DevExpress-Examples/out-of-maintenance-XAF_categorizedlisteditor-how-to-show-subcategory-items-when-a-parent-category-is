Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo

Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Base.General

Namespace WinSample.Module
	<DefaultClassOptions> _
	Public Class Item
		Inherits BaseObject
		Implements ICategorizedItem
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _Name As String
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", _Name, value)
			End Set
		End Property
		Private _Category As Category
		Public Property Category() As Category
			Get
				Return _Category
			End Get
			Set(ByVal value As Category)
				SetPropertyValue("Category", _Category, value)
			End Set
		End Property
		#Region "ICategorizedItem Members"
		Private Property ICategorizedItem_Category() As ITreeNode Implements ICategorizedItem.Category
			Get
				Return Category
			End Get
			Set(ByVal value As ITreeNode)
				Category = CType(value, Category)
			End Set
		End Property
		#End Region
	End Class
End Namespace
