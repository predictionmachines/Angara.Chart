namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("Angara.Chart")>]
[<assembly: AssemblyProductAttribute("Angara.Chart")>]
[<assembly: AssemblyDescriptionAttribute("Data visualization library for F#.")>]
[<assembly: AssemblyVersionAttribute("0.2.3")>]
[<assembly: AssemblyFileVersionAttribute("0.2.3")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.2.3"
    let [<Literal>] InformationalVersion = "0.2.3"
