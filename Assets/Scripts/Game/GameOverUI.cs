using System;
using DG.Tweening;
using ReferenceSharing;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText, scoreText, moneyText, maxHeightText;
    [SerializeField] private Variable<int> score, money;
    [SerializeField] private Variable<float> timer, maxHeight;
    [SerializeField] private float delay = 1f;
    private Sequence _sequence;

    private void OnEnable()
    {
        if (_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
        }

        timerText.text = TimeSpan.Zero.ToString(@"mm\M\,ss\S");
        scoreText.text = "0";
        moneyText.text = "0";
        maxHeightText.text = "0m";

        _sequence = DOTween.Sequence()
            .Append(DOTween.To(() => 0, x =>
                {
                    var timeSpan = TimeSpan.FromSeconds(x);
                    var text = "";
                    if (timeSpan.Minutes > 0)
                    {
                        text += $"{timeSpan.Minutes.ToString()}M, ";
                    }

                    text += $"{timeSpan.Seconds.ToString()}S";

                    timerText.text = text;
                },
                timer.Value, delay).SetEase(Ease.OutCubic))
            .Append(DOTween.To(() => 0, x => scoreText.text = x.ToString(), score.Value, delay)
                .SetEase(Ease.OutCubic))
            .Append(DOTween.To(() => 0, x => moneyText.text = x.ToString(), money.Value, delay)
                .SetEase(Ease.OutCubic))
            .Append(DOTween.To(() => 0, x => maxHeightText.text = $"{x:F0}m", maxHeight.Value, delay)
                .SetEase(Ease.OutCubic));
    }
}