using System;
using System.Collections.Generic;
using Systems.Chunk;
using Systems.Pool;
using Ball;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dot
{
    public class DotManager : ObjectPool
    {
        public static DotManager Instance;
        [SerializeField] private DotSo[] dotSos;
        [SerializeField] private int dotsPerChunk = 10;
        private Dictionary<Chunk, List<GameObject>> dotsMap = new Dictionary<Chunk, List<GameObject>>();

        private void Awake()
        {
            Instance = this;
            foreach (var dotSo in dotSos)
            {
                var dot = _dotsMap[dotSo.GetDotType()];
                dotSo.SetActions(dot.SetUp, dot.Bounce);
            }
        }

        private void Start()
        {
            ChunkManager.Instance.ChunkEvent += HandleDots;
        }

        private readonly Dictionary<DotType, DotActions> _dotsMap = new Dictionary<DotType, DotActions>()
        {
            {
                DotType.Basic, new DotActions(null, (ball, dot, bouncyness) =>
                {
                    var rigid = ball.GetComponent<Rigidbody2D>();
                    rigid.velocity = rigid.velocity.normalized * bouncyness;
                })
            },
            {
                DotType.Enemy, new DotActions(null, (ball, dot, bouncyness) => ball.SetActive(false))
            },
            {
                DotType.Coin, new DotActions(dot => dot.GetComponent<Collider2D>().isTrigger = true, null)
            },
            {
                DotType.Scaler, new DotActions(null, (ball, dot, bouncyness) => ball.transform.localScale = Vector3.one * 2f)
            },
            {
                DotType.Explosive, new DotActions(null, (ball, dot, bouncyness) =>
                {
                    foreach (var col in Physics2D.OverlapCircleAll(dot.transform.position, 15f))
                        col.GetComponent<DotController>()?.Destroy();
                })
            },
            {
                DotType.Spawner, new DotActions(null, (ball, dot, bouncyness) => BallManager.Instance.SpawnBall(dot.transform.position))
            },
            {
                DotType.Direction, new DotActions(dot => dot.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f)), (ball, dot, bouncyness) =>
                {
                    var rigid = ball.GetComponent<Rigidbody2D>();
                    rigid.velocity = dot.transform.right * bouncyness;
                })
            }
        };

        private void HandleDots(Chunk chunk, bool added)
        {
            if (added)
            {
                if (dotsMap.ContainsKey(chunk)) return;
                dotsMap.Add(chunk, new List<GameObject>());
                for (var i = 0; i < dotsPerChunk; i++)
                {
                    foreach (var dotSo in dotSos)
                    {
                        if (!dotSo.Spawn(Random.value)) continue;
                        var dot = SpawnFromPool("Dot", chunk.RandomPointInBounds(), Quaternion.identity);
                        dot.GetComponent<DotController>().Setup(dotSo);
                        dotsMap[chunk].Add(dot);
                    }
                }
            }
            else
            {
                foreach (var dot in dotsMap[chunk])
                {
                    dot.SetActive(false);
                }

                dotsMap.Remove(chunk);
            }
        }
    }

    public class DotActions
    {
        public Action<GameObject> SetUp { get; }
        public Action<GameObject, GameObject, float> Bounce { get; }

        public DotActions(Action<GameObject> setup, Action<GameObject, GameObject, float> bounce)
        {
            SetUp = setup;
            Bounce = bounce;
        }
    }

    public enum DotType
    {
        Basic,
        Enemy,
        Coin,
        Scaler,
        Spawner,
        TimeSlower,
        Attractor,
        Repulsor,
        Explosive,
        Direction,
    }
}