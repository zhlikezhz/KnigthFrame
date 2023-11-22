

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[XLua.LuaCallCSharp]
[RequireComponent(typeof(RectTransform)), ExecuteInEditMode]
public class AnnularSlider : Selectable, IDragHandler, ICanvasElement
{
    [Serializable]
    public class DragEvent : UnityEvent
    {
    }

    [Serializable]
    public class DragValueEvent : UnityEvent<float>
    {
    }

    [SerializeField] private Image _fillImage;
    [SerializeField] private Image.Origin360 _fillOrigin;
    [SerializeField] private bool _clockwise;
    [SerializeField] private bool _wholeNumbers;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue = 1f;
    [SerializeField] private float _maxAngle = 360f;
    [SerializeField] private float _value;

    [SerializeField] private RectTransform _handleRect;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private bool _towardCenter;

    [SerializeField] private DragValueEvent _onValueChanged = new DragValueEvent();
    [SerializeField] private DragEvent _onBeginDragged = new DragEvent();
    [SerializeField] private DragEvent _onDragging = new DragEvent();
    [SerializeField] private DragEvent _onEndDragged = new DragEvent();

    private bool _delayedUpdateVisuals;

    public Image fillImage
    {
        get { return _fillImage; }
        set
        {
            if (SetClass(ref _fillImage, value))
            {
                UpdateCachedReferences();
                UpdateVisuals();
            }
        }
    }

    public Image.Origin360 fillOrigin
    {
        get { return _fillOrigin; }
        set
        {
            if (SetStruct(ref _fillOrigin, value))
            {
                UpdateVisuals();
            }
        }
    }

    public bool clockwise
    {
        get { return _clockwise; }
        set
        {
            if (SetStruct(ref _clockwise, value))
            {
                UpdateVisuals();
            }
        }
    }

    public bool wholeNumbers
    {
        get { return _wholeNumbers; }
        set
        {
            if (SetStruct(ref _wholeNumbers, value))
            {
                UpdateValue(_value);
                UpdateVisuals();
            }
        }
    }

    public float minValue
    {
        get { return _minValue; }
        set
        {
            if (SetStruct(ref _minValue, value))
            {
                UpdateValue(_value);
                UpdateVisuals();
            }
        }
    }

    public float maxValue
    {
        get { return _maxValue; }
        set
        {
            if (SetStruct(ref _maxValue, value))
            {
                UpdateValue(_value);
                UpdateVisuals();
            }
        }
    }

    public float maxAngle
    {
        get { return _maxAngle; }
        set
        {
            if (SetStruct(ref _maxAngle, value))
            {
                UpdateVisuals();
            }
        }
    }

    public float value
    {
        get
        {
            if (_wholeNumbers) return Mathf.Round(_value);
            return _value;
        }

        set { UpdateValue(value); }
    }

    public RectTransform handleRect
    {
        get { return _handleRect; }
        set
        {
            if (SetClass(ref _handleRect, value))
            {
                UpdateVisuals();
            }
        }
    }

    public float radius
    {
        get { return _radius; }
        set
        {
            if (SetStruct(ref _radius, value))
            {
                UpdateVisuals();
            }
        }
    }

    public bool towardCenter
    {
        get { return _towardCenter; }
        set
        {
            if (SetStruct(ref _towardCenter, value))
            {
                UpdateVisuals();
            }
        }
    }

    public DragValueEvent onValueChanged
    {
        get { return _onValueChanged; }
        set { _onValueChanged = value; }
    }

    public DragEvent onBeginDragged
    {
        get { return _onBeginDragged; }
        set { _onBeginDragged = value; }
    }

    public DragEvent onDragging
    {
        get { return _onDragging; }
        set { _onDragging = value; }
    }

    public DragEvent onEndDragged
    {
        get { return _onEndDragged; }
        set { _onEndDragged = value; }
    }

    public float normalizedValue
    {
        get
        {
            if (Mathf.Approximately(_minValue, _maxValue)) return 0;
            return Mathf.InverseLerp(_minValue, _maxValue, value);
        }
        set { value = Mathf.Lerp(_minValue, _maxValue, value); }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateCachedReferences();
        UpdateValue(_value, false);
        UpdateVisuals();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        if (wholeNumbers)
        {
            _minValue = Mathf.Round(_minValue);
            _maxValue = Mathf.Round(_maxValue);
        }

        //Onvalidate is called before OnEnabled. We need to make sure not to touch any other objects before OnEnable is run.
        if (IsActive())
        {
            UpdateCachedReferences();
            UpdateValue(_value, false);
            _delayedUpdateVisuals = true;
        }


        //if (!UnityEditor.PrefabUtility.IsComponentAddedToPrefabInstance(this) && !Application.isPlaying)
        //    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
    }
#endif

