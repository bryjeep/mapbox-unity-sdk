namespace Mapbox.Editor
{
	using System.Collections;
	using System.Collections.Generic;
	using Mapbox.Unity.MeshGeneration.Modifiers;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(FeatureBundleList))]
	public class FeatureBundleListDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, null, property);
			EditorGUILayout.LabelField("FeatureBundleList");
			EditorGUI.EndProperty();
		}
	}
}
			//var features = property.FindPropertyRelative("features");
			/*
			for (int i = 0; i < features.arraySize; i++)
			{
				EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

				//FeatureBundle feature = modifier.features[i];
				var feature = features.GetArrayElementAtIndex(i);

				EditorGUILayout.BeginHorizontal();
				var prefab = feature.FindPropertyRelative("prefab");
				string labelName = "dd";//prefab != null ? prefab.name : "Unnamed";
				EditorGUILayout.LabelField(labelName);
				if (GUILayout.Button(new GUIContent("X"), (GUIStyle)"minibutton", GUILayout.Width(30)))
				{
					features.DeleteArrayElementAtIndex(i);
					//continue;
				}

				EditorGUILayout.EndHorizontal();

				//feature.active = EditorGUILayout.Toggle("Active", feature.active);
				//feature.FindPropertyRelative("active").boolValue = EditorGUILayout.Toggle("Active", feature.FindPropertyRelative("active").boolValue);

				//feature.prefab = EditorGUILayout.ObjectField("Prefab", feature.prefab, typeof(GameObject), false) as GameObject;
				//EditorGUILayout.ObjectField("Prefab", feature.FindPropertyRelative("prefab").objectReferenceValue, typeof(GameObject), false);

				//feature.scaleDownWithWorld = EditorGUILayout.Toggle("Scale down with world", feature.scaleDownWithWorld);
				//EditorGUILayout.Toggle("Scale down with world", feature.FindPropertyRelative("active").boolValue);
				/*
				for (int j = 0; j < feature._prefabLocations.Count; j++)
				{
					EditorGUILayout.BeginHorizontal();
					string latLon = feature._prefabLocations[i];
					latLon = EditorGUILayout.TextField(latLon);
					EditorGUILayout.EndHorizontal();
				}
				if (GUILayout.Button("Add Location"))
				{
					feature._prefabLocations.Add("");
				}
			}
			*/
			//EditorGUILayout.Space();
			//EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			//EditorGUILayout.Space();
			//if (GUILayout.Button("Add Feature"))
			//{
				//Debug.Log("Add");
				//int arraySize = features.arraySize;
				//features.InsertArrayElementAtIndex(arraySize + 1);
			//}
			//EditorGUILayout.PropertyField(extrusionGeometryType, extrusionGeometryGUI);

	/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mapbox.Unity.MeshGeneration.Modifiers;

[CustomEditor(typeof(ReplaceFeatureCollectionModifier))]

public class ReplaceFeatureCollectionModifierEditor : Editor 
{
	GUIStyle titleStyle;
	private const float titleColorValue = 0.9f;
	Color titleColor = new Color(titleColorValue, titleColorValue, titleColorValue);

	void OnEnable()
	{
		titleStyle = new GUIStyle();
		titleStyle.fontSize = 12;
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.normal.textColor = titleColor;
	}

	public override void OnInspectorGUI()
	{
		
		ReplaceFeatureCollectionModifier modifier = (ReplaceFeatureCollectionModifier)target;
		for (int i = 0; i < modifier.features.Count; i++)
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

			FeatureBundle feature = modifier.features[i];

			EditorGUILayout.BeginHorizontal();
			string labelName = feature.prefab != null ? feature.prefab.name : "Unnamed";
			EditorGUILayout.LabelField(labelName, titleStyle);
			if (GUILayout.Button(new GUIContent("X"), (GUIStyle)"minibutton", GUILayout.Width(30)))
			{
				modifier.features.Remove(feature);
				continue;
			}

			EditorGUILayout.EndHorizontal();
			
			feature.active = EditorGUILayout.Toggle("Active", feature.active);
			feature.prefab = EditorGUILayout.ObjectField("Prefab", feature.prefab, typeof(GameObject), false) as GameObject;
			feature.scaleDownWithWorld = EditorGUILayout.Toggle("Scale down with world", feature.scaleDownWithWorld);

			for (int j = 0; j < feature._prefabLocations.Count; j++)
			{
				EditorGUILayout.BeginHorizontal();
				string latLon = feature._prefabLocations[i];
				latLon = EditorGUILayout.TextField(latLon);
				EditorGUILayout.EndHorizontal();
			}
			if (GUILayout.Button("Add Location"))
			{
				feature._prefabLocations.Add("");
			}
		}
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
		EditorGUILayout.Space();
		if (GUILayout.Button("Add Feature"))
		{
			FeatureBundle feature = new FeatureBundle();
			modifier.features.Add(feature);
		}
	}

}

	*/
