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

    let map r (f : 'a -> 'b) = 
        match r with
        | Val x -> ResExt.WithDataFrom(ResultHelpers.Success (f x),  r)
        | Unval -> r.ErrorCast()

type DoBind<'a> = 

    val builder : ResBuilder

    new() = {builder = ResBuilder.Empty()}

    member this.Zero () = ResultHelpers.Blank()

    member this.Bind (a : 'a Res, f : 'a -> 'b Res) = 
        let k = a.Map<'b> f
        this.builder.AddData k; k

    member this.Return a = this.builder.Build a
    member this.ReturnFrom a = 
        match a with 
        | Val x -> this.builder.Build x 
        | Unval -> this.builder.BuildError<_>()

    member _.Yield() = ()


let bind state settings (a : #AST.ASTNode) = a.Bind(state, settings)
let bindMatching state (t : #Structure.LangtType) (a : #AST.ASTNode) = a.BindMatchingExprType(state, t)

let inline dobind<'a> = DoBind<'a> ()

let test (state : AST.ASTPassState) (settings : 'Ignored) (x : AST.Return) = dobind {
    if x.Value = null then 
        return new AST.BoundReturn(x, null)
    else 
        let rtype = state.CTX.CurrentFunction.Type.ReturnType
        let! vr = bindMatching state rtype x.Value 

        return new AST.BoundReturn(x, vr)
}