Imports IMEX.BasicRender
Public Class ImexGraphics

    '╔═╦═╗
    '║ ║ ║
    '╠═╬═╣
    '║ ║ ║
    '╚═╩═╝

    Public Shared Sub drawHeader()
        Echo(".")
        Echo("      /$$$$$$ /$$      /$$ /$$$$$$$$ /$$   /$$", True)
        Echo("     |_  $$_/| $$$    /$$$| $$_____/| $$  / $$", True)
        Echo("       | $$  | $$$$  /$$$$| $$      |  $$/ $$/", True)
        Echo("       | $$  | $$ $$/$$ $$| $$$$$    \  $$$$/ ", True)
        Echo("       | $$  | $$  $$$| $$| $$__/     >$$  $$ ", True)
        Echo("       | $$  | $$\  $ | $$| $$       /$$/\  $$", True)
        Echo("      /$$$$$$| $$ \/  | $$| $$$$$$$$| $$  \ $$", True)
        Echo("     |______/|__/     |__/|________/|__/  |__/", True)
        Echo(".")
        Echo("═════════════════════════════════════════════════", True)
        Echo("                 IncomeMan EXPRESS", True)
        Echo("═════════════════════════════════════════════════", True)
        Echo(".")
    End Sub

    Public Shared Sub HelpScreen()
        Console.WriteLine("IMEX {/TAX or /INCOME} {/IGNORE}")
        Console.WriteLine("")
        Console.WriteLine("/TAX   : Tax all users in the UserList.ISF File")
        Console.WriteLine("/INCOME: Give income to all users in the registry")
        Console.WriteLine("")
        Console.WriteLine("/IGNORE: Ignore the date restrictions on actions")
        Console.WriteLine("")
        Console.WriteLine("═════════════════════════════════════════════════")
        Console.WriteLine("(C)2020 Igtampe, No Rights Reserved.")
        Pause()
    End Sub

    Public Shared Sub DrawLoadBar(Optional ByVal Text As String = "")
        CenterText("Please wait..")
        Echo("", True)
        Console.WriteLine("")
        Console.WriteLine("╔═══════════════════════════════════════════════╗")
        Console.WriteLine("║                                               ║")
        Console.WriteLine("╚═══════════════════════════════════════════════╝")
        Console.WriteLine("")
        Console.WriteLine("                  Loading Users                  ")
        Console.WriteLine("")
        CenterText(Text)
        Console.SetCursorPosition(24, 21)
        Console.WriteLine("|")
        Console.WriteLine("")
    End Sub

    Public Shared Sub DoneScreen(L1 As Integer)
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

    Public Shared Sub ErrorScreen(L1 As Integer)
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

    Public Shared Sub ClearProgressBar()
        For PBST = 1 To 47
            Block(ConsoleColor.Black, PBST, 17)
        Next
    End Sub


End Class
