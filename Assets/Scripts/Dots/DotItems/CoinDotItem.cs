using Balls;
using UnityEngine;
using UnityEngine.Events;

namespace Dot
{
    [CreateAssetMenu(fileName = "New coin dot", menuName = "Dots/Coin", order = 0)]
    public class CoinDotItem : DotItem
    {
        [SerializeField] private UnityEvent<int> onCoinCollected;

        public override void Setup(GameObject dot, Collider2D collider2D)
        {
            base.Setup(dot, collider2D);

            collider2D.isTrigger = true;
        }

        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            dot.Destroy();
        }

        public override void Destroy(DotController dot)
        {
            onCoinCollected?.Invoke(Points);
            base.Destroy(dot);
        }
    }
}