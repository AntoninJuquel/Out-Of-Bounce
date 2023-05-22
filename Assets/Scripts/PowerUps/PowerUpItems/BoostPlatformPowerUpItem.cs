using Effectors;
using UnityEngine;

namespace PowerUp.UpgradeSos
{
    [CreateAssetMenu(fileName = "New boost platform power-up", menuName = "PowerUps/Boost Platform", order = 0)]
    public class BoostPlatformPowerUpItem : PowerUpItem
    {
        [SerializeField] private Color color;
        [SerializeField] private float force, duration;
        [SerializeField] private GameObject areaEffectorPrefab;

        public override void TriggerPowerUp(GameObject gameObject)
        {
            var points = gameObject.GetComponent<EdgeCollider2D>().points;
            var center = (points[0] + points[1]) / 2f;
            var radius = Vector2.Distance(points[0], points[1]) / 2f;
            var right = Vector2.Perpendicular(points[1] - points[0]);
            var angle = Vector2.SignedAngle(Vector2.right, right);
            var eulerAngles = new Vector3(0, 0, angle);

            var areaEffector = Instantiate(areaEffectorPrefab, center, Quaternion.Euler(eulerAngles))
                .GetComponent<AreaEffectorController>();

            areaEffector.SetDuration(duration);
            areaEffector.SetRadius(radius);
            areaEffector.SetForce(force);
            areaEffector.SetColor(color);

            areaEffector.SetOnTriggerEnterAction((other) =>
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 0;
            });
            areaEffector.SetOnTriggerExitAction((other) =>
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 1;
            });

            if (Level != 2) return;

            var oppositeEuler = new Vector3(0, 0, angle + 180);
            var oppositeAreaEffector = Instantiate(areaEffectorPrefab, center, Quaternion.Euler(oppositeEuler))
                .GetComponent<AreaEffectorController>();

            oppositeAreaEffector.SetDuration(duration);
            oppositeAreaEffector.SetRadius(radius);
            oppositeAreaEffector.SetForce(force);
            oppositeAreaEffector.SetColor(color);

            oppositeAreaEffector.SetOnTriggerEnterAction((other) =>
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 0;
            });
            oppositeAreaEffector.SetOnTriggerExitAction((other) =>
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 1;
            });
        }
    }
}