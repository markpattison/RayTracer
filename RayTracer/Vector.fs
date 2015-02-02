module Vector

type Vector =
    {
        x: float;
        y: float;
        z: float;
    }
    static member (~-) (v: Vector) = { Vector.x = -v.x; y = -v.y; z = -v.z }
    static member (*) (v: Vector, d: float) = { Vector.x = d * v.x; y = d * v.y; z = d * v.z }
    static member (*) (d: float, v: Vector) = { Vector.x = d * v.x; y = d * v.y; z = d * v.z }
    static member (+) (v1: Vector, v2: Vector) = { Vector.x = v1.x + v2.x; y = v1.y + v2.y; z = v1.z + v2.z }
    static member (-) (v1: Vector, v2: Vector) = { Vector.x = v1.x - v2.x; y = v1.y - v2.y; z = v1.z - v2.z }
    static member (/) (v: Vector, d: float) = { Vector.x = v.x / d; y = v.y / d; z = v.z / d }
    member v.LengthSqd = v.x * v.x + v.y * v.y + v.z * v.z
    member v.Length = sqrt (v.x * v.x + v.y * v.y + v.z * v.z)
    member v.Normalised =
        match v.Length with
            | 0.0 -> failwith "Cannot normalise vector of length zero"
            | l -> { Vector.x = v.x / l; y = v.y / l; z = v.z / l }

let DotProduct v1 v2 = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z

let NonNegativeDotProduct v1 v2 = max 0.0 (DotProduct v1 v2)

let CrossProduct v1 v2 = { Vector.x = v1.y * v2.z - v1.z * v2.y;
                                  y = v1.z * v2.x - v1.x * v2.z;
                                  z = v1.x * v2.y - v1.y * v2.x }






