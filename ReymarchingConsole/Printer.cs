using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace RaymarchingConsole;

public class Printer
{
    static double Circle(double radius, Vector2D pos, Vector2D camPos)
    {
        return (pos - camPos).Length() - radius;
    }
    static double Cphere(double radius, Vector3D pos, Vector3D camPos)
    {
        return (pos - new Vector3D(camPos.X, camPos.Y, Functions.Mod(camPos.Z + radius, radius * 3) - radius)).Length() - radius;
    }
    
    static double Torus(double radius, Vector3D pos, Vector3D camPos)
    {
        Vector2D q = new Vector2D(Circle(radius * 3, pos.XY(), camPos.XY()), pos.Z);
        return q.Length() -  radius;
    }

    static double Plane(double size, Vector3D pos, Vector3D camPos)
    {
        return camPos.Y - pos.Y;
    }
    
    static void Main()
    {
        string gradient = ".:!/r(l1Z4H9W8$@";
        int screenWidth = 160;
        int screenHeight = 90;
        double charAsset = 0.3;
        double fov = 1;
        
        RaymarchingObj plane = new RaymarchingObj(1, new Vector3D(-1), Plane);
        RaymarchingObj sphere = new RaymarchingObj(1, new Vector3D(0), Cphere);
        
        Console.WriteLine(new Vector2D((double)screenWidth / screenHeight * charAsset * 2, 2));
        
        Raymarching raym = new Raymarching(screenWidth, screenHeight, (double)screenWidth / screenHeight * charAsset * fov, fov, sphere);
        raym.MaxIterations = 200;
        raym.MinDistance = 0.001;
        raym.CameraDirection = new Vector3D(1, 0, 0).Normalized();
        raym.CameraPosition = new Vector3D(-25, 0, 0);
        
        double[,] frame = new double[screenWidth, screenHeight];
        Bitmap picture = new Bitmap(screenWidth, screenHeight);
        
        Color background = Color.DarkSlateBlue;
    
        frame = raym.Render();
        for (int i = screenHeight - 1; i > -1; i--)
        {
            for (int j = 0; j < screenWidth; j++)
            {
                Color pixel;
                if (frame[j, i] < -1)
                {
                    pixel = background;
                }
                else
                {
                    int b = (int)Math.Round(Math.Max(frame[j, i], 0) * 200) + 35;
                    pixel = Color.FromArgb(b, b, b);
                }
                picture.SetPixel(j, i, pixel);
            }
        }
        
        string sourcePath = GetSourceFilePath();
        string? projectDir = Path.GetDirectoryName(sourcePath);
        string fullPath = Path.Combine(projectDir ?? string.Empty, "picture.png");
        picture.Save(fullPath, ImageFormat.Png);
    }

    static string GetSourceFilePath([CallerFilePath] string? path = null) => path ?? string.Empty;
}