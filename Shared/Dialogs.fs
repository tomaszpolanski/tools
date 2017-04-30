module Dialogs
open System.Windows.Forms

let showInfo (message: string) (title: string) =   
    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore

let showError (message: string) (title: string) =   
    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore