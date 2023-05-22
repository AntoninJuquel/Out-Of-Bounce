using System;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    public class AudioSwitcher : MonoBehaviour
    {
        private MMF_Player _audioSwitcher;
        private MMF_MMSoundManagerSoundFade _from, _to;
        private static AudioId _currentAudioId;

        private void Awake()
        {
            _audioSwitcher = GetComponent<MMF_Player>();
            _from = _audioSwitcher.GetFeedbackOfType<MMF_MMSoundManagerSoundFade>("From");
            _to = _audioSwitcher.GetFeedbackOfType<MMF_MMSoundManagerSoundFade>("To");
        }

        [Button]
        public void SwitchAudio(AudioId to)
        {
            if (to == _currentAudioId)
            {
                return;
            }

            _from.SoundID = (int)_currentAudioId;
            _to.SoundID = (int)to;
            _audioSwitcher.PlayFeedbacks();
            _currentAudioId = to;
        }

        [Button]
        public void SwitchAudio(string to)
        {
            var result = Enum.TryParse<AudioId>(to, true, out var audioId);

            if (!result)
            {
                Debug.LogError($"AudioId {to} is not defined");
                return;
            }

            SwitchAudio(audioId);
        }

        [Button]
        public void SwitchAudio(int to)
        {
            if (!Enum.IsDefined(typeof(AudioId), to))
            {
                Debug.LogError($"AudioId {to} is not defined");
                return;
            }

            SwitchAudio((AudioId)to);
        }
    }
}