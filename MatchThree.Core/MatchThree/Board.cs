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
                    if (!FindMatches() && lastState == GemState.Swap)
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

        private void CreateGem(int x, int y)
        {
            Gems[x, y] = GemFactory.Create(CreateGemRectangle(x, y));
            Gems[x, y].ChangeGemState += OnChangeGemState;
            Gems[x, y].XPosition = x;
            Gems[x, y].YPosition = y;
            GemOnMove++;
        }

        private Rectangle CreateGemRectangle(int x, int y)
        {
            return new Rectangle(Cells[x, y].X + GemOffset, Cells[x, y].Y + GemOffset, GemSize, GemSize);
        }

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

        protected virtual void GemSwap(Gem gemOne, Gem gemTwo, GemState state)
        {
            Gems[gemOne.XPosition, gemOne.YPosition] = gemTwo;
            Gems[gemTwo.XPosition, gemTwo.YPosition] = gemOne;
            GemOnMove += 2;
            BoardState = BoardState.Move;
            gemOne.Swap(gemTwo, state);
        }

        protected virtual bool FindMatches()
        {
            var lists = new List<LineDestroyEventArgs>();
            foreach (var gem in Recalculate)
            {
                var matchLine = new List<Gem>();

                if (lists.Any(events => events.Line.Any(x => ReferenceEquals(x, gem))))
                    continue;

                var matchLineHorizontal = FindMatchLine(gem, Right, Left);
                var matchLineVertical = FindMatchLine(gem, Bottom, Top);

                if (matchLineHorizontal != null)
                    matchLine.AddRange(matchLineHorizontal);
                if (matchLineVertical != null)
                    matchLine.AddRange(matchLineVertical);

                if (matchLine.Count > 0)
                    lists.Add(new LineDestroyEventArgs(matchLine, gem));
            }

            var destroyed = new List<Gem>();
            foreach (var destroy in lists)
            {
                BoardState = BoardState.Destroy;

                GetBonus(destroy.Trigger);
                LineDestroy?.Invoke(this, destroy);
                foreach (var dGem in destroy.Line)
                {
                    if (destroyed.Any(x => ReferenceEquals(x, dGem)))
                        continue;

                    if (destroy.TriggerNotDestroy && ReferenceEquals(destroy.Trigger, dGem))
                    {
                    }
                    else
                    {
                        DestroyGem(dGem);
                    }

                    destroyed.Add(dGem);
                }
            }

            Recalculate.Clear();
            return lists.Count > 0;
        }

        protected virtual List<Gem> FindMatchLine(Gem gem, Func<Gem, int, (bool, Gem)> methodOne,
            Func<Gem, int, (bool, Gem)> methodTwo)
        {
            var destroy = new List<Gem> {gem};
            Find(gem, destroy, methodOne, 1);
            Find(gem, destroy, methodTwo, 1);

            if (destroy.Count >= 3)
                return destroy;

            return null;
        }

        protected virtual void Find(Gem gem, List<Gem> destroy, Func<Gem, int, (bool, Gem)> method, int number)
        {
            var (result, nextGem) = method.Invoke(gem, number);

            if (result)
            {
                destroy.Add(nextGem);
                Find(gem, destroy, method, ++number);
            }
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

        protected virtual (bool, Gem) Left(Gem gem, int number)
        {
            return Right(gem, -number);
        }

        protected virtual (bool, Gem) Right(Gem gem, int number)
        {
            if (gem.XPosition + number >= Columns || gem.XPosition + number < 0)
                return (false, null);
            var nextGem = Gems[gem.XPosition + number, gem.YPosition];
            return (gem.Equals(nextGem), nextGem);
        }

        protected virtual (bool, Gem) Top(Gem gem, int number)
        {
            return Bottom(gem, -number);
        }

        protected virtual (bool, Gem) Bottom(Gem gem, int number)
        {
            if (gem.YPosition + number >= Lines || gem.YPosition + number < 0)
                return (false, null);
            var nextGem = Gems[gem.XPosition, gem.YPosition + number];
            return (gem.Equals(nextGem), nextGem);
        }

        protected void GetBonus(Gem gem)
        {
            if (gem.Bonus != null)
            {
                AddBonuses.Add(gem.Bonus);
                gem.Bonus.ChangeGemBonusState += BonusOnChangeGemBonusState;
                gem.Bonus = null;
            }
        }

        private void BonusOnChangeGemBonusState(GemBonusBase gemBonus, GemBonusState currentState)
        {
            if (currentState == GemBonusState.Finish)
                RemoveBonuses.Add(gemBonus);
        }

        protected void DestroyGem(Gem gem)
        {
            if(gem == null)
                return;

            if (gem.GetState != GemState.Destroy)
            {
                GetBonus(gem);
                GemOnDestroy++;
                gem.Destroy();
            }
        }

        public void DestroyGem(int x, int y)
        {
            DestroyGem(Gems[x, y]);
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

        protected virtual void OnGemDestroy()
        {
            GemDestroy?.Invoke(this, EventArgs.Empty);
        }
    }
}