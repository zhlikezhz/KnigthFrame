using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using Huge.Asset;
using Cysharp.Threading.Tasks;
using YooAsset;

public enum MapLayer : int
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

public class MapRenderer
{
    public int Seed;
    public int Width;
    public int Height;

    Grid TileGrid;
    GameObject MapObject;
    Dictionary<MapLayer, TileLayerData> TileMaps = new Dictionary<MapLayer, TileLayerData>();

    public static async UniTask<MapRenderer> CreateAsync(int width, int height, int seed = 1000)
    {
        MapRenderer mapRender = new MapRenderer();
        mapRender.Height = height;
        mapRender.Width = width;
        mapRender.Seed = seed;
        await mapRender.RandomGenerate(seed);
        return mapRender;
    }

    public TileLayerData GetTileLayerData(MapLayer layer)
    {
        if (TileMaps.TryGetValue(layer, out var layerData))
        {
            return layerData;
        }
        return null;
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
        for (MapLayer mapLayer = MapLayer.Bedrock; mapLayer < MapLayer.All; mapLayer++)
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
            if (mapLayer < MapLayer.Surface)
            {
                layerData.renderer.mode = TilemapRenderer.Mode.Chunk;
            }
            else
            {
                layerData.renderer.mode = TilemapRenderer.Mode.Individual;
            }
        }

        await GenerateTerrain();
        await GenerateNature();
    }

    async UniTask GenerateTerrain()
    {
        TileLayerData tileLayerData;
        if (TileMaps.TryGetValue(MapLayer.Bedrock, out tileLayerData))
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

        if (TileMaps.TryGetValue(MapLayer.Accumulation, out tileLayerData))
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

        if (TileMaps.TryGetValue(MapLayer.Soil, out tileLayerData))
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
        if (TileMaps.TryGetValue(MapLayer.WaterBack, out tileLayerData))
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

        if (TileMaps.TryGetValue(MapLayer.WaterFront, out tileLayerData))
        {
            string waterAssetPath = "Assets/Art/FantasyTileset/Palettes/Brushes/isometric-tile_water_01.asset";
            Tile watarTile = await AssetManager.Instance.LoadAssetAsync<Tile>(waterAssetPath); 

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
    }

    async UniTask GenerateNature()
    {
        List<string> objList = new List<string>();
        objList.Add("isometric-object_plant_a_01");
        objList.Add("isometric-object_plant_a_02");
        objList.Add("isometric-object_plant_a_03");
        objList.Add("isometric-object_plant_a_04");
        objList.Add("isometric-object_plant_a_05");
        objList.Add("isometric-object_plant_b_01");
        objList.Add("isometric-object_plant_b_02");
        objList.Add("isometric-object_plant_b_03");
        objList.Add("isometric-object_plant_b_04");
        objList.Add("isometric-object_plant_b_05");
        objList.Add("isometric-object_stone_01");
        objList.Add("isometric-object_stone_02");
        objList.Add("isometric-object_stone_03");
        objList.Add("isometric-object_stone_04");
        objList.Add("isometric-object_stone_05");
        objList.Add("isometric-object_tree_01");
        objList.Add("isometric-object_tree_02");
        objList.Add("isometric-object_tree_03");
        objList.Add("isometric-object_tree_04");
        objList.Add("isometric-object_tree_05");

        int count = 0;
        int totalCount = Width * Height / 10;
        System.Random random = new System.Random(100);
        TileLayerData tileLayerData = TileMaps[MapLayer.Object];
        tileLayerData.gameObject.transform.localPosition = new Vector3(0, 1, 0);
        for (int i = 0; i < totalCount; i++)
        {
            if (count >= 500)
            {
                count = 0;
                await UniTask.DelayFrame(1);
            }
            count++;

            int index = random.Next(0, objList.Count);
            Tile tile = AssetManager.Instance.LoadAsset<Tile>($"Assets/Art/FantasyTileset/Palettes/Brushes/{objList[index]}.asset");
            int x = random.Next(-Width + 1, Width);
            int y = random.Next(-Height + 1, Height);
            tileLayerData.tilemap.SetTile(new Vector3Int(x, y, 0), tile);
        }
    }

    public void AddTile(int tileX, int tileY, MapLayer layer, TileBase tile)
    {

    }

    public void RemoveTile(int tileX, int tileY, MapLayer layer)
    {
        if (TileMaps.TryGetValue(layer, out var tileLayerData))
        {
            if (-Width < tileX && tileX < Width && -Height < tileY && tileY < Height)
            {
                tileLayerData.tilemap.SetTile(new Vector3Int(tileX, tileY, 0), null);
            }
        }
    }
}
