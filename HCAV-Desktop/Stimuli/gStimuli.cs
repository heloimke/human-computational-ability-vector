using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public class gStimuli : Stimuli<Texture2D>
{
    public readonly Vector2 Origin;

    public int Height => this.content.Height;
    public int Width => this.content.Width;

    public void StreamPNG(System.IO.Stream stream) => this.content.SaveAsPng(stream, Width, Height);

    public gStimuli(ContentManager ContainingLibrary, string AssetName, string StimuliCode, Vector2? Origin, bool AutoLoad = true) : base(ContainingLibrary, StimulusType.Visual, AssetName, StimuliCode)
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
}