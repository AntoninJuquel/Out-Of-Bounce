using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.Chunk
{
    public class ChunkManager : MonoBehaviour
    {
        public static ChunkManager Instance;
        [SerializeField] private int chunkSize = 32;
        private Dictionary<Vector3, Chunk> _chunks = new Dictionary<Vector3, Chunk>();
        public bool Started => _chunks.Count != 0;

        public delegate void ChunkEventHandler(Chunk position, bool added);

        public event ChunkEventHandler ChunkEvent;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            foreach (var bounds in _chunks.Select(kvp => kvp.Value.GetBounds()))
            {
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }

        private void GenerateChunksAround(ChunkController chunkController, Vector3 position)
        {
            for (var x = -1; x < 2; x++)
            {
                for (var y = -1; y < 2; y++)
                {
                    var chunkPosition = position + new Vector3(x, y) * chunkSize;
                    if (!_chunks.ContainsKey(chunkPosition))
                    {
                        _chunks.Add(chunkPosition, new Chunk(chunkPosition, chunkSize));
                    }

                    ChunkEvent?.Invoke(_chunks[chunkPosition], true);

                    _chunks[chunkPosition].AddChunkController(chunkController);
                }
            }
        }

        public Chunk GetClosestChunk(ChunkController chunkController, Vector3 oldPosition, Vector3 position)
        {
            var keyValuePair = _chunks.FirstOrDefault(kvp => kvp.Value.GetBounds().Contains(position));

            GenerateChunksAround(chunkController, keyValuePair.Key);
            var delta = (keyValuePair.Key - oldPosition).normalized;
            if (delta == Vector3.zero || oldPosition == position) return keyValuePair.Value;

            if (Mathf.Abs((int) delta.x) != Mathf.Abs((int) delta.y))
                for (var i = -1; i < 2; i++)
                {
                    var chunkPosition = oldPosition - delta * chunkSize + new Vector3(i * delta.y, i * delta.x) * chunkSize;
                    _chunks[chunkPosition].RemoveChunkController(chunkController);
                    if (!_chunks[chunkPosition].IsEmpty()) continue;
                    ChunkEvent?.Invoke(_chunks[chunkPosition], false);
                    _chunks.Remove(chunkPosition);
                }
            else
            {
                Debug.Log("Diagonal");
            }

            return keyValuePair.Value;
        }

        public void StartChunk(Vector3 position)
        {
            foreach (var kvp in _chunks)
            {
                ChunkEvent?.Invoke(_chunks[kvp.Key], false);
            }

            _chunks = new Dictionary<Vector3, Chunk> {{position, new Chunk(position, chunkSize)}};
        }
    }

    [Serializable]
    public class Chunk
    {
        [SerializeField] private Bounds bounds;
        [SerializeField] private List<ChunkController> chunkControllers = new List<ChunkController>();

        public Chunk(Vector3 position, float size)
        {
            bounds = new Bounds(position, Vector3.one * size);
        }

        public Vector3 RandomPointInBounds() =>
            new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

        public Bounds GetBounds() => bounds;

        public void AddChunkController(ChunkController chunkController)
        {
            if (chunkControllers.Contains(chunkController)) return;
            chunkControllers.Add(chunkController);
        }

        public void RemoveChunkController(ChunkController chunkController) => chunkControllers.Remove(chunkController);
        public bool IsEmpty() => chunkControllers.Count == 0;
    }
}