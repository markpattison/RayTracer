module Scene

open ISceneObject
open Plane
open Sphere
open Light
open Camera
open Colour
open Material

type Scene =
    {
        sceneObjects: ISceneObject list
        lights: Light list
        camera: Camera
        background: Colour
    }

let ReferenceScene =
    let marbleFloor =
        {
            diffuse = fun v ->
                match int (floor v.x + floor v.z) with
                | x when x % 2 <> 0 -> { Colour.r = 1.0 ; g = 1.0 ; b = 1.0 }
                | _ -> { Colour.r = 0.0 ; g = 0.0 ; b = 0.0 } ;
            specular = fun _ -> { Colour.r = 1.0 ; g = 1.0 ; b = 1.0 }
            reflects = fun v ->
                match int (floor v.x + floor v.z) with
                | x when x % 2 <> 0 -> 0.1
                | _ -> 0.7 ;
            shininess = 150.0;
        }
    let sphereMaterial1 =
        {
            diffuse = fun _ -> { Colour.r = 1.0 ; g = 1.0 ; b = 1.0 } ;
            specular = fun _ -> { Colour.r = 0.5 ; g = 0.5 ; b = 0.5 }
            reflects = fun _ -> 0.6 ;
            shininess = 100.0;
        }
    let sphereMaterial2 =
        {
            diffuse = fun _ -> { Colour.r = 1.0 ; g = 1.0 ; b = 1.0 } ;
            specular = fun _ -> { Colour.r = 0.5 ; g = 0.5 ; b = 0.5 }
            reflects = fun _ -> 0.6 ;
            shininess = 50.0;
        }
    let (sceneObjects: ISceneObject list) =
        [
            Plane ( marbleFloor, { Vector.x = 0.0 ; y = 1.0 ; z = 0.0 }, 0.0)
            Sphere ( sphereMaterial1, { Point.x = 0.0 ; y = 1.0 ; z = 0.0 }, 1.0)
            Sphere ( sphereMaterial2, { Point.x = -1.0 ; y = 0.5 ; z = 1.5 }, 0.5)
        ]
    let lights =
        [
            { Light.position = { Point.x = -2.0 ; y = 2.5 ; z = 0.0 } ; colour = { Colour.r = 0.49 ; g = 0.07 ; b = 0.07 } }
            { Light.position = { Point.x = 1.5 ; y = 2.5 ; z = 1.5 } ; colour = { Colour.r = 0.07 ; g = 0.07 ; b = 0.49 } }
            { Light.position = { Point.x = 1.5 ; y = 2.5 ; z = -1.5 } ; colour = { Colour.r = 0.07 ; g = 0.49 ; b = 0.07 } }
            { Light.position = { Point.x = 0.0 ; y = 3.5 ; z = 0.0 } ; colour = { Colour.r = 0.21 ; g = 0.21 ; b = 0.35 } }
        ]
    let camera = CreateCamera { Point.x = 3.0 ; y = 2.0 ; z = 4.0 } { Point.x = -1.0 ; y = 0.5 ; z = 0.0 } (System.Math.PI / 4.0)
    let background = Colour.Zero
    { Scene.sceneObjects = sceneObjects ; lights = lights ; camera = camera ; background = background }
