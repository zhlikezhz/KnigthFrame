using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering;
using Huge.Asset;

public enum MapLayers : int
{
    Bedrock = 1,    //基岩层
    Accumulation,   //堆积层
    Soil,           //土壤层
    Surface,        //地表层：草地、矿物等
    Object,         //建筑层：建筑、装饰物、树林、人物等
    All,
}

public class TileLayerData
{
    public Tilemap tilemap;
    public TilemapRenderer renderer;
    public GameObject gameObject;
}

public class MapRenderSystem
{
    public int Seed;
    public int Width;
    public int Height;

    Grid TileGrid;
    GameObject MapObject;
    Dictionary<MapLayers, TileLayerData> TileMaps = new Dictionary<MapLayers, TileLayerData>();

    public static async UniTask<MapRenderSystem> CreateAsync(int width, int height, int seed = 1000)
    {
        MapRenderSystem mapRender = new MapRenderSystem();
        mapRender.Height = height;
        mapRender.Width = width;
        mapRender.Seed = seed;
        await mapRender.RandomGenerate(seed);
        return mapRender;
    }

    async UniTask RandomGenerate(int seed)
    {
        MapObject = new GameObject();
        MapObject.name = "Map";
        TileGrid = MapObject.AddComponent<Grid>();
        TileGrid.cellSize = new Vector3(1.0f, 0.5f, 1.0f);
        TileGrid.cellLayout = GridLayout.CellLayout.Isometric;
        TileGrid.cellSwizzle = GridLayout.CellSwizzle.XYZ;

        //create tilemap layer
        for (MapLayers mapLayer = MapLayers.Bedrock; mapLayer < MapLayers.All; mapLayer++)
        {
            var layerObject = new GameObject();
            layerObject.name = mapLayer.ToString();
            layerObject.transform.SetParent(MapObject.transform, false);
            layerObject.transform.localEulerAngles = Vector3.zero;
            layerObject.transform.localPosition = Vector3.zero;
            layerObject.transform.localScale = Vector3.one;

            TileLayerData layerData = new TileLayerData();
            layerData.renderer = layerObject.AddComponent<TilemapRenderer>();
            layerData.tilemap = layerObject.GetComponent<Tilemap>();
            layerData.gameObject = layerObject;
            TileMaps.Add(mapLayer, layerData);

            //setup tilemap
            layerData.tilemap.tileAnchor = new Vector3(0.5f, 0.5f, 0.0f);
            layerData.tilemap.orientation = Tilemap.Orientation.XY;

            //setup tilemap renderer
            layerData.renderer.sortOrder = TilemapRenderer.SortOrder.TopRight;
            layerData.renderer.sortingOrder = (int)mapLayer;
            if (mapLayer <= MapLayers.Soil)
            {
                layerData.renderer.mode = TilemapRenderer.Mode.Chunk;
            }
            else
            {
                layerData.renderer.mode = TilemapRenderer.Mode.Individual;
            }
        }

        TileLayerData tileLayerData;
        if (TileMaps.TryGetValue(MapLayers.Bedrock, out tileLayerData))
        {
            string assetPath = "Assets/Art/FantasyTileset/Palettes/Brushes/isometric-tile_ground_03.asset";
            Tile tile = await AssetManager.Instance.LoadAssetAsync<Tile>(assetPath); 

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
            tileLayerData.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (TileMaps.TryGetValue(MapLayers.Accumulation, out tileLayerData))
        {
            string assetPath = "Assets/Art/FantasyTileset/Palettes/Brushes/isometric-tile_ground_02.asset";
            Tile tile = await AssetManager.Instance.LoadAssetAsync<Tile>(assetPath); 

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
            tileLayerData.gameObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        if (TileMaps.TryGetValue(MapLayers.Soil, out tileLayerData))
        {
            string assetPath = "Assets/Art/FantasyTileset/Palettes/Brushes/isometric-tile_ground_01.asset";
            Tile tile = await AssetManager.Instance.LoadAssetAsync<Tile>(assetPath); 

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
            tileLayerData.gameObject.transform.localPosition = new Vector3(0, 1, 0);
        }
    }
}
