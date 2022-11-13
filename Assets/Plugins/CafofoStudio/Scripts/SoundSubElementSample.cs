using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CafofoStudio
{
    [System.Serializable]
    public class SoundSubElementSample : ISoundSubElement
    {
        [SerializeField] private List<AudioClip> audioClips;

        [Tooltip("The least seconds between sounds at the lowest intensity.")]
        public float lowIntensityMinSeconds;

        [Tooltip("The most seconds between sounds at the lowest intensity.")]
        public float lowIntensityMaxSeconds;

        [Tooltip("The least seconds between sounds at the highest intensity.")]
        public float highIntensityMinSeconds;

        [Tooltip("The most seconds between sounds at the highest intensity.")]
        public float highIntensityMaxSeconds;

        private float nextSampleCountdown;

        private List<int> availableSoundIndexes = new List<int>();

        /// <summary>
        /// Holds all audio sources for this element.
        /// The playback controler may create more audiosources if it seems fit.
        /// </summary>
        private List<AudioSource> audioSourcePool;

        /// <summary>
        /// Informs whether the audiosource is playing or not. Important for SAMPLE types.
        /// </summary>
        private bool isPlaying = false;

        private GameObject mParentGO;
        private AudioMixerGroup mOutputMixer;

      
        /// <summary>
        /// Initializes the AudioSource pool and saves the parent Gameobjet, to instantiate future AudioSources.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="outputMixer"></param>
        public void InitializeAudioSources(GameObject parent, AudioMixerGroup outputMixer)
        {
            mParentGO = parent;
            mOutputMixer = outputMixer;
            audioSourcePool = new List<AudioSource>();
        }
        

        /// <summary>
        /// Calculates how frequently the randomized samples should play based on the current intensity.
        /// </summary>
        /// <param name="intensity">The intensity, from 0.0 to 1.0.</param>
        /// <param name="volumeMultiplier">The volume multiplier, from 0.0 to 1.0.</param>
        public void CalculateIntensity(float intensity, float volumeMultiplier)
        {
            //First, find the range to randomize.
            float fMin = Mathf.Lerp(lowIntensityMinSeconds, highIntensityMinSeconds, intensity);
            float fMax = Mathf.Lerp(lowIntensityMaxSeconds, highIntensityMaxSeconds, intensity);
            if(fMin > fMax)
            {
                float temp = fMin;
                fMin = fMax;
                fMax = temp;
            }
            float currentDelay = Random.Range(fMin, fMax);
            nextSampleCountdown = currentDelay;
        }

        /// <summary>
        /// Checks if it is time to play a sample.
        /// </summary>
        public void UpdateSampleTimer(float intensity, float volumeMultiplier)
        {
            if (isPlaying && intensity > 0)
            {
                nextSampleCountdown -= Time.deltaTime;

                if (nextSampleCountdown <= 0)
                {
                    PlayAnySample(volumeMultiplier);
                    CalculateIntensity(intensity, volumeMultiplier);
                }
            }
        }

        /// <summary>
        /// Randomly selects a sample and plays it.
        /// </summary>
        /// <param name="volumeMultiplier">The volume multiplier for the played sample.</param>
        private void PlayAnySample(float volumeMultiplier)
        {
            //Requests an AudioSource from the pool.
            AudioSource tempSource = GetAudioSource();
            tempSource.panStereo = Random.Range(-1f, 1f);

            //Picks a random sample to play and randomizes a volume from 0.2 to 1
            tempSource.clip = audioClips[GetRandomSoundIndex()];
            tempSource.volume = Random.Range(.5f, 1f) * volumeMultiplier;
            tempSource.pitch = Random.Range(.8f, 1.2f);

            tempSource.Play();
        }

        /// <summary>
        /// Randomly selects a sample index.
        /// </summary>
        /// <returns>The next sample index to be played.</returns>
        private int GetRandomSoundIndex()
        {
            if(availableSoundIndexes.Count == 0)
            {
                for(int i = 0; i < audioClips.Count; i++)
                {
                    availableSoundIndexes.Add(i);
                }
            }

            int rnd = Random.Range(0, availableSoundIndexes.Count);
            int nextIndex = availableSoundIndexes[rnd];
            availableSoundIndexes.RemoveAt(rnd);
            return nextIndex;
        }

        /// <summary>
        /// Tries to find or create a new audio source that will be used to play a sample.
        /// </summary>
        /// <returns>An AudioSource that is currently not playing.</returns>
        private AudioSource GetAudioSource()
        {
            AudioSource newSource;

            int index = 0;
            foreach (AudioSource audioSource in audioSourcePool)
            {
                if (!audioSource.isPlaying)
                {
                    //Moves the audiosource to the end of the list.
                    audioSourcePool.RemoveAt(index);
                    audioSourcePool.Add(audioSource);
                    return audioSource;
                }
                index++;
            }

            //If no idle audiosource was found, creates a new one.
            newSource = mParentGO.AddComponent<AudioSource>();
            newSource.outputAudioMixerGroup = mOutputMixer;
            newSource.playOnAwake = false;
            audioSourcePool.Add(newSource);

            return newSource;
        }

        public void SetOutputMixerGroup(AudioMixerGroup overrideOutputMixer)
        {
            mOutputMixer = overrideOutputMixer;

            foreach (AudioSource source in audioSourcePool)
            {
                source.outputAudioMixerGroup = overrideOutputMixer;
            }
        }

        public void Play()
        {
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
        }
    }
}
