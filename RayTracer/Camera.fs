module Camera

open Point
open Vector
open Ray

type Camera =
    {
        position: Point
        direction: Vector
        up: Vector
        right: Vector
        horizontalAngleOfView: float
        rightView: Vector
        upView: Vector
    }
    member c.GetRay (x: double) (y: double) =
        let dir = c.direction + x * c.rightView + y * c.upView
        CreateRay c.position dir

let CreateCamera (position: Point) (lookAt: Point) (horizontalAngleOfView: float) =
    let dir = (lookAt - position).Normalised
    let temp = { Vector.x = 0.0 ; y = -1.0 ; z = 0.0 }
    let r = (CrossProduct dir temp).Normalised
    let u = (CrossProduct dir r).Normalised
    let htan = tan (horizontalAngleOfView / 2.0)
    { Camera.position = position ; direction = dir ; up = u ; right = r ; Camera.horizontalAngleOfView = horizontalAngleOfView ; rightView = htan * r ; upView = htan * u }
