[<AutoOpen>]
module Macaw.Types

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Http.Authentication
open System.Security.Claims
open System.Collections
open System.Threading.Tasks
open Microsoft.AspNetCore.Routing

type MacawDI<'Dependency, 'Result> = 'Dependency -> 'Result

let run (d : 'Dependency) (mdi : MacawDI<'Dependency, 'Result>) =
  mdi d


let liftr (f : 'T -> 'Dependency -> 'Result) : 'T -> MacawDI<'Dependency, 'Result> =
  fun a dep -> f a dep

let lift (f : 'T -> 'Dependency -> 'Result) (a : 'T) : MacawDI<'Dependency, 'Result> =
  f a

let lift2 (f : 'T -> 'U -> 'Dependency -> 'Result) (a : 'T) (b : 'U) : MacawDI<'Dependency, 'Result> =
  f a b

type Controller = HttpContext -> RouteValueDictionary -> Async<HttpContext>
