using Balls;
using UnityEngine;
using Utilities.Behaviours;

namespace Dot
{
    [CreateAssetMenu(fileName = "New rewind dot", menuName = "Dots/Rewind", order = 0)]
    public class RewindDotItem : DotItem
    {
        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            base.Bounce(ball, dot, 0);
            ball.GetComponent<RigidbodyRecorder>().StartRewind();
        }
    }
}