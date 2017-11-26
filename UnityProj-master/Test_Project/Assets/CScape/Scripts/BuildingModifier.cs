using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using CScape;
using System.Threading;
//using UnityEditor;

namespace CScape {

      [ExecuteInEditMode]
    public class BuildingModifier : MonoBehaviour
    {

#if UNITY_EDITOR
        public Vector3 lowFloorBound;
        public int prefabFloors;
        public int prefabDepth;
        public int prefabWidth;
        public Vector3 size;
        private Vector3[] originalVertices;
        private Vector2[] originalUVs;
        private Color[] originalColors;
        private Vector4[] vColors;
      //  private Vector2[] secondUVs;
        public float scale = 1;
        public Mesh meshOriginal;
        public Mesh mesh;
        public int floorNumber;
        public float floorHeight;
        public int uniqueMapping;
        public int buildingWidth;
        public int buildingDepth;
        public float normalThreshold = 0.1f;
        public Vector2 id1;
        public Vector2 id2;
        public Vector2 id3;
        public Vector2 id4;
        public int materialId1;
        public int materialId2;
        public int materialId3;
        public int materialId4;
        public int materialId5;
        public int divisionId4;
        public int lightnessFront;
        public int lightnessSide;
        public float windowOpen;
        public GameObject rooftopObject;
        public bool generateLightmappingUV = false;
        public float pattern;
        public Vector4 colorVariation;
        public Vector4 colorVariation2;
        public Vector4 colorVariation3;
        public Vector4 colorVariation4;
        public Vector4 colorVariation5;
        public float lightsOnOff;
        public float lightsOnOff2;

        public bool prefabHasVertexInfo = false;
        public Vector4 advertisingPanelCoord;
        public float lightVec;
        public float lightVec2;
        public float lightVec3;
        public float lightVec4;
        public float lightVec5;
        public float lightVec6;
        public float lightVec7;
        public float lightVec8;
        public CityRandomizer cityRandomizerParent;
        public StreetModifier[] parentStreets; //center, front, right, back, left
        public BuildingModifier[] connectedSections; //self, front, right, back, left
        public Vector2[] roofOffsetX;
        public Vector2[] roofOffsetZ;
        public Vector2[] advertOffsetX;
        public Vector2[] advertOffsetZ;
        public Vector3 advertSide; //advertising on face
        public float[] roofOffsetY;
        public float[] advertOffsetY;
        public bool hasRooftops = false;
        public bool hasAdvertising = false;
        public bool hasBalcony = false;
        public CSRooftops rooftops;
        public CSAdvertising advertising;
        public CSArray balcony;
        public bool distort = false;
        public float distortXZ = 0;
        public bool useAdvertising = true;
        public int borderCol;
        //  public MeshCollider collider;



        private void Awake()
        {
            if (!Application.isPlaying) { 
            advertising = null;
                rooftops = null;
            }
        }

        public void AwakeCity()
        {

            // if (cityRandomizerParent == null) cityRandomizerParent = GameObject.Find("CScape City").GetComponent<CityRandomizer>() as CityRandomizer;
            if (cityRandomizerParent == null) cityRandomizerParent = gameObject.transform.parent.transform.parent.GetComponent<CityRandomizer>() as CityRandomizer;

            rooftops = gameObject.GetComponent<CSRooftops>();
            if (gameObject.GetComponent<CSRooftops>() != null)
            {
                hasRooftops = true;
            }
            else hasRooftops = false;


            advertising = gameObject.GetComponent<CSAdvertising>();
            if (advertising != null)
            {
                hasAdvertising = true;
                advertising.useAdvertising = useAdvertising;
            }
            else hasAdvertising = false;

            balcony = gameObject.GetComponent<CSArray>();
            if (balcony != null)
            {
                hasBalcony = true;
                balcony.useAdvertising = useAdvertising;
            }
            else hasBalcony = false;




            parentStreets = new StreetModifier[5];
            connectedSections = new BuildingModifier[5];

            //meshOriginal = GetComponent<MeshFilter>().sharedMesh;
            originalVertices = meshOriginal.vertices;
            originalUVs = meshOriginal.uv;
            originalColors = meshOriginal.colors;
            vColors = new Vector4[originalVertices.Length];
            mesh = Instantiate(meshOriginal) as Mesh;
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            //		gameObject.transform.GetChild (0);
            //      lightVec = (Mathf.FloorToInt(colorVariation.x * 0.1f) * 10f) + (Mathf.FloorToInt(colorVariation.y) * 0.1f) + (Mathf.FloorToInt(colorVariation.z) * 0.01f) + (Mathf.FloorToInt(colorVariation.w) * 0.001f + 0.00001f);
            //   
            ModifyBuilding();
          //  Thread olix = new Thread (ModifyBuilding);





        }


