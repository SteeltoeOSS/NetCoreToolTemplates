namespace Company.WebApplication.FS.Models
#if (AnyEfCore)
open System

type ErrorViewModel(RequestId: string) =
      member this.ShowRequestId = String.IsNullOrEmpty RequestId
#endif
