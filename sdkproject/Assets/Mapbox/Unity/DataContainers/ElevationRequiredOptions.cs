namespace Mapbox.Unity.Map
{
	using System;
	using UnityEngine;
	using System.ComponentModel;
	//using System.Collections;

	using System.Collections;
	using System.Collections.Generic;
	using Mapbox.Unity.Map;

	[Serializable]
	public class ElevationRequiredOptions : INotifyPropertyChanged
	{

		public ElevationRequiredOptions(Action action)
		{
			OnElevationRequiredOptionsChanged += action;
		}

		private ElevationLayerProperties _elevationLayerProperties;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String propertyName = "")
		{
			AbstractMap map = UnityEngine.Object.FindObjectOfType<AbstractMap>();
			map.TestEvent();
			if (OnElevationRequiredOptionsChanged != null)
			{
				OnElevationRequiredOptionsChanged();
			}
		}

		public event Action OnElevationRequiredOptionsChanged = delegate { };

		[Tooltip("Unity material used for rendering terrain tiles.")]
		public Material baseMaterial;
		[Tooltip("Add Unity Physics collider to terrain tiles, used for detecting collisions etc.")]
		public bool addCollider = false;
		[Range(0, 100)]
		[Tooltip("Multiplication factor to vertically exaggerate elevation on terrain, does not work with Flat Terrain.")]
		public float exaggerationFactor = 1;

		public float ExaggerationFactor
		{
			set
			{
				if (!Mathf.Approximately(value, this.exaggerationFactor))
				{
					this.exaggerationFactor = value;
					NotifyPropertyChanged("ExaggerationFactor");
				}
			}
		}
	}
}
