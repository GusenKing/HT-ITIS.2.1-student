module Hw6.App

open System
open System.Net.Http
open Hw6.Calculator
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe

let extractArgsForCalculatorToArray (context: HttpContext) =
    [|
        context.Request.Query["value1"].ToString();
        context.Request.Query["operation"].ToString();
        context.Request.Query["value2"].ToString()
        |]

let calculatorHandler: HttpHandler =
    fun next ctx ->
        let result: Result<string, string> = Calculator.MaybeBuilder.maybe {
            let! parsedArguments = ctx |> extractArgsForCalculatorToArray |> Parser.parseCalcArguments
            let! result = Calculator.calculate parsedArguments
            return result
            }

        match result with
        | Ok ok -> (setStatusCode 200 >=> text (ok.ToString())) next ctx
        | Error error -> (setStatusCode 400 >=> text error) next ctx

let webApp =
    choose [
        GET >=> choose [
             route "/" >=> text "Use //calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>"
             routeStartsWith "/calculate" >=> calculatorHandler
        ]
        setStatusCode 404 >=> text "Not Found" 
    ]
    
type Startup() =
    member _.ConfigureServices (services : IServiceCollection) =
        services.AddGiraffe().AddMiniProfiler(fun option -> option.RouteBasePath <- "/profiler") |> ignore

    member _.Configure (app : IApplicationBuilder) (_ : IHostEnvironment) (_ : ILoggerFactory) =
        app.UseMiniProfiler().UseGiraffe webApp
        
[<EntryPoint>]
let main _ =
    Host
        .CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun whBuilder -> whBuilder.UseStartup<Startup>() |> ignore)
        .Build()
        .Run()
    0