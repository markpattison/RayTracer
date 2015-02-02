module Plane

open Vector
open Ray
open Material
open ISceneObject

type Plane (material: Material, normal: Vector, offset: float) =
    let normalised = normal.Normalised
    interface ISceneObject with
        member _this.ObjectMaterial = material
        member _this.FirstIntersection (r: Ray) =
            let dot = DotProduct normal r.direction
            match dot with
            | 0.0 -> None
            | x ->
                let t = (offset - DotProduct normal r.origin.ToVector) / x
                match t with
                | tHit when tHit > 0.0 -> Some({ Intersection.objectHit = _this ; t = tHit ; normal = normalised ; position = r.Travel(t) })
                | _ -> None
