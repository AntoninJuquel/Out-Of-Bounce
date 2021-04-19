using System;
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
        private void Start()
        {
            foreach (var slider in sliders)
            {
                var value = PlayerPrefs.GetFloat(slider.name, .5f);
                slider.value = value;
                audioMixer.SetFloat(slider.name, Mathf.Log10(value) * 20f);
            }

            SetActiveCanvas(0);
        }

        public void SetActiveCanvas(int index)
        {
            for (var i = 0; i < canvasArray.Length; i++)
            {
                canvasArray[i].gameObject.SetActive(i == index);
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
    }
}