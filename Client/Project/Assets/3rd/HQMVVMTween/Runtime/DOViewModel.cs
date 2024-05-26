using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using Joy.MVVM.DataBinding;

public static class DOViewModel
{
    public static TweenerCore<float, float, FloatOptions> DOAnimation(this ViewModel vm, DOGetter<float> get, DOSetter<float> set, float to, float duration, bool snapping = false)
    {
        var tween = DOTween.To(get, set, to, duration);
        tween.SetOptions(snapping).SetTarget(vm);
        return tween;
    }

    public static TweenerCore<int, int, NoOptions> DOAnimation(this ViewModel vm, DOGetter<int> get, DOSetter<int> set, int to, float duration)
    {
        var tween = DOTween.To(get, set, to, duration);
        tween.SetTarget(vm);
        return tween;
    }

    public static TweenerCore<double, double, NoOptions> DOAnimation(this ViewModel vm, DOGetter<double> get, DOSetter<double> set, double to, float duration)
    {
        var tween = DOTween.To(get, set, to, duration);
        tween.SetTarget(vm);
        return tween;
    }

    public static TweenerCore<uint, uint, UintOptions> DOAnimation(this ViewModel vm, DOGetter<uint> get, DOSetter<uint> set, uint to, float duration)
    {
        var tween = DOTween.To(get, set, to, duration);
        tween.SetTarget(vm);
        return tween;
    }

    public static TweenerCore<Vector2, Vector2, VectorOptions> DOAnimation(this Vector2VM vec, Vector2 to, float duration, bool snapping = false)
    {
        var tween = DOTween.To(() => vec.ToVector() , (val) => vec = val, to, duration);
        tween.SetOptions(snapping).SetTarget(vec);
        return tween;
    }

    public static TweenerCore<Vector3, Vector3, VectorOptions> DOAnimation(this Vector3VM vec, Vector3 to, float duration, bool snapping = false)
    {
        var tween = DOTween.To(() => vec.ToVector() , (val) => vec.FromVector(val), to, duration);
        tween.SetOptions(snapping).SetTarget(vec);
        return tween;
    }

    public static TweenerCore<Color, Color, ColorOptions> DOAnimation(this ColorVM vec, Color to, float duration, bool snapping = false)
    {
        var tween = DOTween.To(() => vec.ToColor() , (val) => vec.FromColor(val), to, duration);
        tween.SetOptions(snapping).SetTarget(vec);
        return tween;
    }

    public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DOAnimation(this QuaternionVM vec, Vector3 to, float duration, bool snapping = false)
    {
        var tween = DOTween.To(() => vec.ToQuaternion() , (val) => vec.FromQuaternion(val), to, duration);
        tween.SetOptions(snapping).SetTarget(vec);
        return tween;
    }
}
