open System
open Hw5


let returnResult parsedArgs =
    match parsedArgs with
    | Ok (arg1, operation, arg2) ->
        (Calculator.calculate arg1 operation arg2).ToString()
    | Error msg ->
        match msg with
        | Message.WrongArgLength -> "Wrong amount of arguments"
        | Message.WrongArgFormat -> "Wrong format of arguments"
        | Message.WrongArgFormatOperation -> "Wrong operation format"
        | Message.DivideByZero -> "Division by zero"
        | _ -> "Unknown error"


[<EntryPoint>]
let main args =
    args |> Parser.parseCalcArguments |> returnResult |> printfn "%s"
    0