using System.Collections.Generic;
using Systems.Pool;
using UnityEngine;

namespace Ball
{
    public class BallManager : ObjectPool
    {
        public static BallManager Instance;
        private List<GameObject> _balls = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
        }

        public void RemoveBall(GameObject ball)
        {
            if (_balls.Count == 1)
            {
                GameManager.Instance.GameOver();
                return;
            }
            _balls.Remove(ball);
            CameraController.Instance.SetTarget(_balls[0]);
        }

        public GameObject SpawnBall(Vector3 position)
        {
            var ball = SpawnFromPool("Ball", position, Quaternion.identity);
            ball.GetComponent<BallController>().Setup();
            _balls.Add(ball);
            return ball;
        }

        public void SpawnBall(Vector3 position, out Rigidbody2D rb)
        {
            rb = SpawnBall(position).GetComponent<Rigidbody2D>();
        }
    }
}