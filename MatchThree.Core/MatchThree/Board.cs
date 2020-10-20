using System;
using System.Collections.Generic;
using System.Linq;
using MatchThree.Core.Enum;
using MatchThree.Core.Extension;
using MatchThree.Core.Input;
using MatchThree.Core.Interface;
using MatchThree.Core.MatchThree.Bonus;
using MatchThree.Core.MatchThree.Event;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree
{
    /// <summary>
    /// Game board
    /// </summary>
    public class Board : IUpdateDrawable, IDisposable
    {
        protected Texture2D CellTexture2D;
        protected Color CellColor;

        protected Gem Selected;
        protected Color SelectedColor = new Color(Color.White, 128);

        protected IGemFactory GemFactory;
        protected Gem[,] Gems;
        protected int GemOnMove;
        protected int GemOnDestroy;
        protected int GemOffset;
        protected int GemSize;

        protected List<Gem> Recalculate;
        protected Rectangle[,] Cells;
        protected int Lines;
        protected int Columns;
        protected BoardState BoardState;
        protected VectorInput VectorInput;
        protected List<GemBonusBase> Bonuses = new List<GemBonusBase>();
        protected List<GemBonusBase> RemoveBonuses = new List<GemBonusBase>();
        protected List<GemBonusBase> AddBonuses = new List<GemBonusBase>();

        public event EventHandler<LineDestroyEventArgs> LineDestroy;
        public event EventHandler GemDestroy;

        public Board(Texture2D cellTexture2D, Rectangle boardContainer, int lines, int columns, IGemFactory gemFactory,
            VectorInput vectorInput)
        {
            if (lines == 0 || columns == 0)
                throw new ArgumentException("Lines and Columns can't be 0");

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
            GemFactory = gemFactory;
            for (var x = 0; x < columns; x++)
            for (var y = 0; y < lines; y++)
            {
                Cells[x, y] = new Rectangle(startX + x * cellSize, startY + y * cellSize, cellSize, cellSize);
                CreateGem(x, y);
            }

            Lines = lines;
            Columns = columns;
            Recalculate = new List<Gem>(Gems.Length);
            BoardState = BoardState.Move;
            VectorInput = vectorInput;
            VectorInput.Press += VectorInputOnPress;
        }

        /// <summary>
        /// Gem state change handling
        /// </summary>
        /// <param name="gem">Gem whose state has changed</param>
        /// <param name="lastState">Last gem state</param>
        /// <param name="currentState">Current gem state</param>
        protected virtual void OnChangeGemState(Gem gem, GemState lastState, GemState currentState)
        {
            if ((lastState == GemState.Move || lastState == GemState.Swap) && currentState == GemState.Idle)
            {
                GemOnMove--;
                Recalculate.Add(gem);
                if (GemOnMove == 0 && BoardState == BoardState.Move)
                {
                    var first = Recalculate.FirstOrDefault();
                    var two = Recalculate.LastOrDefault();
                    BoardState = BoardState.Idle;
                    if (!FindMatchesAndDestroy() && lastState == GemState.Swap)
                        GemSwap(first, two, GemState.Move);
                }
            }

            if (lastState == GemState.Destroy && currentState == GemState.Idle)
            {
                GemOnDestroy--;
                Gems[gem.XPosition, gem.YPosition].ChangeGemState -= OnChangeGemState;
                Gems[gem.XPosition, gem.YPosition] = null;
                OnGemDestroy();
                if (Bonuses.Count == 0 && GemOnDestroy == 0 && BoardState == BoardState.Destroy)
                    CreateNew();
            }
        }

        /// <summary>
        /// Creating new missing elements and applying gravity
        /// </summary>
        protected virtual void CreateNew()
        {
            for (var x = 0; x < Columns; x++)
            for (var y = Lines - 1; y >= 0; y--)
            {
                if (Gems[x, y] == null && y != 0)
                {
                    var newY = y;
                    do
                    {
                        newY--;
                    } while (Gems[x, newY] == null && newY != 0);

                    if (Gems[x, newY] != null)
                    {
                        Gems[x, y] = Gems[x, newY];
                        Gems[x, newY].Move(x, y, CreateGemRectangle(x, y), GemState.Move);
                        Gems[x, newY] = null;
                        GemOnMove++;
                    }
                }

                if (Gems[x, y] == null)
                    CreateGem(x, y);
            }

            BoardState = BoardState.Move;
        }

        /// <summary>
        /// Create gem
        /// </summary>
        /// <param name="x">X gem position</param>
        /// <param name="y">Y gem position</param>
        private void CreateGem(int x, int y)
        {
            Gems[x, y] = GemFactory.Create(CreateGemRectangle(x, y));
            Gems[x, y].ChangeGemState += OnChangeGemState;
            Gems[x, y].XPosition = x;
            Gems[x, y].YPosition = y;
            GemOnMove++;
        }

        /// <summary>
        /// Create gem rectangle
        /// </summary>
        /// <param name="x">X gem position</param>
        /// <param name="y">Y gem position</param>
        /// <returns></returns>
        private Rectangle CreateGemRectangle(int x, int y)
        {
            return new Rectangle(Cells[x, y].X + GemOffset, Cells[x, y].Y + GemOffset, GemSize, GemSize);
        }


        /// <summary>
        /// Handling clicks from the application
        /// </summary>
        /// <param name="position">Click position</param>
        protected virtual void VectorInputOnPress(Vector2 position)
        {
            if (BoardState != BoardState.Idle)
                return;

            var rectangle = position.CreateRectangle(1, 1);
            for (var x = 0; x < Columns; x++)
            for (var y = 0; y < Lines; y++)
            {
                var cell = Cells[x, y];
                if (cell.Intersects(rectangle))
                {
                    if (Selected == null)
                    {
                        Selected = Gems[x, y];
                        Selected.SetColor(SelectedColor);
                    }
                    else if (Selected.XPosition == x && Selected.YPosition == y)
                    {
                        Selected.SetColor(Color.White);
                        Selected = null;
                    }
                    else
                    {
                        var diffX = Math.Abs(Selected.XPosition - x);
                        var diffY = Math.Abs(Selected.YPosition - y);
                        if (diffX == 0 && diffY == 1 || diffX == 1 && diffY == 0)
                        {
                            Selected.SetColor(Color.White);
                            GemSwap(Selected, Gems[x, y], GemState.Swap);
                            Selected = null;
                        }
                        else
                        {
                            Selected.SetColor(Color.White);
                            Selected = Gems[x, y];
                            Selected.SetColor(SelectedColor);
                        }
                    }

                    break;
                }
            }
        }


        /// <summary>
        /// Swap gem
        /// </summary>
        /// <param name="gemOne">First gem</param>
        /// <param name="gemTwo">Second gem</param>
        /// <param name="state">Gem states</param>
        protected virtual void GemSwap(Gem gemOne, Gem gemTwo, GemState state)
        {
            Gems[gemOne.XPosition, gemOne.YPosition] = gemTwo;
            Gems[gemTwo.XPosition, gemTwo.YPosition] = gemOne;
            GemOnMove += 2;
            BoardState = BoardState.Move;
            gemOne.Swap(gemTwo, state);
        }


        /// <summary>
        /// Matching and Destruction
        /// </summary>
        /// <returns>Did you manage to find the lines</returns>
        protected virtual bool FindMatchesAndDestroy()
        {
            var matches = Gems.FindMatches(Recalculate);

            foreach (var destroy in matches)
            {
                BoardState = BoardState.Destroy;
                GetBonus(destroy.Trigger);
                LineDestroy?.Invoke(this, destroy);
                foreach (var dGem in destroy.Line)
                {
                    if (destroy.TriggerNotDestroy && ReferenceEquals(destroy.Trigger, dGem))
                    {
                    }
                    else
                    {
                        DestroyGem(dGem);
                    }
                }
            }

            Recalculate.Clear();
            return matches.Count > 0;
        }

        /// <summary>
        /// Collecting a bonus from a gem for execution
        /// </summary>
        /// <param name="gem"></param>
        protected virtual void GetBonus(Gem gem)
        {
            if (gem.Bonus != null)
            {
                AddBonuses.Add(gem.Bonus);
                gem.Bonus.ChangeGemBonusState += BonusOnChangeGemBonusState;
                gem.Bonus = null;
            }
        }

        /// <summary>
        /// Destruction of the gem
        /// </summary>
        /// <param name="gem">gem</param>
        protected virtual void DestroyGem(Gem gem)
        {
            if (gem == null)
                return;

            if (gem.GetState != GemState.Destroy)
            {
                GetBonus(gem);
                GemOnDestroy++;
                gem.Destroy();
            }
        }

        /// <summary>
        /// Destruction of the gem
        /// </summary>
        /// <param name="x">X gem position</param>
        /// <param name="y">Y gem position</param>
        public void DestroyGem(int x, int y)
        {
            DestroyGem(Gems[x, y]);
        }

        /// <summary>
        /// Gem bonus state change processing
        /// </summary>
        /// <param name="gemBonus">Who executing</param>
        /// <param name="currentState">Current state</param>
        private void BonusOnChangeGemBonusState(GemBonusBase gemBonus, GemBonusState currentState)
        {
            if (currentState == GemBonusState.Finish)
            {
                gemBonus.ChangeGemBonusState -= BonusOnChangeGemBonusState;
                RemoveBonuses.Add(gemBonus);
            }
        }

        /// <summary>
        /// Gem destroy event
        /// </summary>
        protected virtual void OnGemDestroy()
        {
            GemDestroy?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var gem in Gems)
                gem?.Update(gameTime);
            foreach (var bonus in Bonuses)
                bonus.Update(gameTime, Cells);

            if (RemoveBonuses.Count > 0)
            {
                foreach (var bonus in RemoveBonuses)
                    Bonuses.Remove(bonus);
                RemoveBonuses.Clear();
            }

            if (AddBonuses.Count > 0)
            {
                Bonuses.AddRange(AddBonuses);
                AddBonuses.Clear();
            }

            if (BoardState == BoardState.Destroy && Bonuses.Count == 0 && GemOnDestroy == 0)
                CreateNew();
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var cell in Cells)
                spriteBatch.Draw(CellTexture2D, cell, Color.White);
            foreach (var gem in Gems)
                gem?.Draw(spriteBatch, gameTime);
            foreach (var bonus in Bonuses)
                bonus.Draw(spriteBatch, gameTime);
        }

        public void Dispose()
        {
            //Fix Memory leak observers
            VectorInput.Press -= VectorInputOnPress;
            foreach (var gem in Gems)
                if (gem != null)
                    gem.ChangeGemState -= OnChangeGemState;
            foreach (var bonus in Bonuses)
                bonus.ChangeGemBonusState -= BonusOnChangeGemBonusState;
            foreach (var bonus in AddBonuses)
                bonus.ChangeGemBonusState -= BonusOnChangeGemBonusState;
        }
    }
}