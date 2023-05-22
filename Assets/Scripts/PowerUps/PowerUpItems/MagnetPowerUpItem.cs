using UnityEngine;

namespace PowerUp.UpgradeSos
{
    [CreateAssetMenu(fileName = "New magnet power-up", menuName = "PowerUps/Magnet", order = 0)]
    public class MagnetPowerUpItem : PowerUpItem
    {
        [SerializeField] private LayerMask coinLayer;
        [SerializeField] private float radius, speed;

        public override void UpdatePowerUp(GameObject gameObject)
        {
            var position = gameObject.transform.position;
            foreach (var col in Physics2D.OverlapCircleAll(position, radius, coinLayer))
            {
                col.transform.position =
                    Vector2.MoveTowards(col.transform.position, position, speed * Time.deltaTime);
            }
        }
    }
}