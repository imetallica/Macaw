module Macaw.Auth.Cookies

open System.Security.Claims
open Microsoft.AspNetCore.Authentication.Cookies

open Macaw

type AuthScheme = string
 

let signIn (authScheme : AuthScheme) (claims : ClaimsPrincipal) (conn : Conn) = async {
    do! conn.Authentication.SignInAsync(authScheme, claims)
    return conn
}

let signOut (authScheme : AuthScheme) (conn : Conn) = async {
    do! conn.Authentication.SignOutAsync(authScheme)
    return conn
}


// Callback Helpers

module AppConfigurer = 
    open Microsoft.AspNetCore.Builder

    type CookieAuthOptions = {
        AuthScheme : AuthScheme

        AutomaticChallenge : bool
        AutomaticProof : bool
    }

    let useCookieAuthentication (cookieAuthOptions : CookieAuthOptions) (app : IApplicationBuilder) = 
        let foo = CookieAuthenticationOptions(AuthenticationScheme = cookieAuthOptions.AuthScheme)
        foo.AuthenticationScheme = cookieAuthOptions.AuthScheme
        app.UseCookieAuthentication()

