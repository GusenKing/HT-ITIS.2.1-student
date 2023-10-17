module Hw6.Calculator.Parser

open System
open System.Globalization
open Hw6.Calculator.Calculator
open Hw6.Calculator.MaybeBuilder


[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), string> =
    match operation with
    | Calculator.plus -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | Calculator.minus -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | Calculator.multiply -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | Calculator.divide -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error $"Could not parse value '{operation}'"

let parseValue (arg: string) =
    let outcome, parsedValue = Double.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture)
    if outcome then Ok parsedValue
    else Error $"Could not parse value '{arg}'"

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), string> =
     maybe
        {
            let! arg1 = parseValue args[0]
            let! arg2 = parseValue args[2]
            let! parsedArgsWithOperation = isOperationSupported(arg1, args[1], arg2)
            return parsedArgsWithOperation
        }
        
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe
        {
        let! parsedArgs = parseArgs args
        return parsedArgs
        }
