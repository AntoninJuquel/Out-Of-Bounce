using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New stop dot", menuName = "Dots/Stop", order = 0)]
    public class StopDot : DotSo
    {
        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            base.Bounce(ball, dot, 0);
        }
    }
}