        public void UpdateCity()
        {
            // if (cityRandomizerParent == null) cityRandomizerParent = GameObject.Find("CScape City").GetComponent<CityRandomizer>() as CityRandomizer;
            if (cityRandomizerParent == null) cityRandomizerParent = gameObject.transform.parent.transform.parent.GetComponent<CityRandomizer>() as CityRandomizer;
            if (gameObject.GetComponent<CSRooftops>() != null)
            {
                hasRooftops = true;
            }

            advertising = gameObject.GetComponent<CSAdvertising>();
            if (advertising != null)
            {
                hasAdvertising = true;
                advertising.useAdvertising = useAdvertising;
            }

            balcony = gameObject.GetComponent<CSArray>();
            if (balcony != null)
            {
                hasBalcony = true;
                balcony.useAdvertising = useAdvertising;
            }
            else hasBalcony = false;

            ModifyBuilding();
          //  Thread olix = new Thread(ModifyBuilding);





        }




        public void ModifyBuilding()
        {

            if (gameObject.GetComponent<CSRooftops>() != null)
            {
                hasRooftops = true;
            }
            else hasRooftops = false;

            if (gameObject.GetComponent<CSAdvertising>() != null)
            {
                hasAdvertising = true;
            }
            else hasAdvertising = false;

            balcony = gameObject.GetComponent<CSArray>();
            if (balcony != null)
            {
                hasBalcony = true;
                balcony.useAdvertising = useAdvertising;
            }
            else hasBalcony = false;

            id1.x = CompressIDs(materialId1);
            id2.x = CompressIDs(materialId2);
            id3.x = CompressIDs(materialId3);
            id4.x = CompressIDs(materialId4);



            transform.localScale = new Vector3(scale, 1, 1);
            //mesh = GetComponent<MeshFilter>().mesh;
            Vector4[] vColorsFloat = new Vector4[mesh.uv.Length];
            Vector3[] vertices = mesh.vertices;
            Vector2[] uV = mesh.uv;

            Vector3[] transformVertices = mesh.vertices;
            Vector2[] transformUV = mesh.uv;

            Vector3[] normals = mesh.normals;

            lightVec = Mathf.FloorToInt(colorVariation.x) + Mathf.FloorToInt(colorVariation.y) * 0.1f + Mathf.FloorToInt(colorVariation.z) * 0.01f + Mathf.FloorToInt(colorVariation.w) * 0.001f + Mathf.FloorToInt(lightsOnOff) * 0.0001f + 0.00002f;
            lightVec2 = Mathf.FloorToInt(colorVariation2.x) + Mathf.FloorToInt(colorVariation2.y) * 0.1f + Mathf.FloorToInt(colorVariation2.z) * 0.01f + Mathf.FloorToInt(colorVariation2.w) * 0.001f + Mathf.FloorToInt(lightsOnOff2) * 0.0001f + 0.00002f;
            lightVec3 = Mathf.FloorToInt(colorVariation3.x) * 0.01f + Mathf.FloorToInt(colorVariation3.y) * 0.001f + lightnessFront * 0.0001f + 0.00002f;
            lightVec6 = Mathf.FloorToInt(colorVariation3.x) * 0.01f + Mathf.FloorToInt(colorVariation3.y) * 0.001f + lightnessFront * 0.0001f + 0.00002f;

            lightVec4 = Mathf.FloorToInt(windowOpen) + Mathf.FloorToInt(colorVariation4.x) * 0.1f + (materialId5) * 0.001f + borderCol * 0.0001f + 0.00002f;
            lightVec5 = Mathf.FloorToInt(windowOpen) + Mathf.FloorToInt(colorVariation4.x) * 0.1f + (0) * 0.001f + borderCol * 0.0001f + 0.00002f;
            
            lightVec7 = Mathf.FloorToInt(colorVariation5.x) * 0.01f + Mathf.FloorToInt(colorVariation5.y) * 0.001f + lightnessSide * 0.0001f + 0.00002f;
            lightVec8 = Mathf.FloorToInt(colorVariation5.x) * 0.01f + Mathf.FloorToInt(colorVariation5.y) * 0.001f + lightnessSide * 0.0001f + 0.00002f;
            //   Debug.Log(lightVec);
            int i = 0;
            while (i < vertices.Length)
            {
                transformVertices[i] = new Vector3(0, 0, 0);
                transformUV[i] = new Vector2(0, 0);
                vColors[i] = new Vector4(id1.x + lightVec3, id1.y, lightVec2, lightVec) * 0.1f;
                Vector3 invNormal = normals[i] * -1;

                if (originalVertices[i].y > lowFloorBound.y)
                {
                    if (floorNumber < prefabFloors) floorNumber = prefabFloors;
                    transformVertices[i].y = (floorNumber - prefabFloors) * floorHeight;
                    transformUV[i].y = floorNumber - prefabFloors;

                }

                if (originalVertices[i].x > lowFloorBound.x)
                {
                    if (buildingWidth < prefabWidth) buildingWidth = prefabWidth;
                    transformVertices[i].x = (buildingWidth - prefabWidth) * floorHeight;


                    if (invNormal.z < 1 + normalThreshold && invNormal.z > 1 - normalThreshold)
                    {
                        transformUV[i].x = buildingWidth - prefabWidth;
                    }
                    if (normals[i].z < 1 + normalThreshold && normals[i].z > 1 - normalThreshold)
                    {
                        transformUV[i].x = -(buildingWidth - prefabWidth);
                    }


                }


                if (originalVertices[i].z > lowFloorBound.z)
                {
                    if (buildingDepth < prefabDepth) buildingDepth = prefabDepth;
                    transformVertices[i].z = (buildingDepth - prefabDepth) * floorHeight;

                    if (normals[i].x < 1 + normalThreshold && normals[i].x > 1 - normalThreshold)
                    {
                        transformUV[i].x = (buildingDepth - prefabDepth);
                    }
                    if (invNormal.x < 1 + normalThreshold && invNormal.x > 1 - normalThreshold)
                    {
                        transformUV[i].x = -(buildingDepth - prefabDepth);
                    }



                }
                ///////// manipulate Vertex colors




                if (prefabHasVertexInfo)
                {
                    if (originalColors[i].r == 0f)
                    {

                        if (invNormal.z < 1 + normalThreshold && invNormal.z > 1 - normalThreshold)
                        {
                            vColors[i] = new Vector4(id2.x + lightVec3, lightVec4, lightVec2, lightVec) * 0.1f;
                        }
                        else if (normals[i].z < 1 + normalThreshold && normals[i].z > 1 - normalThreshold)
                        {
                            vColors[i] = new Vector4(id2.x + lightVec6, lightVec4, lightVec2, lightVec) * 0.1f;
                        }
                        else if (normals[i].x < 1 + normalThreshold && normals[i].x > 1 - normalThreshold)
                        {
                            vColors[i] = new Vector4(id3.x + lightVec7, lightVec4, lightVec2, lightVec) * 0.1f;
                        }
                        else if (invNormal.x < 1 + normalThreshold && invNormal.x > 1 - normalThreshold)
                        {
                            vColors[i] = new Vector4(id3.x + lightVec8, lightVec4, lightVec2, lightVec) * 0.1f;
                        }


                    }
                    else
                    {
                        vColors[i] = new Vector4(id1.x + lightVec3, lightVec5, lightVec2, lightVec) * 0.1f;


                    }


                }
                else
                {
                    if (invNormal.z < 1 + normalThreshold && invNormal.z > 1 - normalThreshold)
                    {
                        vColors[i] = new Vector4(id2.x + lightVec3, lightVec4, lightVec2, lightVec) * 0.1f;
                    }
                    if (normals[i].z < 1 + normalThreshold && normals[i].z > 1 - normalThreshold)
                    {
                        vColors[i] = new Vector4(id2.x + lightVec6, lightVec4, lightVec2, lightVec) * 0.1f;
                    }
                    if (normals[i].x < 1 + normalThreshold && normals[i].x > 1 - normalThreshold)
                    {
                        vColors[i] = new Vector4(id3.x + lightVec7, lightVec4, lightVec2, lightVec) * 0.1f;
                    }

                    if (invNormal.x < 1 + normalThreshold && invNormal.x > 1 - normalThreshold)
                    {
                        vColors[i] = new Vector4(id3.x + lightVec8, lightVec4, lightVec2, lightVec) * 0.1f;
                    }

                }
                //     Debug.Log((id2.x + lightVec3) * 0.1f);

                vertices[i] = originalVertices[i] + transformVertices[i];
                uV[i] = new Vector2(originalUVs[i].x, originalUVs[i].y) + transformUV[i] + new Vector2(uniqueMapping, 0);

                if (normals[i].y > 0.01)
                {
                    uV[i].x = vertices[i].x * 0.1f;
                    uV[i].y = vertices[i].z * 0.1f;
                    vColors[i] = new Vector4(id4.x + lightVec3, lightVec4, lightVec2, lightVec) * 0.1f;
                }

                vColorsFloat[i] = vColors[i];
                i++;
            }



            var list = new List<Vector4>(vColors);
            mesh.vertices = vertices;
            mesh.uv = uV;
            mesh.SetUVs(3, list);

            
            mesh.RecalculateNormals();
            
            mesh.RecalculateBounds();

            if (hasRooftops) rooftops.UpdateElements();
            if (hasAdvertising)
            {
                //advertising.Awake();
                advertising.UpdateElements();
            }

            if (hasBalcony)
            {

                balcony.UpdateElements();
            }
        }

