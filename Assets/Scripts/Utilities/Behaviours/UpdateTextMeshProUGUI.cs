using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Utilities.Behaviours
{
    public class UpdateTextMeshProUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private Ease ease;
        [SerializeField] private float duration = .5f;
        [SerializeField] private string prefix = "", suffix = "", format = "";

        private int _intCount;
        private float _floatCount;
        private Tweener _tween;

        private bool TweenActive => _tween != null && _tween.IsActive();
        private bool NoTween => ease is Ease.Unset or Ease.INTERNAL_Zero or Ease.INTERNAL_Custom || duration == 0;

        [Button]
        public void UpdateText(string text)
        {
            textMeshProUGUI.text = $"{prefix}{text}{suffix}";
        }

        [Button]
        public void UpdateText(float floatingPoint)
        {
            if (NoTween)
            {
                UpdateText(floatingPoint.ToString(format));
                return;
            }

            if (TweenActive)
            {
                _tween.Kill();
            }

            _tween = DOTween
                .To(() => _floatCount, x => _floatCount = x, floatingPoint, duration)
                .SetEase(ease)
                .OnUpdate(() => { UpdateText(_floatCount.ToString(format)); });
        }

        [Button]
        public void UpdateText(int integer)
        {
            UpdateText((float)integer);
        }
    }
}