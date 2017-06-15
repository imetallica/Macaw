module Macaw.Session

open System
open System.Text
open Newtonsoft.Json
open Microsoft.AspNetCore.Session
open Microsoft.AspNetCore.Http
        
let getSessionValue<'T> (key : string) (ctx : HttpContext) = async {
    do! ctx.Session.LoadAsync() |> Async.AwaitTask // Always call load async
    
    match ctx.Session.TryGetValue(key) with
    | (true, v) -> 
        if v.Length > 0 then
            let converted = Encoding.UTF8.GetString(v)
            return Some (JsonConvert.DeserializeObject<'T>(converted))
        else 
            return None
    | _ -> return None
}

let setSessionValue (key : string) (value : 'T) (ctx : HttpContext) = async {
    do! ctx.Session.LoadAsync() |> Async.AwaitTask // Always call load async

    let converted = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value))
    ctx.Session.Set(key, converted)

    return ctx
}


// Callback helpers

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection

type MacawSessionOptions = {
    CookieDomain : string
    CookieHttpOnly : bool
    CookieName : string
    CookiePath : string
    CookieSecure : CookieSecurePolicy
    IdleTimeout : TimeSpan
}

type MacawMemoryCacheOptions = {
    ExpirationScanFrequency : TimeSpan
    CompactOnMemoryPressure : bool
}

let defaultMacawSessionOptions = {
    CookieDomain = ""
    CookieHttpOnly = true
    CookieName = ""
    CookiePath = ""
    CookieSecure = CookieSecurePolicy.SameAsRequest
    IdleTimeout = TimeSpan.FromMinutes(60.)
}

let defaultMacawMemoryCacheOptions = {
    ExpirationScanFrequency = TimeSpan.FromMinutes(60.)
    CompactOnMemoryPressure = false
}

type IApplicationBuilder with
    member this.UseMacawSession() = 
        this.UseSession()

type IServiceCollection with
    member this.AddMacawDistributedMemoryCache () = 
        this.AddDistributedMemoryCache()

    member this.AddMacawMemoryCache (opts : MacawMemoryCacheOptions) =
        this.AddMemoryCache(fun o -> 
            o.ExpirationScanFrequency <- opts.ExpirationScanFrequency
            o.CompactOnMemoryPressure <- opts.CompactOnMemoryPressure
        )

    member this.AddMacawSession (opts : MacawSessionOptions) =
        this.AddSession(fun o ->
            o.CookieDomain <- opts.CookieDomain
            o.CookieHttpOnly <- opts.CookieHttpOnly
            o.CookieName <- opts.CookieName
            o.CookiePath <- opts.CookiePath
            o.CookieSecure <- opts.CookieSecure
            o.IdleTimeout <- opts.IdleTimeout
        )