module Macaw.Test.Program

open Microsoft.AspNetCore.Hosting
open Macaw.Logger

[<EntryPoint>]
let main args = 

  do logDebug "Hello Mom!"

  0