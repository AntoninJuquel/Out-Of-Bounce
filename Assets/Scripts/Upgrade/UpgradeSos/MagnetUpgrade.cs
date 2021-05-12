using Dot;
using Managers;
using UnityEngine;

namespace Upgrade.UpgradeSos
{
    [CreateAssetMenu(fileName = "New magnet upgrade", menuName = "Upgrades/Magnet", order = 0)]
    public class MagnetUpgrade : UpgradeSo
    {
        [SerializeField] private LayerMask dotLayer;
        [SerializeField] private float radius, speed;

        public override void UpdateUpgrade(GameObject gameObject)
        {
            if(GameManager.GameStatus != GameStatus.Playing) return;
            var position = gameObject.transform.position;
            foreach (var col in Physics2D.OverlapCircleAll(position, radius, dotLayer))
            {
                if (col.CompareTag("Coin"))
                    col.transform.position = Vector2.MoveTowards(col.transform.position, position, speed * Time.deltaTime);
            }
        }
    }
}