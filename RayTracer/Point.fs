module Point

open Vector

type Point =
    {
        x: float;
        y: float;
        z: float;
    }
    static member (+) (p: Point, v: Vector) = { Point.x = p.x + v.x ; y = p.y + v.y ; z = p.z + v.z }
    static member (-) (p: Point, v: Vector) = { Point.x = p.x - v.x ; y = p.y - v.y ; z = p.z - v.z }
    static member (-) (p1: Point, p2: Point) = { Vector.x = p1.x - p2.x ; y = p1.y - p2.y ; z = p1.z - p2.z }
    member p.ToVector = { Vector.x = p.x ; y = p.y ; z = p.z }
