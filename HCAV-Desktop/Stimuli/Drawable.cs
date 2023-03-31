using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public interface Drawable
{
    public void Draw(ref SpriteBatch batch, Vector2 position, SpriteEffects effect = SpriteEffects.None);
    public void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, SpriteEffects effect);
    public void Draw(ref SpriteBatch batch, Vector2 position, Vector2 scale, float rotation = 0, float layerDepth = 0, SpriteEffects effect = SpriteEffects.None);
    public void Draw(ref SpriteBatch batch, Vector2 position, float scale, float rotation = 0, float layerDepth = 0, SpriteEffects effect  = SpriteEffects.None);
    public void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, float scale, float rotation = 0, float layerDepth = 0, SpriteEffects effect = SpriteEffects.None);

    public void Draw(ref SpriteBatch batch, Rectangle destination);
    public void Draw(ref SpriteBatch batch, Rectangle destination, Color color);
    public void Draw(ref SpriteBatch batch, Rectangle destination, float rotation, float layerDepth = 0);
    public void Draw(ref SpriteBatch batch, Rectangle destination, float rotation, Color color, float layerDepth = 0);
    public void Draw(ref SpriteBatch batch, Rectangle destination, float rotation, Vector2 rotOrigin, float layerDepth = 0);
    public void Draw(ref SpriteBatch batch, Rectangle destination, Color color, float rotation, Vector2 rotOrigin, float layerDepth = 0);
    
    public void Draw(ref SpriteBatch batch, Rectangle destination, Rectangle source, float rotation = 0, float layerDepth = 0);
    public void Draw(ref SpriteBatch batch, Rectangle destination, Rectangle source, Color color, float rotation = 0, float layerDepth = 0);
    public void Draw(ref SpriteBatch batch, Rectangle destination, Rectangle source, Color color, float rotation, Vector2 rotOrigin, float layerDepth = 0);

}