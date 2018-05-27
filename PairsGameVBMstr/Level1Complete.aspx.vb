
Imports System.Data.SqlClient
Imports System.Data
Partial Class Level1Complete
    Inherits System.Web.UI.Page

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("Level") > 2 Then
            restartGame.Visible = True
            NextLevel.Visible = False
        End If

        If HttpContext.Current.Session("Username") IsNot Nothing Then

            thisScore.Visible = True
            lowScore.Visible = True
            Dim GameScore As Integer
            Dim BestScore As Integer

            Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("AdventureWorksDW_WroxSSRS2012ConnectionString").ConnectionString
            Dim cnn As SqlConnection = New SqlConnection(ConnectionString)
            cnn.Open()

            Dim cmd As SqlCommand = New SqlCommand("select isnull (sum(score), 0) as SumScore from pairsgame_score inner join (select max(gameid) as maxgame from PairsGame_score where username = @usrname and level = @level) g on gameid = maxgame", cnn)

            cmd.Parameters.Add("@usrname", SqlDbType.VarChar)
            cmd.Parameters("@usrname").Value = Session("Username")

            cmd.Parameters.Add("@level", SqlDbType.Int)
            cmd.Parameters("@level").Value = Session("level")

            Dim reader As SqlDataReader = cmd.ExecuteReader()

            reader.Read()
            GameScore = reader.Item("SumScore")
            reader.Close()

            thisScore.Text = thisScore.Text + CStr(GameScore)

            Dim cmd2 As SqlCommand = New SqlCommand("Select min(sumscore) as minScore from (select gameid, sum(score) as SumScore From pairsgame_score Where username = @uname And level = @lev Group by gameid) s", cnn)

            cmd2.Parameters.Add("@uname", SqlDbType.VarChar)
            cmd2.Parameters("@uname").Value = Session("Username")

            cmd2.Parameters.Add("@lev", SqlDbType.Int)
            cmd2.Parameters("@lev").Value = Session("level")

            Dim reader2 As SqlDataReader = cmd2.ExecuteReader()

            reader2.Read()
            BestScore = reader2.Item("minScore")
            reader2.Close()
            cnn.Close()

            lowScore.Text = lowScore.Text + CStr(BestScore)

        End If

    End Sub

    Public Sub Next_Level(sender As Object, e As EventArgs)
        'uses session variable defined on master page to get level that has just been completed.  
        Dim level As Integer = Session("Level")
        Dim NextLevel As Integer = level + 1
        Dim NextPage As String = "PairsL" & CStr(NextLevel) & ".aspx"
        Response.Redirect(NextPage)
    End Sub

    Public Sub Restart_Game(sender As Object, e As EventArgs)
        Response.Redirect("pairs.aspx")
    End Sub

End Class
