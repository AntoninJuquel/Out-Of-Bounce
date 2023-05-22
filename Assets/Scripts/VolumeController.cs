using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Controllers
{
    public class VolumeController : MonoBehaviour
    {
        private Volume _volume;
        public UnityEvent<bool> onFXToggle;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.enabled = Convert.ToBoolean(PlayerPrefs.GetInt("FX", 1));
            Application.targetFrameRate = 144;
        }

        private void Start()
        {
            onFXToggle.Invoke(_volume.enabled);
        }

        public void ToggleFX(bool value)
        {
            _volume.enabled = value;
            PlayerPrefs.SetInt("FX", Convert.ToInt32(value));
        }
    }
}