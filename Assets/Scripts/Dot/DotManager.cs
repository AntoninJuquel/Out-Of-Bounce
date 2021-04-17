using System;
using System.Collections.Generic;
using Systems.Chunk;
using Systems.Pool;
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
        }

        private void Start()
        {
            ChunkManager.Instance.ChunkEvent += HandleDots;
        }

        private readonly Dictionary<DotType, Dot> _dotsMap = new Dictionary<DotType, Dot>()
        {
            {
                DotType.Basic, new Dot(null, (ball, dot, bouncyness) =>
                {
                    var rigid = ball.GetComponent<Rigidbody2D>();
                    rigid.velocity = rigid.velocity.normalized * bouncyness;
                })
            },
            {
                DotType.Enemy, new Dot(null, (ball, dot, bouncyness) =>
                {
                    ball.SetActive(false);
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
                        var dotType = dotSo.GetDotType();
                        var dot = SpawnFromPool("Dot", chunk.RandomPointInBounds(), Quaternion.identity);
                        dot.GetComponent<DotController>().Setup(dotSo, _dotsMap[dotType].SetUp(), _dotsMap[dotType].Bounce());
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

    public class Dot
    {
        private readonly Action<GameObject> _setup;
        private readonly Action<GameObject, GameObject, float> _bounce;

        public Dot(Action<GameObject> setup, Action<GameObject, GameObject, float> bounce)
        {
            _setup = setup;
            _bounce = bounce;
        }

        public Action<GameObject> SetUp() => _setup;
        public Action<GameObject, GameObject, float> Bounce() => _bounce;
    }

    public enum DotType
    {
        Basic,
        Enemy
    }
}