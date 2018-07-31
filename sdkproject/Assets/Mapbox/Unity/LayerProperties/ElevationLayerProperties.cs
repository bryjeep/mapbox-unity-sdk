namespace Mapbox.Unity.Map
{
	using System;
	using System.ComponentModel;
	using Mapbox.Unity.MeshGeneration.Factories;

	[Serializable]
	public class ElevationLayerProperties : LayerProperties, INotifyPropertyChanged
	{
		public ElevationSourceType sourceType = ElevationSourceType.MapboxTerrain;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String propertyName = "")
		{
			UnityEngine.Debug.Log(propertyName);
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public LayerSourceOptions sourceOptions = new LayerSourceOptions()
		{
			layerSource = new Style()
			{
				Id = "mapbox.terrain-rgb"
			},
			isActive = true
		};

		private void OnElevationRequiredOptionsChanged()
		{
			NotifyPropertyChanged("OnElevationRequiredOptionsChanged");
		}

		public ElevationLayerType elevationLayerType = ElevationLayerType.FlatTerrain;
		public ElevationRequiredOptions requiredOptions = new ElevationRequiredOptions(OnElevationRequiredOptionsChanged);
		//{
		//	PropertyChanged += OnElevationRequiredOptionsChanged;
		//};
		public ElevationModificationOptions modificationOptions = new ElevationModificationOptions();
		public UnityLayerOptions unityLayerOptions = new UnityLayerOptions();
		public TerrainSideWallOptions sideWallOptions = new TerrainSideWallOptions();

		//requiredOptions.

		public ElevationLayerType ElevationLayerType
		{
			set
			{
				if (value != this.elevationLayerType)
				{
					this.elevationLayerType = value;
					NotifyPropertyChanged("ElevationLayerType");
				}
			}
		}

		public ElevationRequiredOptions ElevationRequiredOptions
		{
			set
			{
				if (value != this.requiredOptions)
				{
					this.requiredOptions = value;
					NotifyPropertyChanged("ElevationRequiredOptions");
				}
			}
		}
	}
}
