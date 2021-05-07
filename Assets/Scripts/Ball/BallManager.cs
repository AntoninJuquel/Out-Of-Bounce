using System.Collections.Generic;
using Systems.Chunk;
using Systems.Pool;
using Controllers;
using Managers;
using ScriptableObjects;
using UnityEngine;
using Upgrade.UpgradeSos;

namespace Ball
{
    public class BallManager : ObjectPool
    {
        [SerializeField] private PlayerSo playerSo;
        public static BallManager Instance;
        private List<GameObject> _balls = new List<GameObject>();
        private float _bouncyness;
        private void Awake()
        {
            Instance = this;
            _bouncyness = playerSo.GetUpgrades().Find(upgradeSo => upgradeSo.GetType() == typeof(BouncynessUpgrade)).GetLevel() * 3 + 15f;
        }

        public void RemoveBall(GameObject ball)
        {
            if (_balls.Count == 1)
            {
                GameManager.Instance.GameOver(ball.GetComponent<ChunkController>().GetPosition());
                _balls.Remove(ball);
                return;
            }
            _balls.Remove(ball);
            CameraController.Instance.SetTarget(_balls[0]);
        }

        public GameObject SpawnBall(Vector3 position)
        {
            var ball = SpawnFromPool("Ball", position, Quaternion.identity);
            ball.GetComponent<BallController>().Setup(_bouncyness);
            _balls.Add(ball);
            return ball;
        }

        public void SpawnBall(Vector3 position, out Rigidbody2D rb)
        {
            rb = SpawnBall(position).GetComponent<Rigidbody2D>();
        }
    }
}