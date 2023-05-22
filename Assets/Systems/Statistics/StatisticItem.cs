using System;
using Save;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Statistics
{
    [CreateAssetMenu(fileName = "New statistic", menuName = "Statistic", order = 0)]
    public class StatisticItem : ScriptableObject, ISave
    {
        [field: SerializeField] public string LastIntro { get; private set; }
        [field: SerializeField] public string BestIntro { get; private set; }
        [field: SerializeField] public string TotalIntro { get; private set; }
        [field: SerializeField] public string ValueFormat { get; private set; }
        [field: SerializeField] public float Last { get; private set; }
        [field: SerializeField] public float Best { get; private set; }
        [field: SerializeField] public float Total { get; private set; }

        [Button]
        public void UpdateStatistic(float inGameValue)
        {
            Last = inGameValue;
            Total += Last;

            if (Last > Best)
            {
                Best = Last;
            }

            Last = 0;
            Save();
        }

        [Button]
        public void UpdateStatistic(int inGameValue)
        {
            UpdateStatistic((float)inGameValue);
        }

        [Button]
        public void ResetStatistic()
        {
            Last = 0;
            Best = 0;
            Total = 0;
            Save();
        }

        public event Action<string, object> OnSave;
        public object DefaultSave => new StatisticSave(Last, Best, Total);
        public string Name => name;

        public void Load(object loadedObject)
        {
            if (loadedObject is StatisticSave statisticSave)
            {
                Last = statisticSave.Last;
                Best = statisticSave.Best;
                Total = statisticSave.Total;
            }
        }

        public void Save()
        {
            OnSave?.Invoke(Name, new StatisticSave(Last, Best, Total));
        }
    }
}