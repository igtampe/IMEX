Imports System.IO
Imports IMEX.TaxCalc
Imports IMEX.BasicRender
Imports IMEX.ImexGraphics

Module Module1

    Public Structure User
        Public ID As String
        Public Category As Integer
        Public TaxInfo As TaxInformation
        Public Income As Long
        Public EI As Long


        Public Function TopBank() As String
            If HasBank("UMSNB") Then Return "UMSNB"
            If HasBank("GBANK") Then Return "GBANK"
            If HasBank("RIVER") Then Return "RIVER"
            Throw NoBankException
        End Function

        Public Function TaxBank() As String
            Dim TopBank As String = "NOBANK"
            Try
                If GetBankBalance(TopBank) < GetBankBalance("UMSNB") Then TopBank = "UMSNB"
                If GetBankBalance(TopBank) < GetBankBalance("GBANK") Then TopBank = "GBANK"
                If GetBankBalance(TopBank) < GetBankBalance("RIVER") Then TopBank = "RIVER"
                Return TopBank
            Catch ex As Exception
                Return TopBank
            End Try
        End Function

        Public Function HasBank(Bank As String) As Boolean
            Return Directory.Exists("..\USERS\" & ID & "\" & Bank)
        End Function

        Public Function GetBankBalance(Bank As String) As Long
            If Bank = "NOBANK" Then Return 0
            If Not HasBank(Bank) Then Return 0

            Try
                FileOpen(4, "..\USERS\" & ID & "\" & Bank & "\BALANCE.DLL", OpenMode.Input)
                Dim Balance As Long = LineInput(4)
                FileClose(4)
                Return Balance
            Catch ex As Exception
                Return 0
            End Try

        End Function

        Public Sub New(ID As String, Category As Long)
            Me.ID = ID
            Me.Category = Category

            Dim NewpondIncome As Long
            Dim UrbiaIncome As Long
            Dim ParadisusIncome As Long
            Dim LaertesIncome As Long
            Dim NOstenIncome As Long
            Dim SOstenIncome As Long


            'GRAB THE INCOME
            Try
                FileOpen(2, "..\USERS\" & ID & "\BREAKDOWN.dll", OpenMode.Input)
                Dim TempBreakdown() As String = LineInput(2).Split(",")
                FileClose(2)
                NewpondIncome = TempBreakdown(0)
                UrbiaIncome = TempBreakdown(1)
                ParadisusIncome = TempBreakdown(2)
                LaertesIncome = TempBreakdown(3)
                NOstenIncome = TempBreakdown(4)
                SOstenIncome = TempBreakdown(5)
            Catch
                ToLog("WARN: Unable to get " & ID & "'s income breakdown.")
                NewpondIncome = 0
                UrbiaIncome = 0
                ParadisusIncome = 0
                LaertesIncome = 0
                NOstenIncome = 0
                SOstenIncome = 0
            End Try

            Try
                FileOpen(2, "..\USERS\" & ID & "\EI.dll", OpenMode.Input)
                EI = LineInput(2)
                FileClose(2)
            Catch
                EI = 0
            End Try

            TaxInfo = New TaxInformation(EI, NewpondIncome, UrbiaIncome, ParadisusIncome, LaertesIncome, NOstenIncome, SOstenIncome, Category)
            Income = TaxInfo.FederalIncome - EI
        End Sub

        Public Sub ClearEI()
            File.Delete("..\USERS\" & ID & "\EI.dll")
        End Sub

        Public Sub Pay()
            'PAY
            NTA(Income)

            'Send to Logs
            FileOpen(4, "..\USERS\" & ID & "\" & TopBank() & "\log.log", OpenMode.Append)
            PrintLine(4, "[" & DateTime.Now.ToString & "] IMEX has applied your monthly income of " & Income.ToString("N0") & "p")
            FileClose(4)
        End Sub

        Public Sub Tax()
            'Take the money out
            NTA(-1 * TaxInfo.TotalTax)

            'Send to Logs
            FileOpen(4, "..\USERS\" & ID & "\" & TopBank() & "\log.log", OpenMode.Append)
            PrintLine(4, "[" & DateTime.Now.ToString & "] IMEX applied a tax of " & TaxInfo.TotalTax.ToString("N0") & "p to your account.")
            PrintLine(4, "[" & DateTime.Now.ToString & "] Your total income (monthly and extra) last month was " & (TaxInfo.FederalIncome).ToString("N0") & "p")
            FileClose(4)

            'Clear the EI file
            ClearEI()

            'Send Taxes to appropriate accounts
            Try
                UMSGov.NTA(TaxInfo.Federal.MoneyOwed, True, ID)
                Newpond.NTA(TaxInfo.Newpond.MoneyOwed, True, ID)
                Paradisus.NTA(TaxInfo.Paradisus.MoneyOwed, True, ID)
                Urbia.NTA(TaxInfo.Urbia.MoneyOwed, True, ID)
                Laertes.NTA(TaxInfo.Laertes.MoneyOwed, True, ID)
                NorthOsten.NTA(TaxInfo.NorthOsten.MoneyOwed, True, ID)
                SouthOsten.NTA(TaxInfo.SouthOsten.MoneyOwed, True, ID)
            Catch ex As Exception
                ToLog("Could not send money to UMSGov")
            End Try

        End Sub

        Public Sub NTA(Amount As Long, Optional Log As Boolean = False, Optional From As String = "")
            Dim top As String = TopBank()
            If top = "NOBANK" Then
                'Do nothing
            Else
                Dim Balance As Long = GetBankBalance(top) + Amount
                FileOpen(4, "..\USERS\" & ID & "\" & TopBank() & "\BALANCE.DLL", OpenMode.Output)
                WriteLine(4, Balance)
                FileClose(4)

                If Log Then
                    FileOpen(4, "..\USERS\" & ID & "\" & TopBank() & "\log.log", OpenMode.Append)
                    PrintLine(4, "[" & DateTime.Now.ToString & "] IMEX Moved " & Amount.ToString("N0") & "p to your account from " & From)
                    FileClose(4)
                End If

            End If
        End Sub
    End Structure

    Public NoBankException As Exception = New Exception("The User has no bank")
    Public UMSGov As User = New User("33118", 2)
    Public Newpond As User = New User("86700", 2)
    Public Paradisus As User = New User("86701", 2)
    Public Urbia As User = New User("86702", 2)
    Public Laertes As User = New User("86703", 2)
    Public NorthOsten As User = New User("86704", 2)
    Public SouthOsten As User = New User("86705", 2)

    Sub Main()

        Echo("Please Wait, Loading IMEX", True)

        Dim Arguements As String()
        Arguements = Environment.GetCommandLineArgs
        Dim Force As Boolean = False

        Console.Clear()

        Console.Title = "IncomeMan Express"
        Console.SetWindowSize(50, 25)
        Console.SetBufferSize(50, 25)
        Color(ConsoleColor.Black, ConsoleColor.DarkCyan)
        drawHeader()

        Select Case Arguements.Count

            Case 1
                HelpScreen()
                Return
            Case 2
                If Not Arguements(1).ToUpper = "/TAX" And Not Arguements(1).ToUpper = "/INCOME" Then
                    HelpScreen()
                    Return
                End If
                DrawLoadBar()
            Case 3
                If Not Arguements(2).ToUpper = "/FORCE" Then
                    HelpScreen()
                    Return
                End If
                DrawLoadBar("FORCE ENABLED")
                Force = True
        End Select

        LogHeader()

        'Load the User Arrays
        Dim NormalUsers() As User = LoadUsers("UserList.isf", 0)
        Dim CorporateUsers() As User = LoadUsers("Corporate.isf", 1)

        'If they're empty they already triggered the errorscreen and we just have to exit NOW
        If IsNothing(NormalUsers) Or IsNothing(CorporateUsers) Then Exit Sub

        Select Case Arguements(1).ToUpper
            Case "/TAX"
                If Date.Now.Day = 15 Or Force = True Then
                    SetPos(0, 14)
                    CenterText("Taxing normal users...")
                    Tax(NormalUsers)

                    ClearProgressBar()

                    SetPos(0, 14)
                    CenterText("Taxing corporate users...")
                    Tax(CorporateUsers)

                    DoneScreen(1)
                Else
                    ErrorScreen(2)
                End If

            Case "/INCOME"
                If Date.Now.Day = 1 Or Force = True Then
                    SetPos(0, 14)
                    CenterText("Paying Normal users...")
                    Payday(NormalUsers)

                    ClearProgressBar()

                    SetPos(0, 14)
                    CenterText("Paying Corporate users...")
                    Payday(CorporateUsers)

                    DoneScreen(0)
                Else
                    ErrorScreen(2)
                End If

        End Select

    End Sub

    Sub Spinner()
        Try
            Static Spinner As Integer
            Spinner += 1
            Console.SetCursorPosition(24, 21)

            Select Case Spinner
                Case 1
                    Console.WriteLine("|")
                Case 2
                    Console.WriteLine("/")
                Case 3
                    Console.WriteLine("-")
                Case 4
                    Console.WriteLine("\")
                    Spinner = 0
            End Select
            Threading.Thread.Sleep(50)
        Catch
            ErrorScreen(1)
            Exit Sub
        End Try


    End Sub

    Function LoadUsers(ISFDir As String, Category As Integer) As User()

        'OPEN THE ISF
        Try
            FileOpen(1, ISFDir, OpenMode.Input)
        Catch ex As Exception
            ToLog("AN ERROR HAS OCCURED: " & vbNewLine & vbNewLine & ex.StackTrace & vbNewLine)
            ErrorScreen(0)
            Return Nothing
        End Try

        Dim Counter As Integer = 1
        Dim TempStringHolder As String
        Dim Users(0) As User

        'READ THE FILE
        While Not EOF(1)
            TempStringHolder = LineInput(1)
            If (TempStringHolder.StartsWith("USER")) Then
                ReDim Preserve Users(Counter - 1)

                'ADD THE USER
                Users(Counter - 1) = New User(TempStringHolder.Replace("USER" & Counter & ":", ""), Category)

                'LOG IT
                ToLog("INFO: Loaded user " & Counter & " which is " & Users(Counter - 1).ID & " and has an income of " & (Users(Counter - 1).TaxInfo.FederalIncome - Users(Counter - 1).EI).ToString("N0") & "p")

                'S P I N
                Spinner()
                Counter += 1

            End If
        End While
        FileClose(1)

        'AND RETURN THE USERS
        Return Users

        'THE NEW EMERGENCY LOAD USERS FUNCTION FROM LEGO CITY
    End Function

    Sub Tax(Users As User())
        'there's 47 characters in the progressbar
        Dim PBS As Integer
        Dim T As Integer

        PBS = (47 / Users.Count)

        For Each Tipillo As User In Users

            'Tax
            Try
                Tipillo.Tax()
                ToLog("INFO: Applied a tax of (" & Tipillo.TaxInfo.TotalTax & ") to " & Tipillo.ID & "'s Income (" & Tipillo.Income & ")")
            Catch EX As Exception
                If EX.Equals(NoBankException) Then
                    ToLog("ERROR: Failed to apply a tax to " & Tipillo.ID & "'s Income since he has no bank!")
                Else
                    ToLog("ERROR: Failed to apply a tax to " & Tipillo.ID & "'s Income. " & EX.Message & vbNewLine & EX.StackTrace)
                End If
            End Try

            'Update Progressbar
            T += 1
            For PBST = 1 To PBS * T
                Block(ConsoleColor.Green, PBST, 17)
            Next

            SetPos(0, 20)
            CenterText("Taxed user " & Tipillo.ID)
            Spinner()
            Threading.Thread.Sleep(50)

        Next

    End Sub

    Sub Payday(Users As User())
        'there's 47 characters in the progressbar

        Dim T As Integer
        Dim PBS As Double

        PBS = (47 / Users.Count)

        For Each tipillo As User In Users

            Try
                tipillo.Pay()
                ToLog("INFO: Payed out " & tipillo.ID & "'s Income (" & tipillo.Income & ")")
            Catch ex As Exception
                If ex.Equals(NoBankException) Then
                    ToLog("ERROR: Failed to pay " & tipillo.ID & "'s Income because he has no bank!")
                Else
                    ToLog("ERROR: Failed to pay " & tipillo.ID & "'s Income." & vbNewLine & ex.StackTrace)
                End If

            End Try

            T += 1
            For PBST = 1 To PBS * T
                Block(ConsoleColor.Green, PBST, 17)
            Next

            SetPos(0, 20)
            CenterText("Paid user " & tipillo.ID)

            Spinner()
            Threading.Thread.Sleep(50)

        Next

    End Sub

    Sub LogHeader()
        FileOpen(50, "IMEXLOG.log", OpenMode.Append)
        PrintLine(50, ":::::IMEX WAS STARTED ON " & DateTime.Now.ToString & ":::::")
        FileClose(50)
    End Sub

    Sub ToLog(message As String)
        FileOpen(50, "IMEXLOG.log", OpenMode.Append)
        PrintLine(50, message)
        FileClose(50)
    End Sub

End Module
