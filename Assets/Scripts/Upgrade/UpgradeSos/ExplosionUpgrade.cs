using Dot;
using UnityEngine;

namespace Upgrade.UpgradeSos
{
    [CreateAssetMenu(fileName = "New explosion upgrade", menuName = "Upgrades/Explosion", order = 0)]
    public class ExplosionUpgrade : UpgradeSo
    {
        [SerializeField] private float radius;
        public override void OnDisableUpgrade(GameObject gameObject)
        {
            foreach (var col in Physics2D.OverlapCircleAll(gameObject.transform.position, radius))
            {
                var dot = col.GetComponent<DotController>();
                if (dot)
                {
                    dot.Destroy();
                }
            }
        }
    }
}