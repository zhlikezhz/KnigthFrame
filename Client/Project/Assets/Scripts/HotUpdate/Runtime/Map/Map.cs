using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Entities;

public class Map
{
    public int Seed {get; set;}
    public int Width {get; set;}
    public int Height {get; set;}

    MapRenderSystem m_MapRenderSystem;

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
        m_MapRenderSystem = await MapRenderSystem.CreateAsync(Width, Height);
    }
}