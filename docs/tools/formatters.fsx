#r "../../packages/Angara.Serialization/lib/net452/Angara.Serialization.dll"
#r "../../packages/docs/Angara.Serialization.Json/lib/net452/Angara.Serialization.Json.dll"
#r "../../packages/docs/Angara.Reinstate/lib/net452/Angara.Reinstate.dll"
#r "../../packages/docs/Angara.Html/lib/net452/Angara.Html.dll"
#r "../../bin/Angara.Chart/Angara.Chart.dll"

open FSharp.Literate 
open FSharp.Markdown 

let createFsiEvaluator () =
    Angara.Charting.Serializers.Register([ Angara.Html.Serializers ])
    
    let transformation (value:obj, typ:System.Type) =
        match value with
        | :? Angara.Charting.Chart as chart ->
            let html = Angara.Html.MakeEmbeddable "400px" chart 
            Some [  InlineBlock html ]
        | _ -> None
        
    let fsiEvaluator = FsiEvaluator()
    fsiEvaluator.RegisterTransformation(transformation);
    fsiEvaluator