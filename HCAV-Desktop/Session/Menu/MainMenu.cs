using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using static HCAV_Desktop.ScreenSpace;
using System;

namespace HCAV_Desktop;

public class MainMenu : Session
{
    tStimuli Title;
    tStimuli Option;

    Clickable testButton;

    string ClickExposureExample = "Clickable State Exposure Location";

    public MainMenu()
    {
        Title   = MenuStimuli.MenuResources.FindFontInstance("Header Thick");
        Option  = MenuStimuli.MenuResources.FindFontInstance("Text Light");

        testButton = new Clickable("Clickable Test", new Vector2(0, -0.15f), 0.25f, 0.25f, MenuStimuli.MenuResources.FindGraphicalStimulus("Test Button"));
        testButton.ClickInitiated += (Clickable clicked) => ClickExposureExample = "Button Clicked!";
        testButton.ClickEnded += (Clickable clicked, TimeSpan time) => ClickExposureExample = $"Button let go after {time.TotalMilliseconds:F1}ms.";
    }

    public void DrawScreen(ref SpriteBatch batch, GameTime gameTime)
    {
        Title.Draw(ref batch, "Human Computational Ability Vector", FullSpace(0, 0.75f));
        Option.Draw(ref batch, "Screenspace Transformation Test.", FullSpace(0, 0.375f));
        testButton.Draw(ref batch);
        Option.Draw(ref batch, ClickExposureExample, FullSpace(0, -0.625f));
    }

    public void ExitBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public void FlowBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public Session Update(GameTime gameTime)
    {
        MouseState mouse = Mouse.GetState();
        testButton.Update(gameTime, FromFullSpace(mouse.X, mouse.Y), mouse.LeftButton == ButtonState.Pressed);
        return this;
    }
}