using UnityEngine;
using System.Collections;
using CScape;


namespace CScape
{

    //[ExecuteInEditMode]
    public class StreetModifier : MonoBehaviour
    {


        public Vector3 lowFloorBound;
        public Vector3 lowFloorBoundSecond;
        public int prefabFloors;
        public int prefabDepth;
        public int prefabWidth;
        public int prefabCenterSectionDepth;
        public int prefabCenterSectionWidth;
        public Vector3 size;
        private Vector3[] originalVertices;
  //      private Vector2[] originalUVs;
        private Color[] originalColors;
        private Color[] vColors;
        public float scale = 1;
        public Mesh meshOriginal;
        public Mesh mesh;
        public int floorNumber;
        public float floorHeight = 3f;
        public int uniqueMapping;
        public int blockWidth;
        public int sectionWidth;
        public int blockDepth;
        public int sectionDepth;
        public float normalThreshold = 0.1f;
        public Vector2 id1;
        public Vector2 id2;
        public Vector2 id3;
        public int materialId1;
        public int materialId2;
        public int materialId3;
        public int sidewalkID;
        public int streetID;
        public float windowOpen;
        public GameObject rooftopObject;
        public float pattern;
        public Vector2 colorVariation;
       // public float idimidodjimi;
        public bool prefabHasVertexInfo = true;
        public StreetModifier[] parentStreets; //self, front, right, back, left
        public BuildingModifier[] connectedSections; //childBuilding, front, right, back, left
        public enum CScapeStreetType { Street, River, Park };
        public enum CScapeStreetStyle { Business, OldTown, Residential, Industrial};
        public CScapeStreetType streetType;






        public void AwakeCity()
        {

            //meshOriginal = GetComponent<MeshFilter>().sharedMesh;
            originalVertices = meshOriginal.vertices;
         //   originalUVs = meshOriginal.uv;
            originalColors = meshOriginal.colors;
            vColors = new Color[originalVertices.Length];
            mesh = Instantiate(meshOriginal) as Mesh;
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            //		gameObject.transform.GetChild (0);
            ModifyBuilding();

        }


        public void UpdateCity()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                this.enabled = false;
            }
            else ModifyBuilding();
#else
        this.enabled = false;
#endif

