namespace RaymarchingConsole;

public static class Functions
{
    public static double Range(double value, double min, double max)
    {
        return Math.Min(Math.Max(value, min), max);
    }
    
    public static double Range(double value, double max)
    {
        return Math.Min(Math.Max(value, 0), max);
    }

    public static double Mod(double value, double max)
    {
        if (value > 0){ return value % max; }
        else { return value % max + max; }
    }
    public static Vector2D Mod(Vector2D vec, Vector2D maxVec)
    {
        return new Vector2D(Mod(vec.X, maxVec.X), Mod(vec.Y, maxVec.Y));
    }
    public static Vector3D Mod(Vector3D vec, Vector3D maxVec)
    {
        return new Vector3D(Mod(vec.X, maxVec.X), Mod(vec.Y, maxVec.Y), Mod(vec.Z, maxVec.Z));
    }
} 

public class Raymarching
{
    public Raymarching(int screenWidth, int screenHeight, double cameraWidth = 16.0/9, double cameraHeight = 1,
        params List<RaymarchingObj> objects)
    {
        MaxIterations = 100;
        MinDistance = 0.01;
        LightDirection = new Vector3D(0, 1, 1).Normalized();
        CameraWidth = cameraWidth;
        CameraHeight = cameraHeight;
        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;
        Objects = objects;
    }
    
    public Raymarching(int screenWidth, int screenHeight, double cameraWidth = 16.0/9, double cameraHeight = 1)
    {
        MaxIterations = 100;
        MinDistance = 0.01;
        LightDirection = new Vector3D(0, 1, 1).Normalized();
        CameraWidth = cameraWidth;
        CameraHeight = cameraHeight;
        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;
        Objects = new List<RaymarchingObj>();
    }
    
    public List<RaymarchingObj> Objects { get; }
    public Vector3D CameraPosition { get; set; }
    public Vector3D CameraDirection { get; set; }
    
    public Vector3D LightDirection { get; set; }
    public int MaxIterations {get; set;}
    public double MinDistance {get; set;}

    public double CameraWidth {get; set;}

    public double CameraHeight {get; set;}
    
    public int ScreenWidth { get; }
    
    public int ScreenHeight { get; }

    public double[,] Render()
    {
        double[,] result =  new double[ScreenWidth, ScreenHeight];
        Vector3D xMovement = new Vector3D(-CameraDirection.Z, 0, CameraDirection.X).Normalized();
        Vector3D yMovement = CameraDirection.PerpY();
        for (int i = 0; i < ScreenWidth; i++)
        { 
            for (int j = 0; j < ScreenHeight; j++)
            {
                Vector3D direction = CameraDirection + xMovement * ((double)i / ScreenWidth - 0.5) * CameraWidth +
                                     yMovement * ((double)j / ScreenHeight - 0.5) * CameraHeight;
                Vector3D? point = Raymarch(CameraPosition, direction.Normalized());
                if (point.HasValue) result[i, j] = Vector3D.Dot(GetNormal(point.Value), LightDirection);
                else result[i, j] = -2;
            }
        }
        return result;
    }
    
    public Vector3D? Raymarch(Vector3D point, Vector3D direction)
    {
        double distance;
        for (int i = 0; i < MaxIterations; i++)
        {
            distance = CalculateDist(point);
            if (distance < MinDistance)  return point;
            point += direction * distance;
        }
        return null;
    }

    public double CalculateDist(Vector3D point)
    {
        if (Objects.Count == 0) return 1e10;
        double result = Objects[0].Distance(point);
        foreach (var obj in Objects)
        {
            result = Math.Min(result, obj.Distance(point));
        }
        return result;
    }

    public Vector3D GetNormal(Vector3D point)
    {
        double distance = CalculateDist(point);
        double e = 0.001;
        Vector3D normal = new Vector3D(distance) - new Vector3D(CalculateDist(point - new Vector3D(e,0,0)),
            CalculateDist(point - new Vector3D(0,e,0)), CalculateDist(point - new Vector3D(0,0,e)));
        return normal.Normalized();
    }
}

public struct RaymarchingObj
{
    public RaymarchingObj(double size, Vector3D position, Func<double, Vector3D, Vector3D, double> formula)
    {
        Size = size;
        Position = position;
        this.formula = formula;
    }
    
    public double Size {get; set;}
    public Vector3D Position {get; set;}
    
    Func<double, Vector3D, Vector3D, double> formula;
    public double Distance(Vector3D point) { return formula(Size, Position, point); }
}