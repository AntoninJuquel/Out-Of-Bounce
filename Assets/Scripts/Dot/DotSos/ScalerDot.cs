using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New scaler dot", menuName = "Dots/Scaler", order = 0)]
    public class ScalerDot : DotSo
    {
        [SerializeField] private float scaleAmount = 2;

        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            base.Bounce(ball, dot, bouncyness);

            ball.transform.localScale = Vector3.one * scaleAmount;
        }
    }
}