using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Entities;

public class World
{
    public int Width {get; set;}
    public int Height {get; set;}

    async public static UniTask<World> GenerateAsync(int width, int height)
    {
        World map = new World();
        map.Height = height;
        map.Width = width;
        await map.InitAsync();
        return map;
    }

    async public UniTask InitAsync()
    {

    }
}