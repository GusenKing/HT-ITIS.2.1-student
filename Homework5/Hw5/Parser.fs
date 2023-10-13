module Hw5.Parser

open System
open System.Globalization
open Hw5.Calculator
open Hw5.MaybeBuilder

let isArgLengthSupported (args:string[]): Result<'a,'b> =
    match args.Length with
    | 3 -> Ok args
    | _ -> Error Message.WrongArgLength
    
[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isOperationSupported (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match operation with
    | Calculator.plus -> Ok (arg1, CalculatorOperation.Plus, arg2)
    | Calculator.minus -> Ok (arg1, CalculatorOperation.Minus, arg2)
    | Calculator.multiply -> Ok (arg1, CalculatorOperation.Multiply, arg2)
    | Calculator.divide -> Ok (arg1, CalculatorOperation.Divide, arg2)
    | _ -> Error Message.WrongArgFormatOperation

let parseValue (arg: string) =
    let outcome, parsedValue = Double.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture)
    if outcome then Ok parsedValue
    else Error Message.WrongArgFormat

let parseArgs (args: string[]): Result<('a * CalculatorOperation * 'b), Message> =
     maybe
        {
            let! args = isArgLengthSupported args
            let! arg1 = parseValue args[0]
            let! arg2 = parseValue args[2]
            let! parsedArgsWithOperation = isOperationSupported(arg1, args[1], arg2)
            return parsedArgsWithOperation
        }

[<System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage>]
let inline isDividingByZero (arg1, operation, arg2): Result<('a * CalculatorOperation * 'b), Message> =
    match (operation, arg2) with
    | (CalculatorOperation.Divide, 0.0) -> Error Message.DivideByZero
    | _ -> Ok (arg1, operation, arg2)
        
let parseCalcArguments (args: string[]): Result<'a, 'b> =
    maybe
        {
        let! parsedArgs = parseArgs args
        let! notDividedByZeroArgs = isDividingByZero parsedArgs
        return notDividedByZeroArgs
        }
