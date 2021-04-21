using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Systems.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        [SerializeField] private Sound[] sounds;

        private void Awake()
        {
            Instance = this;
            foreach (var s in sounds)
            {
                foreach (var a in s.clips)
                {
                    s.sources.Add(gameObject.AddComponent<AudioSource>());
                    s.sources[s.sources.Count - 1].clip = a;
                    s.sources[s.sources.Count - 1].volume = s.volume;
                    s.sources[s.sources.Count - 1].pitch = s.pitch;
                    s.sources[s.sources.Count - 1].loop = s.loop;
                    s.sources[s.sources.Count - 1].spatialBlend = s.spatialBlend;
                    s.sources[s.sources.Count - 1].outputAudioMixerGroup = s.outputAudioMixerGroup;
                }
            }
        }

        private Sound FindSoundByName(string soundName)
        {
            var s = Array.Find(sounds, sound => sound.name == soundName);
            if (s != null) return s;
            Debug.LogWarning("Sound " + soundName + " not found");
            return null;
        }

        private void Play(Sound sound, int index)
        {
            sound.sources[index].Play();
            sound.index = index;
        }

        public void Play(string soundName)
        {
            var s = FindSoundByName(soundName);
            var index = UnityEngine.Random.Range(0, s.sources.Count);
            Play(s, index);
        }

        public void Play(string soundName, int index)
        {
            var s = FindSoundByName(soundName);
            index %= s.sources.Count;
            Play(s, index);
        }

        public void Stop(string soundName)
        {
            var s = Array.Find(sounds, sound => sound.name == soundName);
            if (s == null)
            {
                Debug.LogWarning("Sound " + soundName + " not found");
                return;
            }

            foreach (var source in s.sources.Where(source => source))
            {
                source.Stop();
            }
        }

        public void Pause(string soundName)
        {
            var s = Array.Find(sounds, sound => sound.name == soundName);
            if (s == null)
            {
                Debug.LogWarning("Sound " + soundName + " not found");
                return;
            }

            foreach (var source in s.sources)
            {
                source.Pause();
            }
        }
    }

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip[] clips;
        public AudioMixerGroup outputAudioMixerGroup;

        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 1f)] public float spatialBlend;
        [Range(.1f, 3f)] public float pitch = 1f;
        public bool loop;
        [HideInInspector] public List<AudioSource> sources;
        public int index;
    }
}