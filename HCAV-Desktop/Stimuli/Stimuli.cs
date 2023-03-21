using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public enum StimulusType
{
    Visual,
    Audio,
    Textual,
    Base
}

public class Stimuli<ContentType>
{
    public readonly ContentManager ContainingLibrary;

    public readonly string AssetName;
    public readonly string StimuliCode;
    public readonly StimulusType sType;

    private ContentType _content;
    public ContentType content => _content;

    public Stimuli(ContentManager ContainingLibrary, StimulusType sType, string AssetName, string StimuliCode, bool AutoLoad = true)
    {
        this.ContainingLibrary  = ContainingLibrary;
        this.sType              = sType;
        this.AssetName          = AssetName;
        this.StimuliCode        = StimuliCode;

        if(AutoLoad) LoadContent();
    }

    protected Stimuli(ContentManager ContainingLibrary, StimulusType sType, string AssetName, string StimuliCode, ContentType content)
    {
        this.ContainingLibrary  = ContainingLibrary;
        this.sType              = sType;
        this.AssetName          = AssetName;
        this.StimuliCode        = StimuliCode;

        _content = content;
    }

    public void LoadContent() => _content = ContainingLibrary.Load<ContentType>(AssetName);
}