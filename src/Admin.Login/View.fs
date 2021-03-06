module Admin.Login.View

open System
open Fable.Core
open Fable.Core.JsInterop
open Admin.Login.Types
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.React

module Option =
  let ofElement (elem : ReactElement option) =
    unbox<ReactElement> elem
    

type InputType = Text | Password 
let textInput inputLabel initial inputType (onChange: string -> unit) = 
  let inputType = match inputType with 
                  | Text -> "input"
                  | Password -> "password"
  div 
    [ ClassName "form-group" ]
    [ input [ ClassName "form-control form-control-lg"
              Type inputType
              DefaultValue initial
              Placeholder inputLabel
              OnChange (fun e -> onChange !!e.target?value) ] ]

let loginFormStyle = 
  Style [ Width "400px"
          MarginTop "70px"
          TextAlign "center" ]

let cardBlockStyle = 
  Style [ Padding "30px"
          TextAlign "left"
          BorderRadius 10 ]

[<Emit("null")>]
let emptyElement : ReactElement = jsNative
let errorMessagesIfAny triedLogin = function
  | [ ] -> emptyElement
  | x when triedLogin = false -> emptyElement
  | errors ->
    let errorStyle = Style [ Color "crimson"; FontSize 12 ]
    ul [ ] 
       [ for error in errors -> 
          li [ errorStyle ] [ str error ] ]

let appIcon = 
  img [ Src "/img/fable_logo.png"
        Style [ Height 80; Width 100 ] ]

let render (state: State) dispatch = 

    let loginBtnContent = 
      if state.LoggingIn then i [ ClassName "fa fa-circle-o-notch fa-spin" ] []
      else str "Login"

    let validationRules = 
      [ state.InputUsername.Trim().Length >= 5
        state.InputPassword.Trim().Length >= 5 ]
    
    let canLogin = Seq.forall id validationRules

    let btnClass = 
      if canLogin 
      then "btn btn-success btn-lg"
      else "btn btn-info btn-lg"
    div 
      [ ClassName "container" ; loginFormStyle ]
      [ div 
         [ ClassName "card" ]
         [ div
             [ ClassName "card-block"; cardBlockStyle ]
             [ div 
                [ Style [ TextAlign "center" ] ] 
                [ appIcon ]
               br []
               textInput "Username" state.InputUsername Text (ChangeUsername >> dispatch)
               errorMessagesIfAny state.HasTriedToLogin state.UsernameValidationErrors
               textInput "Password" state.InputPassword Password (ChangePassword >> dispatch)
               errorMessagesIfAny state.HasTriedToLogin state.PasswordValidationErrors
               div
                [ Style [ TextAlign "center" ] ]
                [ button 
                    [ ClassName btnClass
                      OnClick (fun e -> dispatch Login) ] 
                    [ loginBtnContent ] ] ] ] ]