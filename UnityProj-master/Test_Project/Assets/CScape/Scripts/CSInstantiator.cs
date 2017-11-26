#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CScape;
using UnityEditor;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
//using UnityEditor;

namespace CScape
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    public class CSInstantiator : MonoBehaviour
    {


        public int instancesX;
        public int instancesZ;
        public Material mat;
        public Mesh mesh;
        public Mesh meshOriginal;
        public GameObject originalObject;
        public int offsetX;
        public int offsetZ;
        public bool update;
        public int width;
        public int depth;
        public int maxMeshSize;
        public StreetModifier parentSection;
        public LODGroup lodGroup;
        bool isPrefabOriginal;

        // Use this for initialization
        public void AwakeMe()
        {
            //originalVertices = meshOriginal.vertices;
            //originalUVs = meshOriginal.uv;
            //originalColors = meshOriginal.colors;

            //mesh = Instantiate(meshOriginal) as Mesh;
            //MeshFilter meshFilter = GetComponent<MeshFilter>();
            //meshFilter.mesh = mesh;
            //          parentSection = gameObject.transform.parent.gameObject.GetComponent<StreetModifier>();
            lodGroup = GetComponent<LODGroup>();
            UpdateElements();
        }

        // Update is called once per frame

        public void UpdateElements()
        {
#if UNITY_EDITOR
            isPrefabOriginal = PrefabUtility.GetPrefabParent(gameObject) == null && PrefabUtility.GetPrefabObject(gameObject.transform) != null;
#endif
            if (!isPrefabOriginal)
            {


                instancesX = ((depth + 1) * 3 / offsetX);
                instancesZ = ((width + 1) * 3 / offsetZ);
                DeleteSolution();

                maxMeshSize = Mathf.CeilToInt(65000f / (originalObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length * 2));
                //Debug.Log(maxMeshSize + ", " + originalObject.GetComponent<MeshFilter>().sharedMesh.vertices.Length);

                if (instancesX > maxMeshSize) instancesX = maxMeshSize;
                if (instancesX < 1) instancesX = 1;
                Vector3 baseOffset = new Vector3(0.5f, 0, 0.5f);
                Vector3 baseOffset2 = new Vector3(0.5f, 0, -0.5f);
                Vector3 baseOffsetSymetry = new Vector3(0.5f, 0, -0.5f);

                for (int j = 0; j < instancesX; j++)
                {
                    GameObject newObject = Instantiate(originalObject) as GameObject;
                    // newObject.GetComponent<MeshFilter>().mesh = meshOriginal;

                    newObject.transform.position = new Vector3(gameObject.transform.position.x + (j * offsetX), gameObject.transform.position.y, gameObject.transform.position.z) - gameObject.transform.position + baseOffset;
                    newObject.transform.parent = gameObject.transform;
                    newObject.transform.Rotate(new Vector3(0, 0, 180));
                }

                for (int j = 0; j < instancesX; j++)
                {
                    GameObject newObject = Instantiate(originalObject) as GameObject;
                    newObject.transform.position = new Vector3(gameObject.transform.position.x + (j * offsetX), gameObject.transform.position.y, gameObject.transform.position.z + (width) * 3) - gameObject.transform.position + baseOffsetSymetry;
                    newObject.transform.parent = gameObject.transform;
                    newObject.transform.Rotate(new Vector3(0, 0, 0));
                }

                for (int j = 0; j < instancesZ; j++)
                {
                    GameObject newObject = Instantiate(originalObject) as GameObject;
                    newObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + (j * offsetZ)) - gameObject.transform.position + baseOffset;
                    newObject.transform.parent = gameObject.transform;
                    newObject.transform.Rotate(new Vector3(0, 0, -90));
                }

                for (int j = 0; j < instancesZ; j++)
                {
                    GameObject newObject = Instantiate(originalObject) as GameObject;
                    newObject.transform.position = new Vector3(gameObject.transform.position.x + (depth * 3f), gameObject.transform.position.y, gameObject.transform.position.z + (j * offsetZ)) - gameObject.transform.position - baseOffset2;
                    newObject.transform.parent = gameObject.transform;
                    newObject.transform.Rotate(new Vector3(0, 0, 90));
                }

                //for (int j = 0; j < instancesZ; j++)
                //{
                //    GameObject newObject = Instantiate(originalObject) as GameObject;
                //    // newObject.GetComponent<MeshFilter>().mesh = meshOriginal;
                //    newObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z  +(depth - 1) * 3) - gameObject.transform.position - baseOffset;
                //    newObject.transform.parent = gameObject.transform;
                //    newObject.transform.Rotate(new Vector3(0, 0, 90));
                //}

                MergeMeshes();

               // MeshCollider colliderMesh = GetComponent(typeof(MeshCollider)) as MeshCollider;



                //mesh.vertices = vertices;
                //mesh.colors = vColors;
                //mesh.uv = uV;
                //mesh.RecalculateBounds();

            }
        }

        public void MergeMeshes()
        {

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length - 1];

            int index = 0;
            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].sharedMesh != null)
                {
                    // if (meshFilters[i].sharedMesh == null) continue;
                    combine[index].mesh = meshFilters[i].sharedMesh;
                    combine[index++].transform = meshFilters[i].transform.localToWorldMatrix;
                }
            }
            MeshFilter meshF = transform.GetComponent<MeshFilter>();
            meshF.sharedMesh = new Mesh();
            meshF.sharedMesh.name = "lightpoles";
            meshF.sharedMesh.CombineMeshes(combine);
            meshF.sharedMesh.RecalculateBounds();
            if (lodGroup != null) lodGroup.RecalculateBounds();

            //    transform.gameObject.SetActive(true);
            foreach (Transform go in gameObject.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }

            MeshCollider mColl = gameObject.GetComponent<MeshCollider>();
            mColl.sharedMesh = meshF.sharedMesh;
        }

        public void DeleteSolution()
        {

            foreach (Transform go in gameObject.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
            DestroyImmediate(transform.GetComponent<MeshFilter>().sharedMesh);
        }
    }
}
#endif