using MatchThree.Core.MatchThree.Event;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree.Bonus
{
    public class BombBonusFactory : IGemBonusFactory
    {
        private readonly Texture2D _texture2D;

        public BombBonusFactory(Texture2D texture2D)
        {
            _texture2D = texture2D;
        }

        public void LineDestroy(object sender, LineDestroyEventArgs args)
        {
            if (args.Line.Count >= 5)
            {
                args.TriggerNotDestroy = true;
                if (args.Trigger.Bonus == null)
                    args.Trigger.Bonus = new BombBonus(sender as Board, _texture2D, args.Trigger.GemBox, args.Trigger.XPosition, args.Trigger.YPosition); 
            }
        }
    }
}