using Effectors;
using UnityEngine;
using Packages.LeanTween;

namespace Dot.DotSos
{
    [CreateAssetMenu(fileName = "New gravity dot", menuName = "Dots/Gravity", order = 0)]
    public class GravityDot : DotSo
    {
        [SerializeField] private float radius, duration, force, speed;
        [SerializeField] private GameObject areaEffectPrefab;
        [SerializeField] private ParticleSystem.MinMaxGradient gradient;
        [SerializeField] private LayerMask whatIsDot;

        public override void Destroy(GameObject dot)
        {
            foreach (var col in Physics2D.OverlapCircleAll(dot.transform.position, radius, whatIsDot))
            {
                Vector2 direction = col.transform.position - dot.transform.position;
                LeanTween.cancel(col.gameObject);
                LeanTween
                    .move(col.gameObject, col.transform.position + (Vector3) direction.normalized * force, speed)
                    .setEaseOutCirc();
            }
            
            var pointEffectorController = Instantiate(areaEffectPrefab, dot.transform.position, Quaternion.identity).GetComponent<PointEffectorController>();
            var mult = force == 0 ? 0 : Mathf.Sign(force);

            pointEffectorController.SetRadius(radius);
            pointEffectorController.SetDuration(duration);
            pointEffectorController.SetColor(color);

            pointEffectorController.SetInnerRadius(mult > 0 ? radius / 2f : radius, mult != 0 ? 0 : 1);
            pointEffectorController.SetInnerSpeed(mult * 5);
            pointEffectorController.SetInnerLifeTime(radius / 10f);
            
            pointEffectorController.SetInnerColorOverLifeTime(gradient);

            pointEffectorController.SetOnTriggerEnterAction((other) =>
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 0;
            });
            pointEffectorController.SetOnTriggerExitAction((other) =>
            {
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 1;
            });

            pointEffectorController.SetForce(force);
            
            base.Destroy(dot);
        }
    }
}