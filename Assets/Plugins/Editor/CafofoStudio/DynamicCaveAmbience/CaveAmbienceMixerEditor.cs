using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CafofoStudio
{
	[CustomEditor(typeof(CaveAmbientMixer))]
	public class CaveAmbienceMixerEditor : AmbienceMixerEditor<CaveAmbientMixer, CaveAmbientPreset>
    {
		protected override List<string> GetSerializedElementProperties ()
		{
			return new List<string>() { "_atmosphere1", "_atmosphere2", "_atmosphere3", "_sediment", "_waterDrops", "_waterStream", "_sewer", "_fire", "_critters" };
		}

		protected override void ApplyPreset (CaveAmbientPreset preset)
		{
			ApplyPresetConfig("_atmosphere1", preset.atmosphere1Intensity, preset.atmosphere1VolumeMultiplier);
			ApplyPresetConfig("_atmosphere2", preset.atmosphere2Intensity, preset.atmosphere2VolumeMultiplier);
			ApplyPresetConfig("_atmosphere3", preset.atmosphere3Intensity, preset.atmosphere3VolumeMultiplier);
			ApplyPresetConfig("_sediment", preset.sedimentIntensity, preset.sedimentVolumeMultiplier);
			ApplyPresetConfig("_waterDrops", preset.waterDropsIntensity, preset.waterDropsVolumeMultiplier);
			ApplyPresetConfig("_waterStream", preset.waterStreamIntensity, preset.waterStreamVolumeMultiplier);
			ApplyPresetConfig("_sewer", preset.sewerIntensity, preset.sewerVolumeMultiplier);
			ApplyPresetConfig("_fire", preset.fireIntensity, preset.fireVolumeMultiplier);
			ApplyPresetConfig("_critters", preset.crittersIntensity, preset.crittersVolumeMultiplier);
		}

		protected override SoundElement GetSoundElementFromProperty (string propertyName)
		{
			switch (propertyName) {
				case "_atmosphere1":
					return myTarget.Atmosphere1;
				case "_atmosphere2":
					return myTarget.Atmosphere2;
				case "_atmosphere3":
					return myTarget.Atmosphere3;
				case "_sediment":
					return myTarget.Sediment;
				case "_waterDrops":
					return myTarget.WaterDrops;
				case "_waterStream":
					return myTarget.WaterStream;
				case "_sewer":
					return myTarget.Sewer;
				case "_fire":
					return myTarget.Fire;
				default:
					return myTarget.Critters;
			}
		}

    }
}