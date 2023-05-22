using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Trackers
{
    public class ScoreTracker : MonoBehaviour
    {
        [SerializeField] private UnityEvent<int> onScoreChanged, onMultiplierChanged, onEndTracking;
        [SerializeField] private UnityEvent<float> onMultiplierTimerChanged;
        [SerializeField] private UnityEvent onMultiplierTimerExpired, onHighMultiplier, onLowMultiplier;
        [SerializeField] private AnimationCurve updateCountCurve, timeBeforeDecayCurve, decayRateCurve;
        [SerializeField] private int multiplierThreshold;
        private MMF_Player _scoreFeedback;
        private int _score;
        private int _updateScoreCount;
        private float _timeBeforeDecay, _multiplierTimer;
        private int _scoreMultiplier = 1;

        private int Score
        {
            get => _score;
            set
            {
                _score = value;
                onScoreChanged?.Invoke(_score);
            }
        }

        private int ScoreMultiplier
        {
            get => _scoreMultiplier;
            set
            {
                if (value >= multiplierThreshold)
                {
                    if (value >= _scoreMultiplier)
                    {
                        onHighMultiplier?.Invoke();
                    }
                    else
                    {
                        onMultiplierTimerExpired?.Invoke();
                    }
                }
                else
                {
                    onLowMultiplier?.Invoke();
                }

                _scoreMultiplier = value;
                onMultiplierChanged?.Invoke(_scoreMultiplier);
            }
        }

        private float MultiplierTimer
        {
            get => _multiplierTimer;
            set
            {
                _multiplierTimer = value;
                onMultiplierTimerChanged?.Invoke(_multiplierTimer);
            }
        }

        private void Awake()
        {
            _scoreFeedback = GetComponent<MMF_Player>();
        }

        private void Start()
        {
            Score = 0;
            ScoreMultiplier = 1;
            MultiplierTimer = 0.0f;
        }

        private void Update()
        {
            DecayMultiplier();
        }

        private void UpdateMultiplier()
        {
            _updateScoreCount += 1;

            var updateCount = updateCountCurve.Evaluate(ScoreMultiplier);
            _timeBeforeDecay = timeBeforeDecayCurve.Evaluate(ScoreMultiplier);

            if (_updateScoreCount < updateCount)
            {
                return;
            }

            _updateScoreCount = 0;
            ScoreMultiplier = Mathf.Max(1, ScoreMultiplier + 1);
            MultiplierTimer = 1.0f;
        }

        private float CalculateDecayRate()
        {
            var decayRate = decayRateCurve.Evaluate(ScoreMultiplier);
            return decayRate;
        }

        private void DecayMultiplier()
        {
            if (ScoreMultiplier <= 1)
            {
                return;
            }

            if (_timeBeforeDecay > 0.0f)
            {
                _timeBeforeDecay -= Time.deltaTime;
                return;
            }

            var decayRate = CalculateDecayRate();
            MultiplierTimer -= Time.deltaTime * decayRate;

            if (MultiplierTimer > 0.0f)
            {
                return;
            }

            _updateScoreCount = 0;
            ScoreMultiplier = Mathf.Max(1, ScoreMultiplier - 1);
            MultiplierTimer = ScoreMultiplier > 1 ? 1.0f : 0.0f;
        }

        [Button]
        public void UpdateScore(Vector3 position, int value)
        {
            var score = value * ScoreMultiplier;

            _scoreFeedback.PlayFeedbacks(position, score);
            Score += score;

            UpdateMultiplier();
        }

        public void EndTracking()
        {
            onEndTracking?.Invoke(_score);
            _score = 0;
            _updateScoreCount = 0;
            _timeBeforeDecay = 0.0f;
            _multiplierTimer = 0.0f;
            _scoreMultiplier = 1;
        }

        [Button]
        public void ResetScore()
        {
            _score = 0;
            _updateScoreCount = 0;
            _timeBeforeDecay = 0.0f;
            _multiplierTimer = 0.0f;
            _scoreMultiplier = 1;
        }
    }
}