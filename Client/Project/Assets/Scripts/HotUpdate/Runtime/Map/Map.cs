using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Tilemaps;

public class Map
{
    public int Seed {get; set;}
    public int Width {get; set;}
    public int Height {get; set;}

    MapRenderer m_MapRenderer;

    public async static UniTask<Map> CreateAsync(int width, int height, int seed)
    {
        Map map = new Map();
        map.Height = height;
        map.Width = width;
        map.Seed = seed;
        await map.InitAsync();
        return map;
    }

    public async UniTask InitAsync()
    {
        m_MapRenderer = await MapRenderer.CreateAsync(Width, Height);
        TileLayerData layerData = m_MapRenderer.GetTileLayerData(MapLayer.Object);
        GameObject root = layerData.tilemap.gameObject;
        var tileCollider = root.AddComponent<TilemapCollider2D>();
        var rigidbody =  root.AddComponent<Rigidbody2D>();
        var compositeCollider = root.AddComponent<CompositeCollider2D>();
        compositeCollider.offset = new Vector2(0.06f, 0.13f);
        rigidbody.bodyType = RigidbodyType2D.Static;
        tileCollider.usedByComposite = true;
    }
}