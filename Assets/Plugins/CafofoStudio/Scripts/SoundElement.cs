using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CafofoStudio
{
    [System.Serializable]
    public class SoundElement
    {
        [SerializeField] private string soundName;

        [SerializeField] private AudioMixerGroup overrideOutputMixer;

        [SerializeField] private float intensity = 0f;

        [SerializeField] private string maxIntensityLabel;

        [SerializeField] private string minIntensityLabel;

        [SerializeField] private float volumeMultiplier = 1f;

        [SerializeField] private List<SoundSubElementSample> sampleSubElements;

        [SerializeField] private List<SoundSubElementLoop> loopSubElements;

        /// <summary>
        /// Initialize all subelements audiosources.
        /// </summary>
        /// <param name="parent">Gameobject where all audiosources will be instantiated.</param>
        public void InitializeAudioSources(GameObject parent)
        {
            foreach (SoundSubElementLoop s in loopSubElements)
            {
                s.InitializeAudioSources(parent, overrideOutputMixer);
            }

            foreach (SoundSubElementSample s in sampleSubElements)
            {
                s.InitializeAudioSources(parent, overrideOutputMixer);
            }

            CalculateIntensity();
        }

        /// <summary>
        /// Calculates all subelement's intensities.
        /// </summary>
        private void CalculateIntensity()
        {
            foreach (SoundSubElementLoop s in loopSubElements)
            {
                s.CalculateIntensity(this.intensity, this.volumeMultiplier);
            }

            foreach (SoundSubElementSample s in sampleSubElements)
            {
                s.CalculateIntensity(this.intensity, this.volumeMultiplier);
            }
        }

        /// <summary>
        /// Update hook that must be called to control the timer.
        /// </summary>
        public void UpdateSampleTimer()
        {
            foreach (SoundSubElementSample s in sampleSubElements)
            {
                s.UpdateSampleTimer(intensity, volumeMultiplier);
            }
        }

        /// <summary>
        /// Sets the element intensity.
        /// </summary>
        /// <param name="intensity">The new intensity, from 0.0 to 1.0. Any other value will be clamped to this interval.</param>
        public void SetIntensity(float intensity)
        {
            this.intensity = Mathf.Clamp01(intensity);

            CalculateIntensity();
        }

        /// <summary>
        /// Get the element's intensity
        /// </summary>
        /// <returns>The element's current intensity, form 0.0 to 1.0.</returns>
        public float GetIntensity()
        {
            return intensity;
        }

        /// <summary>
        /// Sets the element volume multiplier.
        /// </summary>
        /// <param name="volumeMultiplier">The new volume multiplier, from 0.0 to 1.0. Any other value will be clamped to this interval.</param>
        public void SetVolumeMultiplier(float volumeMultiplier)
        {
            this.volumeMultiplier = Mathf.Clamp01(volumeMultiplier);

            //If it is playing a loop element, the audiosources output volume must be recalculated.
            foreach (SoundSubElementLoop s in loopSubElements)
            {
                s.CalculateIntensity(this.intensity, this.volumeMultiplier);
            }
        }

        /// <summary>
        /// Get the element's volume multiplier.
        /// </summary>
        /// <returns>The element's current volume multiplier, form 0.0 to 1.0.</returns>
        public float GetVolumeMultiplier()
        {
            return volumeMultiplier;
        }

        /// <summary>
        /// Sets the element output mixer group.
        /// By default, every element uses the main mixer group.
        /// </summary>
        /// <param name="overrideOutputMixer">The new mixer group</param>
        public void SetOutputMixerGroup(AudioMixerGroup overrideOutputMixer)
        {
            this.overrideOutputMixer = overrideOutputMixer;

            foreach (SoundSubElementLoop s in loopSubElements)
            {
                s.SetOutputMixerGroup(overrideOutputMixer);
            }

            foreach (SoundSubElementSample s in sampleSubElements)
            {
                s.SetOutputMixerGroup(overrideOutputMixer);
            }
        }

        /// <summary>
        /// Get the element's output mixer group.
        /// </summary>
        /// <returns></returns>
        public AudioMixerGroup GetOutputMixerGroup()
        {
            return overrideOutputMixer;
        }

        /// <summary>
        /// Starts playing the element.
        /// </summary>
        public void Play()
        {
            foreach (SoundSubElementLoop s in loopSubElements)
            {
                s.Play();
            }

            foreach (SoundSubElementSample s in sampleSubElements)
            {
                s.Play();
            }
        }

        /// <summary>
        /// Stops playing the element.
        /// </summary>
        public void Stop()
        {
            foreach (SoundSubElementLoop s in loopSubElements)
            {
                s.Stop();
            }

            foreach (SoundSubElementSample s in sampleSubElements)
            {
                s.Stop();
            }
        }
    }
}