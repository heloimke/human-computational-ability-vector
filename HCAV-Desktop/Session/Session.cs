using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public interface Session
{
    public Session Update(GameTime gameTime);

    public void DrawScreen(ref SpriteBatch batch, GameTime gameTime);
    public void ExitBehaviour();

    public void FlowBehaviour();
}