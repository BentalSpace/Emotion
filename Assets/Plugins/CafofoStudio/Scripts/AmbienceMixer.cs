using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CafofoStudio
{
	public abstract class AmbienceMixer<P> : MonoBehaviour where P:AmbientPreset
    {
        public bool playOnAwake  = true;

		[SerializeField]
		public List<P> presets;

        [SerializeField]
		protected abstract List<SoundElement> elements { get; }

		void OnEnable()
		{
			foreach (SoundElement element in elements)
			{
				element.InitializeAudioSources(gameObject);
			}

			if (playOnAwake)
			{
				Play();
			}
		}

        void Update()
        {
            foreach (SoundElement element in elements)
            {
                element.UpdateSampleTimer();
            }
        }
	

        /**
         * Starts playing the soundscape.
         */
        public void Play()
        {
            foreach (SoundElement element in elements)
            {
                element.Play();
            }
        }

        /**
         * Stops playing the soundscape immediately.
         */
        public void Stop()
        {
            foreach (SoundElement element in elements)
            {
                element.Stop();
            }
        }
			
		void OnDisable()
		{
			AudioSource[] existingSources = GetComponents<AudioSource> ();
			foreach (AudioSource src in existingSources) {
				Destroy (src);
			}
		}

		abstract public void ApplyPreset (P selectedPreset);
    }
}