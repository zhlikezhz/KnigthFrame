using System;
using UnityEngine;
using Unity.Entities;

public struct TileData : IComponentData
{
    public int tileX;
    public int tileY;
}

public struct DepthData : IComponentData
{
    public int depth;
}

public partial struct DepthSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {

    }

    public void OnDestroy(ref SystemState state)
    {

    }
}

