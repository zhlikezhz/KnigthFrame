
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GradientType
{
    OneColor = 0,
    TwoColor,
    ThreeColor
}

[DisallowMultipleComponent]
public class Gradient : BaseMeshEffect
{

    [SerializeField] private GradientType m_GradientType = GradientType.TwoColor;
    [SerializeField] private Color32 m_TopColor = Color.white;
    [SerializeField] private Color32 m_MiddleColor = Color.white;
    [SerializeField] private Color32 m_BottomColor = Color.white;



    [UnityEngine.Range(0.1f, 0.9f)] [SerializeField] private float m_ColorOffset = 0.5f;


    private List<UIVertex> iVertices = new List<UIVertex>();

    private Text m_Text;

    public Text TextGraphic
    {
        get
        {
            if (!this.m_Text && base.graphic)
            {
                this.m_Text = base.graphic as Text;
            }
            else
            {
                if (!base.graphic)
                    throw new Exception("No Find base Graphic!!");
            }

            return this.m_Text;
        }
    }





    private void _ProcessVertices(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        var count = vh.currentVertCount;
        if (count == 0)
            return;

        /*
         *  TL--------TR
         *  |          |^
         *  |          ||
         *  CL--------CR
         *  |          ||
         *  |          |v
         *  BL--------BR
         * **/


        for (int i = 0; i < count; i++)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref vertex, i);
            this.iVertices.Add(vertex);
        }
        vh.Clear();

        for (int i = 0; i < this.iVertices.Count; i += 4)
        {

            UIVertex TL = GeneralUIVertex(this.iVertices[i + 0]);
            UIVertex TR = GeneralUIVertex(this.iVertices[i + 1]);
            UIVertex BR = GeneralUIVertex(this.iVertices[i + 2]);
            UIVertex BL = GeneralUIVertex(this.iVertices[i + 3]);

            //先绘制上四个
            UIVertex CR = default(UIVertex);
            UIVertex CL = default(UIVertex);

            //如果是OneColor模式，则颜色不做二次处理
            if (this.m_GradientType == GradientType.OneColor)
            {

            }
            else
            {
                TL.color = this.m_TopColor;
                TR.color = this.m_TopColor;
                BL.color = this.m_BottomColor;
                BR.color = this.m_BottomColor;
            }



            vh.AddVert(TL);
            vh.AddVert(TR);

            if (this.m_GradientType == GradientType.ThreeColor)
            {
                CR = GeneralUIVertex(this.iVertices[i + 2]);
                CL = GeneralUIVertex(this.iVertices[i + 3]);
                //var New_S_Point = this.ConverPosition(TR.position, BR.position, this.m_ColorOffset);

                CR.position.y = Mathf.Lerp(TR.position.y, BR.position.y - 0.1f, this.m_ColorOffset);
                CL.position.y = Mathf.Lerp(TR.position.y, BR.position.y, this.m_ColorOffset);



                if (Mathf.Approximately(TR.uv0.x, BR.uv0.x))
                {
                    CR.uv0.y = Mathf.Lerp(TR.uv0.y, BR.uv0.y, this.m_ColorOffset);
                    CL.uv0.y = Mathf.Lerp(TL.uv0.y, BL.uv0.y, this.m_ColorOffset);
                }
                else
                {
                    CR.uv0.x = Mathf.Lerp(TR.uv0.x, BR.uv0.x, this.m_ColorOffset);
                    CL.uv0.x = Mathf.Lerp(TL.uv0.x, BL.uv0.x, this.m_ColorOffset);
                }


                CR.color = this.m_MiddleColor;
                CL.color = this.m_MiddleColor;
                // CR.color = Color32.Lerp(this.m_MiddleColor, this.m_BottomColor, this.m_LerpValue);
                // CL.color = Color32.Lerp(this.m_MiddleColor, this.m_BottomColor, this.m_LerpValue);
                vh.AddVert(CR);
                vh.AddVert(CL);
            }

            //绘制下四个
            if (this.m_GradientType == GradientType.ThreeColor)
            {
                vh.AddVert(CL);
                vh.AddVert(CR);
            }
            vh.AddVert(BR);
            vh.AddVert(BL);

        }

        for (int i = 0; i < vh.currentVertCount; i += 4)
        {
            vh.AddTriangle(i + 0, i + 1, i + 2);
            vh.AddTriangle(i + 2, i + 3, i + 0);
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        this.iVertices.Clear();

        this._ProcessVertices(vh);
    }




    public static UIVertex GeneralUIVertex(UIVertex vertex)
    {
        UIVertex result = UIVertex.simpleVert;
        result.normal = new Vector3(vertex.normal.x, vertex.normal.y, vertex.normal.z);
        result.position = new Vector3(vertex.position.x, vertex.position.y, vertex.position.z);
        result.tangent = new Vector4(vertex.tangent.x, vertex.tangent.y, vertex.tangent.z, vertex.tangent.w);
        result.uv0 = new Vector2(vertex.uv0.x, vertex.uv0.y);
        result.uv1 = new Vector2(vertex.uv1.x, vertex.uv1.y);
        result.color = vertex.color;
        return result;
    }
}
