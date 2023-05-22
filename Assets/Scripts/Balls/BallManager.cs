using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.Events;
using PowerUp.UpgradeSos;

namespace Balls
{
    public class BallManager : ObjectPool
    {
        [SerializeField] private HeightPowerUpItem heightPowerUp;
        [SerializeField] private BouncynessPowerUpItem bouncynessPowerUp;
        [SerializeField] private UnityEvent onBallsEmpty;
        public static BallManager Instance;
        private readonly HashSet<BallController> _balls = new();
        private float _bouncyness;

        private void Awake()
        {
            Instance = this;
            _bouncyness = bouncynessPowerUp.Level * 3 + 15f;
        }

        private void Start()
        {
            SpawnBall(Vector3.up * 50f);
        }

        public void RemoveBall(BallController ball)
        {
            _balls.Remove(ball);
            if (_balls.Count == 0)
            {
                onBallsEmpty?.Invoke();
                return;
            }

            var ballController = _balls.GetEnumerator().Current;
            if (ballController != null)
            {
                ballController.BallCamera.SetPriority(10);
            }
        }

        public GameObject SpawnBall(Vector3 position)
        {
            var ball = SpawnFromPool("Ball", position, Quaternion.identity).GetComponent<BallController>();
            _balls.Add(ball);
            ball.Setup(_bouncyness, _balls.Count == 1 ? 10 : 0);
            return ball.gameObject;
        }

        public void SpawnBall(Vector3 position, out Rigidbody2D rb)
        {
            rb = SpawnBall(position).GetComponent<Rigidbody2D>();
        }
    }
}