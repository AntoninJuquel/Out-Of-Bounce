using Systems.SpaceTime;
using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New rewind dot", menuName = "Dots/Rewind", order = 0)]
    public class RewindDot : DotSo
    {
        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            base.Bounce(ball, dot, 0);
            ball.GetComponent<TimeBody>().StartRewind();
        }
    }
}