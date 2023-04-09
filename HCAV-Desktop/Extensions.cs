using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace HCAV_Desktop;

public static class Extensions
{
    public static Keys[] KeyDelta(this KeyboardState next, KeyboardState previous)
    {
        Keys[] incoming = next.GetPressedKeys();
        Keys[] held     = previous.GetPressedKeys();

        return incoming.Except(held).ToArray();
    }

    public static char[] KeyChars(this Keys[] keys, bool Shift = false, bool CapsLock = false, bool NumPad = true)
    {
        List<char> buffer = new List<char>();
        foreach(Keys key in keys)
            switch(key)
            {
                case Keys.A: buffer.Add((CapsLock ^ Shift) ? 'A' : 'a'); break;
                case Keys.B: buffer.Add((CapsLock ^ Shift) ? 'B' : 'b'); break;
                case Keys.C: buffer.Add((CapsLock ^ Shift) ? 'C' : 'c'); break;
                case Keys.D: buffer.Add((CapsLock ^ Shift) ? 'D' : 'd'); break;
                case Keys.E: buffer.Add((CapsLock ^ Shift) ? 'E' : 'e'); break;
                case Keys.F: buffer.Add((CapsLock ^ Shift) ? 'F' : 'f'); break;
                case Keys.G: buffer.Add((CapsLock ^ Shift) ? 'G' : 'g'); break;
                case Keys.H: buffer.Add((CapsLock ^ Shift) ? 'H' : 'h'); break;
                case Keys.I: buffer.Add((CapsLock ^ Shift) ? 'I' : 'i'); break;
                case Keys.J: buffer.Add((CapsLock ^ Shift) ? 'J' : 'j'); break;
                case Keys.K: buffer.Add((CapsLock ^ Shift) ? 'K' : 'k'); break;
                case Keys.L: buffer.Add((CapsLock ^ Shift) ? 'L' : 'l'); break;
                case Keys.M: buffer.Add((CapsLock ^ Shift) ? 'M' : 'm'); break;
                case Keys.N: buffer.Add((CapsLock ^ Shift) ? 'N' : 'n'); break;
                case Keys.O: buffer.Add((CapsLock ^ Shift) ? 'O' : 'o'); break;
                case Keys.P: buffer.Add((CapsLock ^ Shift) ? 'P' : 'p'); break;
                case Keys.Q: buffer.Add((CapsLock ^ Shift) ? 'Q' : 'q'); break;
                case Keys.R: buffer.Add((CapsLock ^ Shift) ? 'R' : 'r'); break;
                case Keys.S: buffer.Add((CapsLock ^ Shift) ? 'S' : 's'); break;
                case Keys.T: buffer.Add((CapsLock ^ Shift) ? 'T' : 't'); break;
                case Keys.U: buffer.Add((CapsLock ^ Shift) ? 'U' : 'u'); break;
                case Keys.V: buffer.Add((CapsLock ^ Shift) ? 'V' : 'v'); break;
                case Keys.W: buffer.Add((CapsLock ^ Shift) ? 'W' : 'w'); break;
                case Keys.X: buffer.Add((CapsLock ^ Shift) ? 'X' : 'x'); break;
                case Keys.Y: buffer.Add((CapsLock ^ Shift) ? 'Y' : 'y'); break;
                case Keys.Z: buffer.Add((CapsLock ^ Shift) ? 'Z' : 'z'); break;

                case Keys.Space: buffer.Add(' '); break;

                case Keys.D1: buffer.Add(Shift ? '!' : '1'); break;
                case Keys.D2: buffer.Add(Shift ? '@' : '2'); break;
                case Keys.D3: buffer.Add(Shift ? '#' : '3'); break;
                case Keys.D4: buffer.Add(Shift ? '$' : '4'); break;
                case Keys.D5: buffer.Add(Shift ? '%' : '5'); break;
                case Keys.D6: buffer.Add(Shift ? '^' : '6'); break;
                case Keys.D7: buffer.Add(Shift ? '&' : '7'); break;
                case Keys.D8: buffer.Add(Shift ? '*' : '8'); break;
                case Keys.D9: buffer.Add(Shift ? '(' : '9'); break;
                case Keys.D0: buffer.Add(Shift ? ')' : '0'); break;

                case Keys.NumPad1: if (NumPad) buffer.Add('1'); break;
                case Keys.NumPad2: if (NumPad) buffer.Add('2'); break;
                case Keys.NumPad3: if (NumPad) buffer.Add('3'); break;
                case Keys.NumPad4: if (NumPad) buffer.Add('4'); break;
                case Keys.NumPad5: if (NumPad) buffer.Add('5'); break;
                case Keys.NumPad6: if (NumPad) buffer.Add('6'); break;
                case Keys.NumPad7: if (NumPad) buffer.Add('7'); break;
                case Keys.NumPad8: if (NumPad) buffer.Add('8'); break;
                case Keys.NumPad9: if (NumPad) buffer.Add('9'); break;
                case Keys.NumPad0: if (NumPad) buffer.Add('0'); break;

                case Keys.Add:      buffer.Add('+'); break;
                case Keys.Subtract: buffer.Add('-'); break;
                case Keys.Multiply: buffer.Add('*'); break;
                case Keys.Divide:   buffer.Add('/'); break;
                case Keys.Decimal:  buffer.Add('.'); break;

                case Keys.OemTilde:         buffer.Add(Shift ? '~' : '`'); break;
                case Keys.OemMinus:         buffer.Add(Shift ? '_' : '-'); break;
                case Keys.OemPlus:          buffer.Add(Shift ? '+' : '='); break;
                case Keys.OemOpenBrackets:  buffer.Add(Shift ? '{' : '['); break;
                case Keys.OemCloseBrackets: buffer.Add(Shift ? '}' : ']'); break;
                case Keys.OemSemicolon:     buffer.Add(Shift ? ':' : ';'); break;
                case Keys.OemQuotes:        buffer.Add(Shift ? '"' : '\''); break;
                case Keys.OemComma:         buffer.Add(Shift ? '<' : ','); break;
                case Keys.OemPeriod:        buffer.Add(Shift ? '>' : '.'); break;
                case Keys.OemQuestion:      buffer.Add(Shift ? '?' : '/'); break;
                case Keys.OemBackslash:
                case Keys.OemPipe:
                case Keys.Separator:        buffer.Add(Shift ? '|' : '\\'); break;

                default: break;
            }

        return buffer.ToArray();
    }
}