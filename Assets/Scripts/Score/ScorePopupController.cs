using System;
using Packages.LeanTween;
using TMPro;
using UnityEngine;

namespace Score
{
    public class ScorePopupController : MonoBehaviour
    {
        private TextMeshPro _mainTextMesh;
        private RectTransform _rectTransform;
        private Transform _transform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _mainTextMesh = GetComponent<TextMeshPro>();
            _transform = transform;
        }

        public void Setup(string mainText)
        {
            _mainTextMesh.text = mainText;

            var startSize = Vector2.one * (mainText.Length - 1);
            LeanTween.scale(_rectTransform, startSize * 2f, .1f)
                .setIgnoreTimeScale(false)
                .setOnStart(() => transform.localScale = startSize)
                .setOnComplete(
                    () => LeanTween.scale(_rectTransform, Vector2.zero, .5f)
                        .setIgnoreTimeScale(false)
                        .setOnUpdate((float value) => _transform.position += Vector3.up * .1f)
                        .setOnComplete(() => gameObject.SetActive(false))
                );
        }
    }
}
