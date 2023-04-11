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
    InputBox testxtBox; //tessckssts.. textst. tec tex teckstst tex.... :thinking:
    InputBox leftJustified;

    string ClickExposureExample = "Clickable State Exposure Location";
    
    int CurrentPosition = 0;
    string MouseString => $"Mouse scrolled {CurrentPosition} since beginning.";

    public MainMenu()
    {
        Title   = MenuStimuli.MenuResources.FindFontInstance("Header Thick");
        Option  = MenuStimuli.MenuResources.FindFontInstance("Text Light");

        testButton = new Clickable("Clickable Test", new Vector2(0, -0.15f), 0.4f, 0.4f, MenuStimuli.MenuResources.FindGraphicalStimulus("Test Button"), ExamSpace: false);
        testButton.ClickInitiated += (Clickable clicked) => ClickExposureExample = "Button Clicked!";
        testButton.ClickEnded += (Clickable clicked, TimeSpan time) => ClickExposureExample = $"Button let go after {time.TotalMilliseconds:F1}ms.";

        testxtBox = new InputBox("Testxt Box", MenuStimuli.MenuResources.FindFontInstance("Input"), 0.6f,
            MenuStimuli.MenuResources.FindGraphicalStimulus("Testxt Box Left Cap"),
            MenuStimuli.MenuResources.FindGraphicalStimulus("Testxt Box Right Cap"),
            MenuStimuli.MenuResources.FindGraphicalStimulus("Testxt Box Middle"),
            MenuStimuli.MenuResources.FindGraphicalStimulus("Testxt Cursor"),
            0.035f, 0.035f, 0.01f, 1f, 0.1f, new Vector2(0, -0.5f), false, LeftJustify: false, PlaceholderText: "Click me and start typing!"
        );

        leftJustified = new InputBox("Left Box", MenuStimuli.MenuResources.FindFontInstance("Input"), 0.6f,
            MenuStimuli.MenuResources.FindGraphicalStimulus("Testxt Box Left Cap"),
            MenuStimuli.MenuResources.FindGraphicalStimulus("Testxt Box Right Cap"),
            MenuStimuli.MenuResources.FindGraphicalStimulus("Testxt Box Middle"),
            MenuStimuli.MenuResources.FindGraphicalStimulus("Testxt Cursor"),
            0.035f, 0.035f, 0.01f, 1f, 0.1f, new Vector2(0, -0.7f), false, LeftJustify: true, PlaceholderText: "Typing is left justified here."
        );
    }

    public void DrawScreen(ref SpriteBatch batch, GameTime gameTime)
    {
        Title.Draw(ref batch, "Human Computational Ability Vector", FullSpace(0, 0.85f), scale: SizeRatio * 0.6f * (Landscape ? (AspectRatio) : 1));
        Option.Draw(ref batch, "Screenspace Transformation Test.", FullSpace(0, 0.65f), scale: SizeRatio * 0.8f);
        Option.Draw(ref batch, $"Time Since Last Update: {gameTime.ElapsedGameTime.TotalMilliseconds}ms", FullSpace(0, 0.5f), scale: SizeRatio * 0.8f);
        Option.Draw(ref batch, ClickExposureExample, FullSpace(0, 0.35f), scale: SizeRatio * 0.8f);
        Option.Draw(ref batch, MouseString, FullSpace(0, 0.2f), scale: SizeRatio * 0.8f);
        testButton.Draw(ref batch);
        testxtBox.Draw(ref batch);
        leftJustified.Draw(ref batch);
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
        CurrentPosition = mouse.ScrollWheelValue;
        testButton.Update(gameTime, FromFullSpace(mouse.X, mouse.Y), mouse.LeftButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space));
        testxtBox.Update(gameTime, FromFullSpace(mouse.X, mouse.Y), mouse.LeftButton == ButtonState.Pressed, mouse.RightButton == ButtonState.Pressed);
        leftJustified.Update(gameTime, FromFullSpace(mouse.X, mouse.Y), mouse.LeftButton == ButtonState.Pressed, mouse.RightButton == ButtonState.Pressed);
        testxtBox.Width = (float)(1f * ((gameTime.TotalGameTime.TotalSeconds % 5) / 5f)) + 0.5f;
        testxtBox.Height = (float)(0.04f * ((gameTime.TotalGameTime.TotalSeconds % 5) / 5f)) + 0.07f;
        return this;
    }
}