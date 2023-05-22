using UnityEngine;

namespace Utilities.Behaviours
{
    public class RotateTowardVelocity : MonoBehaviour
    {
        [SerializeField] private Transform toRotate;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var velocity = _rigidbody2D.velocity;
            toRotate.right = velocity.normalized;
        }
    }
}