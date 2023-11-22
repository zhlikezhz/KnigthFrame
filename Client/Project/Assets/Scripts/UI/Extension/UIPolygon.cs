using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(PolygonCollider2D))]
public class UIPolygon : Image
{
    private PolygonCollider2D _polygon = null;
    private PolygonCollider2D polygon
    {
        get
        {
            if (_polygon == null)
                _polygon = GetComponent<PolygonCollider2D>();
            return _polygon;
        }
    }

    //设置只响应点击，不进行渲染
    //protected UIPolygon()
    //{
    //    useLegacyMeshGeneration = true;
    //}

    //protected override void OnPopulateMesh(VertexHelper vh)
    //{
    //    vh.Clear();
    //}

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector3 point;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);
        var ishit = polygon.OverlapPoint(point);
        return ishit;
    }


}

