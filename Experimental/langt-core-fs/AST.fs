module Langt.AST

type ResultHelpers = Results.Result
type 'a Res = 'a Results.Result

type ResBuilder = Results.ResultBuilder

type ResExt = Results.ResultExtensions

type TCO = AST.TypeCheckOptions
let TCODefault = System.Nullable(TCO())

[<AutoOpen>]
module private Helpers =

    let (|Val|Unval|) (a : 'a Res) = if a.HasValue then Val(a.Value) else Unval
    let (|Succ|Err|) (a : 'a Res) = if a.HasErrors then Err(a.Errors) else Succ

    let (@) r (f : 'a -> 'b) = 
        match r with
        | Val x -> ResultHelpers.Success (f x)
        | Unval -> r.ErrorCast()

    let (!) r = 
        match r with 
        | Val x -> x 
        | Unval -> failwith "Expected a valid result"

    let (/) r a =
        match r with 
        | Val x -> x
        | Unval -> a

    let (*) r a = 
        match r, a with
        | (Val x, Val y) -> ResultHelpers.Success ((x, y))
        | _ -> r.ErrorCast()

    let k = Res 10
    let x = k @ float
    let p = !x
    let z = k / 1
    let m = k * x
    let t = [k, x @ int, m @ fun (x, _) -> x]
