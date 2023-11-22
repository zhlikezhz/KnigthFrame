/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
//using System.Reflection;
//using System.Linq;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class ExampleGenConfig
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>() {
                // UnityEngine.CoreModule 
                typeof(System.Object),
                typeof(UnityEngine.Object),
                typeof(MonoBehaviour),
                typeof(Behaviour),
                typeof(Component),
                typeof(GameObject),
                typeof(Transform),
                typeof(WaitForEndOfFrame),
                typeof(WaitForSeconds),
                typeof(Screen),
                typeof(Camera),
                typeof(SystemInfo),
                typeof(PlayerPrefs),
                typeof(Application),
                typeof(SceneManager),
                typeof(Renderer),
                typeof(Material),
                typeof(Sprite),
                typeof(Shader),
                typeof(TextAsset),
                typeof(Texture2D),
                typeof(Resolution),
                typeof(Color),
                typeof(Color32),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Rect),
                typeof(Quaternion),
                typeof(Time),
                typeof(Animation),
                typeof(Animator),
                typeof(AnimationCurve),
                typeof(AnimationClip),
                typeof(ParticleSystemRenderer),
                typeof(AudioSource),
                typeof(Camera),
                //UnityEngine.UI
               
                typeof(Canvas),
                typeof(RectTransform),
                typeof(RectTransformUtility),
                typeof(LayoutUtility),
                typeof(ContentSizeFitter),
                typeof(Graphic),
                typeof(Image),
                typeof(RawImage),
                typeof(Button),
                typeof(Slider),
                typeof(Scrollbar),
                typeof(ScrollRect),
                typeof(Mask),
                typeof(Text),
                typeof(Shadow),
                typeof(Outline),
                typeof(Toggle),
                typeof(InputField),
                typeof(InputField.InputType),
                typeof(Dropdown),
                typeof(Button.ButtonClickedEvent),
                typeof(Slider.SliderEvent),
                typeof(Toggle.ToggleEvent),
                typeof(InputField.OnChangeEvent),
                typeof(Dropdown.DropdownEvent),
                typeof(Dropdown.OptionData),
                typeof(ToggleGroup),
                typeof(GridLayoutGroup),
                typeof(VerticalLayoutGroup),
                typeof(HorizontalLayoutGroup),
                typeof(HorizontalOrVerticalLayoutGroup),
                typeof(HorizontalWrapMode),
                typeof(Animator),
                typeof(AnimatorStateInfo),
                typeof(RuntimeAnimatorController),
                typeof(AnimationClip),
                typeof(Input),
                typeof(ScrollRect),
                typeof(UnityEvent<int, Transform>),
                typeof(Button.ButtonClickedEvent),
                typeof(Toggle.ToggleEvent),
                typeof(InputField.SubmitEvent),
                typeof(Outline),
                typeof(RectTransform.Edge),
                typeof(RectTransform.Axis),
                typeof(Resources),
                typeof(OnDemandRendering),
                typeof(LayoutElement),
                typeof(LayerMask),
                typeof(PointerEventData),
                typeof(BaseEventData),
                typeof(AxisEventData),
                typeof(Collision),
                typeof(Collision2D),
                typeof(Collider2D),
                typeof(Rigidbody2D),
                typeof(ControllerColliderHit),
                typeof(GUI),
                typeof(GUIStyle),
                typeof(CanvasGroup),        
                typeof(TextAnchor),
            };


    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
                typeof(UnityEngine.Events.UnityAction),
                typeof(System.Collections.IEnumerator)
            };

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()
    {
        new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
        new List<string>(){"UnityEngine.WWW", "movie"},
    #if UNITY_WEBGL
        new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
        new List<string>(){ "UnityEngine.UI.Text", "OnRebuildRequested" },
        new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
        new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
        new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
        new List<string>(){"UnityEngine.Light", "areaSize"},
        new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
        new List<string>(){"UnityEngine.WWW", "MovieTexture"},
        new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
        new List<string>(){ "UnityEngine.UI.Graphic", "OnRebuildRequested" },
        new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
        new List<string>(){"UnityEngine.Application", "ExternalEval","System.String"},
    #endif
        new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
        new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
        new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
       
                         //2022
        new List<string>(){"UnityEngine.AudioSource","PlayOnGamepad" ,"System.Int32"},
        new List<string>(){"UnityEngine.AudioSource","DisableGamepadOutput"},
         new List<string>(){"UnityEngine.AudioSource","gamepadSpeakerOutputType"},
        new List<string>(){"UnityEngine.AudioSource", "GamepadSpeakerSupportsOutputType","UnityEngine.GamepadSpeakerOutputType"  },
        new List<string>(){"UnityEngine.AudioSource","SetGamepadSpeakerMixLevel","System.Int32","System.Int32" },
        new List<string>(){"UnityEngine.AudioSource","SetGamepadSpeakerMixLevelDefault","System.Int32" },
        new List<string>(){"UnityEngine.AudioSource","SetGamepadSpeakerRestrictedAudio" ,"System.Int32","System.Boolean"},

        new List<string>(){ "UnityEngine.Material", "IsChildOf", "UnityEngine.Material" },
        new List<string>(){ "UnityEngine.Material", "RevertAllPropertyOverrides" },
     
        new List<string>(){ "UnityEngine.Material", "RevertPropertyOverride","System.String" },
          new List<string>(){ "UnityEngine.Material", "RevertPropertyOverride","System.Int32" },

        new List<string>(){ "UnityEngine.Material", "SetPropertyLock",  "System.Int32","System.Boolean" },
          new List<string>(){ "UnityEngine.Material", "SetPropertyLock",  "System.String","System.Boolean" },
        new List<string>(){ "UnityEngine.Material", "ApplyPropertyOverride", "UnityEngine.Material", "System.String", "System.Boolean" },
         new List<string>(){ "UnityEngine.Material", "ApplyPropertyOverride", "UnityEngine.Material", "System.Int32", "System.Boolean" },
        new List<string>(){ "UnityEngine.Material", "parent" },
        new List<string>(){ "UnityEngine.Material", "isVariant" },
        //
        new List<string>(){ "UnityEngine.Material", "IsPropertyOverriden","System.Int32" },
        new List<string>(){ "UnityEngine.Material", "IsPropertyOverriden","System.String" },
        new List<string>(){ "UnityEngine.Material", "IsPropertyLocked", "System.Int32" },
        new List<string>(){ "UnityEngine.Material", "IsPropertyLocked", "System.String" },
        new List<string>(){ "UnityEngine.Material", "IsPropertyLockedByAncestor", "System.Int32" },
        new List<string>(){ "UnityEngine.Material", "IsPropertyLockedByAncestor", "System.String" },
    };
//#if UNITY_2018_1_OR_NEWER
    [BlackList]
    public static Func<MemberInfo, bool> MethodFilter = (memberInfo) =>
    {
        if (memberInfo.DeclaringType.IsGenericType && memberInfo.DeclaringType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            if (memberInfo.MemberType == MemberTypes.Constructor)
            {
                ConstructorInfo constructorInfo = memberInfo as ConstructorInfo;
                var parameterInfos = constructorInfo.GetParameters();
                if (parameterInfos.Length > 0)
                {
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(parameterInfos[0].ParameterType))
                    {
                        return true;
                    }
                }
            }
            else if (memberInfo.MemberType == MemberTypes.Method)
            {
                var methodInfo = memberInfo as MethodInfo;
                if (methodInfo.Name == "TryAdd" || methodInfo.Name == "Remove" && methodInfo.GetParameters().Length == 2)
                {
                    return true;
                }
            }
        }
        return false;
    };
//#endif
}
