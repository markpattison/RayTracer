module Material

open Point
open Colour

type Material =
    {
        diffuse: Point -> Colour
        specular: Point -> Colour
        reflects: Point -> double
        shininess: double
    }

