Imports System.Net

Public Class Form1
    Dim html As HtmlDocument
    Dim count As Integer
    Dim filename As String

    Private Sub Form1_DragOver(sender As Object, e As DragEventArgs) Handles MyBase.DragOver
        e.Effect = DragDropEffects.All
    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim async As New WebClient
        AddHandler async.DownloadProgressChanged, AddressOf Downloading
        AddHandler async.DownloadFileCompleted, AddressOf DownloadCompleted
        Me.html = GetHtmlDocument(e.Data.GetData(DataFormats.Html, False))
        Me.count = 0
        Me.TopMost = False
        While (True)
            FolderBrowserDialog1.ShowDialog()
            If (FolderBrowserDialog1.SelectedPath <> "") Then
                async.DownloadFileAsync(New Uri(Me.html.Links(0).GetAttribute("href")), _
                                        FolderBrowserDialog1.SelectedPath & "\" & Trim(Me.html.Links(0).OuterText))
                ProgressBar1.Maximum = Me.html.Links.Count
                ProgressBar1.Value = 0
                Me.filename = Trim(Me.html.Links(Me.count).OuterText)
                Exit While
            End If
        End While
        Me.TopMost = True
    End Sub

    Private Sub Downloading(sender As Object, e As DownloadProgressChangedEventArgs)
        Label1.Text = Me.filename & " 파일 다운로드 중: "
        Label4.Text = e.ProgressPercentage & "% 완료"
        ProgressBar2.Maximum = e.TotalBytesToReceive
        ProgressBar2.Value = e.BytesReceived
    End Sub

    Private Sub DownloadCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
        If (e.Error Is Nothing) Then
            Me.count += 1
            ProgressBar1.Value += 1
        Else
            Me.TopMost = False
            My.Computer.FileSystem.DeleteFile(FolderBrowserDialog1.SelectedPath & "\" & Me.filename)
            MsgBox(Me.filename & " 파일에서 오류가 발생하였습니다.")
            Me.TopMost = True
            Exit Sub
        End If
        If (Me.count <> Me.html.Links.Count) Then
            Dim async As New WebClient
            AddHandler async.DownloadProgressChanged, AddressOf Downloading
            AddHandler async.DownloadFileCompleted, AddressOf DownloadCompleted
            async.DownloadFileAsync(New Uri(Me.html.Links(Me.count).GetAttribute("href")), _
                                    FolderBrowserDialog1.SelectedPath & "\" & Trim(Me.html.Links(Me.count).OuterText))
            Me.filename = Trim(Me.html.Links(Me.count).OuterText)
        Else
            Me.TopMost = False
            Label1.Text = "다운로드 대기 중"
            MsgBox("다운로드가 완료되었습니다.")
            Me.TopMost = True
        End If
    End Sub

    Public Function GetHtmlDocument(ByRef strhtml As String) As HtmlDocument
        Dim browser As New WebBrowser
        Dim result As HtmlDocument
        browser.DocumentText = ""
        browser.ScriptErrorsSuppressed = True
        result = browser.Document.OpenNew(True)
        result.Write(strhtml)
        Return result
    End Function
End Class