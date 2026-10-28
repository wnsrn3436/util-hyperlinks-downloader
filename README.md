# Hyperlinks Downloader

[한국어](README.ko.md)

This program downloads the files that several links point to, one after another, when you select the links on a web page and drag them onto its window. Once you pick a save folder, it downloads them one by one using the link text as the file name, and two bars show the overall progress and the progress of the current file. It is made for downloading a whole list at once when each link's text is the file name, as with blog attachments.

<p>
  <img src="docs/screenshots/screenshot-1.png" width="480" alt="Dragging blog attachment links onto the window">
</p>


## How to use

Download it from Releases and run it. There are x86 and x64 builds, so use the one that matches your Windows. It needs .NET Framework 4.5.

In the browser, select the part of the page that holds the file links and drag it onto the window. A dialog asks for a save folder, and once you choose one, the program downloads the links in the selection one at a time, in order. Each file takes its name from the link text, with leading and trailing spaces trimmed. The window always stays on top, so you can leave it over the browser while using it.

The upper bar shows how many of the links have been downloaded, and the lower bar shows the progress of the current file. When all downloads finish, "다운로드가 완료되었습니다." (download complete) appears. If a download fails, the program deletes that file, shows the error with the file name, and stops.


## How it works

**Links come from the dropped HTML fragment.** The dragged data is read in the `DataFormats.Html` format and written into the blank document of a `WebBrowser` that is never displayed. The browser engine parses the HTML and fills `HtmlDocument.Links` with the links, so the program uses each link's `href` as the address and its `OuterText` as the file name.

```vb
Public Function GetHtmlDocument(ByRef strhtml As String) As HtmlDocument
    Dim browser As New WebBrowser
    Dim result As HtmlDocument
    browser.DocumentText = ""
    browser.ScriptErrorsSuppressed = True
    result = browser.Document.OpenNew(True)
    result.Write(strhtml)
    Return result
End Function
```

**Downloads are chained through the completion event.** `WebClient.DownloadFileAsync` starts downloading the first link. `DownloadFileCompleted` raises the count by one and, if links remain, downloads the next one with a new `WebClient`. Downloading one file at a time keeps the order, and since the calls are asynchronous, the window stays responsive while files download. The lower bar gets the received and total byte counts from `DownloadProgressChanged` unchanged.


## Files

| Path | Content |
|---|---|
| `source/Hyperlinks Downloader.sln` | Visual Studio 2013 solution |
| `source/Hyperlinks Downloader/Form1.vb` | Drop handling, link extraction and downloading |
| `source/Hyperlinks Downloader/Form1.Designer.vb` | Form design |
| `source/Hyperlinks Downloader/Resources/link.png` | Picture shown in the window |
| `docs/screenshots/` | Screenshots |
| Releases | x86 and x64 executables |


## License

zlib. See [LICENSE](LICENSE).