        float CompressIDs(int iDInput)
        {

            float idOut = ((iDInput ) * 0.1f);
      //float idOut = 

            return idOut;


        }

        public void GenerateLightmaps()
        {

#if UNITY_EDITOR
            Mesh newMesh = GetComponent<MeshFilter>().sharedMesh;
            UnityEditor.Unwrapping.GenerateSecondaryUVSet(newMesh);



#endif

        }

        public void OnDestroy()
        {


            if (Application.isEditor && !Application.isPlaying)
            {

                if (useAdvertising)
                {
                    if (advertising != null)
                        DestroyImmediate(advertising.rooftopHolder);
                }

                if (hasBalcony)
                {
                    CSArray CSArray = balcony;
                    for (int x = 0; x < CSArray.numberOfModifiers; x++)
                    {
                        DestroyImmediate(CSArray.rooftopHolder[x]);
                    }

                }
                if (hasRooftops)
                {

                    CSRooftops csRooftops = rooftops;
                  //  if (csRooftops.rooftopHolder != null)
                     //   DestroyImmediate(csRooftops.rooftopHolder);

                }
            }
        }

        
        public void GuessPrefabSize()
        {
            Bounds bounds = meshOriginal.bounds;
            prefabFloors = Mathf.FloorToInt((bounds.size.y ) / 3f);
            prefabDepth = Mathf.FloorToInt((bounds.size.z ) / 3f);
            prefabWidth = Mathf.FloorToInt((bounds.size.x) / 3f);
            lowFloorBound = bounds.size/2f;
        }
#endif

    }
}
