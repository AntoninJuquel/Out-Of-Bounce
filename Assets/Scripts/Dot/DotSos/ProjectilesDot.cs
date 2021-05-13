using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New projectiles dot", menuName = "Dots/Projectiles", order = 0)]
    public class ProjectilesDot : DotSo
    {
        [SerializeField] private GameObject projectileGo;
        [SerializeField] private Sprite projectileSprite;
        [SerializeField] private float projectileSpeed = 7f, projectileDuration = 2f;
        private int projectileNumber => level + 3;

        private const float Radius = 1f;

        public override void Destroy(GameObject dot)
        {
            var angleStep = 360f / projectileNumber;
            var angle = 0f;
            var startPoint = dot.transform.position;

            for (var i = 0; i < projectileNumber; i++)
            {
                var projectileDirXPos = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180f) * Radius;
                var projectileDirYPos = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180f) * Radius;

                var projectileVector = new Vector3(projectileDirXPos, projectileDirYPos);
                var projectileMoveDirection = (projectileVector - startPoint).normalized * projectileSpeed;

                var projectile = Instantiate(projectileGo, startPoint, Quaternion.identity);
                var projectileRb = projectile.GetComponent<Rigidbody2D>();
                var projectileSr = projectile.GetComponent<SpriteRenderer>();
                var projectileTrail = projectile.GetComponent<TrailRenderer>();

                projectile.transform.right = projectileMoveDirection;
                projectileRb.velocity = projectileMoveDirection;
                projectileSr.sprite = projectileSprite;
                projectileSr.color = color;
                projectileTrail.startColor = color;

                angle += angleStep;

                Destroy(projectile, projectileDuration);
            }

            base.Destroy(dot);
        }
    }
}