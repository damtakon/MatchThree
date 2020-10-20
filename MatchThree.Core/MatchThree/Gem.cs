using System;
using MatchThree.Core.Enum;
using MatchThree.Core.Extension;
using MatchThree.Core.Interface;
using MatchThree.Core.MatchThree.Bonus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree
{
    public class Gem : IUpdateDrawable, IEquatable<Gem>
    {
        protected readonly Texture2D Texture2D;
        protected Rectangle Box;
        protected Vector2 Position;
        protected Vector2 Size;
        protected Rectangle EndPosition;
        protected GemState GemState;
        protected int Speed = 2500;
        protected Color GemColor;
        protected float DestroyStep;

        public int XPosition;
        public int YPosition;
        public GemBonusBase Bonus;
        public Rectangle GemBox => Box;
        public GemState GetState => GemState;

        /// <summary>
        /// Gem state change handling
        /// </summary>
        /// <param name="gem">Gem whose state has changed</param>
        /// <param name="lastState">Last gem state</param>
        /// <param name="currentState">Current gem state</param>
        public delegate void ChangeGemStateHandler(Gem gem, GemState lastState, GemState currentState);

        public event ChangeGemStateHandler ChangeGemState;

        public Gem(Texture2D texture2D, Rectangle startPosition, Rectangle endPosition)
        {
            Texture2D = texture2D;
            EndPosition = endPosition;
            Box = startPosition;
            Position = new Vector2(startPosition.X, startPosition.Y);
            GemState = GemState.Move;
            GemColor = Color.White;
            Size = new Vector2(startPosition.Width, startPosition.Height);
            DestroyStep = startPosition.Height * 3f;
        }

        public void Update(GameTime gameTime)
        {
            switch (GemState)
            {
                case GemState.Swap:
                case GemState.Move:
                    Move(gameTime, ref Position.Y, ref EndPosition.Y);
                    Move(gameTime, ref Position.X, ref EndPosition.X);
                    Box.SetXY(Position);
                    Bonus?.ChangePosition(Box);
                    if (Math.Abs(Position.Y - EndPosition.Y) < 0.1 && Math.Abs(Position.X - EndPosition.X) < 0.1)
                        ChangeState(GemState.Idle);
                    break;
                case GemState.Destroy:
                    var step = (float) gameTime.ElapsedGameTime.TotalSeconds * DestroyStep;
                    Position.Add(step);
                    Size.Sub(step);
                    Size.Sub(step);
                    Box.SetXYWH(Position, Size);
                    if (Size.X < 5)
                        ChangeState(GemState.Idle);
                    break;
            }
        }

        /// <summary>
        /// Gem state changes
        /// </summary>
        /// <param name="state">State</param>
        private void ChangeState(GemState state)
        {
            var lastState = GemState;
            GemState = state;
            OnChangeGemState(this, lastState, GemState);
        }


        /// <summary>
        /// Single-axis travel application
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="start">Start point</param>
        /// <param name="end">End point</param>
        private void Move(GameTime gameTime, ref float start, ref int end)
        {
            if (Math.Abs(start - end) > 0.1)
            {
                var plus = end - start > 0;
                var step = plus
                    ? (float) gameTime.ElapsedGameTime.TotalSeconds * Speed
                    : -(float) gameTime.ElapsedGameTime.TotalSeconds * Speed;
                if (plus && end < start + step)
                    start = end;
                else if (!plus && end > start + step)
                    start = end;
                else
                    start += step;
            }
        }

        /// <summary>
        /// Destruction of the gem
        /// </summary>
        public void Destroy()
        {
            GemState = GemState.Destroy;
        }

        /// <summary>
        /// Swapping two gems
        /// </summary>
        /// <param name="gem">Second gem for swap</param>
        /// <param name="state">State gems</param>
        public void Swap(Gem gem, GemState state)
        {
            var endPosition = gem.EndPosition;
            var x = gem.XPosition;
            var y = gem.YPosition;
            gem.Move(XPosition, YPosition, EndPosition, state);
            Move(x, y, endPosition, state);
        }

        /// <summary>
        /// Move a gem to a specific position
        /// </summary>
        /// <param name="x">X gem board position</param>
        /// <param name="y">Y gem board position</param>
        /// <param name="position">Coordinate where the gem should be at the end of the path</param>
        /// <param name="state">State gem</param>
        public void Move(int x, int y, Rectangle position, GemState state)
        {
            if (Bonus != null)
            {
                Bonus.XPosition = x;
                Bonus.YPosition = y;
            }

            XPosition = x;
            YPosition = y;
            GemState = state;
            EndPosition = position;
        }

        /// <summary>
        /// Set paint color gem
        /// </summary>
        /// <param name="color">Paint color</param>
        public void SetColor(Color color)
        {
            GemColor = color;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture2D, Box, GemColor);
            Bonus?.Draw(spriteBatch, gameTime);
        }

        public bool Equals(Gem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Texture2D, other.Texture2D);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Gem) obj);
        }

        public override int GetHashCode()
        {
            return Texture2D != null ? Texture2D.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return $"X: {XPosition} Y: {YPosition} State: {GemState}";
        }

        protected virtual void OnChangeGemState(Gem gem, GemState lastState, GemState currentState)
        {
            ChangeGemState?.Invoke(gem, lastState, currentState);
        }
    }
}