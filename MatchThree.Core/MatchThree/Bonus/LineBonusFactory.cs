using MatchThree.Core.MatchThree.Event;

namespace MatchThree.Core.MatchThree.Bonus
{
    public class LineBonusFactory : IGemBonusFactory
    {
        public void LineDestroy(object sender, LineDestroyEventArgs args)
        {
            if (args.Line.Count == 4)
            {
                args.TriggerNotDestroy = true;
            }
        }
    }
}