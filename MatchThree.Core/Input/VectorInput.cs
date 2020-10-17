using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Input
{
    public abstract class VectorInput : IUpdateDrawable
    {
        public delegate void PressHandler(Vector2 point);
        public event PressHandler Press;

        public delegate void PressedMoveHandler(Vector2 startPoint, Vector2 currentPoint);
        public event PressedMoveHandler PressedMove;

        public delegate void MoveHandler(Vector2 point);
        public event MoveHandler Move;

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        protected virtual void OnPress(Vector2 point)
        {
            Press?.Invoke(point);
        }
        protected virtual void OnPressedMove(Vector2 startPoint, Vector2 currentPoint)
        {
            PressedMove?.Invoke(startPoint, currentPoint);
        }

        protected virtual void OnMove(Vector2 point)
        {
            Move?.Invoke(point);
        }
    }
}