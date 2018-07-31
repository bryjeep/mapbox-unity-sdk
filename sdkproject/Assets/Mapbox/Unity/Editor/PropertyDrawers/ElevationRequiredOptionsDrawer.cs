namespace Mapbox.Editor
{
	using UnityEditor;
	using UnityEngine;
	using Mapbox.Unity.Map;

	[CustomPropertyDrawer(typeof(ElevationRequiredOptions))]
	public class ElevationRequiredOptionsDrawer : PropertyDrawer
	{
		static float lineHeight = EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var map = (AbstractMap)property.serializedObject.targetObject;
			var terrainProp = map.Terrain.LayerProperty;

			EditorGUI.BeginProperty(position, label, property);

			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), property.FindPropertyRelative("baseMaterial"));
			position.y += lineHeight;
			var exaggerationFactor = property.FindPropertyRelative("exaggerationFactor");
			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), exaggerationFactor);
			terrainProp.requiredOptions.ExaggerationFactor = exaggerationFactor.floatValue;
			position.y += lineHeight;
			EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight), property.FindPropertyRelative("addCollider"));

			//terrainProp.requiredOptions.ExaggerationFactor

			EditorGUI.EndProperty();
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			// Reserve space for the total visible properties.
			return 3.0f * lineHeight;
		}
	}
}