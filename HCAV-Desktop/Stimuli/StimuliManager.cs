using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public class StimuliManager
{
    private List<StimuliSet> _openSets;

    public int Sets => _openSets.Count;
    public StimuliSet Set(int index) => _openSets[index];

    public StimuliSet FindSet(string name) => _openSets.Find(x => x.Name == name);
    public int GetSetIndexByName(string name) => _openSets.IndexOf(FindSet(name));

    public StimuliSet[] OpenSets => _openSets.ToArray();
}