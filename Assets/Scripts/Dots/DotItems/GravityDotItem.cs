using DG.Tweening;
using Effectors;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New gravity dot", menuName = "Dots/Gravity", order = 0)]
    public class GravityDotItem : DotItem
    {
        [SerializeField] private float[]
            radiusPerLevel,
            durationPerLevel,
            pointEffectorForcePerLevel;

        [SerializeField] private GameObject areaEffectPrefab;
        [SerializeField] private ParticleSystem.MinMaxGradient gradient;
        [SerializeField] private LayerMask whatIsDot;

        private float Radius => radiusPerLevel[Mathf.Clamp(LevelIndex, 0, radiusPerLevel.Length - 1)];
        private float Duration => durationPerLevel[Mathf.Clamp(LevelIndex, 0, durationPerLevel.Length - 1)];

        private float PointEffectorForce =>
            pointEffectorForcePerLevel[Mathf.Clamp(LevelIndex, 0, pointEffectorForcePerLevel.Length - 1)];

        public override void Destroy(DotController dot)
        {
            var sign = PointEffectorForce == 0 ? 0 : Mathf.Sign(PointEffectorForce);

            if (sign != 0)
            {
                foreach (var col in Physics2D.OverlapCircleAll(dot.transform.position, Radius, whatIsDot))
                {
                    if (col.GetComponent<DotController>().IsEnemy)
                    {
                        continue;
                    }

                    var colTransform = col.transform;
                    var colPosition = colTransform.position;
                    var center = dot.transform.position;

                    var dir = (colPosition - center).normalized;
                    var distance = Vector2.Distance(colPosition, center);
                    var rest = Radius - distance;

                    var endPosition = sign > 0 ? colPosition + dir * rest : colPosition - dir * distance;

                    colTransform
                        .DOKill();
                    colTransform
                        .DOMove(endPosition, Duration)
                        .SetEase(Ease.OutCirc);
                }
            }

            var pointEffectorController = Instantiate(areaEffectPrefab, dot.transform.position, Quaternion.identity)
                .GetComponent<PointEffectorController>();

            pointEffectorController.SetRadius(Radius);
            pointEffectorController.SetDuration(Duration);
            pointEffectorController.SetColor(color);

            pointEffectorController.SetInnerRadius(sign > 0 ? Radius / 2f : Radius, sign != 0 ? 0 : 1);
            pointEffectorController.SetInnerSpeed(sign * 5);
            pointEffectorController.SetInnerLifeTime(Radius / 10f);

            pointEffectorController.SetInnerColorOverLifeTime(gradient);
            pointEffectorController.SetForce(PointEffectorForce);

            pointEffectorController.SetOnTriggerEnterAction((other) =>
            {
                if (!other) return;
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 0;
            });
            pointEffectorController.SetOnTriggerExitAction((other) =>
            {
                if (!other) return;
                var rb = other.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 1;
            });


            base.Destroy(dot);
        }
    }
}