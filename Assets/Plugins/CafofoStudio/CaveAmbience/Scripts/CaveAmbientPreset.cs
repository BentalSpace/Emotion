using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CafofoStudio
{
    [CreateAssetMenu(fileName = "MyCaveAmbientPreset", menuName = "CafofoStudio/Create Custom Preset Asset/Cave", order = 1)]
	public class CaveAmbientPreset : AmbientPreset
    {
		[Range(0, 1)] public float atmosphere1Intensity = 0f;
		[Range(0, 1)] public float atmosphere1VolumeMultiplier = 1f;

		[Range(0, 1)] public float atmosphere2Intensity = 0f;
		[Range(0, 1)] public float atmosphere2VolumeMultiplier = 1f;

		[Range(0, 1)] public float atmosphere3Intensity = 0f;
		[Range(0, 1)] public float atmosphere3VolumeMultiplier = 1f;

		[Range(0, 1)] public float sedimentIntensity = 0f;
		[Range(0, 1)] public float sedimentVolumeMultiplier = 1f;

		[Range(0, 1)] public float waterDropsIntensity = 0f;
		[Range(0, 1)] public float waterDropsVolumeMultiplier = 1f;

		[Range(0, 1)] public float waterStreamIntensity = 0f;
		[Range(0, 1)] public float waterStreamVolumeMultiplier = 1f;

		[Range(0, 1)] public float sewerIntensity = 0f;
		[Range(0, 1)] public float sewerVolumeMultiplier = 1f;

		[Range(0, 1)] public float fireIntensity = 0f;
		[Range(0, 1)] public float fireVolumeMultiplier = 1f;

		[Range(0, 1)] public float crittersIntensity = 0f;
		[Range(0, 1)] public float crittersVolumeMultiplier = 1f;
	}
}