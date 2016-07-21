module Angara.Chart.SerializationTests

open Angara.Charting
open Angara.Serialization
open NUnit.Framework

let lib = SerializerLibrary("Reinstate")
do Angara.Charting.Serializers.Register [lib]

[<Test>]
let ``Serialization of PlotInfo``() =
    let primitives = 
        [ "string", PlotPropertyValue.StringValue "hello, world!"
        ; "real", PlotPropertyValue.RealValue System.Math.PI
        ; "real Array", PlotPropertyValue.RealArray [| System.Math.PI |] ] |> Map.ofList
    let composite =  [ "composite", PlotPropertyValue.Composite primitives ] |> Map.ofList
    let plotInfo : Angara.Charting.PlotInfo = { Kind = "kind"; DisplayName = "display name"; Titles = Map.empty.Add("x", "y"); Properties = composite }
    let chartInfo = [ plotInfo ] |> Chart.ofList
    let infoSet = ArtefactSerializer.Serialize lib chartInfo
    let chartInfo2 = ArtefactSerializer.Deserialize lib infoSet
    Assert.AreEqual(chartInfo, chartInfo2)

[<Test>]
let ``Petals markers are created if size is uncertain``() =
    let x = Array.empty<float>
    let uncertain = 
        { median = x
          lower68 = x
          upper68 = x
          lower95 = x
          upper95 = x } 

    let plot = 
        Plot.markers(x, x, 
          color = MarkersColor.Values x,
          size = MarkersSize.UncertainValues uncertain,
          sizePalette = MarkersSizePalette.Normalized(5.0, 15.0),
          displayName = "wheat")

    Assert.AreEqual("markers", plot.Kind, "kind")
    Assert.AreEqual("petals",   
        (match plot.Properties.["shape"] with
        | StringValue s -> s
        | _ -> "not a string"), 
        "shape")


[<Test>]
let ``Bull-eye markers are created if color is uncertain``() =
    let x = Array.empty<float>
    let uncertain = 
        { median = x
          lower68 = x
          upper68 = x
          lower95 = x
          upper95 = x } 

    let plot = 
        Plot.markers(x, x, 
          color = MarkersColor.UncertainValues uncertain,
          size = MarkersSize.Values x,
          sizePalette = MarkersSizePalette.Normalized(5.0, 15.0),
          displayName = "wheat")

    Assert.AreEqual("markers", plot.Kind, "kind")
    Assert.AreEqual("bulleye",   
        (match plot.Properties.["shape"] with
        | StringValue s -> s
        | _ -> "not a string"), 
        "shape")

[<Test>]
let ``Box-and-whisker markers are created if color is uncertain``() =
    let x = Array.empty<float>
    let uncertain = 
        { median = x
          lower68 = x
          upper68 = x
          lower95 = x
          upper95 = x } 

    let plot = 
        Plot.markers(
          MarkersX.Values x, 
          MarkersY.UncertainValues uncertain, 
          color = MarkersColor.Values x,
          size = MarkersSize.Values x,
          sizePalette = MarkersSizePalette.Normalized(5.0, 15.0),
          displayName = "wheat")

    Assert.AreEqual("markers", plot.Kind, "kind")
    Assert.AreEqual("boxwhisker",   
        (match plot.Properties.["shape"] with
        | StringValue s -> s
        | _ -> "not a string"), 
        "shape")