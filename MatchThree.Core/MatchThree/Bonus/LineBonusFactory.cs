using MatchThree.Core.MatchThree.Event;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree.Bonus
{
    public class LineBonusFactory : IGemBonusFactory
    {
        private readonly Texture2D _horizontal;
        private readonly Texture2D _vertical;
        private readonly Texture2D _breaker;

        public LineBonusFactory(Texture2D horizontal, Texture2D vertical, Texture2D breaker)
        {
            _horizontal = horizontal;
            _vertical = vertical;
            _breaker = breaker;
        }

        public void LineDestroy(object sender, LineDestroyEventArgs args)
        {
            if (args.Line.Count == 4)
            {
                args.TriggerNotDestroy = true;
                if (args.Trigger.Bonus == null)
                {
                    args.Trigger.Bonus = new LineBonus(sender as Board,
                        args.Line[0].YPosition == args.Line[1].YPosition ? _horizontal : _vertical, _breaker,
                        args.Trigger.GemBox, args.Trigger.XPosition, args.Trigger.YPosition,
                        args.Line[0].YPosition == args.Line[1].YPosition);
                }
            }
        }
    }
}