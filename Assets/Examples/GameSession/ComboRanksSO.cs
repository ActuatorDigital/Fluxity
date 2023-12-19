using UnityEngine;

namespace Examples.GameSession
{
    [System.Serializable]
    public struct ComboRank
    {
        public int ComboCount;
        public string RankName;
    }

    //[CreateAssetMenu()]
    public class ComboRanksSO : ScriptableObject
    {
        public ComboRank[] ComboRanks;
    }
}