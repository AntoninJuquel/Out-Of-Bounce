using Ball;
using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New spawner dot", menuName = "Dots/Spawner", order = 0)]
    public class SpawnerDot : DotSo
    {
        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            base.Bounce(ball, dot, bouncyness);

            for (var i = 0; i < level; i++)
            {
                BallManager.Instance.SpawnBall(dot.transform.position);
            }
        }
    }
}