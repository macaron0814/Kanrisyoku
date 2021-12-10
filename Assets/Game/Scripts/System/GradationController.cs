using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GradationController : BaseMeshEffect
{
    public Graphic graphic;

    public Color colorTop = Color.white;
    public Color colorBottom = Color.white;

    void Start()
    {
        graphic = GetComponent<Graphic>();
    }

    //メッシュを生成
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;

        List<UIVertex> vertices = new List<UIVertex>();

        vh.GetUIVertexStream(vertices);

        Gradation(vertices);

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertices);// 変更を加えたメッシュを戻す
    }

    private void Gradation(List<UIVertex> vertices)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            UIVertex newVertex = vertices[i];
            newVertex.color = (i % 6 == 0 || i % 6 == 1 || i % 6 == 5) ? colorTop : colorBottom;

            vertices[i] = newVertex;
        }
    }
}