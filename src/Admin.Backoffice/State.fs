module Admin.Backoffice.State

open Elmish
open Admin.Backoffice.Types

let update msg state = state, Cmd.none

let init() = { Username = "" }, Cmd.none