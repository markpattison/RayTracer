module Colour

type Colour =
    {
        r: float;
        g: float;
        b: float;
    }
    static member (*) (c: Colour, d: float) = { Colour.r = d * c.r ; g = d * c.g ; b = d * c.b }
    static member (*) (d: float, c: Colour) = { Colour.r = d * c.r ; g = d * c.g ; b = d * c.b }
    static member (*) (c1: Colour, c2: Colour) = { Colour.r = c1.r * c2.r ; g = c1.g * c2.g ; b = c1.b * c2.b }
    static member (+) (c1: Colour, c2: Colour) = { Colour.r = c1.r + c2.r ; g = c1.g + c2.g ; b = c1.b + c2.b }
    static member (-) (c1: Colour, c2: Colour) = { Colour.r = c1.r - c2.r ; g = c1.g - c2.g ; b = c1.b - c2.b }
    static member (/) (c: Colour, d: float) = { Colour.r = c.r / d ; g = c.g / d ; b = c.b / d }
    static member Zero = { Colour.r = 0.0; g = 0.0; b = 0.0 }
    member c.Clamped =
        let clamp x = max 0.0 ( min 1.0 x)
        { Colour.r = clamp c.r ; g = clamp c.g ; b = clamp c.b }