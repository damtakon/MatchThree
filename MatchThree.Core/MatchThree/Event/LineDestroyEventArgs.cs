using System;
using System.Collections.Generic;

namespace MatchThree.Core.MatchThree.Event
{
    public class LineDestroyEventArgs : EventArgs
    {
        public List<Gem> Line;
        public Gem Trigger;
        public bool TriggerNotDestroy;

        public LineDestroyEventArgs(List<Gem> line, Gem trigger)
        {
            Line = line;
            Trigger = trigger;
        }
    }
}