using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MatchThree.Core.Extension
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class RectangleExtension
    {
        /// <summary>
        /// Set the X Y position of a rectangle from a vector
        /// </summary>
        /// <param name="rectangle">Rectangle</param>
        /// <param name="vector">Vector</param>
        public static void SetXY(this ref Rectangle rectangle, Vector2 vector)
        {
            rectangle.X = (int)vector.X;
            rectangle.Y = (int)vector.Y;
        }

        /// <summary>
        /// Set the X Y Width Height position of a rectangle from a vector's
        /// </summary>
        /// <param name="rectangle">Rectangle</param>
        /// <param name="vectorXY">Vector X Y</param>
        /// <param name="vectorWH">Vector Width Height</param>
        public static void SetXYWH(this ref Rectangle rectangle, Vector2 vectorXY, Vector2 vectorWH)
        {
            rectangle.SetXY(vectorXY);
            rectangle.Width = (int)vectorWH.X;
            rectangle.Height = (int)vectorWH.Y;
        }
    }
}