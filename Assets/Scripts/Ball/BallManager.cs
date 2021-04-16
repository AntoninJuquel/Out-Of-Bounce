using Systems.Pool;
using UnityEngine;

namespace Ball
{
    public class BallManager : ObjectPool
    {
        public static BallManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public GameObject SpawnBall(Vector3 position)
        {
            var ball = SpawnFromPool("Ball", position, Quaternion.identity);
            ball.GetComponent<BallController>().Setup();
            return ball;
        }

        public void SpawnBall(Vector3 position, out Rigidbody2D rb)
        {
            rb = SpawnBall(position).GetComponent<Rigidbody2D>();
        }
    }
}