namespace RaymarchingConsole;

public struct Vector3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Vector3D(double x, double y, double z) { X = x; Y = y; Z = z; }
    
    public Vector3D(double a) { X = a; Y = a; Z = a; }

    public Vector3D Normalized()
    {
        double mod = Length();
        if (mod == 0) return new Vector3D(0);
        return this / mod;
    }

    public Vector2D XY() { return new Vector2D(X, Y); }
    public Vector2D XZ() { return new Vector2D(X, Z); }
    public Vector2D YZ() { return new Vector2D(Y, Z); }
    
    public static Vector3D operator +(Vector3D a, Vector3D b) { return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
    
    public static Vector3D operator -(Vector3D a, Vector3D b) { return new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }
    
    public static Vector3D operator *(Vector3D a, Vector3D b) { return new Vector3D(a.X * b.X, a.Y * b.Y, a.Z * b.Z); }
    
    public static Vector3D operator *(Vector3D a, double b) { return new Vector3D(a.X * b, a.Y * b, a.Z * b); }
    
    public static Vector3D operator /(Vector3D a, Vector3D b) { return new Vector3D(a.X / b.X, a.Y / b.Y, a.Z / b.Z); }
    
    public static Vector3D operator /(Vector3D a,  double b) { return new Vector3D(a.X / b, a.Y / b, a.Z / b); }

    public Vector3D PerpY()
    {
        if (X == 0 && Z == 0)
        {
            throw new Exception("Vector Format Exception");
        }
        if (Y == 0)
        {
            return new Vector3D(0, 1, 0);
        }
        if (X == 0)
        {
            return new Vector3D(0, 1, -Y / Z).Normalized();
        }
        if (Z == 0)
        {
            return new Vector3D(-Y / X, 1, 0).Normalized();
        }
        Vector3D result = new Vector3D(0,1,0);
        result.X = -Y / (X + Z * Z / X);
        result.Z = -Y / (Z + X * X / Z);
        return result.Normalized();
    }
    public static double Dot(Vector3D a, Vector3D b){ return a.X * b.X + a.Y * b.Y + a.Z * b.Z; }
    
    public double Length() { return Math.Sqrt(X * X + Y * Y + Z * Z); }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}

public class Vector2D
{
    public double X { get; set; }
    public double Y { get; set; }
    
    public Vector2D(double x, double y) { X = x; Y = y; }
    
    public Vector2D(double a) { X = a; Y = a; }

    public Vector2D Normalized()
    {
        double mod = Length();
        if (mod == 0) return new Vector2D(0);
        return this / mod;
    }
    
    public static Vector2D operator +(Vector2D a, Vector2D b) { return new Vector2D(a.X + b.X, a.Y + b.Y); }
    
    public static Vector2D operator -(Vector2D a, Vector2D b) { return new Vector2D(a.X - b.X, a.Y - b.Y); }
    
    public static Vector2D operator *(Vector2D a, Vector2D b) { return new Vector2D(a.X * b.X, a.Y * b.Y); }
    
    public static Vector2D operator *(Vector2D a, double b) { return new Vector2D(a.X * b, a.Y * b); }
    
    public static Vector2D operator /(Vector2D a, Vector2D b) { return new Vector2D(a.X / b.X, a.Y / b.Y); }
    
    public static Vector2D operator /(Vector2D a,  double b) { return new Vector2D(a.X / b, a.Y / b); }
    public double Length() { return Math.Sqrt(X * X + Y * Y); }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
