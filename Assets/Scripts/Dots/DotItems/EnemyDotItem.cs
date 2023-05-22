using Balls;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New enemy dot", menuName = "Dots/Enemy", order = 0)]
    public class EnemyDotItem : DotItem
    {
        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            ball.Die();
            Destroy(dot);
        }
    }
}