using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.ChunkSystem
{
    public class ChunkController : MonoBehaviour
    {
        [SerializeField] private Chunk chunk;
        private ChunkManager _chunkManager;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            _chunkManager = ChunkManager.Instance;
            chunk = _chunkManager.GetClosestChunk(this, Vector3.zero, _transform.position);
        }

        private void Update()
        {
            if (chunk == null) return;
            var position = _transform.position;
            if (chunk.GetBounds().Contains(position)) return;
            chunk = _chunkManager.GetClosestChunk(this, chunk.GetBounds().center, position);
        }
    }
}