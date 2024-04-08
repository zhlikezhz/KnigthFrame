using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering;
using Huge.Asset;
using Unity.Entities.UniversalDelegates;
using Unity.Collections;

public enum MapLayers : int
{
    Bedrock = 1,    //基岩层
    Accumulation,   //堆积层
    WaterBack,      //后面的水
    Soil,           //土壤层
    WaterFront,     //前面的水
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
        //将Z加到Y上来排序
        TileGrid.cellLayout = GridLayout.CellLayout.IsometricZAsY;
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
            if (mapLayer < MapLayers.Surface)
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

            for (int x = -Width; x < Width; x++)
            {
                for (int y = -Height; y < Height; y++)
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

            for (int x = -Width; x < Width; x++)
            {
                for (int y = -Height; y < Height; y++)
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

            for (int x = -Width; x < Width; x++)
            {
                for (int y = -Height; y < Height; y++)
                {
                    tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
            tileLayerData.gameObject.transform.localPosition = new Vector3(0, 1, 0);
        }

        int offset = 50;
        if (TileMaps.TryGetValue(MapLayers.WaterBack, out tileLayerData))
        {
            string waterAssetPath = "Assets/Art/FantasyTileset/Palettes/Brushes/isometric-tile_water_01.asset";
            Tile watarTile = await AssetManager.Instance.LoadAssetAsync<Tile>(waterAssetPath); 
            for (int x = -Width - offset; x < Width + offset; x++)
            {
                for (int y = Height; y < Height + offset; y++)
                {
                    tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), watarTile);
                }
            }
            tileLayerData.gameObject.transform.localPosition = new Vector3(0, 1, 0);

            for (int y = -Height - offset; y < Height; y++)
            {
                for (int x = Width; x < Width + offset; x++)
                {
                    tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), watarTile);
                }
            }
            tileLayerData.gameObject.transform.localPosition = new Vector3(0, 1, 0);
        }

        if (TileMaps.TryGetValue(MapLayers.WaterFront, out tileLayerData))
        {
            string waterAssetPath = "Assets/Art/FantasyTileset/Palettes/RuleTiles/WaterAFront.asset";
            IsometricRuleTile watarTile = await AssetManager.Instance.LoadAssetAsync<IsometricRuleTile>(waterAssetPath); 

            for (int x = -Width - offset; x < Width; x++)
            {
                for (int y = -Height - offset; y < -Height; y++)
                {
                    tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), watarTile);
                }
            }

            for (int x = -Width - offset; x < -Width; x++)
            {
                for (int y = -Height; y < Height; y++)
                {
                    tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), watarTile);
                }
            }
            tileLayerData.gameObject.transform.localPosition = new Vector3(0, 1, 0);
        }
            // int offset = 10;
            // for (int x = -Width - offset; x < Width + offset; x++)
            // {
            //     for (int y = -Height - offset; y < -Height; y++)
            //     {
            //         tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), watarTile);
            //     }

            //     for (int y = Height; y < Height + offset; y++)
            //     {
            //         tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), watarTile);
            //     }
            // }

            // for (int y = -Height; y < Height; y++)
            // {
            //     for (int x = -Width - offset; x < -Width; x++)
            //     {
            //         tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), watarTile);
            //     }

            //     for (int x = Width; x < Width + offset; x++)
            //     {
            //         tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), watarTile);
            //     }
            // }
    }
}
