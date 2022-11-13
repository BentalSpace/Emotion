using UnityEngine;
using UnityEngine.Audio;

namespace CafofoStudio
{
    public interface ISoundSubElement
    {
        void InitializeAudioSources(GameObject parent, AudioMixerGroup outputMixer);
        void CalculateIntensity(float intensity, float volumeMultiplier);
        void SetOutputMixerGroup(AudioMixerGroup overrideOutputMixer);
        void Play();
        void Stop();
    }
}