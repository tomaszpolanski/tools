module FileAccess
open System.IO
open System.Windows.Forms

type FileName = FileName of string

type Extension = 
    | XML
    | TXT
    | CSV
    | STA
    | ALL

let filterExtensions ext =
    let formatExtension = function
    | XML -> "XML Files (.xml)|*.xml"
    | TXT -> "Text Files (.txt)|*.txt"
    | CSV -> "Comma Seperated Value Files (.csv)|*.csv"
    | STA -> "Statement Files (.sta)|*.sta"
    | ALL -> "All files (*.*)|*.*"

    ext 
    |> List.map formatExtension
    |> List.reduce (sprintf "%s|%s")

let getFile message extensions = 
    let dialog = new OpenFileDialog();
    dialog.Title <- message
    dialog.Filter <- filterExtensions extensions
    if dialog.ShowDialog() = DialogResult.OK then
        Some (FileName dialog.FileName)
    else
        printfn "Opening file was cancelled."
        None

let readLines extensions  = 
    let dialog = new OpenFileDialog();
    dialog.Title <- "Open File";
    dialog.Filter <- filterExtensions extensions
    if dialog.ShowDialog() = DialogResult.OK then
        Option.ofObj( seq { 
                            use fileStream = dialog.OpenFile()
                            use sr = new StreamReader(fileStream)
                            while not sr.EndOfStream do yield sr.ReadLine() 
                            })
    else
        printfn "Opening file was cancelled."
        None

let readLinesFromFile (FileName filePath) = 
    if File.Exists(filePath) then 
        seq { 
              use sr = new StreamReader(filePath)
              while not sr.EndOfStream do
                yield sr.ReadLine()
             }  
        |> Option.ofObj
    else 
        printfn "File %s does not exist!" filePath
        Option.None

let writeText (FileName fileName) extensions (text: string)  = 
    let ofd = new SaveFileDialog();
    ofd.Title <- "Save";
    ofd.Filter <- filterExtensions extensions
    ofd.FileName <- fileName
    if ofd.ShowDialog() = DialogResult.OK then
        use fileStream = ofd.OpenFile()
        use sw = new StreamWriter(fileStream)
        sw.WriteLine(text);
        Some ofd.FileName
    else
        printfn "Saving file was cancelled."
        None

let writeTextToFile file text = 
    File.WriteAllText(file, text)

let createDir folder = 
    Directory.CreateDirectory (sprintf "./%s" folder)

let allFiles folder =
    Directory.EnumerateFiles(folder) 
    |> Seq.map FileName