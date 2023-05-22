using Balls;
using DG.Tweening;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New scaler dot", menuName = "Dots/Scaler", order = 0)]
    public class ScalerDotItem : DotItem
    {
        [SerializeField] private float[] scaleAmountPerLevel, scaleDurationPerLevel;
        private float ScaleAmount => scaleAmountPerLevel[Mathf.Clamp(LevelIndex, 0, scaleAmountPerLevel.Length - 1)];

        private float ScaleDuration =>
            scaleDurationPerLevel[Mathf.Clamp(LevelIndex, 0, scaleDurationPerLevel.Length - 1)] * .5f;

        public override void Bounce(BallController ball, DotController dot, float bouncyness)
        {
            base.Bounce(ball, dot, bouncyness);
            var currentScale = ball.transform.localScale;
            var sequence = DOTween.Sequence();
            sequence
                .Append(ball.transform.DOScale(currentScale * ScaleAmount, ScaleDuration))
                .Append(ball.transform.DOScale(Vector3.one, ScaleDuration))
                .SetId(ball.transform);
            sequence.Play();
        }
    }
}