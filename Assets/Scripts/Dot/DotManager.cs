using System;
using System.Collections.Generic;
using Systems.Chunk;
using Systems.Pool;
using Systems.Unlock;
using Ball;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dot
{
    public class DotManager : ObjectPool
    {
        public static DotManager Instance;
        [SerializeField] private UnlockableDataBaseSo dotsDB;
        [SerializeField] private List<DotSo> dotSos = new List<DotSo>();
        [SerializeField] private int dotsPerChunk = 10;
        private Dictionary<Chunk, List<GameObject>> dotsMap = new Dictionary<Chunk, List<GameObject>>();

        private void Awake()
        {
            Instance = this;
            foreach (var unlockableSo in dotsDB.GetUnlockedSos() )
            {
                dotSos.Add(unlockableSo as DotSo);
            }
            dotSos.Reverse();
        }

        private void Start()
        {
            ChunkManager.Instance.ChunkEvent += HandleDots;
        }

        private void OnDisable()
        {
            ChunkManager.Instance.ChunkEvent -= HandleDots;
        }

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
}