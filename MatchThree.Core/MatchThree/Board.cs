using System;
using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree
{
    public class Board : IUpdateDrawable
    {
        protected Texture2D CellTexture2D;
        protected Color CellColor;
        protected Rectangle[,] Cells;
        protected Gem[,] Gems;
        protected IGemFactory _gemFactory;
        protected int GemOffset;
        protected int GemSize;

        public Board(Texture2D cellTexture2D, Rectangle boardContainer, int lines, int columns, IGemFactory gemFactory)
        {
            CellTexture2D = cellTexture2D;
            Gems = new Gem[columns, lines];
            Cells = new Rectangle[columns, lines];
            var widthCell = boardContainer.Width / columns;
            var heightCell = boardContainer.Height / lines;
            var cellSize = Math.Min(widthCell, heightCell);
            GemOffset = (int) (cellSize * 0.1);
            GemSize = cellSize - GemOffset * 2;
            var startX = boardContainer.X + (boardContainer.Width - columns * cellSize) / 2;
            var startY = boardContainer.Y + (boardContainer.Height - lines * cellSize) / 2;
            CellColor = new Color(Color.White, 50);
            _gemFactory = gemFactory;
            for (var i = 0; i < columns; i++)
            for (var j = 0; j < lines; j++)
            {
                Cells[i, j] = new Rectangle(startX + i * cellSize, startY + j * cellSize, cellSize, cellSize);
                Gems[i, j] = _gemFactory.Create(new Rectangle(Cells[i, j].X + GemOffset, Cells[i, j].Y + GemOffset,
                    GemSize, GemSize));
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var gem in Gems)
                gem.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var cell in Cells)
                spriteBatch.Draw(CellTexture2D, cell, Color.White);
            foreach (var gem in Gems)
                gem.Draw(spriteBatch, gameTime);
        }
    }
}