using System.Collections.Generic;
using System.Linq;

namespace Examples.GameSession
{
    public struct ComboRankLookupState
    {
        public IReadOnlyList<ComboRank> ComboRanks;
        public IReadOnlyDictionary<string, int> ComboRankNameToInt;

        public static ComboRankLookupState Create() 
            => new() 
            { 
                ComboRanks = new List<ComboRank>(),
                ComboRankNameToInt = new Dictionary<string, int>(),
            };

        public static ComboRankLookupState Create(IReadOnlyList<ComboRank> comboRanks)
        {
            return new ComboRankLookupState
            {
                ComboRanks = comboRanks,
                ComboRankNameToInt = comboRanks.ToDictionary(x => x.RankName, x => x.ComboCount),
            };
        }
    }
}