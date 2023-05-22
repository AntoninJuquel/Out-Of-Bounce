using DG.Tweening;
using UnityEngine;

namespace Dot
{
    [CreateAssetMenu(fileName = "New time slower dot", menuName = "Dots/Time slower", order = 0)]
    public class TimeSlowerDotItem : DotItem
    {
        [SerializeField] private float[] slowAmountPerLevel, slowDurationPerLevel;
        private static Sequence _slowSequence;
        private float SlowAmount => slowAmountPerLevel[Mathf.Clamp(LevelIndex, 0, slowAmountPerLevel.Length - 1)];

        private float SlowDuration =>
            slowDurationPerLevel[Mathf.Clamp(LevelIndex, 0, slowDurationPerLevel.Length - 1)] * .5f;

        public override void Destroy(DotController dot)
        {
            if (_slowSequence != null && _slowSequence.IsActive())
            {
                _slowSequence.Kill();
            }


            _slowSequence = DOTween.Sequence();
            _slowSequence
                .Append(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, SlowAmount, SlowDuration))
                .Append(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, SlowDuration));
            _slowSequence.Play();

            base.Destroy(dot);
        }
    }
}