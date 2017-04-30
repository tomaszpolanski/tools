module Enumerator
open System.Text.RegularExpressions

let enumerate (str: string) =
    let split = str.Split([|','|])
                |> List.ofArray
    let single = split 
                 |> List.filter (fun i -> i.Contains("..") = false) 
                 |> List.map System.Int32.Parse

    let pair str = 
        let m = Regex.Match(str, "(\d{1,})\.\.(\d{1,})")
        if (m.Success) then Some (m.Groups.[1].Value, m.Groups.[2].Value) 
        else None
    
    let range ((first: string),(second: string)) = 
        match (System.Int32.Parse first, System.Int32.Parse second) with
        | f, s when f < s -> [f..s]
        | _ -> []
    let ranges = split 
                 |> List.filter (fun i -> i.Contains("..")) 
                 |> List.choose pair
                 |> List.collect range
    
    single@ranges
    |> List.sort