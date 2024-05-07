using System;
using UnityEngine;
using System.Collections;

namespace Huge.MVVM
{
    public interface IViewEnterAnimation
    {
        IEnumerator OnPlayEnterAnimation();
    }
}