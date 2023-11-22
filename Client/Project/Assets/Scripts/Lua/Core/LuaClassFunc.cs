using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLua;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LuaFrame
{
    /// <summary>
    /// 表记此Lua事件需要什么Notifier
    /// </summary>
    public class NotifierAttribute : System.Attribute
    {
        public readonly Type notifierType;
        public NotifierAttribute(Type type)
        {
            notifierType = type;
        }
    }

    public enum FuncType
    {
        None,
        BehaviourInit,
        Common,
        UIEvent,
        BaseUIEvent,
        CollisionEvent,
        Collision2DEvent,
        Collider2DEvent,
        BoolEvent,
        IntEvent,
        FloatEvent,
        StringEvent,

    }

    public class FuncAttribute : System.Attribute
    {
        public readonly FuncType funcType;
        public FuncAttribute(FuncType type)
        {
            funcType = type;
        }
    }


    public enum LuaClassFunc
    {
        //base
        nil,
        [Func(FuncType.BehaviourInit)]
        initLuaBehaviour,
        [Func(FuncType.Common)]
        Awake,
        [Func(FuncType.Common)]
        OnEnable,//	This function is called when the object becomes enabled and active.
        [Func(FuncType.Common)]
        Start,//	Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        [Func(FuncType.Common)]
        OnDisable,//	This function is called when the behaviour becomes disabled () or inactive.
        [Func(FuncType.Common)]
        OnDestroy,//	This function is called when the MonoBehaviour will be destroyed.
        [Func(FuncType.Common)]
        OnDestroyed,//	
        [Func(FuncType.IntEvent)]
        OnPooledGet,


        //Updates
        [Notifier(typeof(UpdateNotifier))]
        [Func(FuncType.Common)]
        Update,//	Update is called every frame, if the MonoBehaviour is enabled.
        [Notifier(typeof(FixedUpdateNotifier))]
        [Func(FuncType.Common)]
        FixedUpdate,//	This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        [Notifier(typeof(LateUpdateNotifier))]
        [Func(FuncType.Common)]
        LateUpdate,//	LateUpdate is called every frame, if the Behaviour is enabled.




        // UGUI
        //Pointer
        [Notifier(typeof(PointerEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnPointerClick,
        [Notifier(typeof(PointerEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnPointerDown,
        [Notifier(typeof(PointerEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnPointerUp,
        [Notifier(typeof(PointerEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnPointerEnter,
        [Notifier(typeof(PointerEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnPointerExit,
        [Notifier(typeof(PointerEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnScroll,
 
        //Drag
        [Notifier(typeof(DragEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnInitializePotentialDrag,
        [Notifier(typeof(DragEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnBeginDrag,
        [Notifier(typeof(DragEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnDrag,
        [Notifier(typeof(DragEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnDrop,
        [Notifier(typeof(DragEventNotifier))]
        [Func(FuncType.UIEvent)]
        OnEndDrag,

        //
        [Notifier(typeof(BaseEventNotifier))]

        [Func(FuncType.BaseUIEvent)]
        OnSubmit,
        [Notifier(typeof(BaseEventNotifier))]
        [Func(FuncType.BaseUIEvent)]
        OnUpdateSelected,
        [Notifier(typeof(BaseEventNotifier))]
        [Func(FuncType.BaseUIEvent)]
        OnCancel,
        [Notifier(typeof(BaseEventNotifier))]
        [Func(FuncType.BaseUIEvent)]
        OnDeselect,
        [Notifier(typeof(BaseEventNotifier))]
        [Func(FuncType.BaseUIEvent)]
        OnSelect,
        //MonoBehaviour
        [Notifier(typeof(MouseNotifiler))]
        [Func(FuncType.Common)]
        OnMouseDown,//	OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
        [Notifier(typeof(MouseNotifiler))]
        [Func(FuncType.Common)]
        OnMouseDrag,//	OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
        [Notifier(typeof(MouseNotifiler))]
        [Func(FuncType.Common)]
        OnMouseEnter,//	Called when the mouse enters the GUIElement or Collider.
        [Notifier(typeof(MouseNotifiler))]
        [Func(FuncType.Common)]
        OnMouseExit,//	Called when the mouse is not any longer over the GUIElement or Collider.
        [Notifier(typeof(MouseNotifiler))]
        [Func(FuncType.Common)]
        OnMouseOver,//	Called every frame while the mouse is over the GUIElement or Collider.
        [Notifier(typeof(MouseNotifiler))]
        [Func(FuncType.Common)]
        OnMouseUp,//	OnMouseUp is called when the user has released the mouse button.
        [Notifier(typeof(MouseNotifiler))]
        [Func(FuncType.Common)]
        OnMouseUpAsButton,//	OnMouseUpAsButton is only called when the mouse is released over the same GUIElement or Collider as it was pressed.

        [Notifier(typeof(CollisionNotifier))]
        [Func(FuncType.CollisionEvent)]
        OnCollisionEnter,//	OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
        [Notifier(typeof(CollisionNotifier))]
        [Func(FuncType.CollisionEvent)]
        OnCollisionExit,//	OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
        [Notifier(typeof(CollisionNotifier))]
        [Func(FuncType.CollisionEvent)]
        OnCollisionStay,//	OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.

        [Notifier(typeof(TriggerNotifier))]
        [Func(FuncType.CollisionEvent)]
        OnTriggerEnter,//	OnTriggerEnter is called when the Collider other enters the trigger.
        [Notifier(typeof(TriggerNotifier))]
        [Func(FuncType.CollisionEvent)]
        OnTriggerExit,//	OnTriggerExit is called when the Collider other has stopped touching the trigger.
        [Notifier(typeof(TriggerNotifier))]
        [Func(FuncType.CollisionEvent)]
        OnTriggerStay,//	OnTriggerStay is called once per frame for every Collider other that is touching the trigger.

        [Notifier(typeof(Collision2DNotifier))]
        [Func(FuncType.Collision2DEvent)]
        OnCollisionStay2D,//	Sent each frame where a collider on another object is touching this object's collider (2D physics only).
        [Notifier(typeof(Collision2DNotifier))]
        [Func(FuncType.Collision2DEvent)]
        OnCollisionEnter2D,//	Sent when an incoming collider makes contact with this object's collider (2D physics only).
        [Notifier(typeof(Collision2DNotifier))]
        [Func(FuncType.Collision2DEvent)]
        OnCollisionExit2D,//	Sent when a collider on another object stops touching this object's collider (2D physics only).

        [Notifier(typeof(Trigger2DNotifier))]
        [Func(FuncType.Collider2DEvent)]
        OnTriggerEnter2D,//	Sent when another object enters a trigger collider attached to this object (2D physics only).
        [Notifier(typeof(Trigger2DNotifier))]
        [Func(FuncType.Collider2DEvent)]
        OnTriggerExit2D,//	Sent when another object leaves a trigger collider attached to this object (2D physics only).
        [Notifier(typeof(Trigger2DNotifier))]
        [Func(FuncType.Collider2DEvent)]
        OnTriggerStay2D,//	Sent each frame where another object is within a trigger collider attached to this object (2D physics only).

     

        [Notifier(typeof(GizmosNotifier))]
        [Func(FuncType.Common)]
        OnDrawGizmos,//	Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
        [Notifier(typeof(GizmosNotifier))]
        [Func(FuncType.Common)]
        OnDrawGizmosSelected,//	Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected.
        [Notifier(typeof(GizmosNotifier))]
        [Func(FuncType.Common)]
        OnGUI,//	OnGUI is called for rendering and handling GUI events.

        [Notifier(typeof(TransformChangedNotifier))]
        [Func(FuncType.Common)]
        OnTransformChildrenChanged,//	This function is called when the list of children of the transform of the GameObject has changed.
        [Notifier(typeof(TransformChangedNotifier))]
        [Func(FuncType.Common)]
        OnTransformParentChanged,//	This function is called when the parent property of the transform of the GameObject has changed.
        [Notifier(typeof(TransformChangedNotifier))]
        [Func(FuncType.Common)]
        OnBeforeTransformParentChanged,
        [Notifier(typeof(LevelLoadNotifier))]
        [Func(FuncType.Common)]
        OnLevelWasLoaded,//	This function is called after a new level was loaded.

       

        [Notifier(typeof(VisibleNotifier))]
         [Func(FuncType.Common)]
        OnBecameInvisible,//	OnBecameInvisible is called when the renderer is no longer visible by any camera.
        [Notifier(typeof(VisibleNotifier))]
         [Func(FuncType.Common)]
        OnBecameVisible,//	OnBecameVisible is called when the renderer became visible by any camera.
  
        [Notifier(typeof(ApplicationNotifier))]
        [Func(FuncType.BoolEvent)]
        OnApplicationFocus,//	Sent to all game objects when the player gets or loses focus.
        [Notifier(typeof(ApplicationNotifier))]
        [Func(FuncType.BoolEvent)]
        OnApplicationPause,//	Sent to all game objects when the player pauses.
        [Notifier(typeof(ApplicationNotifier))]
        [Func(FuncType.BoolEvent)]
        OnApplicationQuit,//	Sent to all game objects before the application is quit.



        [Notifier(typeof(UIBehaviourNotifier))]
         [Func(FuncType.Common)]
        OnRectTransformDimensionsChange,
        [Notifier(typeof(UIBehaviourNotifier))]
         [Func(FuncType.Common)]
        OnCanvasGroupChanged,
        [Notifier(typeof(UIBehaviourNotifier))]
         [Func(FuncType.Common)]
        OnCanvasHierarchyChanged,
        [Notifier(typeof(UIBehaviourNotifier))]
         [Func(FuncType.Common)]
        OnDidApplyAnimationProperties,
        COUNT,

    }

}
