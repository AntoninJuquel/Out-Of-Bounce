using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Trackers
{
    public class TimeTracker : MonoBehaviour
    {
        [SerializeField] private UnityEvent<float> onEndTracking;

        private float _startTime;
        private float _startPause;
        private float _totalTimePaused;
        private float TotalTimePlayed => Time.realtimeSinceStartup - _startTime - _totalTimePaused;

        private void Start()
        {
            StartTimer();
        }

        private void StartTimer()
        {
            _startTime = Time.realtimeSinceStartup;
            _totalTimePaused = 0;
            _startPause = 0;
        }

        public void Pause()
        {
            Time.timeScale = 0;
            _startPause = Time.realtimeSinceStartup;
        }

        public void Resume()
        {
            Time.timeScale = 1;
            var timePaused = Time.realtimeSinceStartup - _startPause;
            _totalTimePaused += timePaused;
        }

        [Button]
        public void EndTracking()
        {
            onEndTracking?.Invoke(TotalTimePlayed);
        }
    }
}