using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CafofoStudio
{
	public class CaveAmbientMixer : AmbienceMixer<CaveAmbientPreset>
    {

		[SerializeField] private SoundElement _atmosphere1;
		public SoundElement Atmosphere1
		{
			get { return _atmosphere1; }
			private set { _atmosphere1 = Atmosphere1; }
		}

		[SerializeField] private SoundElement _atmosphere2;
		public SoundElement Atmosphere2
		{
			get { return _atmosphere2; }
			private set { _atmosphere2 = Atmosphere2; }
		}

		[SerializeField] private SoundElement _atmosphere3;
		public SoundElement Atmosphere3
		{
			get { return _atmosphere3; }
			private set { _atmosphere3 = Atmosphere3; }
		}

		[SerializeField] private SoundElement _sediment;
		public SoundElement Sediment
		{
			get { return _sediment; }
			private set { _sediment = Sediment; }
		}

		[SerializeField] private SoundElement _waterDrops;
		public SoundElement WaterDrops
		{
			get { return _waterDrops; }
			private set { _waterDrops = WaterDrops; }
		}

		[SerializeField] private SoundElement _waterStream;
		public SoundElement WaterStream
		{
			get { return _waterStream; }
			private set { _waterStream = WaterStream; }
		}

		[SerializeField] private SoundElement _sewer;
		public SoundElement Sewer
		{
			get { return _sewer; }
			private set { _sewer = Sewer; }
		}

		[SerializeField] private SoundElement _fire;
		public SoundElement Fire
		{
			get { return _fire; }
			private set { _fire = Fire; }
		}

		[SerializeField] private SoundElement _critters;
		public SoundElement Critters
		{
			get { return _critters; }
			private set { _critters = Critters; }
		}

		protected override List<SoundElement> elements {
			get {
				return new List<SoundElement>() { _atmosphere1, _atmosphere2, _atmosphere3, _sediment, _waterDrops, _waterStream, _sewer, _fire, _critters};
			}
		}

		override public void ApplyPreset(CaveAmbientPreset selectedPreset) 
		{
			_atmosphere1.SetIntensity (selectedPreset.atmosphere1Intensity);
			_atmosphere1.SetVolumeMultiplier (selectedPreset.atmosphere1VolumeMultiplier);

			_atmosphere2.SetIntensity (selectedPreset.atmosphere2Intensity);
			_atmosphere2.SetVolumeMultiplier (selectedPreset.atmosphere2VolumeMultiplier);

			_atmosphere3.SetIntensity (selectedPreset.atmosphere3Intensity);
			_atmosphere3.SetVolumeMultiplier (selectedPreset.atmosphere3VolumeMultiplier);

			_sediment.SetIntensity(selectedPreset.sedimentIntensity);
			_sediment.SetVolumeMultiplier(selectedPreset.sedimentVolumeMultiplier);

			_waterDrops.SetIntensity (selectedPreset.waterDropsIntensity);
			_waterDrops.SetVolumeMultiplier (selectedPreset.waterDropsVolumeMultiplier);

			_waterStream.SetIntensity(selectedPreset.waterDropsIntensity);
			_waterStream.SetVolumeMultiplier(selectedPreset.waterDropsVolumeMultiplier);

			_sewer.SetIntensity(selectedPreset.sewerIntensity);
			_sewer.SetVolumeMultiplier(selectedPreset.sewerVolumeMultiplier);

			_fire.SetIntensity(selectedPreset.fireIntensity);
			_fire.SetVolumeMultiplier(selectedPreset.fireVolumeMultiplier);

			_critters.SetIntensity(selectedPreset.crittersIntensity);
			_critters.SetVolumeMultiplier(selectedPreset.crittersVolumeMultiplier);
		}
			
    }
}