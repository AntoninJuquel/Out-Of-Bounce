using System;
using System.Collections.Generic;

namespace Systems.Statistic
{
    public static class StatisticUtilities
    {
        public static IEnumerable<StatisticType> StatisticTypesArray() => (StatisticType[]) Enum.GetValues(typeof(StatisticType));
    }

    [Serializable]
    public class Statistic
    {
        public StatisticType statisticType;
        public float last;
        public float best;
        public float total;

        public void UpdateStatistic(float lastValue)
        {
            last = lastValue;
            total += lastValue;
            if (lastValue <= best) return;
            best = lastValue;
        }
    }
    
    [Serializable]
    public enum StatisticType
    {
        Score,
        Height,
        Time,
        Kills,
        Money
    }
}