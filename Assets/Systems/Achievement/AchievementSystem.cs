using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Achievement
{
    public static class AchievementUtilities
    {
        public static IEnumerable<AchievementType> AchievementTypesArray() => (AchievementType[]) Enum.GetValues(typeof(AchievementType));
    }

    [Serializable]
    public class Achievement
    {
        public AchievementType achievementType;
        public float last;
        public float best;
        public float total;

        public void UpdateAchievement(float lastValue)
        {
            last = lastValue;
            total += lastValue;
            if (lastValue <= best) return;
            best = lastValue;
        }
    }
    
    [Serializable]
    public enum AchievementType
    {
        Score,
        Height,
        Time,
        Kills,
        Money
    }
}