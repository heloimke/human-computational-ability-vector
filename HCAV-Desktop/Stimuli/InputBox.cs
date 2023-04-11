using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static HCAV_Desktop.ScreenSpace;

namespace HCAV_Desktop;

public class InputBox
{
    public enum InputState : byte
    {
        Inactive = 0xF0,
        Idle = 0x01,
        Typing = 0x02,
        Moving = 0x04,
        Deleting = 0x08
    }

    public delegate void InputSimpleEvent(InputBox sender);
    public delegate void InputSimpleTimedEvent(InputBox sender, TimeSpan duration);

    //Enter -> code '\n' | Tab -> code '\t' | Space -> code ' '
    public delegate void InputCommandEvent(InputBox sender, char code, bool ctrl = true, bool alt = false);

    public delegate void InputPositionalEvent(InputBox sender, int position);
    public delegate void InputCharacterEvent(InputBox sender, int position, char character);
    public delegate void InputCharactersEvent(InputBox sender, int start, int stop, string characters);

    public event InputSimpleEvent Activated;
    public event InputSimpleTimedEvent Deactivated;

    public event InputSimpleEvent ClearedInput;
    
    public event InputCharacterEvent InputCharacter;

    public event InputCommandEvent CommandSent;

    public event InputPositionalEvent MovedCursor;
    public event InputCharacterEvent DeletedCharacter;
    public event InputCharactersEvent DeletedWord;

    public event InputSimpleEvent StartedTyping;
    public event InputSimpleTimedEvent Typing;
    public event InputSimpleTimedEvent StoppedTyping;

    public event InputSimpleEvent StartedIdling;
    public event InputSimpleTimedEvent Idling;
    public event InputSimpleTimedEvent FinishedIdling;

    public event InputSimpleEvent StartedDeleting;
    public event InputSimpleTimedEvent Deleting;
    public event InputSimpleTimedEvent FinishedDeleting;

    public event InputSimpleEvent StartedMoving;
    public event InputSimpleTimedEvent Moving;
    public event InputSimpleTimedEvent FinishedMoving;

    public string ShortCode;

    public Drawable BoxLeftCap;
    public Drawable BoxRightCap;
    public Drawable BoxMiddle;

    public float BoxLeftCapWidth;
    public float BoxRightCapWidth;

    public bool LeftJustify;

    public Drawable Cursor;
    public float CursorWidth;

    public TimeSpan CursorFlickerCycle;
    public Color CursorColor { get; protected set; }
    public void UpdateCursorColor(TimeSpan elapsedTime)
    {
        //Map time modulo to [-1 -> 1] where 0 is "peak"
        float phase = (elapsedTime.Ticks % (CursorFlickerCycle.Ticks * 2)) / (float)(CursorFlickerCycle.Ticks) - 1;
        //Map phase to [0-255] where squared distance from phase peak is distance from 255.
        int brightness = (int)Math.Floor(255 * (1 - Math.Abs(phase * phase)));
        //Set RGBA to same value to vary luminosity and opacity without shifting Hue.
        CursorColor = new Color(brightness, brightness, brightness, brightness);
    }

    public tStimuli Font                    { get; protected set; }
    public float FontRelativeHeight         { get; protected set; }
    public Vector2 TestStringMeasurement    { get; protected set; }
    public float CharWidth                  { get; protected set; }
    public const string MeasurementString = "`Test|y@%^&*,";

    public float Width;
    public float Height;

    //NOTE: Center Point by default behaviour.
    public Vector2 Location;
    public bool ExamSpace;

    public Color ActiveColor;
    public Color HoverColor;
    public Color NormalColor;

    public TimeSpan TypingTimeSpan;
    public TimeSpan DeletingTimeSpan;
    public TimeSpan MovingTimeSpan;

    public TimeSpan TypingLastTime      { get; protected set; }
    public TimeSpan DeletingLastTime    { get; protected set; }
    public TimeSpan MovingLastTime      { get; protected set; }

    public TimeSpan activeTime      { get; protected set; }
    public TimeSpan idleTime        { get; protected set; }
    public TimeSpan typingTime      { get; protected set; }
    public TimeSpan deletingTime    { get; protected set; }
    public TimeSpan movingTime      { get; protected set; }

    private bool PreviousMouseInteractionState;
    private KeyboardState PreviousKeyboardState;

    public bool MouseOver           { get; protected set; }
    public bool Active              { get; protected set; }
    public InputState State         { get; protected set; }

