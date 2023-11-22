using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine.UI
{
 public    class MirrorSprite:BaseMeshEffect
    {
        public enum eMirrorType
        {
            eHor_left,
            eHor_right,
            eVer_up,
            eVer_down,
        }

        [SerializeField]
        private eMirrorType mMirrorType;

        private List<UIVertex> mVerts = new List<UIVertex>();
        private List<int> mIndices = new List<int>();
        private UIVertex mVertex = new UIVertex();

        protected override void Awake()
        {
            mMirrorType = eMirrorType.eHor_right;
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            mVerts.Clear();
            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref mVertex, i);
                mVerts.Add(mVertex);
            }

            if (graphic is Image)
            {
                Image image = graphic as Image;
                switch (image.type)
                {
                    case Image.Type.Simple:
                        {
                            DrawSimple();
                        }
                        break;
                    case Image.Type.Sliced:
                        {
                            DrawSliced();
                        }
                        break;
                    case Image.Type.Tiled:
                        {

                        }
                        break;
                }
            }

            vh.Clear();
            vh.AddUIVertexStream(mVerts, mIndices);
        }

        public void SetIndice()
        {
            mIndices.Clear();
            for (int i = 0; i < mVerts.Count;)
            {
                mIndices.Add(i);
                mIndices.Add(i + 1);
                mIndices.Add(i + 2);

                mIndices.Add(i);
                mIndices.Add(i + 2);
                mIndices.Add(i + 3);

                i = i + 4;
            }
        }

        //为了解决拼接处的缝隙问题，让uv进行偏移
        public void FixUV()
        {
            for (int i = 0; i < mVerts.Count; i++)
            {
                var vertex = mVerts[i];
                Vector2 uv = vertex.uv0;
                int remainder = i % 4;

                if (mMirrorType == eMirrorType.eHor_right)
                {
                    if (remainder == 2 || remainder == 3)
                    {
                        uv.x -= 1.5f / Screen.width;
                    }
                }

                if (mMirrorType == eMirrorType.eVer_up)
                {
                    if (remainder == 1 || remainder == 2)
                    {
                        uv.y -= 1.5f / Screen.height;
                    }
                }

                vertex.uv0 = uv;
                mVerts[i] = vertex;
            }
        }

        //放缩
        //public void SimpleScale()
        //{
        //    Rect rect = graphic.GetPixelAdjustedRect();
        //    RectTransform rt = graphic.rectTransform;

        //    for (int i = 0; i < mVerts.Count; i++)
        //    {
        //        var vertex = mVerts[i];
        //        Vector3 position = vertex.position;


        //        if(mMirrorType == eMirrorType.eHor)
        //        {
        //            position.x = (position.x + rect.x) * 0.5f;
        //        }

        //        if (mMirrorType == eMirrorType.eVer)
        //        {
        //            position.y = (position.y + rect.y) * 0.5f;
        //        }

        //        vertex.position = position;
        //        mVerts[i] = vertex;
        //    }
        //}

        //镜像图片
        public void Mirror()
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            switch (mMirrorType)
            {
                case eMirrorType.eHor_right:
                    {
                        int count = mVerts.Count;
                        for (int i = 0; i < count; i++)
                        {
                            //公式: x1 - x = x - x2 => x1 = 2x - x2 (x1在对称轴x的右侧，x2在对称轴x的左侧)
                            var vertex = mVerts[i];
                            var position = vertex.position;
                            position.x = rect.max.x * 2 - position.x;
                            vertex.position = position;
                            mVerts.Add(vertex);
                        }
                    }
                    break;
                case eMirrorType.eHor_left:
                    {
                        int count = mVerts.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var vertex = mVerts[i];
                            var position = vertex.position;
                            position.x = rect.min.x * 2 - position.x;
                            vertex.position = position;
                            mVerts.Add(vertex);
                        }
                    }
                    break;
                case eMirrorType.eVer_up:
                    {
                        int count = mVerts.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var vertex = mVerts[i];
                            var position = vertex.position;
                            position.y = rect.max.y * 2 - position.y;
                            vertex.position = position;
                            mVerts.Add(vertex);
                        }
                    }
                    break;
                case eMirrorType.eVer_down:
                    {
                        int count = mVerts.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var vertex = mVerts[i];
                            var position = vertex.position;
                            position.y = rect.min.y * 2 - position.y;
                            vertex.position = position;
                            mVerts.Add(vertex);
                        }
                    }
                    break;
            }
        }

        public void DrawSimple()
        {
            FixUV();
            Mirror();
            SetIndice();
        }

        public void DrawSliced()
        {
            FixUV();
            Mirror();
            SetIndice();
        }
    }
}
