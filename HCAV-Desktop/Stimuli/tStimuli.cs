using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public enum TextualStimuliType
{
    Font,
    Dictionary,
    Word
}

public class tStimuli : Stimuli<SpriteFont>
{
    public readonly TextualStimuliType containerType;

    public SpriteFont font => this.content;

    public tStimuli(ContentManager ContainingLibrary, string AssetName, string StimuliCode, bool AutoLoad = true) : base(ContainingLibrary, StimulusType.Textual, AssetName, StimuliCode, AutoLoad)
    {
        this.containerType = TextualStimuliType.Font;
    }

    public tStimuli(ContentManager ContainingLibrary, TextualStimuliType type, string AssetName, string StimuliCode, bool AutoLoad = true) : base(ContainingLibrary, StimulusType.Textual, AssetName, StimuliCode, AutoLoad)
    {
        this.containerType = type;
    }

    public tStimuli(ContentManager ContainingLibrary, TextualStimuliType type, string AssetName, string StimuliCode, SpriteFont font) : base(ContainingLibrary, StimulusType.Textual, AssetName, StimuliCode, font)
    {
        this.containerType = type;
    }

    public void Draw(ref SpriteBatch batch, string text, Vector2 position) => Draw(ref batch, text, position, Vector2.One, 0, Color.White, SpriteEffects.None, 0);

    public void Draw(ref SpriteBatch batch, string text, Vector2 origin, Vector2 position, Vector2 scale, float rotation, Color color, SpriteEffects effect, float layerDepth) =>
        batch.DrawString(this.font, text, position, color, rotation, origin, scale, effect, layerDepth);
    public void Draw(ref SpriteBatch batch, string text, Vector2 position, Vector2 scale, float rotation, Color color, SpriteEffects effect, float layerDepth) =>
        batch.DrawString(this.font, text, position, color, rotation, font.MeasureString(text) / 2, scale, effect, layerDepth);
    public void Draw(ref SpriteBatch batch, string text, Vector2 position, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) =>
        batch.DrawString(this.font, text, position, Color.White, rotation, font.MeasureString(text) / 2, scale, effect, layerDepth);
    public void Draw(ref SpriteBatch batch, string text, Vector2 position, Color color, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) =>
        batch.DrawString(this.font, text, position, color, rotation, font.MeasureString(text) / 2, scale, effect, layerDepth);

    public virtual void Draw(ref SpriteBatch batch, Vector2 position) => Draw(ref batch, $"{StimuliCode} - ({AssetName})", position);

    public virtual void Draw(ref SpriteBatch batch, Vector2 position, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, $"{StimuliCode} - ({AssetName})", position, Color.White, scale, rotation, effect, layerDepth);
    public virtual void Draw(ref SpriteBatch batch, Vector2 position, Color color, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, $"{StimuliCode} - ({AssetName})", position, color, scale, rotation, effect, layerDepth);

    public virtual void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, $"{StimuliCode} - ({AssetName})", origin, position, Vector2.One * scale, rotation, Color.White, effect, layerDepth);
    public virtual void Draw(ref SpriteBatch batch, Vector2 position, Vector2 scale, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, $"{StimuliCode} - ({AssetName})", font.MeasureString($"{StimuliCode} - ({AssetName})") / 2, position, scale, rotation, Color.White, effect, layerDepth);
    public virtual void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, Vector2 scale, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, $"{StimuliCode} - ({AssetName})", origin, position, scale, rotation, Color.White, effect, layerDepth);
}

public class DictionaryStimuli : tStimuli
{
    private string[] _dictionary;

    public int Words => _dictionary.Length;

    public DictionaryStimuli(ContentManager ContainingLibrary, tStimuli font, string[] dictionary) : base(ContainingLibrary, TextualStimuliType.Dictionary, font.AssetName, font.StimuliCode, font.font)
    {
        _dictionary = dictionary;
    }

    public WordStimuli this[int i]
    {
        get
        {
            return new WordStimuli(this.ContainingLibrary, this, _dictionary[i]);
        }
        set
        {
            this._dictionary[i] = value.Word;
        }
    }

    public void Draw(ref SpriteBatch batch, int index, Vector2 position) => Draw(ref batch, this._dictionary[index], position);

    public void Draw(ref SpriteBatch batch, int index, Vector2 position, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, this._dictionary[index], position, Color.White, scale, rotation, effect, layerDepth);
    public void Draw(ref SpriteBatch batch, int index, Vector2 position, Color color, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, this._dictionary[index], position, color, scale, rotation, effect, layerDepth);

    public void Draw(ref SpriteBatch batch, int index, Vector2 origin, Vector2 position, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, this._dictionary[index], origin, position, Vector2.One * scale, rotation, Color.White, effect, layerDepth);
    public void Draw(ref SpriteBatch batch, int index, Vector2 position, Vector2 scale, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, this._dictionary[index], font.MeasureString(this._dictionary[index]) / 2, position, scale, rotation, Color.White, effect, layerDepth);
    public void Draw(ref SpriteBatch batch, int index, Vector2 origin, Vector2 position, Vector2 scale, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, this._dictionary[index], origin, position, scale, rotation, Color.White, effect, layerDepth);

}

public class WordStimuli : tStimuli
{
    private string _word;
    public string Word => _word;

    public WordStimuli(ContentManager ContainingLibrary, tStimuli font, string word) : base(ContainingLibrary, TextualStimuliType.Word, font.AssetName, font.StimuliCode, font.font)
    {
        this._word = word;
    }

    public override void Draw(ref SpriteBatch batch, Vector2 position) => Draw(ref batch, Word, position);

    public override void Draw(ref SpriteBatch batch, Vector2 position, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, Word, position, Color.White, scale, rotation, effect, layerDepth);
    public override void Draw(ref SpriteBatch batch, Vector2 position, Color color, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, Word, position, color, scale, rotation, effect, layerDepth);

    public override void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, float scale = 0, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, Word, origin, position, Vector2.One * scale, rotation, Color.White, effect, layerDepth);
    public override void Draw(ref SpriteBatch batch, Vector2 position, Vector2 scale, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, Word, font.MeasureString(Word) / 2, position, scale, rotation, Color.White, effect, layerDepth);
    public override void Draw(ref SpriteBatch batch, Vector2 origin, Vector2 position, Vector2 scale, float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0) => Draw(ref batch, Word, origin, position, scale, rotation, Color.White, effect, layerDepth);
}