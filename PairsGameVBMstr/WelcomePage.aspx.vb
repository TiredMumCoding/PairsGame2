Imports System.Data
Imports System.Data.SqlClient
Partial Class WelcomePage
    Inherits System.Web.UI.Page

    Protected Sub Submit_button(sender As Object, e As EventArgs)
        Dim Username As String = uname.Text
        Dim Password As String = pword.Text

        invalid.Visible = False

        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("AdventureWorksDW_WroxSSRS2012ConnectionString").ConnectionString
        Dim cnn As SqlConnection = New SqlConnection(ConnectionString)
        cnn.Open()
        Dim cmd As SqlCommand = New SqlCommand("select username, Password From pairsgame_users Where Username = @usrname And Password = @psword", cnn)

        cmd.Parameters.Add("@usrname", SqlDbType.VarChar)
        cmd.Parameters("@usrname").Value = Username

        cmd.Parameters.Add("@psword", SqlDbType.VarChar)
        cmd.Parameters("@psword").Value = Password

        Dim Reader As SqlDataReader = cmd.ExecuteReader()

        If Reader.HasRows() = True Then
            HttpContext.Current.Session("Username") = Username
            Response.Redirect("~/pairs.aspx")
        Else
            invalid.Visible = True
            uname.Text = ""
            pword.Text = ""
        End If

        Reader.Close()
        cnn.Close()

    End Sub

    Protected Sub Create_Account(sender As Object, e As EventArgs)
        Response.Redirect("~/CreateAccount.aspx")

    End Sub

    Protected Sub Just_Play(sender As Object, e As EventArgs)
        Response.Redirect("~/Pairs.aspx")

    End Sub

End Class
