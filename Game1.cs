using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;

namespace RoamingMarsGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RiveRenderer _riveRenderer;
    private Texture2D _riveTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Set Graphics settings
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferFormat = SurfaceFormat.Alpha8;
        //_graphics.IsFullScreen = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        

        // TODO: use this.Content to load your game content here

        // initalize Rive Renderer
        string riveFilePath = Path.Combine(Content.RootDirectory, "roversketch_02.riv");
        _riveRenderer = new RiveRenderer(riveFilePath, 1920, 1080);

        // Create a texture for MonoGame to use the rendered Rive Frame
        _riveTexture = new Texture2D(GraphicsDevice, 1920, 1080);
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;
        _riveRenderer.Update(elapsedSeconds);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        // TODO: Add your drawing code here

        var skImage = _riveRenderer.Render();

        using (var pixmap = skImage.PeekPixels())
        {
            _riveTexture.SetData(pixmap.GetPixelSpan().ToArray());
        }

        _spriteBatch.Begin();
        _spriteBatch.Draw(_riveTexture, new Vector2(0, 0), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        _riveRenderer.Dispose();
        base.UnloadContent();
    }
}
