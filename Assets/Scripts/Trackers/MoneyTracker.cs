using UnityEngine;
using UnityEngine.Events;

namespace Trackers
{
    public class MoneyTracker : MonoBehaviour
    {
        [SerializeField] private UnityEvent<int> onMoneyChanged, onEndTracking;
        private int _money;

        private int Money
        {
            get => _money;
            set
            {
                _money = value;
                onMoneyChanged?.Invoke(_money);
            }
        }

        public void AddMoney(int amount)
        {
            Money += amount;
        }

        private void Start()
        {
            Money = 0;
        }

        public void EndTracking()
        {
            onEndTracking?.Invoke(_money);
        }
    }
}