            //		ModifyNormals();

        }




        public void ModifyBuilding()
        {

            id1.x = CompressIDs(materialId1);
            id2.x = CompressIDs(materialId2);
            id3.x = CompressIDs(materialId3);



            transform.localScale = new Vector3(scale, 1, 1);

            Vector3[] vertices = mesh.vertices;
            Vector2[] uV = mesh.uv;

            Vector3[] transformVertices = mesh.vertices;
            Vector2[] transformUV = mesh.uv;

            Vector3[] normals = mesh.normals;
            Vector2[] uV3 = mesh.uv;

            int i = 0;
            while (i < vertices.Length)
            {
                transformVertices[i] = new Vector3(0, 0, 0);
                transformUV[i] = new Vector2(0, 0);

              //  vColors[i] = new Color(id1.x, id1.y, pattern, CompressVector2(colorVariation)) / 10;
          //      Vector3 invNormal = normals[i] * -1;





                if (originalVertices[i].x > lowFloorBound.x && originalVertices[i].x < lowFloorBoundSecond.x)
                {
                    if (blockWidth < prefabWidth) blockWidth = prefabWidth;
                    transformVertices[i].x = (sectionWidth - prefabWidth +1) * floorHeight;
                }


                if (originalVertices[i].z > lowFloorBound.z && originalVertices[i].z < lowFloorBoundSecond.z)
                {
                    if (blockDepth < prefabDepth) blockDepth = prefabDepth;
                    transformVertices[i].z = (sectionDepth - prefabDepth + 1) * floorHeight;
                }

                if (originalVertices[i].x > lowFloorBoundSecond.x)
                {
                    if (blockWidth < prefabWidth) blockWidth = prefabWidth;
                    transformVertices[i].x = (blockWidth - prefabWidth) * floorHeight;
                }


                if (originalVertices[i].z > lowFloorBoundSecond.z)
                {
                    if (blockDepth < prefabDepth) blockDepth = prefabDepth;
                    transformVertices[i].z = (blockDepth - prefabDepth) * floorHeight;

                    transformUV[i].x = transformVertices[i].x / 3f;
                    transformUV[i].y = transformVertices[i].z / 3f;
                }

                transformUV[i].x = originalVertices[i].x + transformVertices[i].x / 3f;
                transformUV[i].y = originalVertices[i].z + transformVertices[i].z / 3f;
                Vector3 invNormal = normals[i] * -1;

                ///////// manipulate Vertex colors
                if (prefabHasVertexInfo)
                {
                    if (originalColors[i].r == 1f && originalColors[i].g == 0f && originalColors[i].b == 0f)
                    {

                        vColors[i] = new Color((materialId1 * 0.1f) + 0.00001f, 0, 0, 0);

                    }

                    else if (originalColors[i].r == 1f && originalColors[i].g == 1f && originalColors[i].b == 0f)
                    {

                        vColors[i] = new Color((materialId2 * 0.1f) + 0.00001f, 0, 0, 0);

                    }

                    else if (originalColors[i].r == 0f && originalColors[i].g == 1f && originalColors[i].b == 0f)
                    {

                        vColors[i] = new Color(0, (materialId3 * 0.1f) + 0.00001f, 0, 0);

                    }

                    else if (originalColors[i].r == 1f && originalColors[i].g == 0f && originalColors[i].b == 1f)
                    {

                        vColors[i] = new Color((materialId3 * 0.1f) + 0.00001f, 0, 0, 0);

                    }

                    else if (originalColors[i].r == 1f && originalColors[i].g == 1f && originalColors[i].b == 1f)
                    {

                        vColors[i] = new Color(((materialId3 + 1) * 0.1f) + 0.00001f, 0, 0, 0);

                    }
                    else vColors[i] = new Color(0, 0, 0, 0);

//Set IDS

                    if (originalColors[i].r == 0f && originalColors[i].g == 0f && originalColors[i].b == 0f)
                    {
                        uV3[i] = new Vector2(sidewalkID, streetID);
                    }
                    else uV3[i] = new Vector2(streetID, 0f);

                }


                ////////

                vertices[i] = originalVertices[i] + transformVertices[i];
                if (invNormal.z < 1 + normalThreshold && invNormal.z > 1 - normalThreshold)
                {
                    uV[i] = new Vector2(transformVertices[i].y, transformVertices[i].z);
                }

                else if (normals[i].z < 1 + normalThreshold && normals[i].z > 1 - normalThreshold)
                {
                    uV[i] = new Vector2(originalVertices[i].x + transformVertices[i].x, originalVertices[i].z + transformVertices[i].z) / 3f;
                }
                else if (normals[i].x < 1 + normalThreshold && normals[i].x > 1 - normalThreshold)
                {
                    uV[i] = new Vector2(originalVertices[i].x + transformVertices[i].x, originalVertices[i].z + transformVertices[i].z) / 3f;
                }
                else if (invNormal.x < 1 + normalThreshold && invNormal.x > 1 - normalThreshold)
                {
                    uV[i] = new Vector2(originalVertices[i].x + transformVertices[i].x, originalVertices[i].z + transformVertices[i].z) / 3f;
                }

                else uV[i] = new Vector2(originalVertices[i].x + transformVertices[i].x, originalVertices[i].z + transformVertices[i].z) / 3f;
                i++;
            }


            mesh.vertices = vertices;
            mesh.colors = vColors;
            // mesh.colors = originalColors;
            mesh.uv = uV;
            mesh.uv4 = uV3;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();



        }

        float CompressIDs(int iDInput)
        {

            float idOut = (iDInput + 1f) / 10f;


            return idOut;


        }

        float CompressVector2(Vector2 vectorInput)
        {
            ///compress vector2 Index
            float idOut = 0;
            idOut = (Mathf.Floor(vectorInput.x)) + (vectorInput.y % 1);


            return idOut;


        }



    }
}