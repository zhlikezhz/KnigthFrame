using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Entities;

public class World
{
    public int Width {get; set;}
    public int Height {get; set;}

    MapRenderSystem m_MapRenderSystem;

    public async static UniTask<World> CreateAsync(int width, int height)
    {
        World map = new World();
        map.Height = height;
        map.Width = width;
        await map.InitAsync();
        return map;
    }

    public async UniTask InitAsync()
    {
        m_MapRenderSystem = await MapRenderSystem.CreateAsync(Width, Height);
    }
}