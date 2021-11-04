namespace Company.WebApplication1.Models

#if (AnyEfCore)
open Microsoft.EntityFrameworkCore
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema

type SampleContext(options : DbContextOptions) =
    inherit DbContext(options)
#endif
