using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HCAV_Desktop;

public static class ScreenSpace
{
    public static GraphicsDeviceManager GraphicsManager;

    private static int Height;
    private static int Width;

    private static bool WidthLonger;
    private static int SquareConstant;
    private static int SquareError;
    private static int SquareAdjustment;

    private static int MiddleX;
    private static int MiddleY;

    public static float AspectRatio;
    public static float SizeRatio;

    public static void Setup(GraphicsDeviceManager manager)
    {
        GraphicsManager = manager;

        Width   = GraphicsManager.PreferredBackBufferWidth;
        Height  = GraphicsManager.PreferredBackBufferHeight;

        if(Width > Height)
        {
            WidthLonger     = true;
            SquareConstant  = Height;
            SquareError     = Width - Height;
        }
        else
        {
            WidthLonger     = false;
            SquareConstant  = Width;
            SquareError     = Height - Width;
        }

        SquareAdjustment = SquareError / 2;

        MiddleX = Width / 2;
        MiddleY = Height / 2;

        AspectRatio = (float)Width / (float)Height;
        SizeRatio   = (float)SquareConstant / 1080f;
    }

    public static Vector2 ExamSpace(float x, float y) =>
        (WidthLonger) ? new Vector2(SquareAdjustment + (x * SquareConstant), y * SquareConstant)
        :               new Vector2(SquareConstant * x, SquareAdjustment + (y * SquareConstant));

    public static Vector2 ExamSpace(Vector2 input) => 
        (WidthLonger) ? new Vector2(SquareAdjustment + (input.X * SquareConstant), input.Y * SquareConstant)
        :               new Vector2(SquareConstant * input.X, SquareAdjustment + (input.Y * SquareConstant));

    public static Vector2 FullSpace(float x, float y) => new Vector2(
            MiddleX - (x * (SquareConstant / 2)), 
            MiddleY - (y * (SquareConstant / 2))
        );

    public static Vector2 FullSpace(Vector2 input) => new Vector2(
            MiddleX + (input.X * (SquareConstant / 2)), 
            MiddleY + (input.Y * (SquareConstant / 2))
        );
}