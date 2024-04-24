using System;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    SpineRenderer renderer;
    HeroController controller;

    public static Hero CreateAsync()
    {
        var spineRenderer = SpineRenderer.CreateHero();
        Hero hero = spineRenderer.AddComponent<Hero>();
        hero.renderer = spineRenderer;
        spineRenderer.SetScale(new Vector3(.2f, .2f, .2f));
        spineRenderer.SetSortingOrder(7);
        spineRenderer.SetActive(true);
        spineRenderer.PlayAnimtion("Stand", true);

        hero.controller = hero.AddComponent<HeroController>();
        return hero;
    }
}