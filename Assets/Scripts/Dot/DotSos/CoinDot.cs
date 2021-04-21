using Systems.Audio;
using UnityEngine;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New coin dot", menuName = "Dots/Coin", order = 0)]
    public class CoinDot : DotSo
    {
        public override void Setup(GameObject dot, Collider2D collider2D)
        {
            collider2D.isTrigger = true;
        }

        public override void Bounce(GameObject ball, GameObject dot, float bouncyness)
        {
            ScoreManager.Instance.UpdateMoney(GetPoints());
            Destroy(dot);
        }

        public override void Destroy(GameObject dot)
        {
            base.Destroy(dot);
            AudioManager.Instance.Play(destroySound);
        }
    }
}