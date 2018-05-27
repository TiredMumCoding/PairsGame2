Imports System.Data
Imports System.Data.SqlClient

Partial Class CreateAccount
    Inherits System.Web.UI.Page

    Protected Sub Submit_button(ByVal sender As Object, e As EventArgs)

        notvalid.Visible = False

        Dim Username As String = uname.Text
        Dim Password As String = pword.Text

        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("AdventureWorksDW_WroxSSRS2012ConnectionString").ConnectionString
        Dim cnn As SqlConnection = New SqlConnection(ConnectionString)
        cnn.Open()
        Dim cmd As SqlCommand = New SqlCommand("if not exists (Select username from PairsGame_users where username = @usrname) insert into PairsGame_users (username, password) values(@usrname, @psword)", cnn)

        cmd.Parameters.Add("@usrname", SqlDbType.VarChar)
        cmd.Parameters("@usrname").Value = Username

        cmd.Parameters.Add("@psword", SqlDbType.VarChar)
        cmd.Parameters("@psword").Value = Password

        Dim ret As Integer = cmd.ExecuteNonQuery()
        If ret < 1 Then
            notvalid.Visible = True
        Else
            HttpContext.Current.Session("Username") = Username
            Response.Redirect("~/pairs.aspx")
        End If



        cnn.Close()
    End Sub

End Class
