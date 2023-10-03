open Hw4

[<EntryPoint>]
let main args =
    try
        let parsedArgs = Parser.parseCalcArguments args
        let result = Calculator.calculate parsedArgs.arg1 parsedArgs.operation parsedArgs.arg2
        printfn $"{result}"
    with
        | ex -> printfn $"{ex.Message}"
    0