    public string PlaceholderText;
    public string Text  { get; protected set; }
    public int Position { get; protected set; }

    public bool ConsideredTyping(TimeSpan totalElapsedTime)   => (totalElapsedTime < TypingLastTime   + TypingTimeSpan);
    public bool ConsideredDeleting(TimeSpan totalElapsedTime) => (totalElapsedTime < DeletingLastTime + DeletingTimeSpan); 
    public bool ConsideredMoving(TimeSpan totalElapsedTime)   => (totalElapsedTime < MovingLastTime   + MovingTimeSpan);

    public InputBox(
        string ShortCode, tStimuli Font, float FontRelativeHeight,
        Drawable BoxLeftCap, Drawable BoxRightCap, Drawable BoxMiddle, Drawable Cursor,
        float BoxLeftCapWidth, float BoxRightCapWidth, float CursorWidth,
        float Width, float Height, Vector2 Location, bool ExamSpace = true, bool LeftJustify = true, 
        Color? ActiveColor = null, Color? HoverColor = null, Color? NormalColor = null, TimeSpan? CursorFlickerCycle = null,
        TimeSpan? TypingTimeSpan = null, TimeSpan? DeletingTimeSpan = null, TimeSpan? MovingTimeSpan = null,
        string PlaceholderText = ""
    ) {
        this.ShortCode = ShortCode;

        this.LeftJustify        = LeftJustify;
        this.Font               = Font;
        this.FontRelativeHeight = FontRelativeHeight;

        this.BoxMiddle          = BoxMiddle;
        this.BoxLeftCap         = BoxLeftCap;
        this.BoxRightCap        = BoxRightCap;
        this.BoxLeftCapWidth    = BoxLeftCapWidth;
        this.BoxRightCapWidth   = BoxRightCapWidth;
        
        this.Cursor             = Cursor;
        this.CursorWidth        = CursorWidth;
        this.CursorFlickerCycle = CursorFlickerCycle ?? new TimeSpan(10 * 1000 * 1000);

        this.Width      = Width;
        this.Height     = Height;
        this.Location   = Location;
        this.ExamSpace  = ExamSpace;

        this.ActiveColor = ActiveColor  ?? new Color(210, 210, 230);
        this.HoverColor  = HoverColor   ?? new Color(230, 230, 240);
        this.NormalColor = NormalColor  ?? Color.White;

        this.TypingTimeSpan     = TypingTimeSpan    ?? new TimeSpan(10 * 1000 * 300); //300ms between characters
        this.DeletingTimeSpan   = DeletingTimeSpan  ?? new TimeSpan(10 * 1000 * 500); //500ms between deletions
        this.MovingTimeSpan     = MovingTimeSpan    ?? new TimeSpan(10 * 1000 * 400); //400ms between movements

        TypingLastTime   = new TimeSpan(0);
        DeletingLastTime = new TimeSpan(0);
        MovingLastTime   = new TimeSpan(0);

        activeTime   = new TimeSpan(0);
        idleTime     = new TimeSpan(0);
        typingTime   = new TimeSpan(0);
        deletingTime = new TimeSpan(0);
        movingTime   = new TimeSpan(0);

        PreviousMouseInteractionState   = false;
        PreviousKeyboardState           = new KeyboardState();

        MouseOver   = false;
        Active      = false;
        State       = InputState.Inactive;

        TestStringMeasurement = this.Font.content.MeasureString(MeasurementString);
        CharWidth             = TestStringMeasurement.X / (float)MeasurementString.Length;

        Text     = "";
        Position = 0;

        this.PlaceholderText = PlaceholderText;
    }

    protected virtual bool CalculateIntersection(Vector2 cursor)
    {
        //Function designed to minimise comptuation before rejection.
        //Justifcation: p(inside) << p(outside)
        float horizontalStart = Location.X - (Width / 2);
        
        //Is within horizontal section
        if(cursor.X >= horizontalStart)
            if(cursor.X <= horizontalStart + Width)
            {
                float verticalStart = Location.Y - (Height / 2);

                //Is within vertical section
                if(cursor.Y >= verticalStart)
                    if(cursor.Y <= verticalStart + Height)
                        return true;
            }

        return false;
    }

