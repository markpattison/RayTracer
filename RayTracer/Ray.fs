module Ray

open Vector
open Point

type Ray =
    {
        origin: Point;
        direction: Vector;
    }
    member r.Travel (t: double) = r.origin + t * r.direction

let CreateRay (origin: Point) (direction: Vector) =
    let normalised = direction.Normalised
    { Ray.origin = origin ; direction = normalised }

