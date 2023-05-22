using UnityEngine;

namespace PowerUp.UpgradeSos
{
    [CreateAssetMenu(fileName = "New attract platform power-up", menuName = "PowerUps/Attract Platform", order = 0)]
    public class GravityPlatformPowerUpItem : PowerUpItem
    {
        [SerializeField] private float force, radiusMultiplier = 1f;
        [SerializeField] private LayerMask whatIsBall;
        [SerializeField] private GameObject attractParticles;

        public override void TriggerPowerUp(GameObject gameObject)
        {
            var points = gameObject.GetComponent<EdgeCollider2D>().points;
            var center = (points[0] + points[1]) / 2f;
            var radius = Vector2.Distance(points[0], points[1]) * radiusMultiplier;

            var ps = Instantiate(attractParticles, center, Quaternion.identity).GetComponent<ParticleSystem>();
            var shape = ps.shape;
            shape.radius = radius;

            var ballHits = Physics2D.CircleCastAll(center, radius, Vector2.zero, 0, whatIsBall);

            foreach (var hit in ballHits)
            {
                var hitRigidbody = hit.rigidbody;
                var hitPosition = hitRigidbody.position;
                var direction = (Vector2)hitPosition - center;
                hitRigidbody.AddForce(direction.normalized * force, ForceMode2D.Impulse);
            }
        }
    }
}