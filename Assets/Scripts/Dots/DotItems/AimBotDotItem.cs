using Balls;
using UnityEngine;
using Utilities.Behaviours;

namespace Dot
{
    [CreateAssetMenu(fileName = "New aim dot", menuName = "Dots/Aim", order = 0)]
    public class AimBotDotItem : DotItem
    {
        [SerializeField]
        private float[] aimSpeedPerLevel, aimRotationSpeedPerLevel, aimBotDurationPerLevel, radiusPerLevel;

        [SerializeField] protected LayerMask whatIsDot;

        private float AimSpeed =>
            aimSpeedPerLevel[Mathf.Clamp(LevelIndex, 0, aimSpeedPerLevel.Length - 1)];

        private float AimRotationSpeed =>
            aimRotationSpeedPerLevel[Mathf.Clamp(LevelIndex, 0, aimRotationSpeedPerLevel.Length - 1)];

        private float AimBotDuration =>
            aimBotDurationPerLevel[Mathf.Clamp(LevelIndex, 0, aimBotDurationPerLevel.Length - 1)];


        private float Radius => radiusPerLevel[Mathf.Clamp(LevelIndex, 0, radiusPerLevel.Length - 1)];

        public override void Setup(GameObject dot, Collider2D collider2D)
        {
            base.Setup(dot, collider2D);
            collider2D.isTrigger = true;
        }

        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            var aim = ball.gameObject.AddComponent<AimForLayer>();
            aim.Setup(AimSpeed, AimRotationSpeed, Radius, 0, 0, whatIsDot);
            Destroy(aim, AimBotDuration);
            dot.Destroy();
        }
    }
}