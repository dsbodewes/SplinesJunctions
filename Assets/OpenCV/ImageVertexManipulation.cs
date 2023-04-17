using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageVertexManipulation : RawImage
{
    // Start is called before the first frame update
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);

        Debug.Log("OnPopulateMesh");

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            UIVertex vert = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref vert, i);
            Vector3 position = vert.position;

            //
            //manipulate position
            //

            vert.position = position;
            vh.SetUIVertex(vert, 1);
        }
    }
}
