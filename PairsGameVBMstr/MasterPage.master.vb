Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    'create a way of accessing session variables. This says whether it's first or second click in a pair 
    Public ReadOnly Property clickNo() As Integer
        Get
            Return Session("Click")
        End Get
    End Property

    'records ID of the first button clicked, so it can be reset to background if no match
    Public ReadOnly Property FirstButtonID() As String
        Get
            Return Session("FirstButtonID")
        End Get
    End Property

    ' records URL of first button clicked so it can be compared to the second
    Public ReadOnly Property Image() As String
        Get
            Return Session("Image")
        End Get
    End Property

    'sets a flag for whether or not the session has been started - if true the player is able to turn tiles
    Public ReadOnly Property Start() As Boolean
        Get
            Return Session("Start")
        End Get
    End Property

    'stores the id of the game in progress. so that scores can be entered into the scores table related to the correct game
    Public ReadOnly Property Game() As Integer
        Get
            Return Session("GameID")
        End Get
    End Property

    'stores the level currently in play, so that scores can be entered into the scores table related to the correct level
    Public ReadOnly Property Level() As Integer
        Get
            Return Session("Level")
        End Get
    End Property

    'stores the number of times the start game loop needs to go tround
    Public ReadOnly Property Rows() As Integer
        Get
            Return Session("Rows")
        End Get
    End Property

    Public ReadOnly Property Table() As Table
        Get
            Return Session("Table")
        End Get
    End Property

    'Display UserName on game screen and store level and row (in table) numbers as session variables
    Public Sub WelcomeUser(LevelNo As Integer, RowNo As Integer)

        Session("Level") = LevelNo
        Session("Rows") = RowNo

        ' show the table which corresponds to the current level
        Dim TableName As String = "table" & LevelNo
        Dim cphold As ContentPlaceHolder
        cphold = Master.FindControl("ContentPlaceHolder1")
        Dim Table As Table = cphold.FindControl(TableName)
        Table.Visible = True
        Session("Table") = Table

        If HttpContext.Current.Session("Username") IsNot Nothing Then
            welcome.Text = "Welcome " & HttpContext.Current.Session("Username") & "!"
        End If
    End Sub

    Public Sub Start_Game(ByVal sender As Object, ByVal e As EventArgs)

        'obtaining the id of the first cell in the table. For tables two and three is more than one.  Can't use item number because this is used to monitor length of list of random variables
        Dim FirstCellID As Integer
        Dim Cell As TableCell
        Cell = Table.Rows(0).Cells(0)
        Dim FirstCellIDString As String = Cell.ID
        FirstCellID = Regex.Match(FirstCellIDString, "\d+").Value


        'assign a random number to each cell in the table.  each mumber corresponds to a game image
        Dim Rand As Random = New Random
        Dim itemno As Integer = 1
        Dim List As List(Of Integer) = New List(Of Integer)
        Dim Randomno As Integer = 0

        ' work through all the rows in the table

        For Each Row As TableRow In Table.Rows
            ' for each cell generate random number and check it hasn't been used before
            For Each Cell In Row.Cells

                Do While List.Count < itemno
                    'rows is session variable, set on page load to correspond with number of tiles at each level
                    Dim Checkno As Integer = Rand.Next(1, Rows)
                    If Not (List.Contains(Checkno)) Then
                        List.Add(Checkno)
                        Randomno = Checkno

                        Dim ImageButtonID As String = "ImageButton" & CStr(FirstCellID)
                        Dim LabelID As String = "Label" & CStr(FirstCellID)
                        ' access the label control corresponding to the number of times we've bene through this loop.  Becauase page has a master, we need to access content placeholder of the master page first.
                        Dim CPlHold As ContentPlaceHolder
                        CPlHold = Master.FindControl("ContentPlaceHolder1")
                        Dim TextName As Label = CPlHold.FindControl(LabelID)
                        TextName.Text = CStr(Randomno)


                    End If
                Loop
                itemno = itemno + 1
                FirstCellID = FirstCellID + 1
            Next

        Next

        Session("Start") = True

        'set the game id, so that scores can be recorded relative to the correct game
        Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("AdventureWorksDW_WroxSSRS2012ConnectionString").ConnectionString
        Dim cnn As SqlConnection = New SqlConnection(ConnectionString)
        cnn.Open()

        'if a user is logged in then find the number of games they've played and add one to get the new gameid
        If HttpContext.Current.Session("Username") IsNot Nothing Then
            Dim cmd As SqlCommand = New SqlCommand("select isnull (max(gameid), 0) as MaxID from pairsgame_score", cnn)
            'where username = @usrname

            'cmd.Parameters.Add("@usrname", SqlDbType.VarChar)
            ' cmd.Parameters("@usrname").Value = HttpContext.Current.Session("Username")

            Dim Reader As SqlDataReader = cmd.ExecuteReader()
            Dim MaxID As Integer


            Reader.Read()
            MaxID = Reader.Item("MaxID")

            Reader.Close()
            cnn.Close()
            Session("GameID") = MaxID + 1


        End If
    End Sub

    Public Sub ButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        'swapping the backround image for the pairs image
        ' identify the button that was clicked

        If Start = True Then
            If ResetTiles.Visible = False Then

                Dim Button As ImageButton = sender

                'make sure that image hasn't already been clicked
                If Button.ImageUrl = "background.jpg" Then


                    ' get the position of the button in the grid.  Can't use length - 1 method because number 10 would need -2.  Using regular expressions, where "\d+" is looking for digits.
                    Dim ButtonID As String = CStr(Button.ID)
                    Dim ButtonID2 = Regex.Match(ButtonID, "\d+").Value
                    Dim LabelID As String = "Label" & ButtonID2

                    'Get the id number for the image to use from the label control
                    Dim CPlHold As ContentPlaceHolder
                    CPlHold = Master.FindControl("ContentPlaceHolder1")
                    Dim TextName As Label = CPlHold.FindControl(LabelID)
                    Dim LabelIDNo As Integer = CInt(TextName.Text)

                    'connect to database
                    Dim ConnectionString As String = ConfigurationManager.ConnectionStrings("AdventureWorksDW_WroxSSRS2012ConnectionString").ConnectionString
                    Dim cnn As SqlConnection = New SqlConnection(ConnectionString)
                    cnn.Open()
                    Dim cmd As SqlCommand = New SqlCommand("select imageurl from PairsGame_Images where id = @id", cnn)

                    cmd.Parameters.Add("@id", SqlDbType.Int)
                    cmd.Parameters("@id").Value = LabelIDNo

                    Dim Reader As SqlDataReader = cmd.ExecuteReader
                    Dim url As String = ""

                    While Reader.Read
                        url = Reader.GetString(0)
                    End While

                    Button.ImageUrl = url

                    Reader.Close()
                    'cnn.Close()


                    If clickNo = 0 Then
                        Session("Click") = clickNo + 1
                        Session("Image") = url
                        Session("FirstButtonID") = ButtonID

                        'add score to table, if the player has a username
                        If HttpContext.Current.Session("Username") IsNot Nothing Then
                            Dim cmd1 As SqlCommand = New SqlCommand("insert into pairsgame_score (date, username, score, gameid, level) values (@date, @name, 1, @gameid, @level)", cnn)
                            cmd1.Parameters.Add("@date", SqlDbType.DateTime)
                            cmd1.Parameters("@date").Value = DateTime.Now

                            cmd1.Parameters.Add("@name", SqlDbType.VarChar)
                            cmd1.Parameters("@name").Value = Session("Username")

                            cmd1.Parameters.Add("@gameid", SqlDbType.Int)
                            cmd1.Parameters("@gameid").Value = Session("GameID")

                            cmd1.Parameters.Add("@level", SqlDbType.Int)
                            cmd1.Parameters("@level").Value = Session("Level")

                            cmd1.ExecuteNonQuery()
                        End If

                    Else
                        If url <> Image Then
                            Session("SecondButtonID") = ButtonID
                            ResetTiles.Visible = True
                            ResetTiles.Text = "Uh Oh.. Tiles don't Match.  Click to Reset"
                            Session("Click") = 0

                            'add score to table, if the player has a username
                            If HttpContext.Current.Session("Username") IsNot Nothing Then
                                Dim cmd1 As SqlCommand = New SqlCommand("insert into pairsgame_score (date, username, score, gameid, level) values (@date, @name, 1, @gameid, @level)", cnn)
                                cmd1.Parameters.Add("@date", SqlDbType.DateTime)
                                cmd1.Parameters("@date").Value = DateTime.Now

                                cmd1.Parameters.Add("@name", SqlDbType.VarChar)
                                cmd1.Parameters("@name").Value = Session("Username")

                                cmd1.Parameters.Add("@gameid", SqlDbType.Int)
                                cmd1.Parameters("@gameid").Value = Session("GameID")

                                cmd1.Parameters.Add("@level", SqlDbType.Int)
                                cmd1.Parameters("@level").Value = Session("Level")

                                cmd1.ExecuteNonQuery()
                            End If

                        End If
                        If url = Image Then
                            Session("Click") = 0
                            ResetTiles.Visible = True
                            ResetTiles.Text = "Well Done, you've matched a pair! Click to find the next pair"

                            'add score to table, if the player has a username
                            If HttpContext.Current.Session("Username") IsNot Nothing Then
                                Dim cmd1 As SqlCommand = New SqlCommand("insert into pairsgame_score (date, username, score, gameid, level) values (@date, @name, 1, @gameid, @level)", cnn)
                                cmd1.Parameters.Add("@date", SqlDbType.DateTime)
                                cmd1.Parameters("@date").Value = DateTime.Now

                                cmd1.Parameters.Add("@name", SqlDbType.VarChar)
                                cmd1.Parameters("@name").Value = Session("Username")

                                cmd1.Parameters.Add("@gameid", SqlDbType.Int)
                                cmd1.Parameters("@gameid").Value = Session("GameID")

                                cmd1.Parameters.Add("@level", SqlDbType.Int)
                                cmd1.Parameters("@level").Value = Session("Level")

                                cmd1.ExecuteNonQuery()
                            End If

                            'finish game if no buttons left with url "background.jpg"
                            Dim EndGame As Boolean = True
                            'Dim ItemNo As Integer = 1

                            Dim FirstCellID As Integer
                            Dim Cell As TableCell
                            Cell = Table.Rows(0).Cells(0)
                            Dim FirstCellIDString As String = Cell.ID
                            FirstCellID = Regex.Match(FirstCellIDString, "\d+").Value

                            For Each row In Table.Rows

                                For Each Cell In row.cells

                                    Dim ImageButton As String = "ImageButton" & CStr(FirstCellID)

                                    Dim CPHold As ContentPlaceHolder
                                    CPHold = Master.FindControl("ContentPlaceHolder1")
                                    Dim ButtonName As ImageButton = CPHold.FindControl(ImageButton)

                                    If ButtonName.ImageUrl = "background.jpg" Then
                                        EndGame = False
                                    End If

                                    FirstCellID = FirstCellID + 1

                                Next
                            Next

                            If EndGame = True Then Response.Redirect("~/Level1Complete.aspx")


                        End If
                    End If


                    cnn.Close()

                End If

            End If
        End If

    End Sub

    Public Sub Reset_Tiles(sender As Object, e As EventArgs)

        ' if the images didn't match
        If ResetTiles.Text = "Uh Oh.. Tiles don't Match.  Click to Reset" Then

            'reset the second image clicked
            Dim CPlHold As ContentPlaceHolder
            CPlHold = Master.FindControl("ContentPlaceHolder1")
            Dim secondButton As ImageButton = CPlHold.FindControl(Session("SecondButtonID"))
            secondButton.ImageUrl = "background.jpg"
            'reset the first image clicked
            Dim firstButton As ImageButton = CPLHold.FindControl(Session("FirstButtonID"))
            firstButton.ImageUrl = "background.jpg"

        End If

        ResetTiles.Visible = False

    End Sub

End Class

