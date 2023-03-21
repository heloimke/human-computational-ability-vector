using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public class aStimuli : Stimuli<SoundEffect>
{
    private SoundEffectInstance _instance;
    public SoundEffectInstance instance => _instance;
    public SoundEffectInstance instance_safe {
        get {
            if (_instance == null) _instance = this.content.CreateInstance();
            return _instance;
        }
    }

    public void SetPitch(float pitch) => instance_safe.Pitch = MathHelper.Clamp(pitch, -1, 1);
    public void SetVolume(float volume) => instance_safe.Volume = MathHelper.Clamp(volume, 0, 1);
    public void SetPan(float pan) => instance_safe.Pan = MathHelper.Clamp(pan, -1, 1);

    public aStimuli(ContentManager ContainingLibrary, string AssetName, string StimuliCode, bool DefaultInstance = true, bool AutoLoad = true) : base (ContainingLibrary, StimulusType.Audio, AssetName, StimuliCode, AutoLoad)
    {
        if(DefaultInstance)
        {
            _instance = this.content.CreateInstance();
        }
    }

    public void Play()
    {
        if(_instance == null)
            _instance = this.content.CreateInstance();

        else if(_instance.State == SoundState.Playing)
            _instance.Stop();

        _instance.Play();
    }
}