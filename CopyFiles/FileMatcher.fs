module FileMatcher
open System.Text.RegularExpressions
open FileAccess

let matchingFiles (ranges: int list) files = 
    let group (FileName file) =
        let m = Regex.Match(file, "(\d{1,}).*\.\w+$")
        if (m.Success) then Some (System.Int32.Parse m.Groups.[1].Value, FileName file) else None

    files |> List.choose group
          |> List.filter (fun (i,_) -> (List.contains i ranges) )
          |> List.map (fun (_,file) -> file)