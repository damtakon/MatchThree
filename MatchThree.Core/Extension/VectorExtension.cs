using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MatchThree.Core.Extension
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class VectorExtension
    {
        public static void Add(this ref Vector2 vector, float num)
        {
            vector.X += num;
            vector.Y += num;
        }

        public static void Sub(this ref Vector2 vector, float num)
        {
            vector.X -= num;
            vector.Y -= num;
        }

        public static void SetXY(this ref Vector2 vector, Rectangle rectangle)
        {
            vector.X = rectangle.X;
            vector.Y = rectangle.Y;
        }

        public static Rectangle CreateRectangle(this ref Vector2 vector, int width, int height)
        {
            return new Rectangle((int) vector.X, (int) vector.Y, width, height);
        }
    }
}