    protected virtual void Update()
    {
        if (_delayedUpdateVisuals)
        {
            _delayedUpdateVisuals = false;
            UpdateVisuals();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (MayEvent(eventData))
        {
            onBeginDragged.Invoke();
            //Debug.Log("OnBeginDragged");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!MayEvent(eventData)) return;
        _onDragging.Invoke();

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_fillImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            //根据鼠标在rect的位置=>求出角度（相对于X轴右侧 180°表示）=>相对于起始点的角度（360° 表示）=>_fillImage.fillAmount、_handleRect.localPosition
            var angle = GetAngleFromFillOrigin(localPoint); //相对角度 [0f-360f]
            //超出最大角度时，等于相近的边界值
            if (angle > _maxAngle)
            {
                angle = angle - _maxAngle < 360 - angle ? _maxAngle : 0f;
            }

            normalizedValue = angle / _maxAngle;
            UpdateVisuals();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (MayEvent(eventData))
        {
            onEndDragged.Invoke();
            //Debug.Log("OnEndDragged");
        }
    }

    public void Rebuild(CanvasUpdate executing)
    {
    }

    public void LayoutComplete()
    {
    }

    public void GraphicUpdateComplete()
    {
    }

    /// <summary>
    /// 返回是否可交互
    /// </summary>
    /// <returns></returns>
    private bool MayEvent(PointerEventData eventData)
    {
        return IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
    }

    /// <summary>
    /// 更新缓存引用
    /// </summary>
    private void UpdateCachedReferences()
    {
        if (_fillImage)
        {
            _fillImage.type = Image.Type.Filled;
            _fillImage.fillMethod = Image.FillMethod.Radial360;
            _fillImage.fillOrigin = (int) _fillOrigin;
            _fillImage.fillClockwise = _clockwise;
        }
    }

    /// <summary>
    /// 更新视觉效果
    /// </summary>
    private void UpdateVisuals()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            UpdateCachedReferences();
#endif

        var angle = normalizedValue * _maxAngle;

        if (_fillImage)
        {
            _fillImage.fillAmount = angle / 360f;
        }

        if (_handleRect)
        {
            _handleRect.transform.localPosition = GetPointFromFillOrigin(ref angle);
            if (_towardCenter)
            {
                _handleRect.transform.localEulerAngles = new Vector3(0f, 0f, angle);
            }
        }
    }

    /// <summary>
    /// 更新Value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="sendCallback"></param>
    private void UpdateValue(float value, bool sendCallback = true)
    {
        value = Mathf.Clamp(value, _minValue, _maxValue);
        if (_wholeNumbers) value = Mathf.Round(value);
        if (_value.Equals(value)) return;
        _value = value;

        UpdateVisuals();
        if (sendCallback)
        {
            _onValueChanged.Invoke(_value);
            //Debug.Log("OnValueChanged" + _value);
        }
    }

    /// <summary>
    /// 返回基于起始点的角度（0°~360°）
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private float GetAngleFromFillOrigin(Vector2 point)
    {
        var angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg; //相对于X轴右侧（FillOrigin.Right）的角度
        //转换为相对于起始点的角度
        switch (_fillOrigin)
        {
            case Image.Origin360.Bottom:
                angle = _clockwise ? 270 - angle : 90 + angle;
                break;
            case Image.Origin360.Right:
                angle = _clockwise ? -angle : angle;
                break;
            case Image.Origin360.Top:
                angle = _clockwise ? 90 - angle : 270 + angle;
                break;
            case Image.Origin360.Left:
                angle = _clockwise ? 180 - angle : 180 + angle;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //转 360 °表示
        if (angle > 360)
        {
            angle -= 360;
        }

        if (angle < 0)
        {
            angle += 360;
        }

        return angle;
    }

    /// <summary>
    /// 返回基于起始点、角度、半径的位置
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private Vector2 GetPointFromFillOrigin(ref float angle)
    {
        //转化为相对于X轴右侧（FillOrigin.Right）的角度
        switch (_fillOrigin)
        {
            case Image.Origin360.Bottom:
                angle = _clockwise ? 270 - angle : angle - 90;
                break;
            case Image.Origin360.Right:
                angle = _clockwise ? -angle : angle;
                break;
            case Image.Origin360.Top:
                angle = _clockwise ? 90 - angle : 90 + angle;
                break;
            case Image.Origin360.Left:
                angle = _clockwise ? 180 - angle : 180 + angle;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var radian = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian) * _radius, Mathf.Sin(radian) * _radius);
    }

    private static bool SetStruct<T>(ref T setValue, T value) where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(setValue, value)) return false;
        setValue = value;
        return true;
    }

    private static bool SetClass<T>(ref T setValue, T value) where T : class
    {
        if (setValue == null && value == null || setValue != null && setValue.Equals(value)) return false;
        setValue = value;
        return true;
    }
}