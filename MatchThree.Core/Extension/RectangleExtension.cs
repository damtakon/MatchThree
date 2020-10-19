using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MatchThree.Core.Extension
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class RectangleExtension
    {
        public static void SetXY(this ref Rectangle rectangle, Vector2 vector)
        {
            rectangle.X = (int)vector.X;
            rectangle.Y = (int)vector.Y;
        }

        public static void SetXYWH(this ref Rectangle rectangle, Vector2 vectorXY, Vector2 vectorWH)
        {
            rectangle.SetXY(vectorXY);
            rectangle.Width = (int)vectorWH.X;
            rectangle.Height = (int)vectorWH.Y;
        }
    }
}