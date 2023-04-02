using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public class gStimuli : Stimuli<Texture2D>, Drawable
{
    public readonly Vector2 Origin;

    public int Height => this.content.Height;
    public int Width => this.content.Width;

    public void StreamPNG(System.IO.Stream stream) => this.content.SaveAsPng(stream, Width, Height);

    public gStimuli(ContentManager ContainingLibrary, string AssetName, string StimuliCode, Vector2? Origin = null, bool AutoLoad = true) : base(ContainingLibrary, StimulusType.Visual, AssetName, StimuliCode)
    {
        this.Origin = Origin ?? Vector2.Zero;
    }

    public void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, Vector2 scale, float rotation, float layerDepth, SpriteEffects effect) =>
        batch.Draw(this.content, position, null, Color.White, rotation, origin, scale, effect, layerDepth);

    public void Draw(ref SpriteBatch batch, Vector2 position, SpriteEffects effect = SpriteEffects.None) => Draw(ref batch, this.Origin, position, Vector2.One, 0, 0, effect);
    public void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, SpriteEffects effect) => Draw(ref batch,origin, position, Vector2.One, 0, 0, effect);
    public void Draw(ref SpriteBatch batch, Vector2 position, Vector2 scale, float rotation = 0, float layerDepth = 0, SpriteEffects effect = SpriteEffects.None) => Draw(ref batch, this.Origin, position, scale, rotation, layerDepth, effect);
    public void Draw(ref SpriteBatch batch, Vector2 position, float scale, float rotation = 0, float layerDepth = 0, SpriteEffects effect  = SpriteEffects.None) => Draw(ref batch, this.Origin, position, Vector2.One * scale, rotation, layerDepth, effect);
    public void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, float scale, float rotation = 0, float layerDepth = 0, SpriteEffects effect = SpriteEffects.None) => Draw(ref batch, origin, position, Vector2.One * scale, rotation, layerDepth, effect);

    public void Draw(ref SpriteBatch batch, Rectangle destination) => batch.Draw(this.content, destination, Color.White);
    public void Draw(ref SpriteBatch batch, Rectangle destination, Color color) => batch.Draw(this.content, destination, color);
    public void Draw(ref SpriteBatch batch, Rectangle destination, float rotation, float layerDepth = 0) => batch.Draw(this.content, destination, null, Color.White, rotation, Vector2.Zero, SpriteEffects.None, layerDepth);
    public void Draw(ref SpriteBatch batch, Rectangle destination, float rotation, Color color, float layerDepth = 0) => batch.Draw(this.content, destination, null, color, rotation, Vector2.Zero, SpriteEffects.None, layerDepth);
    public void Draw(ref SpriteBatch batch, Rectangle destination, float rotation, Vector2 rotOrigin, float layerDepth = 0) => batch.Draw(this.content, destination, null, Color.White, rotation, rotOrigin, SpriteEffects.None, layerDepth);
    public void Draw(ref SpriteBatch batch, Rectangle destination, Color color, float rotation, Vector2 rotOrigin, float layerDepth = 0) => batch.Draw(this.content, destination, null, color, rotation, rotOrigin, SpriteEffects.None, layerDepth);
    
    public void Draw(ref SpriteBatch batch, Rectangle destination, Rectangle source, float rotation = 0, float layerDepth = 0) => batch.Draw(this.content, destination, source, Color.White, rotation, Vector2.Zero, SpriteEffects.None, layerDepth);
    public void Draw(ref SpriteBatch batch, Rectangle destination, Rectangle source, Color color, float rotation = 0, float layerDepth = 0) => batch.Draw(this.content, destination, source, color, rotation, Vector2.Zero, SpriteEffects.None, layerDepth);
    public void Draw(ref SpriteBatch batch, Rectangle destination, Rectangle source, Color color, float rotation, Vector2 rotOrigin, float layerDepth = 0) => batch.Draw(this.content, destination, source, color, rotation, rotOrigin, SpriteEffects.None, layerDepth);
    
}