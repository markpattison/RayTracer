module RayTrace

open Vector
open Point
open Ray
open Colour
open Light
open ISceneObject
open Scene

// TODO: remove next 2 opens and references to PresentationCore and WindowsBase

open System.Windows.Media
open System.Windows.Media.Imaging

let epsilon = 0.000001
let maxBounces = 5

let firstIntersection (scene: Scene) (ray: Ray) =
    let getPossibleIntersection (sceneObject: ISceneObject) =
        sceneObject.FirstIntersection ray
    let intersections = List.choose getPossibleIntersection scene.sceneObjects
    match intersections with
    | [] -> None
    | intersections -> Some(List.minBy (fun x -> x.t) intersections)

let reflectedRay (point: Point) (direction: Vector) (normal: Vector) =
    let reflectedDirection = direction - 2.0 * normal * (DotProduct direction normal)
    CreateRay point reflectedDirection

let litColour (scene: Scene) (ray: Ray) (intersection: Intersection) =
    let hitPoint = intersection.position
    let material = intersection.objectHit.ObjectMaterial
    let reflected = reflectedRay hitPoint ray.direction intersection.normal
    let canSeeLight (light: Light) =
        let intersectionToLight = light.position - hitPoint
        let rayToLight = CreateRay (hitPoint + epsilon * intersectionToLight) intersectionToLight
        let possibleIntersection = firstIntersection scene rayToLight
        match possibleIntersection with
        | None -> true
        | Some(x) when x.t > intersectionToLight.Length -> true
        | _ -> false
    let visibleLights = List.filter canSeeLight scene.lights
    let visibleLightsToLight = List.map (fun (l: Light) -> (l, (l.position - hitPoint).Normalised)) visibleLights
    let diffuseLight (light: Light, toLight: Vector) =
        light.colour * (NonNegativeDotProduct toLight intersection.normal)
    let specularLight (light: Light, toLight: Vector) =
        light.colour * (NonNegativeDotProduct toLight reflected.direction) ** material.shininess
    let totalDiffuse = (material.diffuse hitPoint) * List.sumBy diffuseLight visibleLightsToLight
    let totalSpecular = (material.specular hitPoint) * List.sumBy specularLight visibleLightsToLight
    totalDiffuse + totalSpecular

let rec TraceRay (scene: Scene) (ray: Ray) (bounces: int) =
    let possibleIntersection = firstIntersection scene ray
    match possibleIntersection with
    | None -> scene.background
    | Some(intersection) ->
        let lit = litColour scene ray intersection
        let reflected =
            match bounces with
            | b when b >= maxBounces -> Colour.Zero
            | b ->
                let hitPoint = ray.Travel intersection.t
                let reflects = intersection.objectHit.ObjectMaterial.reflects hitPoint
                match reflects with
                | 0.0 -> Colour.Zero
                | r ->
                    let reflected = reflectedRay hitPoint ray.direction intersection.normal
                    r * TraceRay scene reflected (b + 1)
        lit + reflected

let RenderPixels (scene: Scene) (width: int) (height: int) =
    let pixelColour (i: int) =
        let x = i % width
        let y = i / width
        let xd = ((1.0 + 2.0 * double x) / double width) - 1.0
        let yd = 1.0 - ((1.0 + 2.0 * double y) / double width)
        let ray = scene.camera.GetRay xd yd
        TraceRay scene ray 0
    Array.Parallel.init (width * height) pixelColour

let PixelsToBitmap (pixels: Colour []) (width: int) (height: int) =
    let stride = 4 * width
    let byteArrayInit (i: int) =
        let pixel = i / 4
        let x = pixel % width
        let y = pixel / height
        match i % 4 with
        | 0 -> byte (pixels.[x + width * y].b * 255.0)
        | 1 -> byte (pixels.[x + width * y].g * 255.0)
        | 2 -> byte (pixels.[x + width * y].r * 255.0)
        | _ -> byte (255.0)
    let bytes = Array.init (height * stride) byteArrayInit
    BitmapSource.Create(width, height, 96.0, 96.0, PixelFormats.Bgra32, null, bytes, stride)

let Render (scene: Scene) (width: int) (height: int) =
    let pixels = RenderPixels scene width height
    PixelsToBitmap pixels width height

let RenderAsync (scene: Scene) (width: int) (height: int) = 
    let stride = 4 * width
    let bytes = Array.create (height * stride) (byte 0)
    let pixelColour (i: int) =
        let x = i % width
        let y = i / width
        let xd = ((1.0 + 2.0 * double x) / double width) - 1.0
        let yd = 1.0 - ((1.0 + 2.0 * double y) / double width)
        let ray = scene.camera.GetRay xd yd
        TraceRay scene ray 0
    let updateBytes (i: int) (c: Colour) =
        Array.set bytes (i * 4) (byte (c.b * 255.0))
        Array.set bytes (i * 4 + 1) (byte (c.g * 255.0))
        Array.set bytes (i * 4 + 2) (byte (c.r * 255.0))
        Array.set bytes (i * 4 + 3) (byte 255)
    let combined (i: int) =
        let colour = pixelColour i
        updateBytes i colour
    let processPixels =
        Async.Parallel [ for i in 0 .. width * height - 1 -> async { combined i } ]
        |> Async.RunSynchronously
    BitmapSource.Create(width, height, 96.0, 96.0, PixelFormats.Bgra32, null, bytes, stride)


