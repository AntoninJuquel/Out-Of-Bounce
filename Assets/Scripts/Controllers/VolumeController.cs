using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Controllers
{
    public class VolumeController : MonoBehaviour
    {
        [SerializeField] private float colorFadeInTime = .1f, colorFadeOutTime = .1f, colorFadeDelay;
        private Volume _volume;
        private Bloom _bloom;
        private Vignette _vignette;
        private ChromaticAberration _chromaticAberration;
        private Color _startColor;

        private void Awake()
        {
            _volume = GetComponent<Volume>();
            _volume.profile.TryGet(out _bloom);
            _volume.profile.TryGet(out _vignette);
            _volume.profile.TryGet(out _chromaticAberration);

            _startColor = _bloom.tint.value;
            
            _bloom.active = PlayerPrefs.GetInt("Bloom", 1) == 1;
            _bloom.intensity.Override(PlayerPrefs.GetFloat("BloomIntensity", 1) * 5f);
        }

        private IEnumerator ColorLerp(Color startColor, Color endColor, float time, bool pingPong)
        {
            for (var t = 0f; t < colorFadeInTime; t += Time.deltaTime)
            {
                _bloom.tint.Override(Color.Lerp(startColor, endColor, t / time));
                yield return null;
            }

            if (!pingPong) yield break;
            yield return new WaitForSeconds(colorFadeDelay);
            StartCoroutine(ColorLerp(_bloom.tint.value, _startColor, colorFadeOutTime, false));
        }

        public void SetBloomTint(Color endColor)
        {
            StopCoroutine(nameof(ColorLerp));
            StartCoroutine(ColorLerp(_bloom.tint.value, endColor, colorFadeInTime, true));
        }

        public void EnableBloom(bool enable)
        {
            PlayerPrefs.SetInt("Bloom", enable ? 1 : 0);
            _bloom.active = enable;
        }

        public void SetBloom(float value)
        {
            PlayerPrefs.SetFloat("BloomIntensity", value);
            _bloom.intensity.Override(value * 5f);
        }
    }
}