    public virtual void Draw(ref SpriteBatch batch)
    {
        Vector2 transformedPosition = (ExamSpace) ? ExamSpace(Location) : FullSpace(Location);

        float transformedHeight = Height * SizeRatio * (ExamSpace ? IdealSquare : IdealSquare / 2f);
        float transformedWidth  = Width  * SizeRatio * (ExamSpace ? IdealSquare : IdealSquare / 2f);

        int transformedLeftCapWidth  = (int)Math.Ceiling(BoxLeftCapWidth  * SizeRatio * (ExamSpace ? IdealSquare : IdealSquare / 2f));
        int transformedRightCapWidth = (int)Math.Ceiling(BoxRightCapWidth * SizeRatio * (ExamSpace ? IdealSquare : IdealSquare / 2f));

        float transformedCursorWidth = CursorWidth  * SizeRatio * (ExamSpace ? IdealSquare : IdealSquare / 2f);

        int MiddleWidth = (int)Math.Ceiling(transformedWidth - transformedLeftCapWidth - transformedRightCapWidth);

        BoxLeftCap.Draw(ref batch, new Rectangle(
                (int)(transformedPosition.X - (transformedWidth  / 2f)), 
                (int)(transformedPosition.Y - (transformedHeight / 2f)), 

                (int)(transformedLeftCapWidth), (int)transformedHeight
            ), 
            (Active) ? ActiveColor : (MouseOver) ? HoverColor : NormalColor
        ); //NOTE: Should this be disable-able? - It's overridable.
        
        BoxMiddle.Draw(ref batch, new Rectangle(
                (int)(transformedPosition.X - (transformedWidth  / 2f) + transformedLeftCapWidth), 
                (int)(transformedPosition.Y - (transformedHeight / 2f)), 

                MiddleWidth, (int)transformedHeight
            ), 
            (Active) ? ActiveColor : (MouseOver) ? HoverColor : NormalColor
        ); //NOTE: Should this be disable-able? - It's overridable.
        
        BoxRightCap.Draw(ref batch, new Rectangle(
                (int)(transformedPosition.X + (transformedWidth  / 2f) - transformedRightCapWidth), 
                (int)(transformedPosition.Y - (transformedHeight / 2f)), 

                (int)(transformedRightCapWidth), (int)transformedHeight
            ), 
            (Active) ? ActiveColor : (MouseOver) ? HoverColor : NormalColor
        ); //NOTE: Should this be disable-able? - It's overridable.

        //TODO: Allow right justification and precompute justification and buffer ?= placeholder here.o

        string TextBuffer;
        float CursorOffset;

        Vector2 TextMeasurement;
        float CalculatedTextWidth;

        if(!Active && Text == "")
        {
            TextBuffer = PlaceholderText;
            CursorOffset = 0; //Irrelevant.

            TextMeasurement = this.Font.content.MeasureString(PlaceholderText);
            CalculatedTextWidth = TextMeasurement.X * FontRelativeHeight * (transformedHeight / TestStringMeasurement.Y);

            if(CalculatedTextWidth > MiddleWidth)
            {
                int AcceptableCharacters = (int)Math.Floor(((MiddleWidth / CalculatedTextWidth) * PlaceholderText.Length));
                TextBuffer = PlaceholderText.Substring(0, AcceptableCharacters - 3) + "...";
            }
        }
        else
        {
            TextMeasurement = this.Font.content.MeasureString(Text);
            CalculatedTextWidth = TextMeasurement.X * FontRelativeHeight * (transformedHeight / TestStringMeasurement.Y);

            if(CalculatedTextWidth > MiddleWidth)
            {
                int AcceptableCharacters = (int)Math.Floor(((MiddleWidth / CalculatedTextWidth) * Text.Length));

                //Have to clip right
                if(Text.Length - Position > Math.Ceiling(AcceptableCharacters / 2f))
                {
                    //Also have to clip left - we're in the middle
                    if(Position > Math.Floor(AcceptableCharacters / 2f))
                    {
                        TextBuffer = ".." + Text.Substring(Position - (AcceptableCharacters / 2) + 2, AcceptableCharacters - 4) + "..";
                        TextMeasurement = this.Font.content.MeasureString(TextBuffer);
                        CalculatedTextWidth = (TextMeasurement.X * FontRelativeHeight * (transformedHeight / TestStringMeasurement.Y));
                        CursorOffset = (LeftJustify 
                            ? ((CalculatedTextWidth - transformedWidth) / 2f) + transformedLeftCapWidth
                            : (AcceptableCharacters % 2) == 0 ? 0 : -(CalculatedTextWidth / AcceptableCharacters) / 2f
                        );  //In the case of odd character length - it will be exactly half a character to the left.
                    }
                    else //We're at the beginning
                    {
                        TextBuffer = Text.Substring(0, AcceptableCharacters - 3) + "...";
                        TextMeasurement = this.Font.content.MeasureString(TextBuffer);
                        CalculatedTextWidth = (TextMeasurement.X * FontRelativeHeight * (transformedHeight / TestStringMeasurement.Y));
                        CursorOffset = (LeftJustify 
                            ? (CalculatedTextWidth * (Position / (float)AcceptableCharacters)) - (transformedWidth / 2f) + transformedLeftCapWidth
                            : (CalculatedTextWidth * (Position / (float)AcceptableCharacters)) - (CalculatedTextWidth / 2f)
                        );
                    }
                }
                else //We're at the end, if it's too long, and there's not too many on our RHS
                {
                    TextBuffer = "..." + Text.Substring(Text.Length - AcceptableCharacters + 3, AcceptableCharacters - 3);
                    TextMeasurement = this.Font.content.MeasureString(TextBuffer);
                    CalculatedTextWidth = (TextMeasurement.X * FontRelativeHeight * (transformedHeight / TestStringMeasurement.Y));
                    CursorOffset = (LeftJustify 
                        ? (CalculatedTextWidth * ((AcceptableCharacters - (Text.Length - Position)) / (float)AcceptableCharacters)) - (transformedWidth / 2f) + transformedLeftCapWidth 
                        : (CalculatedTextWidth * ((AcceptableCharacters - (Text.Length - Position)) / (float)AcceptableCharacters)) - (CalculatedTextWidth / 2f)
                    );
                }
            }
            else
            {
                TextBuffer = Text;
                CursorOffset = (LeftJustify 
                    ? (CalculatedTextWidth * (Position / (float)Text.Length)) - (transformedWidth / 2f) + transformedLeftCapWidth 
                    : (CalculatedTextWidth * (Position / (float)Text.Length)) - (CalculatedTextWidth / 2f)
                );
            }
        }

        Font.Draw(ref batch, TextBuffer, new Vector2(
                (int)(transformedPosition.X + (LeftJustify ? transformedLeftCapWidth - ((transformedWidth - CalculatedTextWidth) / 2f)  : 0)), 
                (int)(transformedPosition.Y)
            ),
            scale: FontRelativeHeight * (transformedHeight / TestStringMeasurement.Y)
        );
        
        if(Active) Cursor.Draw(ref batch, new Rectangle(
                (int)(transformedPosition.X + CursorOffset - (transformedCursorWidth / 2f)), 
                (int)(transformedPosition.Y - ((transformedHeight * FontRelativeHeight) / 2f)), 

                (int)(transformedCursorWidth), (int)(transformedHeight * FontRelativeHeight)
            ), 
            CursorColor
        );
    }

