using System;
using Huge.Asset;
using UnityEngine;
using UnityEngine.Rendering;
using Spine.Unity;
using Unity.VisualScripting;

public class SpineRenderer : MonoBehaviour
{
    SortingGroup m_SortingGroup;
    SkeletonAnimation m_SkeletonAnimation;

    public static SpineRenderer CreateHero()
    {
        return CreateByPrefab("Assets/Art/Prefabs/Hero.prefab");
    }

    public static SpineRenderer CreateByPrefab(string assetPath)
    {
        GameObject prefab = AssetManager.Instance.LoadAsset<GameObject>(assetPath);
        GameObject root = GameObject.Instantiate(prefab);
        return CreateByGameObject(root, root.GetComponent<SkeletonAnimation>());
    }

    public static SpineRenderer CreateByGameObject(GameObject root, SkeletonAnimation skeletonAnimation)
    {
        SpineRenderer spine = root.AddComponent<SpineRenderer>();
        spine.m_SkeletonAnimation = skeletonAnimation;
        spine.m_SortingGroup = skeletonAnimation.GetOrAddComponent<SortingGroup>();
        return spine;
    }

    public void SetActive(bool isActived)
    {
        gameObject.SetActive(isActived);
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetParent(Transform parent, bool worldPositionStays = false)
    {
        transform.SetParent(parent, worldPositionStays);
    }

    public void SetSortingOrder(int order)
    {
        m_SortingGroup.sortingOrder = order;
    }

    public void PlayAnimation(string animName, bool isLoop = false)
    {
        m_SkeletonAnimation.loop = isLoop;
        m_SkeletonAnimation.AnimationName = animName;
    }

    public void StopAnimation()
    {
        m_SkeletonAnimation.AnimationName = null;
    }
}
