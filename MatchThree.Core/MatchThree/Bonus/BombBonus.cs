using MatchThree.Core.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree.Bonus
{
    public sealed class BombBonus : GemBonusBase
    {
        public BombBonus(Board board, Texture2D texture2D, Rectangle gemBox, int xPosition, int yPosition) : base(board,
            texture2D, gemBox, xPosition, yPosition)
        {
        }

        public override void Update(GameTime gameTime, Rectangle[,] cells)
        {
            switch (State)
            {
                case GemBonusState.Idle:
                    ChangeState(GemBonusState.Run);
                    break;
                case GemBonusState.Run:
                    var columns = cells.GetLength(0);
                    var lines = cells.GetLength(1);
                    if (XPosition + 1 < columns)
                    {
                        Board.DestroyGem(XPosition + 1, YPosition);
                        if (YPosition - 1 >= 0)
                            Board.DestroyGem(XPosition + 1, YPosition - 1);
                        if (YPosition + 1 < lines)
                            Board.DestroyGem(XPosition + 1, YPosition + 1);
                    }

                    if (XPosition - 1 >= 0)
                    {
                        Board.DestroyGem(XPosition - 1, YPosition);
                        if (YPosition - 1 >= 0)
                            Board.DestroyGem(XPosition - 1, YPosition - 1);
                        if (YPosition + 1 < lines)
                            Board.DestroyGem(XPosition - 1, YPosition + 1);
                    }

                    if (YPosition - 1 >= 0)
                        Board.DestroyGem(XPosition, YPosition - 1);
                    if (YPosition + 1 < lines)
                        Board.DestroyGem(XPosition, YPosition + 1);

                    ChangeState(GemBonusState.Finish);
                    break;
                case GemBonusState.Finish:
                    break;
            }
        }
    }
}