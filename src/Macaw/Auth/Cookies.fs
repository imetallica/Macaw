module Macaw.Auth.Cookies

open System.Security.Claims
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Authentication.Cookies

type AuthScheme = string
 

let signIn (authScheme : AuthScheme) (claims : ClaimsPrincipal) (ctx : HttpContext) = async {
    do! ctx.Authentication.SignInAsync(authScheme, claims) |> Async.AwaitTask
    return ctx
}

let signOut (authScheme : AuthScheme) (ctx : HttpContext) = async {
    do! ctx.Authentication.SignOutAsync(authScheme) |> Async.AwaitTask
    return ctx
}


// Callback Helpers

module AppConfigurer = 
    open Microsoft.AspNetCore.Builder

    type CookieAuthOptions = {
        AuthScheme : AuthScheme
    }

    let useCookieAuthentication (app : IApplicationBuilder) = 
        app.UseCookieAuthentication()

(*
type MacawAuthCookieMiddleware (next : RequestDelegate,
                                loggerFactory : ILoggerFactory) =
    member this.Invoke(ctx : HttpContext) = 
        async {
            let logger = loggerFactory.CreateLogger<MacawAuthCookieMiddleware>()
            
            return 0
        } |> Async.StartAsTask

        *)