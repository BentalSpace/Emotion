using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CafofoStudio
{
	public abstract class AmbienceMixerEditor<T, P>: Editor where T: AmbienceMixer<P> where P: AmbientPreset
	{
		protected T myTarget;

		protected P selectedCustomPreset;

		protected int selectedPresetIndex = 0;

		protected SerializedProperty propPlayOnAwake;

		void OnEnable()
		{
			myTarget = target as T;
			propPlayOnAwake = serializedObject.FindProperty("playOnAwake");
		}

		abstract protected List<string> GetSerializedElementProperties ();

		abstract protected SoundElement GetSoundElementFromProperty (string propertyName);

		abstract protected void ApplyPreset(P preset);

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			List<P> presetList = myTarget.presets;

			List<string> presetNames = new List<string>();
			presetNames.Add ("Select...");
			presetNames.Add ("Custom preset file...");
			foreach (P preset in presetList)
			{
				presetNames.Add (preset.presetName);
			}

			selectedPresetIndex = EditorGUILayout.Popup ("Preset", selectedPresetIndex, presetNames.ToArray ());

			if (selectedPresetIndex == 1) {
				selectedCustomPreset = EditorGUILayout.ObjectField ("Custom preset file", selectedCustomPreset, typeof(P), false) as P;
			}

			if (selectedPresetIndex != 0 && GUILayout.Button("Apply Preset"))
			{
				P presetToApply = (selectedPresetIndex == 1) ? selectedCustomPreset : presetList [selectedPresetIndex - 2];
				if (presetToApply != null)
				{
					ApplyPreset (presetToApply);
					selectedPresetIndex = 0;
					selectedCustomPreset = null;
					if (Application.isPlaying) {
						myTarget.ApplyPreset (presetToApply);
					}
					serializedObject.ApplyModifiedProperties ();
					serializedObject.Update ();
				}
			}

			EditorGUILayout.EndVertical();

			EditorGUILayout.PropertyField(propPlayOnAwake, new GUIContent("Play On Awake"));

			EditorGUILayout.Space();

			//Draw sliders for each element

			var elementNames = GetSerializedElementProperties ();
			foreach(string elementName in elementNames) {
				var elementObject = GetSoundElementFromProperty (elementName);
				DrawSoundElementInspector(elementName, elementObject);
				if (elementName != elementNames [elementNames.Count - 1]) {
					GUILayout.Box ("", GUILayout.ExpandWidth (true), GUILayout.Height (1));
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// Draws a customized slider with small labels underneath.
		/// </summary>
		/// <param name="label">The sliders name</param>
		/// <param name="property">Property displayed and controlled by the slider</param>
		/// <param name="minIntensityLabel">The first label below the start of the slider</param>
		/// <param name="maxIntensityLabel">The second label below the end of the slider</param>
		private void DrawSlider(string label, SerializedProperty property, string minIntensityLabel, string maxIntensityLabel)
		{
			Rect position = EditorGUILayout.GetControlRect(false, 2 * EditorGUIUtility.singleLineHeight);
			position.height *= 0.5f;

			EditorGUI.Slider(position, property, 0f, 1f, new GUIContent(label));

			position.y += position.height * 0.9f;
			position.x += EditorGUIUtility.labelWidth;
			position.width -= EditorGUIUtility.labelWidth + 54;

			GUIStyle style = EditorStyles.centeredGreyMiniLabel;
			style.alignment = TextAnchor.UpperLeft;
			EditorGUI.LabelField(position, minIntensityLabel, style);

			style.alignment = TextAnchor.UpperRight;
			EditorGUI.LabelField(position, maxIntensityLabel, style);
		}

		protected void DrawSoundElementInspector(string propertyName, SoundElement element)
		{
			SerializedProperty propElement = serializedObject.FindProperty(propertyName);
			SerializedProperty propIntensity = propElement.FindPropertyRelative("intensity");
			SerializedProperty propVolMult = propElement.FindPropertyRelative("volumeMultiplier");
			SerializedProperty propMixer = propElement.FindPropertyRelative("overrideOutputMixer");
			string minIntensityLabel = propElement.FindPropertyRelative("minIntensityLabel").stringValue;
			string maxIntensityLabel = propElement.FindPropertyRelative("maxIntensityLabel").stringValue;
			string soundName = propElement.FindPropertyRelative("soundName").stringValue;

			EditorGUILayout.LabelField(soundName, EditorStyles.boldLabel);

			/* 
             * Only do calculations if the application is playing and the value was changed.
             */

			DrawSlider("Intensity", propIntensity, minIntensityLabel, maxIntensityLabel);
			if (propIntensity.floatValue != element.GetIntensity() && Application.isPlaying)
			{
				element.SetIntensity(propIntensity.floatValue);
			}

			DrawSlider("Volume", propVolMult, "0%", "100%");
			if (propVolMult.floatValue != element.GetVolumeMultiplier() && Application.isPlaying)
			{
				element.SetVolumeMultiplier(propVolMult.floatValue);
			}
				
			EditorGUILayout.PropertyField(propMixer);
			if (propMixer.objectReferenceValue != element.GetOutputMixerGroup())
			{
				element.SetOutputMixerGroup((UnityEngine.Audio.AudioMixerGroup)propMixer.objectReferenceValue);
			}
		}

		protected void ApplyPresetConfig(string elementPropertyName, float intensity, float volume) 
		{
			SerializedProperty propElement = serializedObject.FindProperty(elementPropertyName);
			propElement.FindPropertyRelative ("intensity").floatValue = intensity;
			propElement.FindPropertyRelative ("volumeMultiplier").floatValue = volume;
		}

	}
}

