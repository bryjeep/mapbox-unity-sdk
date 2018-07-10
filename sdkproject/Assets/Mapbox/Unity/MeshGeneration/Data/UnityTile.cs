namespace Mapbox.Unity.MeshGeneration.Data
{
	using UnityEngine;
	using Mapbox.Unity.MeshGeneration.Enums;
	using Mapbox.Unity.Utilities;
	using Utils;
	using Mapbox.Map;
	using System;
	using Mapbox.Unity.Map;
	using System.Collections.Generic;

	public class UnityTile : MonoBehaviour
	{
		[SerializeField]
		public Texture2D RasterData;
		private Texture2D _heightTexture;
		private float[] _heightData;
		private VectorTile _vectorData;
		public VectorTile VectorData
		{
			get { return _vectorData; }
			set
			{
				_vectorData = value;
				OnVectorDataChanged(this);
			}
		}

		private Texture2D _loadingTexture;
		private List<Tile> _tiles = new List<Tile>();

		#region CachedUnityComponents
		MeshRenderer _meshRenderer;
		public MeshRenderer MeshRenderer
		{
			get
			{
				if (_meshRenderer == null)
				{
					_meshRenderer = GetComponent<MeshRenderer>();
				}
				return _meshRenderer;
			}
		}

		private MeshFilter _meshFilter;
		public MeshFilter MeshFilter
		{
			get
			{
				if (_meshFilter == null)
				{
					_meshFilter = GetComponent<MeshFilter>();
				}
				return _meshFilter;
			}
		}

		private Collider _collider;
		public Collider Collider
		{
			get
			{
				if (_collider == null)
				{
					_collider = GetComponent<Collider>();
				}
				return _collider;
			}
		}
		#endregion

		#region Tile Positon/Scale Properties
		public RectD Rect { get; private set; }
		public int InitialZoom { get; private set; }
		public float TileScale { get; private set; }
		public UnwrappedTileId UnwrappedTileId { get; private set; }
		public CanonicalTileId CanonicalTileId { get; private set; }

		private float _relativeScale;
		#endregion

		public TilePropertyState RasterDataState;
		public TilePropertyState HeightDataState;
		public TilePropertyState VectorDataState;
		public bool IsReadyForVectorMeshGeneration;

		

		internal void Initialize(IMapReadable map, UnwrappedTileId tileId, float scale, int zoom, Texture2D loadingTexture = null)
		{
			TileScale = scale;
			_relativeScale = 1 / Mathf.Cos(Mathf.Deg2Rad * (float)map.CenterLatitudeLongitude.x);
			Rect = Conversions.TileBounds(tileId);
			UnwrappedTileId = tileId;
			CanonicalTileId = tileId.Canonical;
			_loadingTexture = loadingTexture;

			float scaleFactor = 1.0f;
			if (InitialZoom == 0)
			{
				InitialZoom = zoom;
			}

			scaleFactor = Mathf.Pow(2, (map.InitialZoom - zoom));
			gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
			gameObject.SetActive(true);

			OnHeightDataChanged += UpdateDataState;
			OnRasterDataChanged += UpdateDataState;
		}

		private void UpdateDataState(UnityTile tile)
		{
			if (RasterDataState != TilePropertyState.Fetching && RasterDataState != TilePropertyState.Processing &&
				HeightDataState != TilePropertyState.Fetching && HeightDataState != TilePropertyState.Processing)
			{
				if (!IsReadyForVectorMeshGeneration)
				{
					IsReadyForVectorMeshGeneration = true;
					OnReadyForMeshGeneration(tile);
				}
			}
		}

		#region SetTileData
		public void SetHeightData(byte[] data, float heightMultiplier = 1f, bool useRelative = false)
		{
			// HACK: compute height values for terrain. We could probably do this without a texture2d.
			if (_heightTexture == null)
			{
				_heightTexture = new Texture2D(0, 0);
			}

			_heightTexture.LoadImage(data);
			byte[] rgbData = _heightTexture.GetRawTextureData();

			// Get rid of this temporary texture. We don't need to bloat memory.
			_heightTexture.LoadImage(null);

			if (_heightData == null)
			{
				_heightData = new float[256 * 256];
			}

			var relativeScale = useRelative ? _relativeScale : 1f;
			for (int xx = 0; xx < 256; ++xx)
			{
				for (int yy = 0; yy < 256; ++yy)
				{
					float r = rgbData[(xx * 256 + yy) * 4 + 1];
					float g = rgbData[(xx * 256 + yy) * 4 + 2];
					float b = rgbData[(xx * 256 + yy) * 4 + 3];
					_heightData[xx * 256 + yy] = relativeScale * heightMultiplier * Conversions.GetAbsoluteHeightFromColor(r, g, b);
				}
			}

			HeightDataState = TilePropertyState.Finished;
			OnHeightDataChanged(this);

			if (RasterDataState != TilePropertyState.Fetching || RasterDataState != TilePropertyState.Processing)
			{

				IsReadyForVectorMeshGeneration = true;
			}

			if (RasterData != null)
			{
				MeshRenderer.material.mainTexture = RasterData;
			}
		}

		public void SetRasterData(byte[] data, bool useMipMap, bool useCompression)
		{
			if (MeshRenderer == null || MeshRenderer.material == null)
			{
				return;
			}
			// Don't leak the texture, just reuse it.
			if (RasterData == null)
			{
				RasterData = new Texture2D(0, 0, TextureFormat.RGB24, useMipMap);
				RasterData.wrapMode = TextureWrapMode.Clamp;
			}

			RasterData.LoadImage(data);
			if (useCompression)
			{
				// High quality = true seems to decrease image quality?
				RasterData.Compress(false);
			}

			MeshRenderer.material.mainTexture = RasterData;
			RasterDataState = TilePropertyState.Finished;
			OnRasterDataChanged(this);

			if (HeightDataState != TilePropertyState.Fetching || HeightDataState != TilePropertyState.Processing)
			{
				IsReadyForVectorMeshGeneration = true;
			}
		}

		public void SetVectorData(VectorTile vectorTile)
		{
			VectorData = vectorTile;
		}
		#endregion

		public float QueryHeightData(float x, float y)
		{
			if (_heightData != null)
			{
				var intX = (int)Mathf.Clamp(x * 256, 0, 255);
				var intY = (int)Mathf.Clamp(y * 256, 0, 255);
				return _heightData[intY * 256 + intX] * TileScale;
			}

			return 0;
		}

		public void SetLoadingTexture(Texture2D texture)
		{
			MeshRenderer.material.mainTexture = texture;
		}
		
		internal void AddTile(Tile tile)
		{
			_tiles.Add(tile);
		}

		#region Cancel/Recycle/Destroy
		public void Cancel()
		{
			for (int i = 0, _tilesCount = _tiles.Count; i < _tilesCount; i++)
			{
				_tiles[i].Cancel();
			}
		}

		public void Recycle()
		{
			if (_loadingTexture && MeshRenderer != null)
			{
				MeshRenderer.material.mainTexture = _loadingTexture;
			}

			gameObject.SetActive(false);

			// Reset internal state.
			RasterDataState = TilePropertyState.None;
			HeightDataState = TilePropertyState.None;
			VectorDataState = TilePropertyState.None;

			OnHeightDataChanged = delegate { };
			OnRasterDataChanged = delegate { };
			OnVectorDataChanged = delegate { };

			Cancel();
			_tiles.Clear();
		}

		protected virtual void OnDestroy()
		{
			Cancel();
			if (_heightTexture != null)
			{
				Destroy(_heightTexture);
			}
			if (RasterData != null)
			{
				Destroy(RasterData);
			}
		}
		#endregion

		#region Events
		public event Action<UnityTile> OnHeightDataChanged = delegate { };
		public event Action<UnityTile> OnRasterDataChanged = delegate { };
		public event Action<UnityTile> OnVectorDataChanged = delegate { };
		public event Action<UnityTile> OnReadyForMeshGeneration = delegate { };
		#endregion
	}
}
