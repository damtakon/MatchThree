using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MatchThree.Core.Extension
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class VectorExtension
    {
        /// <summary>
        /// Add a number to the vector at both positions
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="num">Number</param>
        public static void Add(this ref Vector2 vector, float num)
        {
            vector.X += num;
            vector.Y += num;
        }

        /// <summary>
        /// Subtract a number to the vector at both positions
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="num">Number</param>
        public static void Sub(this ref Vector2 vector, float num)
        {
            vector.X -= num;
            vector.Y -= num;
        }

        /// <summary>
        /// Set the X Y position of a vector from a rectangle
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="rectangle">Rectangle</param>
        public static void SetXY(this ref Vector2 vector, Rectangle rectangle)
        {
            vector.X = rectangle.X;
            vector.Y = rectangle.Y;
        }

        /// <summary>
        /// Create a rectangle from a vector, width and height
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns>Rectangle</returns>
        public static Rectangle CreateRectangle(this ref Vector2 vector, int width, int height)
        {
            return new Rectangle((int) vector.X, (int) vector.Y, width, height);
        }
    }
}