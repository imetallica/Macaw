module Macaw.Test.Program

open Microsoft.AspNetCore.Hosting
open Macaw.Logger

[<EntryPoint>]
let main args = 
  use f = new Microsoft.Extensions.Logging.LoggerFactory()

  do logDebug "Hello Mom!"

  0