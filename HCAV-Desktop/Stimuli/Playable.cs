using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public interface Playable
{
    public void SetPitch(float pitch);
    public void SetVolume(float volume);
    public void SetPan(float pan);

    public void Set3DPos(AudioListener listener, AudioEmitter emitter);

    public void Play();
}