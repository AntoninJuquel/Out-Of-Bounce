using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Achievement
{
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
    public class AchievementValue
    {
        public AchievementType achievementType;
        public float value;
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