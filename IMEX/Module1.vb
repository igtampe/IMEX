Imports System.IO

Module Module1

    Public Structure UserStructure
        Public ID As String
        Public Income As Long
        Public EI As Long
        Public Category As Integer
    End Structure

    Public User() As UserStructure


    Sub Main()



        Dim Arguements As String()
        Arguements = Environment.GetCommandLineArgs

        Console.WriteLine("Please Wait, Loading IMEX")

        Console.Clear()


        Console.Title = "IncomeMan Express"
        Console.SetWindowSize(50, 25)
        Console.SetBufferSize(50, 25)
        Console.ForegroundColor = ConsoleColor.DarkCyan


        Console.WriteLine("")
        Console.WriteLine("      /$$$$$$ /$$      /$$ /$$$$$$$$ /$$   /$$")
        Console.WriteLine("     |_  $$_/| $$$    /$$$| $$_____/| $$  / $$")
        Console.WriteLine("       | $$  | $$$$  /$$$$| $$      |  $$/ $$/")
        Console.WriteLine("       | $$  | $$ $$/$$ $$| $$$$$    \  $$$$/ ")
        Console.WriteLine("       | $$  | $$  $$$| $$| $$__/     >$$  $$ ")
        Console.WriteLine("       | $$  | $$\  $ | $$| $$       /$$/\  $$")
        Console.WriteLine("      /$$$$$$| $$ \/  | $$| $$$$$$$$| $$  \ $$")
        Console.WriteLine("     |______/|__/     |__/|________/|__/  |__/")
        Console.WriteLine("")
        Console.WriteLine("═════════════════════════════════════════════════")
        Console.WriteLine("                 IncomeMan EXPRESS")
        Console.WriteLine("═════════════════════════════════════════════════")
        Console.WriteLine("")


        Select Case Arguements.Count

            '╔═╦═╗
            '║ ║ ║
            '╠═╬═╣
            '║ ║ ║
            '╚═╩═╝
            Case 1
                Console.WriteLine("IMEX {/TAX or /INCOME} {/IGNORE}")
                Console.WriteLine("")
                Console.WriteLine("/TAX   : Tax all users in the UserList.ISF File")
                Console.WriteLine("/INCOME: Give income to all users in the registry")
                Console.WriteLine("")
                Console.WriteLine("/IGNORE: Ignore the date restrictions on actions")
                Console.WriteLine("")
                Console.WriteLine("═════════════════════════════════════════════════")
                Console.WriteLine("(C)2019 Igtampe, No Rights Reserved.")
                Console.ReadKey(True)

            Case 2

                Console.WriteLine("Please wait..")
                Console.WriteLine("")
                Console.WriteLine("╔═══════════════════════════════════════════════╗")
                Console.WriteLine("║                                               ║")
                Console.WriteLine("╚═══════════════════════════════════════════════╝")
                Console.WriteLine("")
                Console.WriteLine("                  Loading Users                  ")
                Console.SetCursorPosition(24, 21)
                Console.WriteLine("|")
                Console.WriteLine("")

                LoadUsers()

                Select Case Arguements(1).ToUpper
                    Case "/TAX"
                        Tax(False)

                    Case "/INCOME"
                        Payday(False)

                End Select

            Case 3

                Select Case Arguements(2).ToUpper
                    Case "/FORCE"
                        Console.WriteLine("Please wait..")
                        Console.WriteLine("")
                        Console.WriteLine("╔═══════════════════════════════════════════════╗")
                        Console.WriteLine("║                                               ║")
                        Console.WriteLine("╚═══════════════════════════════════════════════╝")
                        Console.WriteLine("")
                        Console.WriteLine("                  Loading Users                  ")
                        Console.WriteLine("")
                        Console.WriteLine("               !!FORCE IS ENABLED!!              ")
                        Console.SetCursorPosition(24, 21)
                        Console.WriteLine("|")
                        Console.WriteLine("")

                        LoadUsers()

                        Select Case Arguements(1).ToUpper
                            Case "/TAX"
                                Tax(True)

                            Case "/INCOME"
                                Payday(True)

                        End Select
                    Case "/NOCORPORATE"
                        Console.WriteLine("Please wait..")
                        Console.WriteLine("")
                        Console.WriteLine("╔═══════════════════════════════════════════════╗")
                        Console.WriteLine("║                                               ║")
                        Console.WriteLine("╚═══════════════════════════════════════════════╝")
                        Console.WriteLine("")
                        Console.WriteLine("                  Loading Users                  ")
                        Console.WriteLine("")
                        Console.WriteLine("             !!NOT TAXING CORPORATE!!            ")
                        Console.SetCursorPosition(24, 21)
                        Console.WriteLine("|")
                        Console.WriteLine("")

                        LoadUsers(True)

                        Select Case Arguements(1).ToUpper
                            Case "/TAX"
                                Tax(True)

                            Case "/INCOME"
                                Payday(True)

                        End Select

                    Case "/OLDNOCORPORATE"
                        Console.WriteLine("Please wait..")
                        Console.WriteLine("")
                        Console.WriteLine("╔═══════════════════════════════════════════════╗")
                        Console.WriteLine("║                                               ║")
                        Console.WriteLine("╚═══════════════════════════════════════════════╝")
                        Console.WriteLine("")
                        Console.WriteLine("                  Loading Users                  ")
                        Console.WriteLine("")
                        Console.WriteLine("             !!NOT TAXING CORPORATE!!            ")
                        Console.SetCursorPosition(24, 21)
                        Console.WriteLine("|")
                        Console.WriteLine("")

                        LoadUsers(True)
                        Tax(True, False)


                End Select

        End Select

    End Sub

    Sub Spinner()
        Try
            Static Spinner As Integer
            Spinner = Spinner + 1
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

    Sub LoadUsers(Optional nocorporate As Boolean = False)
        Try
            FileSystem.FileOpen(1, String.Concat("UserList.isf"), OpenMode.Input, OpenAccess.[Default], OpenShare.[Default], -1)
        Catch
            ErrorScreen(0)
            Exit Sub
        End Try
        'The counter
        Dim num8 As Integer = 1
        'The string holder
        Dim Str14 As String
        While Not FileSystem.EOF(1)
            Str14 = FileSystem.LineInput(1)
            If (Str14.StartsWith("USER")) Then
                ReDim Preserve User(num8 - 1)
                'Grab the USER ID
                User(num8 - 1).ID = Str14.Replace("USER" & num8 & ":", "")

                'GRAB THE INCOME
                Try
                    FileOpen(2, "..\USERS\" & User(num8 - 1).ID & "\INCOME.dll", OpenMode.Input)
                    User(num8 - 1).Income = LineInput(2)
                    FileClose(2)
                Catch
                    User(num8 - 1).Income = 0
                End Try

                Try
                    FileOpen(2, "..\USERS\" & User(num8 - 1).ID & "\EI.dll", OpenMode.Input)
                    User(num8 - 1).EI = LineInput(2)
                    FileClose(2)
                    File.Delete("..\USERS\" & User(num8 - 1).ID & "\EI.dll")
                Catch
                    User(num8 - 1).EI = 0
                End Try

                User(num8 - 1).Category = 0

                'LOG IT
                ToLog("Loaded user " & num8 & " which is " & User(num8 - 1).ID & " and has an income of " & User(num8 - 1).Income.ToString("N0") & "p")

                'S P I N
                Spinner()
                num8 = num8 + 1


            End If
        End While
        FileClose(1)

        If nocorporate = True Then Exit Sub


        Try
            FileSystem.FileOpen(1, String.Concat("Corporate.isf"), OpenMode.Input, OpenAccess.[Default], OpenShare.[Default], -1)
        Catch
            Exit Sub
        End Try
        Dim CorporateCounter = 1
        While Not FileSystem.EOF(1)
            Str14 = FileSystem.LineInput(1)
            If (Str14.StartsWith("USER")) Then
                ReDim Preserve User(num8 - 1)
                'Grab the USER ID
                User(num8 - 1).ID = Str14.Replace("USER" & CorporateCounter & ":", "")

                'GRAB THE INCOME
                Try
                    FileOpen(2, "..\USERS\" & User(num8 - 1).ID & "\INCOME.dll", OpenMode.Input)
                    User(num8 - 1).Income = LineInput(2)
                    FileClose(2)
                Catch
                    User(num8 - 1).Income = 0
                End Try

                Try
                    FileOpen(2, "..\USERS\" & User(num8 - 1).ID & "\EI.dll", OpenMode.Input)
                    User(num8 - 1).EI = LineInput(2)
                    FileClose(2)
                    File.Delete("..\USERS\" & User(num8 - 1).ID & "\EI.dll")
                Catch
                    User(num8 - 1).EI = 0
                End Try

                User(num8 - 1).Category = 1

                'LOG IT
                ToLog("Loaded Corporate user " & num8 & " which is " & User(num8 - 1).ID & " and has an income of " & User(num8 - 1).Income.ToString("N0") & "p")

                'S P I N
                Spinner()
                num8 = num8 + 1
                CorporateCounter = CorporateCounter + 1


            End If
        End While
        FileClose(1)

    End Sub

    Sub Tax(FORCE As Boolean, Optional UseNUTAC As Boolean = True)
        Console.SetCursorPosition(0, 14)
        Console.WriteLine("Taxing all users...")
        If DateTime.Now.Day = 15 Then
taxanyway:

            'there's 47 characters in the progressbar

            Dim T As Integer
            Dim Tax As Long
            Dim taxb As Single
            Dim topbank As String
            Dim topbankB As Long
            Dim NtopbankB As Long
            Dim PBS As Double
            Dim PBST As Integer

            PBS = (47 / User.Count)


            For T = 0 To User.Count - 1

                Select Case User(T).Category
                    Case 0
                        'Personal User

                        Select Case User(T).Income + User(T).EI
                            Case > 5000000
                                taxb = 0.05
                            Case Else
                                taxb = 0
                                Exit Select
                        End Select
                        Tax = (User(T).Income + User(T).EI) * taxb
                    Case 1


                        Select Case User(T).Income + User(T).EI

                            Case > 5000000
                                taxb = 0.03
                                    Exit Select
                                Case Else
                                    taxb = 0
                                    Exit Select
                            End Select
                            Tax = (User(T).Income + User(T).EI) * taxb

                        'old code

                        'Select Case User(T).Income + User(T).EI

                        '                    Case > 1000000
                        '                       Tax = (User(T).Income + User(T).EI) * 0.05
                        '                      taxb = 0.05
                        '                     Exit Select
                        '                Case > 100000
                        '                   Tax = (User(T).Income + User(T).EI) * 0.03
                        '                  taxb = 0.03
                        '                 Exit Select
                        '            Case Else
                        '               Tax = (User(T).Income + User(T).EI) * 0.01
                        '              taxb = 0.01
                        '             Exit Select
                        '    End Select


                End Select



                If Directory.Exists("..\USERS\" & User(T).ID & "\UMSNB") Then
                    topbank = "UMSNB"
                ElseIf Directory.Exists("..\USERS\" & User(T).ID & "\GBANK") Then
                    topbank = "GBANK"
                ElseIf Directory.Exists("..\USERS\" & User(T).ID & "\RIVER") Then
                    topbank = "RIVER"
                Else
                    topbank = "NOBANK"
                    GoTo NoBankNoTax
                End If
                topbankB = 0

                FileOpen(4, "..\USERS\" & User(T).ID & "\" & topbank & "\BALANCE.DLL", OpenMode.Input)
                topbankB = LineInput(4)
                FileClose(4)

                NtopbankB = topbankB - Tax

                FileOpen(4, "..\USERS\" & User(T).ID & "\" & topbank & "\BALANCE.DLL", OpenMode.Output)
                WriteLine(4, NtopbankB)
                FileClose(4)

                FileOpen(4, "..\USERS\" & User(T).ID & "\" & topbank & "\log.log", OpenMode.Append)
                PrintLine(4, "[" & DateTime.Now.ToString & "] IMEX applied a tax of " & Tax.ToString("N0") & "p to your account.")
                PrintLine(4, "[" & DateTime.Now.ToString & "] Your total income (monthly and extra) last month was " & (User(T).Income + User(T).EI).ToString("N0") & "p")
                FileClose(4)

NoBankNoTax:
                ToLog("Applied a tax of (" & Tax & ") to " & User(T).ID & "'s Income (" & User(T).Income & ") and completed operation " & topbankB & "-(" & User(T).Income & "*" & taxb & ")=" & NtopbankB)

                Console.SetCursorPosition(1, 17)
                Console.ForegroundColor = ConsoleColor.Green
                Console.BackgroundColor = ConsoleColor.Green


                For PBST = 1 To CInt(PBS * T)
                    Console.Write("_")
                Next

                Console.ForegroundColor = ConsoleColor.DarkCyan
                Console.BackgroundColor = ConsoleColor.Black
                Console.SetCursorPosition(0, 20)

                Console.WriteLine(("Taxing income to user " & User(T).ID) & "      ")
                Spinner()
                Threading.Thread.Sleep(50)


            Next

            DoneScreen(1)

        Else

            If FORCE = True Then GoTo taxanyway
            ErrorScreen(2)
            Exit Sub

        End If



    End Sub

    Sub Payday(FORCE As Boolean)
        Console.SetCursorPosition(0, 14)
        Console.WriteLine("Paying all users...")

        If DateTime.Now.Day = 1 Then
payanyway:

            'there's 47 characters in the progressbar

            Dim T As Integer
            Dim topbank As String
            Dim topbankB As Long
            Dim NtopbankB As Long

            Dim PBS As Double
            Dim PBST As Integer

            PBS = (47 / User.Count)

            For T = 0 To User.Count - 1

                If Directory.Exists("..\USERS\" & User(T).ID & "\UMSNB") Then
                    topbank = "UMSNB"
                ElseIf Directory.Exists("..\USERS\" & User(T).ID & "\GBANK") Then
                    topbank = "GBANK"
                ElseIf Directory.Exists("..\USERS\" & User(T).ID & "\RIVER") Then
                    topbank = "RIVER"
                Else
                    topbank = "NOBANK"
                    GoTo NoBankNoTax
                End If
                topbankB = 0

                FileOpen(4, "..\USERS\" & User(T).ID & "\" & topbank & "\BALANCE.DLL", OpenMode.Input)
                topbankB = LineInput(4)
                FileClose(4)

                NtopbankB = topbankB + User(T).Income

                FileOpen(4, "..\USERS\" & User(T).ID & "\" & topbank & "\BALANCE.DLL", OpenMode.Output)
                WriteLine(4, NtopbankB)
                FileClose(4)

                FileOpen(4, "..\USERS\" & User(T).ID & "\" & topbank & "\log.log", OpenMode.Append)
                PrintLine(4, "[" & DateTime.Now.ToString & "] IMEX has applied your monthly income of " & User(T).Income.ToString("N0") & "p")
                FileClose(4)


NoBankNoTax:
                ToLog("Payed out " & User(T).ID & "'s Income (" & User(T).Income & ") and completed operation " & topbankB & "+" & User(T).Income & "=" & NtopbankB)

                Console.SetCursorPosition(1, 17)
                Console.ForegroundColor = ConsoleColor.Green
                Console.BackgroundColor = ConsoleColor.Green


                For PBST = 1 To CInt(PBS * T)
                    Console.Write("_")
                Next

                Console.ForegroundColor = ConsoleColor.DarkCyan
                Console.BackgroundColor = ConsoleColor.Black
                Console.SetCursorPosition(0, 20)

                Console.WriteLine(("Applying income to user " & User(T).ID & "      "))
                Spinner()
                Threading.Thread.Sleep(50)

            Next

            DoneScreen(0)


        Else

            If FORCE = True Then GoTo PayAnyway

            ErrorScreen(2)
            Exit Sub

        End If

        'there's 47 


    End Sub

    Sub DoneScreen(L1 As Integer)
        Console.SetCursorPosition(0, 14)
        Console.ForegroundColor = ConsoleColor.Green
        Console.BackgroundColor = ConsoleColor.Black
        Console.WriteLine("╔═══════════════════════════════════════════════╗")
        Console.WriteLine("║        Operation Completed Successfully       ║")
        Console.WriteLine("╠═══════════════════════════════════════════════╣")

        Select Case L1
            Case 0
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║          Everyone was payed properly          ║")
                Console.WriteLine("║     The log was generated so check it JIC     ║")
                Console.WriteLine("║                                               ║")
            Case 1
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║          Everyone was taxed properly          ║")
                Console.WriteLine("║     The log was generated so check it JIC     ║")
                Console.WriteLine("║                                               ║")
            Case Else
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║        An unknown error has occurred.         ║")
                Console.WriteLine("║           Check the Log pls thanks            ║")
                Console.WriteLine("║                                               ║")


        End Select
        Console.WriteLine("╚═══════════════════════════════════════════════╝")
        Console.ReadKey(True)
        Exit Sub

    End Sub


    Sub ErrorScreen(L1 As Integer)
        Console.SetCursorPosition(0, 14)
        Console.ForegroundColor = ConsoleColor.Red
        Console.WriteLine("╔═══════════════════════════════════════════════╗")
        Console.WriteLine("║                   E R R O R                   ║")
        Console.WriteLine("╠═══════════════════════════════════════════════╣")

        Select Case L1
            Case 99
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║                                               ║")
            Case 0
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║         Couldn't find the USERLIST ISF        ║")
                Console.WriteLine("║  Are you running IMEX in the right directory  ║")
                Console.WriteLine("║                                               ║")
            Case 1
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║           The Spinner failed to load          ║")
                Console.WriteLine("║         I don't know how that happened        ║")
                Console.WriteLine("║                                               ║")
            Case 2
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║       It's not the right date to execute      ║")
                Console.WriteLine("║    use /IGNORE if you want to do it anyway    ║")
                Console.WriteLine("║                                               ║")

            Case Else
                Console.WriteLine("║                                               ║")
                Console.WriteLine("║        An unknown error has occurred.         ║")
                Console.WriteLine("║           Check the Log pls thanks            ║")
                Console.WriteLine("║                                               ║")


        End Select
        Console.WriteLine("╚═══════════════════════════════════════════════╝")
        Console.ReadKey(True)
        Exit Sub
    End Sub

    Sub ToLog(message As String)

        FileOpen(50, "IMEXLOG.log", OpenMode.Append)
        PrintLine(50, "[" & DateTime.Now.ToString & "] " & message)
        FileClose(50)

    End Sub


End Module
