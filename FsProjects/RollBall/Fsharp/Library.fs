namespace Fsharp

open UnityEngine

type SimpleScript () =
    inherit MonoBehaviour()
    member this.Start() =
        Debug.Log("Hello World from F#")
    


