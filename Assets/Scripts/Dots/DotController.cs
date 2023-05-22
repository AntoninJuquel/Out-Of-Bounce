using Balls;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dot
{
    public class DotController : MonoBehaviour, IBounceBall
    {
        private DotItem _dotItem;
        private Collider2D _collider2D;
        private DotRenderer _dotRenderer;
        private bool _isDestroyed;
        public Bounds ChunkBounds { get; private set; }
        public bool IsEnemy => typeof(EnemyDotItem) == _dotItem.GetType();

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _dotRenderer = GetComponent<DotRenderer>();
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        private void OnDrawGizmos()
        {
            if (!_dotItem)
            {
                return;
            }

            _dotItem.DrawGizmos(this);
        }

        public void Setup(DotItem dotItem, Bounds chunk)
        {
            _dotItem = dotItem;
            _dotItem.Setup(gameObject, _collider2D);
            _dotRenderer.Setup(dotItem);
            ChunkBounds = chunk;
            gameObject.name = dotItem.name;
            _collider2D.enabled = true;
            _isDestroyed = false;
        }

        public void Bounce(BallController ball, float bouncyness)
        {
            _dotItem.Bounce(ball, this, bouncyness);
        }

        [Button]
        public void Destroy()
        {
            if (_isDestroyed)
            {
                return;
            }

            _isDestroyed = true;
            _dotItem.Destroy(this);
            DotManager.Instance.RemoveDot(ChunkBounds, gameObject, _dotItem.Points);
            _collider2D.enabled = false;
            gameObject.SetActive(false);
        }
    }
}