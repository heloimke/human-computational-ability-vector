using System;
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

    public delegate void InputPositionalEvent(InputBox sender, int position);
    public delegate void InputCharacterEvent(InputBox sender, int position, char character);
    public delegate void InputCharactersEvent(InputBox sender, int start, int stop, char[] characters);

    public event InputSimpleEvent Activated;
    public event InputSimpleTimedEvent Deactivated;

    public event InputSimpleEvent ClearedInput;
    
    public event InputCharacterEvent InputCharacter;

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

    public tStimuli Font;

    public float Width;
    public float Height;

    //NOTE: Center Point by default behaviour.
    public Vector2 Location;
    public bool ExamSpace;

    public TimeSpan activeTime      { get; protected set; }
    public TimeSpan idleTime        { get; protected set; }
    public TimeSpan typingTime      { get; protected set; }
    public TimeSpan deletingTime    { get; protected set; }
    public TimeSpan movingTime      { get; protected set; }

    public bool Active              { get; protected set; }
    public InputState State         { get; protected set; }

    public string Text  { get; protected set; }
    public int Position { get; protected set; }

    public InputBox(
        string ShortCode, tStimuli Font, 
        Drawable BoxLeftCap, Drawable BoxRightCap, Drawable BoxMiddle, 
        float BoxLeftCapWidth, float BoxRightCapWidth, 
        float Width, float Height, Vector2 Location, bool ExamSpace = true
    ) {
        this.ShortCode = ShortCode;

        this.Font = Font;

        this.BoxMiddle          = BoxMiddle;
        this.BoxLeftCap         = BoxLeftCap;
        this.BoxRightCap        = BoxRightCap;
        this.BoxLeftCapWidth    = BoxLeftCapWidth;
        this.BoxRightCapWidth   = BoxRightCapWidth;

        this.Width      = Width;
        this.Height     = Height;
        this.Location   = Location;
        this.ExamSpace  = ExamSpace;


        activeTime   = new TimeSpan(0);
        idleTime     = new TimeSpan(0);
        typingTime   = new TimeSpan(0);
        deletingTime = new TimeSpan(0);
        movingTime   = new TimeSpan(0);

        Active = false;
        State  = InputState.Inactive;

        Text     = "";
        Position = 0;
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

        float transformedLeftCapWidth  = BoxLeftCapWidth  * SizeRatio * (ExamSpace ? IdealSquare : IdealSquare / 2f);
        float transformedRightCapWidth = BoxRightCapWidth * SizeRatio * (ExamSpace ? IdealSquare : IdealSquare / 2f);

        BoxLeftCap.Draw(ref batch, new Rectangle(
            (int)(transformedPosition.X - (transformedWidth  / 2f)), 
            (int)(transformedPosition.Y - (transformedHeight / 2f)), 

            (int)(transformedLeftCapWidth), (int)transformedHeight
        ), (Active) ? Color.LightGray : Color.White); //NOTE: Should this be disable-able? - It's overridable.
        
        BoxMiddle.Draw(ref batch, new Rectangle(
            (int)(transformedPosition.X - (transformedWidth  / 2f) + transformedLeftCapWidth), 
            (int)(transformedPosition.Y - (transformedHeight / 2f)), 

            (int)Math.Ceiling(transformedWidth - transformedLeftCapWidth - transformedRightCapWidth), (int)transformedHeight
        ), (Active) ? Color.LightGray : Color.White); //NOTE: Should this be disable-able? - It's overridable.
        
        BoxRightCap.Draw(ref batch, new Rectangle(
            (int)(transformedPosition.X + (transformedWidth  / 2f) - transformedRightCapWidth), 
            (int)(transformedPosition.Y - (transformedHeight / 2f)), 

            (int)(transformedRightCapWidth), (int)transformedHeight
        ), (Active) ? Color.LightGray : Color.White); //NOTE: Should this be disable-able? - It's overridable.
    }

    //NOTE: When writing docs - include tips to order states like I have, most likely states first.
    public virtual void Update(GameTime time, Vector2 relativeLocation, bool ClickInteraction)
    {

    }
}