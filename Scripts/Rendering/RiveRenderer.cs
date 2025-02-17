using RiveSharp;
using SkiaSharp;
using System.IO;
using System;

public class RiveRenderer
{
    private readonly Scene _scene;
    private readonly Renderer _renderer;
    private SKSurface _surface;
    
    public RiveRenderer(string riveFilePath, int width, int height)
    {
        // Initalize the Skia Renderer
        _surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Rgba8888));

        _renderer = new Renderer(_surface.Canvas);


        // Load the Rive file
        _scene = new Scene();
        using (var stream = new FileStream(riveFilePath, FileMode.Open))
        {
            if(!_scene.LoadFile(stream))
            {
                throw new System.Exception("failed to load rive file");
            }
            Console.WriteLine("Rive file loaded successfully.");
        }

        // Load the first artboard by default
        if(!_scene.LoadArtboard("Artboard"))
        {
            throw new System.Exception("Failed to load artboard");
        }
        Console.WriteLine($"Artboard loaded: {_scene.Name}");

        _scene.LoadStateMachine("State Machine 1");
        
    }

    public void Update(double elapsedSeconds)
    {
        // Attempt to advance the scene
        bool advanced = _scene.AdvanceAndApply(elapsedSeconds);
        if (!advanced)
        {
            throw new Exception("Failed to advance animation");
        }

    }

    public SKImage Render()
    {
        var canvas = _surface.Canvas;
        canvas.Clear(SKColors.White);


        // Render the scene
        _scene.Draw(_renderer);

        return _surface.Snapshot();
    }

    public void Dispose()
    {
        _surface?.Dispose();
    }
}