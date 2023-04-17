using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnVertex : MonoBehaviour
{
    public Material image;

    public GameObject vertex;

    public GameObject gO;

    private GameObject vertex1;
    private GameObject vertex2;
    private GameObject vertex3;
    private GameObject vertex4;

    private List <GameObject> vertices = new List<GameObject>();

    private Mesh mesh;

    private MeshFilter targetObject;

    private int uvChannel = 1;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();

        mesh.name = "Mesh";

        targetObject = gO.GetComponent<MeshFilter>();
        //creating the vertices
        vertex4 = GameObject.Instantiate(vertex, transform.position + new Vector3(250, 0, 0), Quaternion.identity);
        vertex2 = GameObject.Instantiate(vertex, transform.position + new Vector3(250, 250, 0), Quaternion.identity);
        vertex3 = GameObject.Instantiate(vertex, transform.position - new Vector3(250, 0, 0), Quaternion.identity);
        vertex1 = GameObject.Instantiate(vertex, transform.position - new Vector3(250, -250, 0), Quaternion.identity);

        mesh.vertices = new Vector3[] { vertex1.transform.position, vertex2.transform.position, vertex3.transform.position, vertex4.transform.position };
        mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        Vector2[] uvs = new Vector2[] {new Vector2(1,1), new Vector2(0,1), new Vector2(1,0), new Vector2(0,0)};
        mesh.uv = uvs;
        //adding the vertices to the list
        vertices.Add(vertex1);
        vertices.Add(vertex2);
        vertices.Add(vertex3);
        vertices.Add(vertex4);

        //setting the canvas as the parent of the vertices
        vertex1.transform.parent = transform;
        vertex2.transform.parent = transform;
        vertex3.transform.parent = transform;
        vertex4.transform.parent = transform;

        //setting the names of the vertices for clarity
        vertex1.name = "Vertex 1";
        vertex2.name = "Vertex 2";
        vertex3.name = "Vertex 3";
        vertex4.name = "Vertex 4";

        gO.GetComponent<MeshFilter>().mesh = mesh;
        gO.GetComponent<MeshRenderer>().material = image;

        gO.transform.position += new Vector3(0, 0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(vertex1.GetComponent<VertexMovement>().hasChanged || vertex2.GetComponent<VertexMovement>().hasChanged || vertex3.GetComponent<VertexMovement>().hasChanged || vertex4.GetComponent<VertexMovement>().hasChanged)
        {
            mesh.vertices = new Vector3[] { vertex1.transform.position, vertex2.transform.position, vertex3.transform.position, vertex4.transform.position };
            GenerateUV();
        }
    }

    void GenerateUV()
    {
        var targetMesh = targetObject.sharedMesh;
        var vertices2 = targetMesh.vertices.Select(v =>
        {
            var point = targetObject.transform.TransformPoint(v);
            return point;
        }).ToArray();
        var uvList = vertices2.Select(v => GetComponent<Camera>().WorldToViewportPoint(v)).ToList();
        var newMesh = Instantiate(targetMesh);
        newMesh.SetUVs(uvChannel, uvList);

        var path = AssetDatabase.GetAssetPath(targetMesh);
        path = System.IO.Path.Combine("Assets/Prefabs/", targetMesh.name + string.Format("_ProjectedUV{0}.asset", uvChannel));
		AssetDatabase.CreateAsset(newMesh, path);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();

        Selection.activeObject = newMesh;

        var go = new GameObject(name + " UV Editor");
        go.AddComponent<MeshFilter>().sharedMesh = newMesh;

        var editor = go.AddComponent<ProjectionUVEditor>();
        editor.projectionCam = GetComponent<Camera>();
		editor.uvChannel = uvChannel;
		newMesh.GetUVs(uvChannel, editor.editUVs);
    }

    //not needed anymore, but maybe useful later on

    void Recreate()
    {
        mesh.vertices = new Vector3[] { vertex1.transform.position, vertex2.transform.position, vertex3.transform.position, vertex4.transform.position };
    }
}
