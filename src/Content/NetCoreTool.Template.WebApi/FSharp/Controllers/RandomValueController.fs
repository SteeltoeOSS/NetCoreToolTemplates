namespace Company.WebApplication.FS.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging

type RandomValue = {intVal : int; longVal: int64; int10: int; long10: int64; int10_20: int; long100_200: int64; uuid: string; stringVal: string}

[<ApiController>]
[<Route("[controller]")>]
type RandomValueController (logger : ILogger<RandomValueController>, _config: IConfiguration) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() =
        { intVal = int _config.["random:int"]
          longVal = int64 _config.["random:long"]
          int10 = int _config.["random:int(10)"]
          long10 = int64 _config.["random:long(100)"]
          int10_20 = int _config.["random:int(10,20)"]
          long100_200 = int64 _config.["random:long(100,200)"]
          uuid = _config.["random:uuid"]
          stringVal = _config.["random:string"] }
