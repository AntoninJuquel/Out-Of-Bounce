using Balls;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New stop dot", menuName = "Dots/Stop", order = 0)]
    public class StopDotItem : DotItem
    {
        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            base.Bounce(ball, dot, 0);
        }
    }
}