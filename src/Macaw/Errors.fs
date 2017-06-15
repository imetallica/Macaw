module Macaw.Errors

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection

type IApplicationBuilder with
  member this.UseMacawDeveloperExceptionPage() = 
    this.UseDeveloperExceptionPage()

  member this.UseMacawExceptionHandler(path : string) = 
    this.UseExceptionHandler(path)

  member this.UseMacawStatusCodePage() = 
    this.UseStatusCodePages()