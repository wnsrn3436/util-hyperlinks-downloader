# Hyperlinks Downloader

A program that downloads the files behind several links in turn when you select them on a web page and drag them onto its window. After a save folder is chosen, the files are downloaded one by one with the link text as the file name, and two bars show the overall progress and the progress of the current file. It is suited to fetching a list of links whose text is the file name, such as blog attachments, all at once.

<p>
  <img src="docs/screenshots/screenshot-1.png" width="480" alt="Dragging blog attachment links onto the window">
</p>


## How to use

Download from Releases and run it. There are x86 and x64 builds, so use the one that matches your Windows, and .NET Framework 4.5 is required.

In the browser, select the part of the page that holds the file links and drag it onto the window. A folder dialog opens, and once a folder is chosen the links in the selection are downloaded one by one in order. Each file is named after its link text with the surrounding spaces trimmed. The window always stays on top, so it can sit over the browser while you drag.

The upper bar counts the downloaded files out of all links, and the lower bar shows the progress of the current file. When everything is done, "다운로드가 완료되었습니다." (download complete) appears. If a download fails, that file is deleted, an error naming the file is shown, and the downloading stops.


## How it works

**Links are pulled out of the dropped HTML fragment.** The dragged data is read in the `DataFormats.Html` format and written into a blank document of a `WebBrowser` that is never shown. The browser engine parses the HTML and fills `HtmlDocument.Links` with the links, so each link's `href` serves as the address and its `OuterText` as the file name.

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

**Downloads are chained through the completion event.** `WebClient.DownloadFileAsync` starts on the first link, and `DownloadFileCompleted` raises the count by one and, if links remain, starts the next one with a new `WebClient`. Only one file is downloaded at a time, so the order is kept, and because it is asynchronous the window stays responsive meanwhile. The lower bar takes the received and total bytes reported by `DownloadProgressChanged` as they are.


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
