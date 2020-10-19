using Microsoft.Xna.Framework;

namespace MatchThree.Core.Extension
{
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

        public static Rectangle CreateRectangle(this ref Vector2 vector, int width, int height)
        {
            return new Rectangle((int) vector.X, (int) vector.Y, width, height);
        }
    }
}