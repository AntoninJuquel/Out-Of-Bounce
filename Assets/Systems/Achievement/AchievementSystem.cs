using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Achievement
{
    public static class AchievementUtilities
    {
        public static IEnumerable<StatisticType> AchievementTypesArray() => (StatisticType[]) Enum.GetValues(typeof(StatisticType));
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