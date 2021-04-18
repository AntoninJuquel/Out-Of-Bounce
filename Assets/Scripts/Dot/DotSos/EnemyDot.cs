using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New enemy dot", menuName = "Dots/Enemy", order = 0)]
    public class EnemyDot : DotSo
    {
        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            ball.SetActive(false);
        }
    }
}