Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo

Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl

Namespace WinSample.Module
	<DefaultClassOptions> _
	Public Class Category
		Inherits HCategory
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
	End Class

End Namespace