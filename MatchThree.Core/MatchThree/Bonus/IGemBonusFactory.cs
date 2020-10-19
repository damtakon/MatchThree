using MatchThree.Core.MatchThree.Event;

namespace MatchThree.Core.MatchThree.Bonus
{
    public interface IGemBonusFactory
    {
        void LineDestroy(object sender, LineDestroyEventArgs args);
    }
}