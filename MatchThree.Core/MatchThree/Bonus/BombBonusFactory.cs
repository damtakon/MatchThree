using MatchThree.Core.MatchThree.Event;

namespace MatchThree.Core.MatchThree.Bonus
{
    public class BombBonusFactory : IGemBonusFactory
    {
        public void LineDestroy(object sender, LineDestroyEventArgs args)
        {
            if (args.Line.Count >= 5)
            {
                args.TriggerNotDestroy = true;
            }
        }
    }
}