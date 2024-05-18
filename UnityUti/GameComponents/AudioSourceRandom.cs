using PlugRMK.GenericUti;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Audio Source Random")]
    [Icon(GameComponentsIcon.UI_SOUND)]
    [RequireComponent(typeof(AudioSource))]

    public class AudioSourceRandom : MonoBehaviour
    {
        [Serializable]
        public class AudioPack
        {
            public string packName = "";

            public List<AudioClip> clips = new List<AudioClip>();

            public float volume = 1f;
            
            public float volumeRandomRange = 0f;

            public float pitch = 1f;

            public float pitchRandomRange = 0.15f;

            public float delay = 0f;

            public float delayRandomRange = 0f;

            public IEnumerator Play(AudioSource audioSource)
            {
                var _delay = Random.Range(delay - delayRandomRange, delay + delayRandomRange);
                yield return new WaitForSeconds(_delay);
                audioSource.pitch = Random.Range(pitch - pitchRandomRange, pitch + pitchRandomRange);

                if (clips.GetRandom() == null)
                    Debug.Log(audioSource.gameObject.name);
                audioSource.PlayOneShot(clips.GetRandom(), Random.Range(volume - volumeRandomRange, volume + volumeRandomRange));
            }

            public IEnumerator PlayAllClips(AudioSource audioSource)
            {
                var _delay = Random.Range(delay - delayRandomRange, delay + delayRandomRange);
                yield return new WaitForSeconds(_delay);
                foreach (var clip in clips)
                {
                    audioSource.pitch = Random.Range(pitch - pitchRandomRange, pitch + pitchRandomRange);
                    audioSource.PlayOneShot(clip, Random.Range(volume - volumeRandomRange, volume + volumeRandomRange));
                }
            }
        }

        [SerializeField, Range(0,1)]
        float playProbability = 1f;

        [SerializeField]
        List<AudioPack> audioPacks = new();

        [Header("Audio Source")]
        [SerializeField]
        AudioClip audioClip;

        [SerializeField]
        AudioMixerGroup output;

        [SerializeField]
        bool mute;

        [SerializeField]
        bool bypassEffect;

        [SerializeField]
        bool bypassListenerEffect;

        [SerializeField]
        bool bypassReverbZones;

        [SerializeField]
        bool playOnAwake = true;

        [SerializeField]
        UnityInitialMethod invokeIn = UnityInitialMethod.Awake;

        [SerializeField]  
        bool loop;

        [SerializeField]
        float loopPeriod = 3f;

        [SerializeField, Range(0,256)]
        int priority = 128;

        [SerializeField, Range(0,1)]
        float volume = 1;

        [SerializeField, Range(-3,3)]
        float pitch = 1;

        [SerializeField, Range(-1,1)]
        float stereoPan = 0;

        [SerializeField, Range(0,1)]
        float spatialBlend;

        [SerializeField, Range(0,1.1f)]
        float reverbZoneMix = 1;

        AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            SyncAudioSourceProperties();

            if (playOnAwake && invokeIn == UnityInitialMethod.Awake)
            {
                if (loop)
                    PlayLoop();
                else
                    Play();
            }
        }

        void OnEnable()
        {
            if (playOnAwake && invokeIn == UnityInitialMethod.OnEnable)
            {
                if (loop)
                    PlayLoop();
                else
                    Play();
            }
        }

        void Start()
        {
            if (playOnAwake && invokeIn == UnityInitialMethod.Start)
            {
                if (loop)
                    PlayLoop();
                else
                    Play();
            }
        }

        [ContextMenu("Sync")]
        void SyncAudioSourceProperties()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = output;

            audioSource.mute = mute;
            audioSource.bypassEffects = bypassEffect;
            audioSource.bypassListenerEffects = bypassListenerEffect;
            audioSource.bypassReverbZones = bypassReverbZones;
            audioSource.playOnAwake = playOnAwake;
            audioSource.loop = loop;

            audioSource.priority = priority;
            audioSource.volume = volume;
            audioSource.pitch= pitch;
            audioSource.panStereo = stereoPan;
            audioSource.spatialBlend = spatialBlend;
            audioSource.reverbZoneMix = reverbZoneMix;
        }

        [ContextMenu("Play")]
        public void Play()
        {
#if UNITY_EDITOR
            audioSource = GetComponent<AudioSource>();
#endif
            var probability = Random.Range(0f, 1f);
            if (probability <= playProbability)
                foreach (var pack in audioPacks)
                    StartCoroutine(pack.Play(audioSource));
        }

        public void PlayLoop()
        {
            StartCoroutine(Looping());
            IEnumerator Looping()
            {
                Play();

                var time = 0f;
                while (true)
                {
                    if (time > loopPeriod)
                    {
                        Play();
                        time = 0f;
                    }

                    time += Time.deltaTime;
                    yield return null;
                }
            }

        }

        public void PlayOneClipFromPack(string packName)
        {
            bool isPlayed = false;
            foreach (var pack in audioPacks)
            {
                if(pack.packName == packName)
                {
                    StartCoroutine(pack.Play(audioSource));
                    isPlayed = true;
                    break;
                }
            }

            if (!isPlayed) Debug.LogWarning($"Cannot find packName: {packName}");
        }

        public void PlayAllClipsFromPack(string packName)
        {
            foreach (var pack in audioPacks)
            {
                if (pack.packName == packName)
                {
                    StartCoroutine(pack.PlayAllClips(audioSource));
                    break;
                }

            }
        }

        public void Stop()
        {
            audioSource.Stop();
        }
    }
}
