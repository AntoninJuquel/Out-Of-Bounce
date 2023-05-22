using UnityEngine;

namespace Utilities.Behaviours
{
    public class RigidbodyTimeScale : MonoBehaviour
    {
        [SerializeField] private float slowDownFactor = 0.99f;
        [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Force;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            //_rigidbody2D.AddForce(-_rigidbody2D.velocity * slowDownFactor, forceMode2D);
            _rigidbody2D.position = (_rigidbody2D.position - _rigidbody2D.velocity * Time.fixedDeltaTime) +
                                    _rigidbody2D.velocity * (slowDownFactor * Time.fixedDeltaTime);
        }
    }
}