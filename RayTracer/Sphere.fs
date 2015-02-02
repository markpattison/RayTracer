module Sphere

open Vector
open Point
open Ray
open Material
open ISceneObject

type Sphere (material: Material, centre: Point, radius: float) =
    let radiusSqd = radius * radius
    interface ISceneObject with
        member _this.ObjectMaterial = material
        member _this.FirstIntersection (r: Ray) =
            let dist = r.origin - centre
            let b = DotProduct dist r.direction
            let d = b * b - dist.LengthSqd + radiusSqd
            match d with
                | x when x <= 0.0 -> None
                | x ->
                    let t = -b - sqrt x
                    match t with
                    | tHit when tHit > 0.0 ->
                        let hitPoint = r.Travel t
                        let normal = (hitPoint - centre).Normalised
                        Some({ Intersection.objectHit = _this ; t = tHit ; normal = normal ; position = hitPoint })
                    | _ -> None
