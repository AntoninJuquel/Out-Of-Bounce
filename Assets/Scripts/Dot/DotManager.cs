using System.Collections.Generic;
using Systems.Chunk;
using Systems.Pool;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dot
{
    public class DotManager : ObjectPool
    {
        public static DotManager Instance;
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private int dotsPerChunk = 10;
        private List<DotSo> _dotSos = new List<DotSo>();
        private Dictionary<Chunk, List<GameObject>> dotsMap = new Dictionary<Chunk, List<GameObject>>();

        private void Awake()
        {
            Instance = this;
            _dotSos = playerSo.GetDots().FindAll(dotSo => dotSo.Unlocked());
            _dotSos.Reverse();
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
                    foreach (var dotSo in _dotSos)
                    {
                        if (!dotSo.Spawn(Random.value)) continue;
                        var dot = SpawnFromPool("Dot", chunk.RandomPointInBounds(), Quaternion.identity);
                        dot.GetComponent<DotController>().Setup(dotSo, chunk);
                        dotsMap[chunk].Add(dot);
                        break;
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

        public void RemoveDot(Chunk chunk, GameObject dot)
        {
            if (dotsMap[chunk].Contains(dot))
                dotsMap[chunk].Remove(dot);
        }
    }
}