using System;
using System.Collections.Generic;
using System.Linq;
using ChunkSystem;
using Pool;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Dot
{
    public class DotManager : ObjectPool, IListenChunk
    {
        public static DotManager Instance;
        [SerializeField] private DotItem[] dots;
        [SerializeField] private int dotsPerChunk = 10;
        [SerializeField] private UnityEvent<Vector3, int> onDotDestroyed;
        private List<DotItem> _selectedDots = new();
        private readonly Dictionary<Bounds, List<GameObject>> _dotsMap = new();

        private void Awake()
        {
            Instance = this;
            _selectedDots = Array.FindAll(dots, dot => dot.Purchased && dot.Selected)
                .ToList();
            _selectedDots.Reverse();
        }

        private void SpawnDots(Bounds bounds)
        {
            if (_dotsMap.ContainsKey(bounds))
            {
                return;
            }

            _dotsMap.Add(bounds, new List<GameObject>());

            if (bounds.center.y < 0)
            {
                return;
            }

            var spawnRemainDict = _selectedDots.ToDictionary(dotItem => dotItem.name,
                dotItem => Mathf.CeilToInt(dotsPerChunk * dotItem.SpawnChance));

            DotItem GetRandomDot()
            {
                while (true)
                {
                    var dotItem = _selectedDots[Random.Range(0, _selectedDots.Count)];

                    if (spawnRemainDict[dotItem.name] <= 0 || Random.value > dotItem.SpawnChance)
                    {
                        continue;
                    }

                    spawnRemainDict[dotItem.name]--;
                    return dotItem;
                }
            }

            for (var i = 0; i < dotsPerChunk; i++)
            {
                var dotItem = GetRandomDot();
                var position = bounds.RandomPointInBounds();

                if (position.y <= 0)
                {
                    continue;
                }

                var dot = SpawnFromPool("Dot", position, Quaternion.identity);
                dot.GetComponent<DotController>().Setup(dotItem, bounds);
                _dotsMap[bounds].Add(dot);
            }
        }

        private void DisableDots(Bounds bounds)
        {
            var valid = _dotsMap.TryGetValue(bounds, out var dots);

            if (!valid)
            {
                return;
            }

            if (dots == null || dots.Count == 0)
            {
                return;
            }

            foreach (var dot in dots.Where(dot => dot))
            {
                dot.SetActive(false);
            }

            _dotsMap.Remove(bounds);
        }

        public void RemoveDot(Bounds chunk, GameObject dot, int points)
        {
            if (!_dotsMap.ContainsKey(chunk))
            {
                return;
            }

            if (!_dotsMap[chunk].Contains(dot))
            {
                return;
            }

            if (points > 0)
            {
                onDotDestroyed?.Invoke(dot.transform.position, points);
            }

            _dotsMap[chunk].Remove(dot);
        }

        public void ChunkCreatedHandler(Bounds bounds)
        {
            SpawnDots(bounds);
        }

        public void ChunkDisabledHandler(Bounds bounds)
        {
            DisableDots(bounds);
        }

        public void ChunkEnabledHandler(Bounds bounds)
        {
            SpawnDots(bounds);
        }
    }
}