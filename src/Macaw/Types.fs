[<AutoOpen>]
module Macaw.Types

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Http.Authentication
open System.Security.Claims
open System.Collections
open System.Threading.Tasks

type Conn = {
    Request : HttpRequest
    Response : HttpResponse
    Authentication : AuthenticationManager
    Session : ISession
    WebSockets : WebSocketManager
    User : ClaimsPrincipal
}

type Params = IDictionary
type Controller = Conn -> Params -> Conn
type AsyncController = Conn -> Params -> Async<Conn>

type Microsoft.FSharp.Control.AsyncBuilder with
    member x.Bind(t : Task<'T>, f : 'T -> Async<'R>) : Async<'R> =
        async.Bind(Async.AwaitTask t, f)

    member x.Bind(t : Task, f : 'T -> Async<'R>) : Async<'R> = 
        async.Bind(Async.AwaitTask t, f)

let httpContextToConn (ctx : HttpContext) : Conn = {
    Request = ctx.Request
    Response = ctx.Response
    Authentication = ctx.Authentication
    Session = ctx.Session
    WebSockets = ctx.WebSockets
    User = ctx.User
}