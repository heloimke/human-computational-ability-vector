using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using static HCAV_Desktop.ScreenSpace;

namespace HCAV_Desktop;

public class MainMenu : Session
{
    tStimuli Title;
    tStimuli Option;

    public MainMenu()
    {
        Title   = MenuStimuli.MenuResources.FindFontInstance("Header Thick");
        Option  = MenuStimuli.MenuResources.FindFontInstance("Text Light");
    }

    public void DrawScreen(ref SpriteBatch batch, GameTime gameTime)
    {
        Title.Draw(ref batch, "Human Computational Ability Vector", FullSpace(0, 0.75f));
        Option.Draw(ref batch, "Screenspace Transformation Test.", FullSpace(0, 0));
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
        return this;
    }
}