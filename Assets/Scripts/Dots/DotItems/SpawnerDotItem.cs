using Balls;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New spawner dot", menuName = "Dots/Spawner", order = 0)]
    public class SpawnerDotItem : DotItem
    {
        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            base.Bounce(ball, dot, bouncyness);

            var dotTransform = dot.transform;
            var dotPosition = dotTransform.position;

            for (var i = 0; i < Level; i++)
            {
                var position = dotPosition + (Vector3)Random.insideUnitCircle * .5f * i;
                var direction = (position - dotPosition).normalized;
                var newBall = BallManager.Instance.SpawnBall(position);
                newBall.GetComponent<Rigidbody2D>().velocity = direction * 25f;
            }
        }
    }
}