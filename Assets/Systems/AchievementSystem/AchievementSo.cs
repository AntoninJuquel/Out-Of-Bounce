using System.Collections.Generic;
using UnityEngine;
using System;

namespace Systems.AchievementSystem
{
    [CreateAssetMenu(fileName = "Achievement", menuName = "Test")]
    public class AchievementSo : ScriptableObject
    {
        private Dictionary<AchievementType, Achievement> _achievements = new Dictionary<AchievementType, Achievement>();

        public void LoadAchievements()
        {
            foreach (var achievementType in (AchievementType[]) Enum.GetValues(typeof(AchievementType)))
            {
                _achievements.Add(achievementType, new Achievement());
            }
        }

        public void UpdateAchievement(AchievementType achievementType, float last) => _achievements[achievementType].UpdateAchievement(last);
    }

    [Serializable]
    public class Achievement
    {
        private float _last;
        private float _best;
        private float _total;

        public void UpdateAchievement(float last)
        {
            _last = last;
            _total += last;
            if (last <= _best) return;
            _best = last;
        }

        public float GetLast() => _last;
        public float GetBest() => _best;
        public float GetTotal() => _total;
    }

    public enum AchievementType
    {
        Score,
        Height,
        Time,
        Kills
    }
}