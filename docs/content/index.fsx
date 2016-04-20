(*** hide ***)
#I "../../bin/Angara.Chart"

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

Samples & documentation
-----------------------


 
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
