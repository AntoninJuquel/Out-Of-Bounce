using System;
using UnityEngine;

namespace Statistics
{
    [Serializable]
    public class StatisticSave
    {
        [field: SerializeField] public float Last { get; private set; }
        [field: SerializeField] public float Best { get; private set; }
        [field: SerializeField] public float Total { get; private set; }

        public StatisticSave(float last, float best, float total)
        {
            Last = last;
            Best = best;
            Total = total;
        }
    }
}