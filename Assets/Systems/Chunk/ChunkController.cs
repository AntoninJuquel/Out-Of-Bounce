using UnityEngine;

namespace Systems.Chunk
{
    public class ChunkController : MonoBehaviour
    {
        private ChunkManager _chunkManager;
        private Chunk _chunk;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _chunk = null;
        }

        private void Start()
        {
            _chunkManager = ChunkManager.Instance;
        }

        private void Update()
        {
            if(!_chunkManager.started) return;
            var position = _transform.position;
            _chunk ??= _chunkManager.GetClosestChunk(this, position, position);
            if (_chunk.GetBounds().Contains(position)) return;
            _chunk = _chunkManager.GetClosestChunk(this, _chunk.GetBounds().center, position);
        }
    }
}