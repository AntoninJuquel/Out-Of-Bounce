using UnityEngine;

namespace Utilities.Behaviours
{
    public class AimForLayer : MonoBehaviour
    {
        [SerializeField] private float radius;

        [Header("MOVEMENT")] [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;

        [Header("DEVIATION")] [SerializeField] private float deviationAmount = 50;
        [SerializeField] private float deviationSpeed = 2;

        [SerializeField] private LayerMask layer;

        private Transform _target;
        private Rigidbody2D _rigidbody2D;

        private bool HasTarget => _target && _target.gameObject.activeSelf;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            layer = LayerMask.GetMask("Dot");
        }

        public void Setup(float mSpeed, float mRotationSpeed, float mRadius, float mDeviationAmount,
            float mDeviationSpeed, LayerMask mLayer)
        {
            speed = mSpeed;
            rotationSpeed = mRotationSpeed;
            radius = mRadius;
            deviationAmount = mDeviationAmount;
            deviationSpeed = mDeviationSpeed;
            layer = mLayer;
        }

        private void Update()
        {
            if (!HasTarget)
            {
                GetClosest();
            }
        }

        private void FixedUpdate()
        {
            if (HasTarget)
            {
                Aim();
            }
            else
            {
                Roam();
            }
        }

        private void GetClosest()
        {
            if (layer == default)
            {
                return;
            }

            var colliders = Physics2D.OverlapCircleAll(transform.position, radius, layer);

            if (colliders.Length == 0)
            {
                return;
            }

            var closestDot = colliders[0].transform;
            var closestDistance = Vector2.Distance(transform.position, closestDot.position);
            foreach (var col in colliders)
            {
                var distance = Vector2.Distance(transform.position, col.transform.position);

                if (distance >= closestDistance)
                {
                    continue;
                }

                closestDistance = distance;
                closestDot = col.transform;
            }

            _target = closestDot;
        }

        private Vector2 AddDeviation(Vector2 direction)
        {
            var perpendicular = Vector2.Perpendicular(direction);
            var sin = Mathf.Sin(Time.time * deviationSpeed);

            var deviation = perpendicular * (deviationAmount * sin);

            var velocity = _rigidbody2D.velocity.normalized;
            var deviationDirection = (direction + deviation).normalized;
            var distanceDelta = rotationSpeed * Time.deltaTime;

            var velocityDirection = Vector2.MoveTowards(velocity, deviationDirection, distanceDelta);

            return velocityDirection;
        }

        private void Aim()
        {
            var direction = (Vector2)(_target.position - transform.position).normalized;
            var velocityDirection = AddDeviation(direction);
            _rigidbody2D.velocity = velocityDirection * speed;
        }

        private void Roam()
        {
            _rigidbody2D.velocity = AddDeviation(transform.right) * speed;
        }
    }
}