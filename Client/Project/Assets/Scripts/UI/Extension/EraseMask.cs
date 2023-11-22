using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
using UnityEngine.UI;
using UnityEngine.Events;
namespace UnityEngine.UI
{
    [XLua.LuaCallCSharp]
    public class EraseMask : MonoBehaviour, IDragHandler
    {

        /// <summary> 擦除的像素数量 </summary>
        private int m_PixelAcount = 0;

        /// <summary> 是否擦除成功 </summary>
        private bool m_IsDrag = false;

        /// <summary> 擦除范围大小 </summary>
        [SerializeField]
        [Range(10, 100)]
        private int Radius = 10;

        /// <summary> 擦除完成度（不超过1）</summary>
        [SerializeField]
        [Range(0, 1)]
        private float m_Complete;

        private RawImage m_UITex;

        private Texture2D m_MyTex;

        [SerializeField]
        private Color m_Col = Color.clear;

        private int[][] m_PixelArray;

        private Dictionary<int, TexturePixel> m_TexPixelDic = new Dictionary<int, TexturePixel>();

        private bool isStartEraser;
        public UnityEvent onComplete = new UnityEvent();
        public UnityEvent onStart = new UnityEvent();

        void Awake()
        {
            m_IsDrag = false;
            m_UITex = GetComponent<RawImage>();
            var tex = m_UITex.texture as Texture2D;

            m_MyTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32,
                false);

            m_MyTex.SetPixels(tex.GetPixels());
            m_MyTex.Apply();
            m_UITex.texture = m_MyTex;

            int value = 0;
            m_PixelArray = new int[m_MyTex.width][];
            for (int i = 0; i < m_PixelArray.Length; i++)
            {
                m_PixelArray[i] = new int[m_MyTex.height];
                for (int j = 0; j < m_MyTex.height; j++)
                {
                    m_PixelArray[i][j] = value;

                    m_TexPixelDic.Add(value, new TexturePixel(m_MyTex, i, j));
                    value++;
                }
            }

        }

        public void Reset()
        {
            if (this.m_PixelAcount > 0)
            {
                this.m_PixelAcount = 0;
                m_IsDrag = false;
                isStartEraser = false;
                foreach (var item in m_TexPixelDic.Values)
                {
                    Profiler.BeginSample("text1");
                    item.reset();
                    Profiler.EndSample();
                }
                Profiler.BeginSample("text2");
                m_MyTex.Apply();
                Profiler.EndSample();
                Profiler.BeginSample("text3");
                Profiler.EndSample();
                m_UITex.texture = m_MyTex;
                m_UITex.color = Color.white;
            }
        }

        /// <summary>
        ///  改变Texture2D像素点颜色
        /// </summary>
        /// <param name="x">Texture2D像素点X轴位置</param>
        /// <param name="y">Texture2D像素点Y轴位置</param>
        /// <param name="radius">改变像素的范围</param>
        /// <param name="col">改变后的颜色</param>
        void ChangePixelColorByCircle(int x, int y, int radius, Color col)
        {
            for (int i = -Radius; i < Radius; i++)
            {
                var py = y + i;
                if (py < 0 || py >= m_MyTex.height)
                {
                    continue;
                }

                for (int j = -Radius; j < Radius; j++)
                {
                    var px = x + j;
                    if (px < 0 || px >= m_MyTex.width)
                    {
                        continue;
                    }

                    if (new Vector2(px - x, py - y).magnitude > Radius)
                    {
                        continue;
                    }

                    Profiler.BeginSample("text1");
                    TexturePixel tp; //= texPixelDic[pixelArray[MyTex.width - 1][py]];

                    if (px == 0)
                    {
                        tp = m_TexPixelDic[m_PixelArray[m_MyTex.width - 1][py]];
                        tp.Scratch(m_Col);

                    }

                    tp = m_TexPixelDic[m_PixelArray[px][py]];
                    if (!tp.GetPixel())
                    {
                        m_PixelAcount++;
                    }
                    tp.Scratch(m_Col);

                    Profiler.EndSample();
                }
            }

            Profiler.BeginSample("text2");
            m_MyTex.Apply();
            Profiler.EndSample();
            Profiler.BeginSample("text3");
            Profiler.EndSample();
        }
        /// <summary>
        ///  擦除点
        /// </summary>
        /// <param name="mousePos">鼠标位置</param>
        /// <returns>擦除点</returns>
        Vector2 ScreenPoint2Pixel(Vector2 mousePos)
        {
            float imageWidth = m_UITex.rectTransform.rect.width;
            float imageHeight = m_UITex.rectTransform.rect.height;
            Vector3 imagePos = m_UITex.rectTransform.anchoredPosition3D;
            //求鼠标在image上的位置
            float HorizontalPercent =
                (mousePos.x - (Screen.width / 2 + imagePos.x - imageWidth / 2)) / imageWidth; //鼠标在Image 水平上的位置  %
            float verticalPercent =
                (mousePos.y - (Screen.height / 2 + imagePos.y - imageHeight / 2)) / imageHeight; //鼠标在Image 垂直上的位置  %
            float x = HorizontalPercent * m_MyTex.width;
            float y = verticalPercent * m_MyTex.height;
            return new Vector2(x, y);
        }


        /// <summary>
        ///  拖拽中。。。
        /// </summary>
        /// <param name="eventData">拖拽数据</param>
        public void OnDrag(PointerEventData eventData)
        {
            if (!m_IsDrag)
            {
                var posA = ScreenPoint2Pixel(eventData.position);
                ChangePixelColorByCircle((int)posA.x, (int)posA.y, Radius, m_Col);
                SetAllPixelFadeAlpha();

            }
        }

        /// <summary>
        ///  擦除完成时调用
        /// </summary>
        public void SetAllPixelFadeAlpha()
        {
            if (++m_PixelAcount >= m_MyTex.height * m_MyTex.width * m_Complete)
            {
                m_UITex.color = Color.clear;
                m_IsDrag = true;
                Debug.Log("擦除完成");
                this.onComplete.Invoke();
            }

            if (m_PixelAcount > 0 && !isStartEraser)
            {
                isStartEraser = true;
                this.onStart.Invoke();
            }
        }



        private class TexturePixel
        {
            public Texture2D myTex;
            private int x;      //像素坐标X
            private int y;      //像素坐标Y
            private Color origin;
            public TexturePixel(Texture2D tex, int x, int y)
            {
                myTex = tex;
                this.x = x;
                this.y = y;
                this.origin=myTex.GetPixel(x, y);  
            }

            public void Scratch(Color targetCol)
            {
                myTex.SetPixel(x, y, targetCol);
            }

            public void reset()
            {
                myTex.SetPixel(x, y, origin);
            }

            public bool GetPixel()
            {
                Color color = myTex.GetPixel(x, y);

                return color.a <= 0;
            }
        }


    }
}