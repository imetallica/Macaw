module Macaw.Router

open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Routing
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection

type RequestMethods = 
  | DELETE 
  | GET 
  | POST 
  | PUT 
  
type Route = {
  RequestMethod : RequestMethods
  UrlPath : string
  Controller : Controller  
}

let private controllerExecutor (controller : Controller) (ctx : HttpContext) =
  async {
    let rd = ctx.GetRouteData()
    let! _ = controller ctx rd.Values
    return ()
  } |> Async.StartAsTask :> Task

type IApplicationBuilder with
  member this.UseMacawRouter(routes : Route list) =
    let routeBuilder = RouteBuilder(this)

    let _ = 
      Seq.map (fun x -> 
        match x.RequestMethod with
        | DELETE -> 
          routeBuilder.MapDelete(x.UrlPath, RequestDelegate(controllerExecutor x.Controller))
        | GET -> 
          routeBuilder.MapGet(x.UrlPath, RequestDelegate(controllerExecutor x.Controller))
        | POST -> 
          routeBuilder.MapPost(x.UrlPath, RequestDelegate(controllerExecutor x.Controller))
        | PUT -> 
          routeBuilder.MapPut(x.UrlPath, RequestDelegate(controllerExecutor x.Controller))
      ) routes
    
    let routes = routeBuilder.Build()
    this.UseRouter(routes)