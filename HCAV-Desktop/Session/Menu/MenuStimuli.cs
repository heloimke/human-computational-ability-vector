using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace HCAV_Desktop;

public static class MenuStimuli
{
    public static StimuliSet MenuResources;

    public static void BuildSet(ContentManager manager)
    {
        MenuResources = new StimuliSet(
            "Menu Stimuli",
            new List<gStimuli> {
                new gStimuli(manager, "Placeholders/testclickablegstim", "Test Button"),
                new gStimuli(manager, "Placeholders/testxtboxleft",      "Testxt Box Left Cap"),
                new gStimuli(manager, "Placeholders/testxtboxright",     "Testxt Box Right Cap"),
                new gStimuli(manager, "Placeholders/testxtboxmiddle",    "Testxt Box Middle"),
                new gStimuli(manager, "Placeholders/testxtcursor",       "Testxt Cursor")
            },
            new List<aStimuli>(),
            new List<tStimuli> {
                new tStimuli(manager, "Fonts/latoregular",      "Header"),
                new tStimuli(manager, "Fonts/latolight",        "Header Light"),
                new tStimuli(manager, "Fonts/latoblack",        "Header Thick"),
                new tStimuli(manager, "Fonts/quicksandmedium",  "Text"),
                new tStimuli(manager, "Fonts/quicksandlight",   "Text Light"),
                new tStimuli(manager, "Fonts/quicksandbold",    "Text Thick"),
                new tStimuli(manager, "Fonts/ubuntumono",       "Input")
            },
            new List<DictionaryStimuli>()
        );
    }
}