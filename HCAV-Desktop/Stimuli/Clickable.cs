using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static HCAV_Desktop.ScreenSpace;

namespace HCAV_Desktop;

public class Clickable
{
    public enum MouseRelativeState : byte
    {
        Outside = 0x01,
        Entering = 0x02,
        Inside = 0x04,
        Leaving = 0x08
    }

    public enum MouseRelativeClickState : byte
    {
        Nothing = 0x01,
        Clicking = 0x02,
        Held = 0x04,
        Lifting = 0x08,

        EnteredHeld = 0xF0
    }

    public delegate void ClickableEvent(Clickable sender);
    public delegate void ClickableTimedEvent(Clickable sender, TimeSpan time);

    public event ClickableEvent ClickInitiated;
    public event ClickableTimedEvent ClickEnded;

    public event ClickableEvent HoverEnter;
    public event ClickableTimedEvent HoverExit;

    public event ClickableTimedEvent Clicking;
    public event ClickableTimedEvent Hovering;

    public string ShortCode;

    public Drawable defaultGS;

    //Texture2D Size. Not explicitly intersection region - only in default behaviour.
    //NOTE: Make this clear in documentation - overwriting intersection calculation should be unaffected by this.
    //      Hence - why event's are a forced pattern, and you should not externally calculate intersection.
    public float Width;
    public float Height;

    //NOTE: Center Point by default behaviour.
    public Vector2 Location;
    public bool ExamSpace;

    public TimeSpan hoverTime                           { get; protected set; }
    public TimeSpan clickTime                           { get; protected set; }
    public MouseRelativeState relativeState             { get; protected set; }
    public MouseRelativeClickState relativeClickState   { get; protected set; }

    public Clickable(string ShortCode, Vector2 Location, float width, float height, Drawable defaultGS, bool ExamSpace = false)
    {
        this.ShortCode = ShortCode;

        this.defaultGS  = defaultGS;

        Width   = width;
        Height  = height;

        this.Location   = Location;
        this.ExamSpace  = ExamSpace;

        this.hoverTime          = new TimeSpan(0);
        this.relativeState      = MouseRelativeState.Outside;
        this.relativeClickState = MouseRelativeClickState.Nothing;
    }

    public Clickable(string ShortCode, Vector2 Location, gStimuli defaultGS, bool ExamSpace = true)
    {
        this.ShortCode = ShortCode;

        this.defaultGS  = defaultGS;

        //Default Size should be 1:1 pixel for pixel on a 1920x1080 screen.
        Width   = (defaultGS.Width  / 1080f) / ScreenSpace.SizeRatio;
        Height  = (defaultGS.Height / 1080f) / ScreenSpace.SizeRatio;

        this.Location   = Location;
        this.ExamSpace  = ExamSpace;

        this.hoverTime          = new TimeSpan(0);
        this.relativeState      = MouseRelativeState.Outside;
        this.relativeClickState = MouseRelativeClickState.Nothing;
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
        
        defaultGS.Draw(ref batch, new Rectangle(
            (int)(transformedPosition.X - (transformedWidth  / 2f)), 
            (int)(transformedPosition.Y - (transformedHeight / 2f)), 

            (int)transformedWidth, (int)transformedHeight
        ), (relativeState == MouseRelativeState.Inside) ? Color.LightGray : Color.White); //NOTE: Should this be disable-able? - It's overridable.
    }

