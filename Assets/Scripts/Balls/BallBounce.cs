using UnityEngine;
using UnityEngine.Events;

namespace Balls
{
    public class BallBounce : MonoBehaviour
    {
        [SerializeField] private UnityEvent<GameObject> onBounce;
        private float _bouncyness = 15f;
        private BallController _ballController;

        private void Awake()
        {
            _ballController = GetComponent<BallController>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            other.gameObject.TryGetComponent(out IBounceBall collide);
            collide?.Bounce(_ballController, _bouncyness);
            onBounce?.Invoke(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.TryGetComponent(out IBounceBall collide);
            collide?.Bounce(_ballController, _bouncyness);
            onBounce?.Invoke(other.gameObject);
        }

        public void SetBouncyness(float bouncyness)
        {
            _bouncyness = bouncyness;
        }
    }
}