using Doozy.Runtime.UIManager.Components;
using MoreMountains.Tools;
using UnityEngine;

namespace Audio
{
    public class AudioTrackLoader : MonoBehaviour
    {
        const float MUTED_VOLUME = 0.0001f;

        [SerializeField] private UIToggle toggle;
        [SerializeField] private MMSoundManager.MMSoundManagerTracks tracks;
        private MMSoundManager _soundManager;

        private void Start()
        {
            _soundManager = MMSoundManager.Current;
            var volume = _soundManager.GetTrackVolume(tracks, false);
            var muted = Mathf.Approximately(volume, MUTED_VOLUME);
            toggle.isOn = !muted;
        }
    }
}