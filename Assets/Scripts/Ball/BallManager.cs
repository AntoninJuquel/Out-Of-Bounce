using System.Collections.Generic;
using Systems.Pool;
using Controllers;
using Managers;
using ScriptableObjects;
using UnityEngine;

namespace Ball
{
    public class BallManager : ObjectPool
    {
        public static BallManager Instance;
        [SerializeField] private PlayerSo playerSo;
        private List<GameObject> _balls = new List<GameObject>();
        private List<SkinSo> _skinSos = new List<SkinSo>();

        private void Awake()
        {
            Instance = this;
            _skinSos = playerSo.GetBallSkins().FindAll(skinSo => skinSo.Selected() && skinSo.Unlocked());
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
            var sprites = _skinSos[Random.Range(0, _skinSos.Count)].GetSprites();
            ball.GetComponent<BallController>().Setup(sprites);
            _balls.Add(ball);
            return ball;
        }

        public void SpawnBall(Vector3 position, out Rigidbody2D rb)
        {
            rb = SpawnBall(position).GetComponent<Rigidbody2D>();
        }
    }
}