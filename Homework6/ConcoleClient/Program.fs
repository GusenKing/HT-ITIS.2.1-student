open System.Net.Http
open System

let convertOperation operation =
    match operation with
    | "+" -> "Plus"
    | "-" -> "Minus"
    | "*" -> "Multiply"
    | "/" -> "Divide"
    | _ -> operation

let convertInputToUrl (input: string) =
    let splitInput = input.Split(" ")
    match splitInput.Length with
    | 3 ->
        let operation = convertOperation splitInput[1]
        Ok $"http://localhost:5000/calculate?value1={splitInput[0]}&operation={operation}&value2={splitInput[2]}"
    |_ -> Error "Wrong amount of arguments for request"

let sendRequestAsync(client: HttpClient, url: string) =
    async {
        let! response = Async.AwaitTask(client.GetAsync url)
        return response
    }

[<EntryPoint>]
let main args =
    while true do
        let input = Console.ReadLine()
        let client = new HttpClient()
        let urlResult = convertInputToUrl input
        match urlResult with
        | Ok url ->
            let serverResult = Async.RunSynchronously(sendRequestAsync(client, url))
            printfn $"Result from server: {serverResult.Content.ReadAsStringAsync().Result}"
        | Error err ->
            printfn $"{err}"
    0