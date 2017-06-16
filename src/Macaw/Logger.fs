module Macaw.Logger

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging

let logHelper (logLevel : LogLevel) = 
  fun (msg : string) (logger : ILoggerFactory) -> 
    let f = logger.CreateLogger("Macaw.Logger")
    match logLevel with
    | LogLevel.Trace -> f.LogTrace msg
    | LogLevel.Debug -> f.LogDebug msg
    | LogLevel.Information -> f.LogInformation msg
    | LogLevel.Warning -> f.LogWarning msg
    | LogLevel.Error -> f.LogError msg
    | LogLevel.Critical -> f.LogCritical msg
    | _ -> f.LogError "Please, select a proper LogLevel"

let logDebug message = 
  liftr (logHelper LogLevel.Debug) message
  
type ILoggerFactory with
  member this.AddMacawConsole() =
    this.AddConsole()