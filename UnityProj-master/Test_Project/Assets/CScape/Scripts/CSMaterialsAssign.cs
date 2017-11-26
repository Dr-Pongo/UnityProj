using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSMaterialsAssign : MonoBehaviour
{

    private Vector3[] originalVertices;
    private Vector2[] originalUVs;
    private Color[] originalColors;
    private Vector4[] vColors;
    public Mesh meshOriginal;
    public Mesh mesh;
    public int diffuseID; // diff + mettalic
    public int normalID; // norm + depth + roughness
    public int transparencyID; // transparentMask, illumination

    public void Awake()
    {

        originalVertices = meshOriginal.vertices;
        originalUVs = meshOriginal.uv;
        originalColors = meshOriginal.colors;
        vColors = new Vector4[originalVertices.Length];
        mesh = Instantiate(meshOriginal) as Mesh;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        ModifyMesh();
    }

    // Use this for initialization
    public void ModifyMesh()
    {

        Vector4[] vColorsFloat = new Vector4[mesh.uv.Length];
        Vector3[] vertices = mesh.vertices;
        Vector2[] uV = mesh.uv;

        Vector3[] transformVertices = mesh.vertices;
        Vector2[] transformUV = mesh.uv;

        Vector3[] normals = mesh.normals;

        int i = 0;
        while (i < vertices.Length)
        {
            vColors[i] = new Vector4(diffuseID + normalID * 0.01f + transparencyID * 0.0001f, 0, 0, 0);
            i++;
        }



        var list = new List<Vector4>(vColors);
      //  mesh.vertices = vertices;
      //  mesh.uv = uV;
        mesh.SetUVs(3, list);


     //   mesh.RecalculateNormals();
    //    mesh.RecalculateBounds();

       
    }




    // Update is called once per frame
    void Update()
    {

    }
}
