module Macaw.Session

open System
open System.Text
open Newtonsoft.Json
open Microsoft.AspNetCore.Session
open Microsoft.AspNetCore.Http
        
let getSessionValue<'T> (key : string) (conn : Conn) = async {
    do! conn.Session.LoadAsync() // Always call load async
    
    match conn.Session.TryGetValue(key) with
    | (true, v) -> 
        if v.Length > 0 then
            let converted = Encoding.Unicode.GetString(v)
            return Some (JsonConvert.DeserializeObject<'T>(converted))
        else 
            return None
    | _ -> return None
}

let setSessionValue (key : string) (value : 'T) (conn : Conn) = async {
    do! conn.Session.LoadAsync() // Always call load async

    let converted = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(value))
    conn.Session.Set(key, converted)

    return conn
}


// Callback helpers

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection


module AppConfigurer =
    let useSession (app : IApplicationBuilder) = 
        app.UseSession()

module ServiceConfigurer = 
    type SessionOptions = {
        CookieDomain : string
        CookieHttpOnly : bool
        CookieName : string
        CookiePath : string
        CookieSecure : CookieSecurePolicy
        IdleTimeout : TimeSpan
    }

    type MemoryCacheOptions = {
        ExpirationScanFrequency : TimeSpan
        CompactOnMemoryPressure : bool
    }

    let defaultSessionOptions = {
        CookieDomain = ""
        CookieHttpOnly = true
        CookieName = ""
        CookiePath = ""
        CookieSecure = CookieSecurePolicy.SameAsRequest
        IdleTimeout = TimeSpan.FromMinutes(60.)
    }

    let defaultMemoryCacheOptions = {
        ExpirationScanFrequency = TimeSpan.FromMinutes(60.)
        CompactOnMemoryPressure = false
    }

    let addDistributedMemoryCache (services : IServiceCollection) = 
        services.AddDistributedMemoryCache()

    let addMemoryCache (opts : MemoryCacheOptions) (services : IServiceCollection) = 
        services.AddMemoryCache(fun o -> 
            o.ExpirationScanFrequency <- opts.ExpirationScanFrequency
            o.CompactOnMemoryPressure <- opts.CompactOnMemoryPressure
        )

    let addSession (opts : SessionOptions) (services : IServiceCollection) =
        services.AddSession(fun o -> 
            o.CookieDomain <- opts.CookieDomain
            o.CookieHttpOnly <- opts.CookieHttpOnly
            o.CookieName <- opts.CookieName
            o.CookiePath <- opts.CookiePath
            o.CookieSecure <- opts.CookieSecure
            o.IdleTimeout <- opts.IdleTimeout
        )