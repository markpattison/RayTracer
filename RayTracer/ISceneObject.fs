module ISceneObject

open Material
open Ray
open Vector
open Point

type ISceneObject =
    abstract ObjectMaterial: Material
    abstract FirstIntersection: Ray -> Intersection option

and Intersection =
    {
        objectHit: ISceneObject
        t: double
        position: Point
        normal: Vector
    }
