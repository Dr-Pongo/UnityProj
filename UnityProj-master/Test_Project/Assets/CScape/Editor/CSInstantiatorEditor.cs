using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using CScape;

namespace CScape
{
    [CustomEditor(typeof(CSInstantiator))]

    public class SCInstantiatorEditor : Editor
    {
        private Texture banner;
        public bool configurePrefab = false;


        void OnEnable()
        {
            CSInstantiator bm = (CSInstantiator)target;
            banner = Resources.Load("CSHeader") as Texture;
            bm.AwakeMe();
            bm.UpdateElements();



        }

        public override void OnInspectorGUI()
        {
            CSInstantiator bm = (CSInstantiator)target;
            GUILayout.Box(banner, GUILayout.ExpandWidth(true));


            bm.originalObject = EditorGUILayout.ObjectField("Original Object", bm.originalObject, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button("Update Template"))
            {
                bm.AwakeMe();
                bm.UpdateElements();
            }

            bm.instancesX = EditorGUILayout.IntField("Instances", bm.instancesX);
            bm.offsetX = EditorGUILayout.IntField("Offset X", bm.offsetX);
            bm.instancesZ = EditorGUILayout.IntField("Instances", bm.instancesZ);
            bm.offsetZ = EditorGUILayout.IntField("Offset Z", bm.offsetZ);
            bm.width = EditorGUILayout.IntField("Width", bm.width);
            bm.depth = EditorGUILayout.IntField("Depth", bm.depth);
           




            if (GUI.changed)
            {
                bm.AwakeMe();
                bm.UpdateElements();
                EditorUtility.SetDirty(bm);

            }
        }
    }
}