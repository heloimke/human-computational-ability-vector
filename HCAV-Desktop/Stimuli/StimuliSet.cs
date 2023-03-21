using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public class StimuliSet
{

    public readonly string Name;

    private List<gStimuli> _gStim;
    private List<aStimuli> _aStim;
    private List<tStimuli> _fonts;
    private List<DictionaryStimuli> _dictionaries;

    public gStimuli Graphic(int index) => _gStim[index];
    public aStimuli Audio(int index) => _aStim[index];
    public tStimuli Font(int index) => _fonts[index];
    public DictionaryStimuli Dicionary(int index) => _dictionaries[index];

    public int GraphicalStimuliCount => _gStim.Count;
    public int AudioStimuliCount => _aStim.Count;
    public int FontInstanceCount => _fonts.Count;
    public int Dictionaries => _dictionaries.Count;

    public int Words {
        get{
            int buffer = 0;
            foreach(DictionaryStimuli dictionary in _dictionaries)
                buffer += dictionary.Words;
            return buffer;
        }
    }

    public gStimuli[] AllGraphicalStimuli => _gStim.ToArray();
    public aStimuli[] AllAudioStimuli => _aStim.ToArray();
    public tStimuli[] AllFontInstances => _fonts.ToArray();
    public DictionaryStimuli[] AllDictionaries => _dictionaries.ToArray();

    public gStimuli FindGraphicalStimulus(string Code) => _gStim.Find(x => x.StimuliCode == Code);
    public int GetGraphicsReferenceIndexByCode(string Code) => _gStim.IndexOf(FindGraphicalStimulus(Code));

    public aStimuli FindAudioStimulus(string Code) => _aStim.Find(x => x.StimuliCode == Code);
    public int GetAudioReferenceIndexByCode(string Code) => _aStim.IndexOf(FindAudioStimulus(Code));

    public tStimuli FindFontInstance(string Code) => _fonts.Find(x => x.StimuliCode == Code);
    public int GetFontReferenceIndexByCode(string Code) => _fonts.IndexOf(FindFontInstance(Code));

    public DictionaryStimuli FindDictionaryStimulus(string Code) => _dictionaries.Find(x => x.StimuliCode == Code);
    public int GeDictionaryReferenceIndexByCode(string Code) => _dictionaries.IndexOf(FindDictionaryStimulus(Code));

    public StimuliSet(string Name)
    {
        this.Name = Name;

        this._gStim = new List<gStimuli>();
        this._aStim = new List<aStimuli>();

        this._fonts          = new List<tStimuli>();
        this._dictionaries   = new List<DictionaryStimuli>();
    }

    public StimuliSet(string Name, List<gStimuli> graphical_stimulus, List<aStimuli> auditory_stimulus, List<tStimuli> fonts, List<DictionaryStimuli> dictionaries)
    {
        this.Name = Name;

        this._gStim = graphical_stimulus;
        this._aStim = auditory_stimulus;

        this._fonts         = fonts;
        this._dictionaries  = dictionaries;
    }
}