    //NOTE: When writing docs - include tips to order states like I have, most likely states first.
    public virtual void Update(GameTime time, Vector2 relativeLocation, bool ClickInteraction)
    {
        bool intersecting = CalculateIntersection(relativeLocation);
        if(!intersecting)
        {
            switch(relativeState)
            {
                //Shouldn't ever require processing if it was outside the previous frame too.
                case MouseRelativeState.Outside: break;

                case MouseRelativeState.Leaving:
                    if(relativeClickState == MouseRelativeClickState.Nothing) break;

                    //Handle (Exit = Lift) Behaviour
                    else if(relativeClickState == MouseRelativeClickState.Lifting)
                    {
                        relativeClickState = MouseRelativeClickState.Nothing;
                        break;
                    }
                    else //Shouldn't get here but just in case... (haha get it case?)
                    {
                        ClickEnded?.Invoke(this, clickTime);
                        clickTime = new TimeSpan(0);
                        relativeClickState = MouseRelativeClickState.Lifting;
                        break;
                    }

                case MouseRelativeState.Inside:
                    HoverExit?.Invoke(this, hoverTime);
                    hoverTime = new TimeSpan(0);
                    relativeState = MouseRelativeState.Leaving;

                    //Handle Leaving Behaviour (might be clicking)
                    switch(relativeClickState)
                    {
                        case MouseRelativeClickState.Nothing: break;

                        //Letting go or not having interacted is identical.
                        case MouseRelativeClickState.EnteredHeld:
                        case MouseRelativeClickState.Lifting:
                            relativeClickState = MouseRelativeClickState.Nothing;
                            break;

                        //Click one frame or many frames before is identical.
                        case MouseRelativeClickState.Clicking:
                        case MouseRelativeClickState.Held:
                            ClickEnded?.Invoke(this, clickTime);
                            clickTime = new TimeSpan(0);
                            relativeClickState = MouseRelativeClickState.Lifting;
                            break;
                    }
                    break;

                case MouseRelativeState.Entering:
                    HoverExit?.Invoke(this, hoverTime);
                    hoverTime = new TimeSpan(0);
                    relativeState = MouseRelativeState.Leaving;

                    //Handle Fly-by Behaviour (could not have clicked yet)
                    relativeClickState = MouseRelativeClickState.Nothing;
                    break;
            }
        }
        else
        {
            switch(relativeState)
            {
                case MouseRelativeState.Inside:
                    hoverTime += time.ElapsedGameTime;
                    Hovering?.Invoke(this, hoverTime);
                    if(ClickInteraction)
                    {
                        switch(relativeClickState)
                        {
                            case MouseRelativeClickState.Held:
                                clickTime += time.ElapsedGameTime;
                                Clicking?.Invoke(this, clickTime);
                                break;
                            
                            case MouseRelativeClickState.Clicking:
                                clickTime += time.ElapsedGameTime;
                                Clicking?.Invoke(this, clickTime);
                                relativeClickState = MouseRelativeClickState.Held;
                                break;

                            //Lifting + Click Interaction is essentially a rapid Nothing + Click Interaction
                            //Only possible to replicate if click ended is invoked as soon as Held -> Lifting.
                            case MouseRelativeClickState.Lifting:
                            case MouseRelativeClickState.Nothing:
                                ClickInitiated?.Invoke(this);
                                relativeClickState = MouseRelativeClickState.Clicking;
                                break;

                            //Ignore click interaction if user was already clicking to begin with.
                            case MouseRelativeClickState.EnteredHeld: break;
                        }
                    }
                    else
                    {
                        switch(relativeClickState)
                        {
                            //Ignore, no state transitions are occurring.
                            case MouseRelativeClickState.Nothing: break;

                            //Entered Held with no Click Interaction is just a special case of Lifting.
                            case MouseRelativeClickState.EnteredHeld:
                            case MouseRelativeClickState.Lifting:
                                relativeClickState = MouseRelativeClickState.Nothing;
                                break;

                            case MouseRelativeClickState.Held:
                                ClickEnded?.Invoke(this, clickTime);
                                clickTime = new TimeSpan(0);
                                relativeClickState = MouseRelativeClickState.Lifting;
                                break;

                            //Rapid click - clickTime would not have progressed, can simply call click ended without resetting timespan.
                            case MouseRelativeClickState.Clicking:
                                ClickEnded?.Invoke(this, clickTime);
                                break;
                        }
                    }
                    break;

                //Should be treated as though we are inside - after updating such that we are.
                case MouseRelativeState.Entering:
                    relativeState = MouseRelativeState.Inside;
                    goto case MouseRelativeState.Inside;

                case MouseRelativeState.Outside:
                    HoverEnter?.Invoke(this);
                    relativeState = MouseRelativeState.Entering;
                    if(ClickInteraction) relativeClickState = MouseRelativeClickState.EnteredHeld;
                    else                 relativeClickState = MouseRelativeClickState.Nothing;
                    break;

                //Leaving -> Intersected is essentially rapid re-entry.
                //Edge Case, clicking during re-entry
                //Solution: check for click interaction before going to case Inside.
                //Edge^2 Case - hold is timed; behaviour: identical to having let go, and then clicked outside, and entered whilst still holding.
                case MouseRelativeState.Leaving:
                    if(ClickInteraction) relativeClickState = MouseRelativeClickState.EnteredHeld;
                    else                 relativeClickState = MouseRelativeClickState.Nothing;
                    relativeState = MouseRelativeState.Inside;
                    goto case MouseRelativeState.Inside;
            }
        }
    }
}