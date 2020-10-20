using MatchThree.Core.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree.Bonus
{
    public abstract class GemBonusBase
    {
        protected Board Board;
        protected Texture2D Texture2D;
        protected Rectangle Box;
        protected GemBonusState State;
        public int XPosition;
        public int YPosition;

        /// <summary>
        /// Gem bonus state change processing
        /// </summary>
        /// <param name="gemBonus">Who executing</param>
        /// <param name="currentState">Current state</param>
        public delegate void ChangeGemBonusStateHandler(GemBonusBase gemBonus, GemBonusState currentState);

        public event ChangeGemBonusStateHandler ChangeGemBonusState;

        protected GemBonusBase(Board board, Texture2D texture2D, Rectangle gemBox, int xPosition, int yPosition)
        {
            Board = board;
            Texture2D = texture2D;
            ChangePosition(gemBox);
            Box.Width = (int)(gemBox.Width * 0.5);
            Box.Height = (int)(gemBox.Height * 0.5);
            XPosition = xPosition;
            YPosition = yPosition;
            State = GemBonusState.Idle;
        }

        public abstract void Update(GameTime gameTime, Rectangle[,] cells);

        /// <summary>
        /// Position change
        /// </summary>
        /// <param name="gemBox">Position gem</param>
        public void ChangePosition(Rectangle gemBox)
        {
            Box.X = (int)(gemBox.X + gemBox.Width * 0.25);
            Box.Y = (int)(gemBox.Y + gemBox.Height * 0.25);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture2D, Box, Color.White);
        }

        /// <summary>
        /// Change of state
        /// </summary>
        /// <param name="state">State</param>
        protected void ChangeState(GemBonusState state)
        {
            State = state;
            OnChangeGemBonusState(this, State);
        }

        protected virtual void OnChangeGemBonusState(GemBonusBase gemBonus, GemBonusState currentState)
        {
            ChangeGemBonusState?.Invoke(gemBonus, currentState);
        }
    }
}