    public virtual void Activate()
    {
        if(Active) return; //Already active!

        Activated?.Invoke(this);
        Active  = true;
        State   = InputState.Idle;

        //Signals for idling will be handled in the Update state machine.
    }

    public virtual void Deactivate(TimeSpan totalElapsedTime)
    {
        if(!Active) return; //Already deactivated!

        Deactivated?.Invoke(this, activeTime);
        Active = false;

        if(ConsideredTyping(totalElapsedTime))   StoppedTyping?.Invoke(this, typingTime);
        if(ConsideredDeleting(totalElapsedTime)) FinishedDeleting?.Invoke(this, deletingTime);
        if(ConsideredMoving(totalElapsedTime))   FinishedMoving?.Invoke(this, movingTime);

        if(State == InputState.Idle) FinishedIdling?.Invoke(this, idleTime);

        TypingLastTime      = new TimeSpan(0);
        DeletingLastTime    = new TimeSpan(0);
        MovingLastTime      = new TimeSpan(0);

        activeTime      = new TimeSpan(0);
        idleTime        = new TimeSpan(0);
        typingTime      = new TimeSpan(0);
        deletingTime    = new TimeSpan(0);
        movingTime      = new TimeSpan(0);

        State = InputState.Inactive;
    }

    //NOTE: When writing docs - include tips to order states like I have, most likely states first.
    public virtual void Update(GameTime time, Vector2 relativeLocation, bool ClickInteraction, bool ClearInteraction)
    {
        UpdateCursorColor(time.TotalGameTime); //TODO: Only when not typing. - i.e. use idle time.
        MouseOver = CalculateIntersection(relativeLocation);

        //Only interested in click down - only down if previously up.
        bool clickDown = (PreviousMouseInteractionState) ? false : ClickInteraction;
        PreviousMouseInteractionState = ClickInteraction;
        
        if(clickDown)
        {
            if(MouseOver) Activate();
            else Deactivate(time.TotalGameTime);
        }

        //TODO: Signal clear.
        if(ClearInteraction) Text = "";

        if(!Active) return; //No more processing if inactive...

        KeyboardState nextState = Keyboard.GetState();
        Keys[] keyPresses = nextState.KeyDelta(PreviousKeyboardState);
        PreviousKeyboardState = nextState;
        //We don't need to keep the previous state, assign [previous = next] up here to stay concice.

        char[] characters = keyPresses.KeyChars(nextState.IsKeyDown(Keys.LeftShift) || nextState.IsKeyDown(Keys.RightShift), nextState.CapsLock);

        if(nextState.IsKeyDown(Keys.LeftControl) || nextState.IsKeyDown(Keys.RightControl))
        {
            if(nextState.IsKeyDown(Keys.LeftAlt) || nextState.IsKeyDown(Keys.RightAlt))
            {
                if(keyPresses.Contains(Keys.Back) || keyPresses.Contains(Keys.Delete))
                {
                    Text = ""; //Delete everything.
                    DeletingLastTime = time.TotalGameTime;
                    ClearedInput?.Invoke(this);
                }
                if(keyPresses.Contains(Keys.Left))
                {
                    Position = 0;
                    MovingLastTime = time.TotalGameTime;
                    MovedCursor?.Invoke(this, Position);
                }
                if(keyPresses.Contains(Keys.Right))
                {
                    Position = Text.Length;
                    MovingLastTime = time.TotalGameTime;
                    MovedCursor?.Invoke(this, Position);
                }
            }
            else
            {
                if(keyPresses.Contains(Keys.Back) && Text.Length != 0)
                {
                    //TODO Signals.
                    Text = Text.Substring(0, Position).TrimSmart() + Text.Substring(Position);
                    //OH I need to return the total jump length.
                }
                if(keyPresses.Contains(Keys.Delete) && Position != Text.Length)
                {
                    //TODO Signals.
                    Text = Text.Substring(0, Position) + Text.Substring(Position + Text.Substring(Position + 1).SeekFwdSmart());
                }
                if(keyPresses.Contains(Keys.Left) && Position != 0)
                {
                    if(Position > 1) Position = Text.Substring(0, Position - 1).SeekBackSmart();
                    if(Position == 1) Position = 0; //catch both else and 1 before end above ^
                    MovingLastTime = time.TotalGameTime;
                    MovedCursor?.Invoke(this, Position);
                }
                if(keyPresses.Contains(Keys.Right) && Position != Text.Length)
                {
                    Position += Text.Substring(Position + 1).SeekFwdSmart();
                    MovingLastTime = time.TotalGameTime;
                    MovedCursor?.Invoke(this, Position);
                }
            }

            if(characters.Length != 0)
            {
                foreach(char c in characters)
                {
                    CommandSent?.Invoke(this, c, 
                        nextState.IsKeyDown(Keys.LeftControl) || nextState.IsKeyDown(Keys.RightControl), 
                        nextState.IsKeyDown(Keys.LeftAlt) || nextState.IsKeyDown(Keys.RightAlt)
                    );
                }
            }
        }
        else
        {
            if(characters.Length != 0)
            {
                TypingLastTime = time.TotalGameTime;

                //If you're pressing two keys at a time I don't think you could assume an intended order XD
                foreach(char c in characters)
                {
                    if(Position != Text.Length) Text = Text.Substring(0, Position) + c + Text.Substring(Position);//useful for insert! + Text.Substring(Position + 1);
                    else Text += c;
                    Position++;
                    MovedCursor?.Invoke(this, Position);
                    InputCharacter?.Invoke(this, Position, c);
                }
            }

            if(keyPresses.Contains(Keys.Back) && Text.Length != 0)  //We can add letters before removing them too
            {                                   //  if I clicked a letter + bksp I'd expect nothing to happen, not a letter to be replaced.
                DeletedCharacter?.Invoke(this, Position, Text[Position]);
                Text = Text.Remove(Position - 1, 1);  
                Position--;
                DeletingLastTime = time.TotalGameTime;
                MovedCursor?.Invoke(this, Position);
            }

            if(keyPresses.Contains(Keys.Delete) && Position != Text.Length)
            {
                DeletedCharacter?.Invoke(this, Position, Text[Position + 1]);
                Text = Text.Remove(Position, 1);
                DeletingLastTime = time.TotalGameTime;
            }

            if(keyPresses.Contains(Keys.Right))
            {
                Position++;
                MovingLastTime = time.TotalGameTime;
                MovedCursor?.Invoke(this, Position);
            }
            if(keyPresses.Contains(Keys.Left))
            {
                Position--;
                MovingLastTime = time.TotalGameTime;
                MovedCursor?.Invoke(this, Position);
            }
        }

        if(keyPresses.Contains(Keys.Home) || keyPresses.Contains(Keys.PageUp))
        {
            Position = 0;
            MovingLastTime = time.TotalGameTime;
            MovedCursor?.Invoke(this, Position);
        }
        if(keyPresses.Contains(Keys.End)  || keyPresses.Contains(Keys.PageDown))
        {
            Position = Text.Length;
            MovingLastTime = time.TotalGameTime;
            MovedCursor?.Invoke(this, Position);
        }

        if(keyPresses.Contains(Keys.Enter))
        {
            CommandSent?.Invoke(this, '\n', 
                nextState.IsKeyDown(Keys.LeftControl) || nextState.IsKeyDown(Keys.RightControl), 
                nextState.IsKeyDown(Keys.LeftAlt) || nextState.IsKeyDown(Keys.RightAlt)
            );
        }

        if(keyPresses.Contains(Keys.Tab))
        {
            CommandSent?.Invoke(this, '\t', 
                nextState.IsKeyDown(Keys.LeftControl) || nextState.IsKeyDown(Keys.RightControl), 
                nextState.IsKeyDown(Keys.LeftAlt) || nextState.IsKeyDown(Keys.RightAlt)
            );
        }

        Position = (Position >= 0) ? (Position <= Text.Length) ? Position : Text.Length : 0;

        bool typing     = ConsideredTyping(time.TotalGameTime);
        bool deleting   = ConsideredDeleting(time.TotalGameTime);
        bool moving     = ConsideredMoving(time.TotalGameTime);

        if(typing || deleting || moving)
        {
            if(State == InputState.Idle)
            {
                FinishedIdling?.Invoke(this, idleTime);
                idleTime = new TimeSpan(0);
            }
        }
        else
        {
            if(State == InputState.Idle)
            {
                idleTime += time.ElapsedGameTime;
                Idling?.Invoke(this, idleTime);
            }
            else
            {
                StartedIdling?.Invoke(this);
                idleTime = time.ElapsedGameTime;
            }
        }

        //Justification of order; if one is moving, that's is a primary activity, and deleting is a normal part of typing.
        if(deleting)
        {
            State = InputState.Deleting;
            if(deletingTime.Ticks == 0)
            {
                StartedDeleting?.Invoke(this);
                deletingTime = time.ElapsedGameTime;
                Deleting?.Invoke(this, deletingTime);
            }
            else
            {
                deletingTime += time.ElapsedGameTime;
                Deleting?.Invoke(this, deletingTime);
            }
        }
        else if (deletingTime.Ticks != 0) //We must've JUST stopped.
        {
            FinishedDeleting?.Invoke(this, deletingTime);
            deletingTime = new TimeSpan(0);
        }   //State should've been changed above.


        if(typing)
        {
            State = InputState.Typing;
            if(typingTime.Ticks == 0)
            {
                StartedTyping?.Invoke(this);
                typingTime = time.ElapsedGameTime;
                Typing?.Invoke(this, typingTime);
            }
            else
            {
                typingTime += time.ElapsedGameTime;
                Typing?.Invoke(this, typingTime);
            }
        }
        else if (typingTime.Ticks != 0) //We must've JUST stopped.
        {
            StoppedTyping?.Invoke(this, typingTime);
            typingTime = new TimeSpan(0);
        }   //State should've been changed above.


        if(moving)
        {
            State = InputState.Moving;
            if(typingTime.Ticks == 0)
            {
                StartedMoving?.Invoke(this);
                movingTime = time.ElapsedGameTime;
                Moving?.Invoke(this, movingTime);
            }
            else
            {
                movingTime += time.ElapsedGameTime;
                Moving?.Invoke(this, movingTime);
            }
        }
        else if (movingTime.Ticks != 0) //We must've JUST stopped.
        {
            FinishedMoving?.Invoke(this, movingTime);
            movingTime = new TimeSpan(0);
        }   //State should've been changed above.
    }
}