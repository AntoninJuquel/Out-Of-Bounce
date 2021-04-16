using UnityEngine;

namespace Systems.Chunk
{
    public class ChunkController : MonoBehaviour
    {
        [SerializeField] private Chunk chunk;
        private ChunkManager _chunkManager;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            chunk = null;
        }

        private void Start()
        {
            _chunkManager = ChunkManager.Instance;
        }

        private void Update()
        {
            if(!_chunkManager.started) return;
            var position = _transform.position;
            chunk ??= _chunkManager.GetClosestChunk(this, position, position);
            if (chunk.GetBounds().Contains(position)) return;
            chunk = _chunkManager.GetClosestChunk(this, chunk.GetBounds().center, position);
        }
    }
}