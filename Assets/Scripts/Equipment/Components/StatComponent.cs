using System;
using System.Collections.Generic;

namespace Equipment
{
    [Serializable]
    public sealed class StatComponent : IComponent
    {
        public IReadOnlyDictionary<StatType, int> Stats;

        public StatComponent(IDictionary<StatType, int> stats)
        {
            Stats = new Dictionary<StatType, int>(stats);
        }
    }
}