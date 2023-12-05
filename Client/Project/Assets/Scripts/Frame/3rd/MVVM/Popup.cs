using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huge.MVVM
{
    public class Popup : View
    {
        internal override void AfterInit(params object[] args)
        {
            base.AfterInit();
        }

        internal override void BeforeDestroy()
        {
            base.BeforeDestroy();
        }

        internal void RefreshPopupMask()
        {

        }
    }
}
