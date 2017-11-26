using UnityEngine;
using UnityEditor;
using System.Collections;
using CScape;
//using UnityEditor;

namespace CScape
{


    public class CScapeMenu : MonoBehaviour
    {
        //	private GameObject VRPano;
        [MenuItem("GameObject/CScape/Create MegaCity", false, 10)]
        static void CreateVRCameraObject(MenuCommand menuCommand)
        {


            GameObject CScapeCity = PrefabUtility.InstantiatePrefab(Resources.Load("CScapeCity")) as GameObject;
            CScapeCity.name = "CScape City";
            //PrefabUtility.DisconnectPrefabInstance (VRPano);
            GameObjectUtility.SetParentAndAlign(CScapeCity, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(CScapeCity, "Create " + CScapeCity.name);
            Selection.activeObject = CScapeCity;
        }



    }
}

