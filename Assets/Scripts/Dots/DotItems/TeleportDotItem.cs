using Balls;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New teleport dot", menuName = "Dots/Teleport", order = 0)]
    public class TeleportDotItem : DotItem
    {
        [SerializeField] private float[] minRangePerLevel, maxRangePerLevel;

        private float MinRange => minRangePerLevel[Mathf.Clamp(LevelIndex, 0, minRangePerLevel.Length - 1)];
        private float MaxRange => maxRangePerLevel[Mathf.Clamp(LevelIndex, 0, maxRangePerLevel.Length - 1)];
        private float Distance => Random.Range(MinRange, MaxRange);

        public override void Setup(GameObject dot, Collider2D collider2D)
        {
            base.Setup(dot, collider2D);

            collider2D.isTrigger = true;
        }

        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            Vector3 direction = ball.GetComponent<Rigidbody2D>().velocity.normalized;
            var teleportPosition = dot.transform.position + direction * Distance;
            ball.transform.position = teleportPosition;
            Instantiate(deathFeedback, teleportPosition, Quaternion.identity);
            dot.Destroy();
        }
    }
}