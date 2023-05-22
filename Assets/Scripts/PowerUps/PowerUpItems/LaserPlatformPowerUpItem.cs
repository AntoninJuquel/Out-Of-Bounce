using Dot;
using UnityEngine;

namespace PowerUp.UpgradeSos
{
    [CreateAssetMenu(fileName = "New laser platform power-up", menuName = "PowerUps/Laser Platform", order = 0)]
    public class LaserPlatformPowerUpItem : PowerUpItem
    {
        [SerializeField] private LayerMask whatIsDot;
        [SerializeField] private GameObject laserParticles;

        public override void TriggerPowerUp(GameObject gameObject)
        {
            var points = gameObject.GetComponent<EdgeCollider2D>().points;

            var rot = Quaternion.LookRotation(Vector2.Perpendicular(points[1] - points[0]).normalized, Vector3.forward);

            var particleSystems = Instantiate(laserParticles, (points[0] + points[1]) / 2f, rot).GetComponentsInChildren<ParticleSystem>();

            foreach (var particleSystem in particleSystems)
            {
                var shape = particleSystem.shape;
                shape.radius = (points[1] - points[0]).magnitude;
            }

            var hits = Physics2D.LinecastAll(points[0], points[1], whatIsDot);
            foreach (var hit in hits)
            {
                hit.collider.gameObject.GetComponent<DotController>().Destroy();
            }
        }
    }
}