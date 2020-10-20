using MatchThree.Core.Enum;
using MatchThree.Core.Extension;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree.Bonus
{
    public sealed class LineBonus : GemBonusBase
    {
        private readonly Texture2D _breaker;
        private Rectangle _breakerBoxPlus;
        private Rectangle _breakerBoxMinus;
        private readonly bool _horizontal;
        private const int Speed = 6000;
        private Vector2 _plusVector;
        private Vector2 _minusVector;
        private Rectangle _plus;
        private Rectangle _minus;
        private int _xMax;
        private int _yMax;
        private int _xMin;
        private int _yMin;

        public LineBonus(Board board, Texture2D texture2D, Texture2D breaker, Rectangle gemBox, int xPosition, int yPosition,
            bool horizontal) : base(board, texture2D, gemBox, xPosition, yPosition)
        {
            _breaker = breaker;
            _horizontal = horizontal;
            _breakerBoxPlus = Box;
            _breakerBoxMinus = Box;
        }

        public override void Update(GameTime gameTime, Rectangle[,] cells)
        {
            switch (State)
            {
                case GemBonusState.Idle:
                    _plus.X = Box.X + Box.Width / 2;
                    _plus.Y = Box.Y + Box.Height / 2;
                    _plus.Height = 1;
                    _plus.Width = 1;
                    _plusVector.SetXY(_plus);
                    _minusVector.SetXY(_plus);
                    _minus = _plus;
                    var lastX = cells[cells.GetLength(0) - 1, 0];
                    var lastY = cells[0, cells.GetLength(1) - 1];
                    var minXY = cells[0, 0];
                    _xMax = lastX.X + lastX.Width;
                    _yMax = lastY.Y + lastY.Height;
                    _xMin = minXY.X;
                    _yMin = minXY.Y;
                    ChangeState(GemBonusState.Run);
                    break;
                case GemBonusState.Run:
                    var step = (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
                    if (_horizontal)
                    {
                        _plusVector.X += step;
                        _minusVector.X -= step;
                        if (_minusVector.X <= _xMin)
                            _minusVector.X = _xMin;
                        if (_plusVector.X >= _xMax)
                            _plusVector.X = _xMax;
                        if(_minusVector.X <= _xMin && _plusVector.X >= _xMax)
                            ChangeState(GemBonusState.Finish);
                    }
                    else
                    {
                        _plusVector.Y += step;
                        _minusVector.Y -= step;
                        if (_minusVector.Y <= _yMin)
                            _minusVector.Y = _yMin;
                        if (_plusVector.Y >= _yMax)
                            _plusVector.Y = _yMax;
                        if (_minusVector.Y <= _yMin && _plusVector.Y >= _yMax)
                            ChangeState(GemBonusState.Finish);
                    }
                    _plus.SetXY(_plusVector);
                    _minus.SetXY(_minusVector);
                    _breakerBoxPlus.X = _plus.X - Box.Width / 2;
                    _breakerBoxPlus.Y = _plus.Y - Box.Height / 2;
                    _breakerBoxMinus.X = _minus.X - Box.Width / 2;
                    _breakerBoxMinus.Y = _minus.Y - Box.Height / 2;
                    for (var x = 0; x < cells.GetLength(0); x++)
                    for (var y = 0; y < cells.GetLength(1); y++)
                    {
                        if (cells[x, y].Intersects(_plus))
                            Board.DestroyGem(x, y);
                        if (cells[x, y].Intersects(_minus))
                            Board.DestroyGem(x, y);
                    }

                    break;
                case GemBonusState.Finish:
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (State)
            {
                case GemBonusState.Idle:
                    base.Draw(spriteBatch, gameTime);
                    break;
                case GemBonusState.Run:
                    if (_plusVector.X < _xMax && _plusVector.Y < _yMax)
                        spriteBatch.Draw(_breaker, _breakerBoxPlus, Color.White);
                    if (_minusVector.X > _xMin && _minusVector.Y > _yMin)
                        spriteBatch.Draw(_breaker, _breakerBoxMinus, Color.White);
                    break;
                case GemBonusState.Finish:
                    break;
            }

            
        }
    }
}