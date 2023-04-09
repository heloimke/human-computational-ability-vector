using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HCAV_Desktop;

public class HCAVDesktop : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Session RunningSession;

    public HCAVDesktop()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1280;//GraphicsDevice.DisplayMode.Width;
        _graphics.PreferredBackBufferHeight = 720;//GraphicsDevice.DisplayMode.Height;
        _graphics.IsFullScreen = false;

        _graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = new System.TimeSpan(10 * 1000); //1ms
        _graphics.ApplyChanges();

        ScreenSpace.Setup(_graphics);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        MenuStimuli.BuildSet(Content);

        RunningSession = new MainMenu();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        RunningSession = RunningSession.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MidnightBlue);

        _spriteBatch.Begin();
        RunningSession.DrawScreen(ref _spriteBatch, gameTime);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}