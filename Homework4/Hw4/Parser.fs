module Hw4.Parser

open System
open Hw4.Calculator


type CalcOptions = {
    arg1: float
    arg2: float
    operation: CalculatorOperation
}

let isArgLengthSupported (args : string[]) =
    args.Length = 3
    
let parseOperation (arg : string) =
    match arg with
    |"+" -> CalculatorOperation.Plus
    |"-" -> CalculatorOperation.Minus
    |"*" -> CalculatorOperation.Multiply
    |"/" -> CalculatorOperation.Divide
    |_ -> raise (ArgumentException("Wrong operation"))
        
let parseCalcArguments(args : string[]) =
    let mutable n1 = 0
    let mutable n2 = 0
    if not (isArgLengthSupported args) then ArgumentException("Incorrect amount of arguments") |> raise
    if not Double.TryParse args[0] n1 then ArgumentException("First argument must be a number") |> raise
    let parsedOperation = parseOperation args[1]
    if not (Double.TryParse(args[2] n2)) then ArgumentException("Third argument must be a number") |> raise
    
    let result = {
        arg1 = n1;
        arg2 = n2;
        operation = parsedOperation}    
    result 
