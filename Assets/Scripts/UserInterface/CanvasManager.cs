using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterface
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider[] sliders;
        [SerializeField] private Canvas[] canvasArray;
        [SerializeField] private TextMeshProUGUI heightText, scoreText;

        private void Start()
        {
            foreach (var slider in sliders)
            {
                var value = PlayerPrefs.GetFloat(slider.name, .5f);
                slider.value = value;
                audioMixer.SetFloat(slider.name, Mathf.Log10(value) * 20f);
            }

            SetActiveCanvas(canvasArray[0]);
        }

        public void SetActiveCanvas(Canvas target)
        {
            foreach (var canvas in canvasArray)
            {
                canvas.gameObject.SetActive(target == canvas);
            }
        }

        public void OpenUrl(string url) => Application.OpenURL(url);

        public void SetVolume(float value)
        {
            if (!EventSystem.current.currentSelectedGameObject) return;
            var sliderName = EventSystem.current.currentSelectedGameObject.name;
            PlayerPrefs.SetFloat(sliderName, value);
            audioMixer.SetFloat(sliderName, Mathf.Log10(value) * 20f);
        }

        public void SetHeightText(float value)
        {
            value = Mathf.Max(0, value);
            heightText.text = string.Concat(value.ToString("000.00"), "m");
        }

        public void SetScoreText()
        {
        }
    }
}