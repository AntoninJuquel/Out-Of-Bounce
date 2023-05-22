using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New projectiles dot", menuName = "Dots/Projectiles", order = 0)]
    public class ProjectilesDotItem : DotItem
    {
        [SerializeField] private GameObject projectileGo;
        [SerializeField] private Sprite projectileSprite;

        [SerializeField] private float[] projectileSpeedPerLevel, projectileDurationPerLevel, projectileNumberPerLevel;

        private int ProjectileNumber =>
            Mathf.CeilToInt(projectileNumberPerLevel[Mathf.Clamp(LevelIndex, 0, projectileNumberPerLevel.Length - 1)]);

        private float ProjectileSpeed =>
            projectileSpeedPerLevel[Mathf.Clamp(LevelIndex, 0, projectileSpeedPerLevel.Length - 1)];

        private float ProjectileDuration =>
            projectileDurationPerLevel[Mathf.Clamp(LevelIndex, 0, projectileDurationPerLevel.Length - 1)];

        private const float Radius = 1f;

        public override void Destroy(DotController dot)
        {
            var angleStep = 360f / ProjectileNumber;
            var angle = 0f;
            var startPoint = dot.transform.position;

            for (var i = 0; i < ProjectileNumber; i++)
            {
                var projectileDirXPos = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180f) * Radius;
                var projectileDirYPos = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180f) * Radius;

                var projectileVector = new Vector3(projectileDirXPos, projectileDirYPos);
                var projectileMoveDirection = (projectileVector - startPoint).normalized * ProjectileSpeed;

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

                Destroy(projectile, ProjectileDuration);
            }

            base.Destroy(dot);
        }
    }
}