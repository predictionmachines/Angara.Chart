(*** hide ***)
#I "../../bin/Angara.Chart"
#I "../../packages/docs/Angara.Table/lib/net452"
#I "../../packages/docs/System.Collections.Immutable/lib/portable-net45+win8+wp8+wpa81"

(**
Angara.Chart
======================

An F# data visualization library. Supports visualization of uncertain values. 
Powered by JavaScript library [InteractiveDataDisplay](https://github.com/predictionmachines/InteractiveDataDisplay).

<div class="row">
  <div class="span1"></div>
  <div class="span6">
    <div class="well well-small" id="nuget">
      The Angara.Chart library can be <a href="https://nuget.org/packages/Angara.Chart">installed from NuGet</a>:
      <pre>PM> Install-Package Angara.Chart</pre>
    </div>
  </div>
  <div class="span1"></div>
</div>

Example
-------

The example builds a chart consisting of the line and markers plots:

*)
#r "Angara.Chart.dll"

open Angara.Charting

open System

let x = [| for i in 0..99 -> float(i) / 10.0 |]
let y = x |> Array.map (fun x -> sin x)
let z = x |> Array.map (fun x -> cos x)

let chart = [ Plot.line(x, y); Plot.markers(x, z) ] |> Chart.ofList

(*** include-value: chart ***)


(**

Plot Gallery 
------------

This section is a gallery of plots supported by the Angara.Chart.
Here we use [Angara.Table](http://github.com/predictionmachines/Angara.Table) to load sample 
data which then is displayed with Angara.Chart.

### Preparing data

The code loads several tables that will be used in the following charts.
*)


#r "Angara.Table.dll"
#r "System.Collections.Immutable.dll"
open Angara.Data
open System.Linq

let wheat = Table.Load("../files/data/wheat.csv")
let uwheat = Table.Load("../files/data/uwheat.csv")
let site = Table.Load("../files/data/site.csv")
let npz = Table.Load("../files/data/npz.csv")
let grid = Table.Load("../files/data/grid.csv")
let ugrid = Table.Load("../files/data/ugrid.csv")

// This function returns a float array of the given table column 
let col (colName:string) (t:Table) : float[] = t.[colName].Rows.AsReal.ToArray()
// This function returns an array of records representing quantiles,
// so instead of a value it gives an information about its uncertainty.
let quantiles prefix table = 
    { median = table |> col (prefix + "_median")
      lower68 = table |> col (prefix + "_lb68")
      upper68 = table |> col (prefix + "_ub68")
      lower95 = table |> col (prefix + "_lb95")
      upper95 = table |> col (prefix + "_ub95") }

(**

### Line and uncertainty bands

The functions `Plot.line`allow to define a plot displayed straight line segments connecting a series of data points.

*)

(** It is enough to provide just `y` data series to define a line plot: *)

let y = Array.init 100 (fun i -> let x = float(i-50)/10.0 in x*x)

let line_y = [ Plot.line(y) ] |> Chart.ofList

(*** include-value: line_y ***)

(** The following plot takes both `x` and `y` series, defines `stroke` color, line `thickness`
and axis titles.
See the API Reference for other optional parameteres of the line plot. *)

let t = site |> col "t"
let p = site |> col "p"

let line_tp = 
  [ Plot.line(t, p, stroke = "#00007F", thickness = 2.0, titles = Titles.line("time", "p")) ] 
  |> Chart.ofList

(*** include-value: line_tp ***)

(**
If a variable that determines the position on the vertical axis is uncertain, 
bands corresponding to the given quantiles of the uncertain values are displayed in addition to the line segments,
which represent median values. *)

let y_uncertain = npz |> quantiles "p"

let line_uncertain = 
  [ Plot.line(LineX.Values t, LineY.UncertainValues y_uncertain, titles = Titles.line("time", "p")) ] 
  |> Chart.ofList

(*** include-value: line_uncertain ***)

(**
### Area

*)

let y1 = npz |> col "p_lb68"
let y2 = npz |> col "p_ub68"

let area = 
  [ Plot.area(t, y1, y2, titles = Titles.area("time", "p - lower bound", "p - upper bound")) ] 
  |> Chart.ofList

(*** include-value: area ***)

(**
### Markers

It is a plot which displays data as a collection of points, each having the value of one data series determining 
the position on the horizontal axis and the value of the other data series determining 
the position on the vertical axis. Also data series can be bound to marker size and color, 
and other appearance settings.

Markers plot has default collection of supported shapes such as box, circle, cross, triangle etc, 
but still it allows creating new shapes.
*)

let lon = wheat |> col "Lon"
let lat = wheat |> col "Lat"
let wheat = wheat |> col "wheat"

let markers = [ Plot.markers(lon, lat, displayName = "wheat") ] |> Chart.ofList

(*** include-value: markers ***)

let markers_colors =
  [ Plot.markers(lon, lat, 
      color = MarkersColor.Values wheat, 
      colorPalette = "0=Red=Green=Yellow=Blue=10", 
      shape = MarkersShape.Circle, 
      displayName = "wheat",
      titles = Titles.markers("lon", "lat", color = "wheat production")) ] 
  |> Chart.ofList
        
(*** include-value: markers_colors ***)

let markers_colors_sizes = 
  [ Plot.markers(lon, lat, 
      color = MarkersColor.Values wheat, colorPalette = "Red,Green,Yellow,Blue", 
      size = MarkersSize.Values wheat, sizePalette = MarkersSizePalette.Normalized(), 
      shape = MarkersShape.Diamond, 
      displayName = "wheat",
      titles = Titles.markers(color = "wheat production", size = "wheat production")) ] 
  |> Chart.ofList

(*** include-value: markers_colors_sizes ***)

(**
The shapes `petals`, `bull-eye`, `boxwhisker` allow drawing  data  
if one of the properties is uncertain. Uncertain variable is represented as a set of quantiles. 
Particular qualities for each shape are described below.

**Petals** is a kind of markers when shape indicates level of uncertainty.
Data series `size` is a set of quantiles which must contain `lower95` and `upper95`.
*)

let wheat_uncertain = uwheat |> quantiles "w"

let markers_petals =
  [ Plot.markers(lon, lat, 
      color = MarkersColor.Values wheat_uncertain.median,
      size = MarkersSize.UncertainValues wheat_uncertain, sizeRange = (5.0, 25.0),
      displayName = "wheat")] 
  |> Chart.ofList

(*** include-value: markers_petals ***)

(** **Bull-eyes** is a kind of markers when outer and inner colors indicate level of uncertainty. 
Data series `size` is a size in pixels, same for all markers;
`color` is an array of quantiles which must contains `median`, `lower95` and `upper95`. 
*)

let markers_bulleyes =
  [ Plot.markers(lon, lat, 
      color = MarkersColor.UncertainValues wheat_uncertain,
      size = MarkersSize.Value 15.0,
      displayName = "wheat")] 
  |> Chart.ofList

(*** include-value: markers_bulleyes ***)

(** **Box-and-whisker** plot is displayed if `y` is uncertain and given as an array of quantiles,
which may contain all or some of properties `median`, `lower95`, `upper95`,
`lower68` and `upper68`. )
*)

let markers_box =
  [ Plot.markers(MarkersX.Values t, MarkersY.UncertainValues y_uncertain, 
    displayName = "p",
    titles = Titles.markers("time", "p"))] 
  |> Chart.ofList

(*** include-value: markers_box ***)

(** **Heatmap** displays a graphical representation of data where the individual values contained 
in a matrix are represented as colors. If the values are uncertain, 
it allows to see quantiles of each point and highlight regions with similar values. *)

let grid_lon = grid |> col "lon"
let grid_lat = grid |> col "lat"
let grid_value = grid |> col "value"

let heatmap_gradient = [ Plot.heatmap(grid_lon, grid_lat, grid_value) ] |> Chart.ofList

(*** include-value: heatmap_gradient ***)

let heatmap_matrix = 
  [ Plot.heatmap(grid_lon, grid_lat, grid_value, treatAs = HeatmapTreatAs.Discrete) ] 
  |> Chart.ofList

(*** include-value: heatmap_matrix ***)

(** Heatmap values also can be uncertain, if they are defined as quantiles
containing  `median`, `lower68`, `upper68`. The plot displays median values,
but if a probe is pulled on the heatmap, the probe card contains "Highlight similar"
checkbox which highlights areas of the heatmap with values similar to the value
under the probe.
*)

let ugrid_lon = ugrid |> col "lon"
let ugrid_lat = ugrid |> col "lat"
let ugrid_value = ugrid |> quantiles "value"

let heatmap_uncertain = 
  [ Plot.heatmap(ugrid_lon, ugrid_lat, 
      HeatmapValues.TabularUncertainValues ugrid_value, 
      colorPalette = "blue,white,yellow,orange") ] 
  |> Chart.ofList

(*** include-value: heatmap_uncertain ***)


(** See more details about plots [here](https://github.com/predictionmachines/InteractiveDataDisplay/wiki).*)

(**
 
Contributing and copyright
--------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork 
the project and submit pull requests. If you're adding a new public API, please also 
consider adding [samples][content] that can be turned into a documentation. You might
also want to read the [library design notes][readme] to understand how it works.

The library is available under Public Domain license, which allows modification and 
redistribution for both commercial and non-commercial purposes. For more information see the 
[License file][license] in the GitHub repository. 

  [content]: https://github.com/predictionmachines/Angara.Chart/tree/master/docs/content
  [gh]: https://github.com/Microsoft/Angara.Chart
  [issues]: https://github.com/Microsoft/Angara.Chart/issues
  [readme]: https://github.com/Microsoft/Angara.Chart/blob/master/README.md
  [license]: https://github.com/Microsoft/Angara.Chart/blob/master/LICENSE.txt
*)
