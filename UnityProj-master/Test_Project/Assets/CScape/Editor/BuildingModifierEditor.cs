using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using CScape;


namespace CScape
{
    [CustomEditor(typeof(BuildingModifier))]
    // [CanEditMultipleObjects]


    public class BuildingModifierEditor : Editor
    {
        private Texture banner;
        public bool configurePrefab = false;
        //   float range = 30;


        void OnEnable()
        {
            BuildingModifier bm = (BuildingModifier)target;
            bm.AwakeCity();
            bm.UpdateCity();
            banner = Resources.Load("CSHeader") as Texture;

        }




        public override void OnInspectorGUI()
        {
            BuildingModifier bm = (BuildingModifier)target;


            GUILayout.Box(banner, GUILayout.ExpandWidth(true));

            GUILayout.BeginVertical("box");
            GUILayout.Label("Facade assignement");
            bm.colorVariation4.x = EditorGUILayout.IntSlider("Floor Level ID", Mathf.FloorToInt(bm.colorVariation4.x), 0, 9);
            bm.materialId1 = EditorGUILayout.IntSlider("Material ID 1", bm.materialId1, 20, 29);
            bm.materialId2 = EditorGUILayout.IntSlider("Material ID 2", bm.materialId2, 0, 19);
            bm.materialId3 = EditorGUILayout.IntSlider("Material ID 3", bm.materialId3, 0, 19);
            bm.materialId4 = EditorGUILayout.IntSlider("Material ID 4", bm.materialId4, 30, 39);
            bm.materialId5 = EditorGUILayout.IntSlider("Subdivision ID", bm.materialId5, 0, 30);
            GUILayout.EndHorizontal();
            //configure Prefab
            GUILayout.BeginVertical("box");
            configurePrefab = EditorGUILayout.Foldout(configurePrefab, "Prefab Configuration");
            if (configurePrefab)
            {
                bm.meshOriginal = EditorGUILayout.ObjectField("Mesh Template", bm.meshOriginal, typeof(Mesh), true) as Mesh;
                if (GUILayout.Button("Update Mesh Template"))
                {
                    bm.AwakeCity();
                    bm.UpdateCity();
                }
                if (GUILayout.Button("Guess building size"))
                {
                    bm.GuessPrefabSize();
                    bm.AwakeCity();
                    bm.UpdateCity();
                }

                bm.lowFloorBound = EditorGUILayout.Vector3Field("Resizer Center", bm.lowFloorBound);
                bm.prefabFloors = EditorGUILayout.IntField("Prefab floor number", bm.prefabFloors);
                bm.prefabWidth = EditorGUILayout.IntField("Prefab width", bm.prefabWidth);
                bm.prefabDepth = EditorGUILayout.IntField("Prefab depth", bm.prefabDepth);
                bm.normalThreshold = EditorGUILayout.Slider("Normal threshold", bm.normalThreshold, 0f, 0.5f);
                bm.floorHeight = EditorGUILayout.FloatField("Floor Height", bm.floorHeight);
                bm.advertisingPanelCoord = EditorGUILayout.Vector3Field("Advert Panel Zone", bm.advertisingPanelCoord);

                bm.prefabHasVertexInfo = EditorGUILayout.Toggle("Prefab has vertex Info", bm.prefabHasVertexInfo);




                ///Rooftops Array
                GUILayout.BeginVertical("Box");
                for (int i = 0; i < bm.roofOffsetY.Length; i++)
                {
                    bm.roofOffsetX[i] = EditorGUILayout.Vector2Field("Roof Elements X", bm.roofOffsetX[i]);
                    bm.roofOffsetZ[i] = EditorGUILayout.Vector2Field("Roof Elements Z", bm.roofOffsetZ[i]);
                    bm.roofOffsetY[i] = EditorGUILayout.FloatField("Roof Height", bm.roofOffsetY[i]);
                }



                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                {
                    System.Array.Resize(ref bm.roofOffsetX, bm.roofOffsetX.Length - 1);
                    System.Array.Resize(ref bm.roofOffsetY, bm.roofOffsetY.Length - 1);
                    System.Array.Resize(ref bm.roofOffsetZ, bm.roofOffsetZ.Length - 1);
                }
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                {
                    System.Array.Resize(ref bm.roofOffsetX, bm.roofOffsetX.Length + 1);
                    System.Array.Resize(ref bm.roofOffsetY, bm.roofOffsetY.Length + 1);
                    System.Array.Resize(ref bm.roofOffsetZ, bm.roofOffsetZ.Length + 1);
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                /////Advertising Panels layout
                GUILayout.BeginVertical("Box");
                for (int i = 0; i < bm.advertOffsetY.Length; i++)
                {
                    bm.advertOffsetX[i] = EditorGUILayout.Vector2Field("Roof Elements X", bm.advertOffsetX[i]);
                    bm.advertOffsetZ[i] = EditorGUILayout.Vector2Field("Roof Elements Z", bm.advertOffsetZ[i]);
                    bm.advertOffsetY[i] = EditorGUILayout.FloatField("Roof Height", bm.advertOffsetY[i]);
                }



                GUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("-", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                {
                    System.Array.Resize(ref bm.advertOffsetX, bm.advertOffsetX.Length - 1);
                    System.Array.Resize(ref bm.advertOffsetY, bm.advertOffsetY.Length - 1);
                    System.Array.Resize(ref bm.advertOffsetZ, bm.advertOffsetZ.Length - 1);
                }
                if (GUILayout.Button("+", "Label", GUILayout.Width(20), GUILayout.Height(15)))
                {
                    System.Array.Resize(ref bm.advertOffsetX, bm.advertOffsetX.Length + 1);
                    System.Array.Resize(ref bm.advertOffsetY, bm.advertOffsetY.Length + 1);
                    System.Array.Resize(ref bm.advertOffsetZ, bm.advertOffsetZ.Length + 1);
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();





            }
            GUILayout.EndVertical();


            GUILayout.BeginVertical("box");
            GUILayout.Label("Variations");
            bm.windowOpen = EditorGUILayout.Slider("Window Lights", bm.windowOpen, 0f, 9f);
            bm.pattern = EditorGUILayout.Slider("Floor Pattern", bm.pattern, 0f, 0.9f);
            bm.colorVariation3.x = EditorGUILayout.Slider("Faccade Mat 1 front/Back", bm.colorVariation3.x, 0f, 9f);
            bm.colorVariation3.y = EditorGUILayout.Slider("Faccade Mat 2 front/Back", bm.colorVariation3.y, 0f, 9f);
            bm.colorVariation5.x = EditorGUILayout.Slider("Faccade Mat 1 sides", bm.colorVariation5.x, 0f, 9f);
            bm.colorVariation5.y = EditorGUILayout.Slider("Faccade Mat 2 sides", bm.colorVariation5.y, 0f, 9f);


            bm.colorVariation2.x = EditorGUILayout.Slider("Window Gloss", bm.colorVariation2.x, 0f, 9f);
            bm.colorVariation2.y = EditorGUILayout.Slider("H. separation", (bm.colorVariation2.y), 0f, 9f);
            bm.colorVariation2.z = EditorGUILayout.Slider("W. separation", (bm.colorVariation2.z), 0f, 9f);
            bm.colorVariation2.w = EditorGUILayout.Slider("Blinds open", (bm.colorVariation2.w), 0f, 9f);
            bm.borderCol = EditorGUILayout.IntSlider("Window Border Col", (bm.borderCol), 0, 9);
            bm.lightsOnOff2 = EditorGUILayout.Slider("AO Scale", (bm.lightsOnOff2), 0f, 9f);





            bm.colorVariation.x = EditorGUILayout.Slider("Faccade Lights Height", bm.colorVariation.x, 0f, 9f);
            bm.lightsOnOff = EditorGUILayout.Slider("Faccade Lights Tiling", (bm.lightsOnOff), 0f, 9f);
            bm.colorVariation.y = EditorGUILayout.Slider("Lights R", (bm.colorVariation.y), 0f, 9f);
            bm.colorVariation.z = EditorGUILayout.Slider("Lights G", (bm.colorVariation.z), 0f, 9f);
            bm.colorVariation.w = EditorGUILayout.Slider("Lights B", (bm.colorVariation.w), 0f, 9f);
            bm.lightnessFront = EditorGUILayout.IntSlider("Lightness", (bm.lightnessFront), 0, 9);
            bm.lightnessSide = EditorGUILayout.IntSlider("Lightness", (bm.lightnessSide), 0, 9);

            bm.uniqueMapping = EditorGUILayout.IntField("Unique Mapping", bm.uniqueMapping);
            bm.floorNumber = EditorGUILayout.IntField("Floor Number", bm.floorNumber);
            bm.buildingDepth = EditorGUILayout.IntField("Building depth", bm.buildingDepth);
            bm.buildingWidth = EditorGUILayout.IntField("Building Width", bm.buildingWidth);
            bm.distortXZ = EditorGUILayout.FloatField("Building Width", bm.distortXZ);
            bm.useAdvertising = EditorGUILayout.Toggle("Use Advertising Panels", bm.useAdvertising);

            //if (GUILayout.Button("Generate Lightmapping UV"))
            //{
            //    bm.GenerateLightmaps();
            //}
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Split Building X"))
            {
                SplitX();

            }
            if (GUILayout.Button("Split Building Z"))
            {
                SplitZ();

            }

            if (GUILayout.Button("Randomize Materials"))
            {

                bm.colorVariation.x = Random.Range(0, 9);

                bm.colorVariation2.x = Random.Range(0, 9);
                bm.colorVariation2.y = Random.Range(0, 9);
                bm.colorVariation2.z = Random.Range(0, 9);
                bm.colorVariation2.w = Random.Range(2, 9);

                bm.colorVariation3.x = Random.Range(0, 9);
                bm.colorVariation3.y = Random.Range(0, 9);
                bm.colorVariation3.z = Random.Range(0, 9);
                bm.colorVariation3.w = Random.Range(0, 9);

                bm.colorVariation4.x = Random.Range(0, 9);
                bm.colorVariation4.y = Random.Range(0, 9);
                bm.colorVariation4.z = Random.Range(0, 9);
                bm.colorVariation4.w = Random.Range(0, 9);
                bm.lightnessFront = Random.Range(0, 9);
                bm.lightnessSide = Random.Range(0, 9);
                bm.colorVariation5.x = Random.Range(0, 9);
                bm.colorVariation5.y = Random.Range(0, 9);
            }
            GUILayout.EndHorizontal();




            if (GUI.changed)
            {
                bm.UpdateCity();
                EditorUtility.SetDirty(bm);
#if UNITY_5_4_OR_NEWER
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }

        }




        void OnSceneGUI()
        {


            bool editMe;
            BuildingModifier t = (target as BuildingModifier);
            Quaternion newRotation = t.gameObject.transform.rotation;
            Undo.RecordObject(t, "Undo building Modification");
            Vector3 point = Camera.current.WorldToScreenPoint(t.transform.position);
            Vector3 splitPointX = Camera.current.WorldToScreenPoint(t.transform.position + new Vector3((t.buildingWidth / 2) * 3f, 0, 0));
            Vector3 splitPointZ = Camera.current.WorldToScreenPoint(t.transform.position + new Vector3(0, 0, (t.buildingDepth / 2) * 3f));


            Handles.BeginGUI();
            if (Event.current.modifiers == EventModifiers.Alt)
            {
                editMe = true;

            }
            else editMe = false;
            GUILayout.BeginVertical();
            if (editMe)
            {
                if (GUI.Button(new Rect(splitPointX.x - 40, Screen.height - splitPointX.y - 40, 80, 20), "Split X"))
                {
                    SplitX();
                }

                if (GUI.Button(new Rect(splitPointZ.x - 40, Screen.height - splitPointZ.y - 40, 80, 20), "Split Z"))
                {
                    SplitZ();
                }
            }
            EditorGUI.BeginChangeCheck();
            if (editMe)
            {
                GUILayout.BeginVertical();

                GUI.Label(new Rect(splitPointX.x - 40, Screen.height - splitPointX.y - 20, 100, 30), "id2");
                t.materialId2 = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(splitPointX.x - 40, Screen.height - splitPointX.y - 10, 80, 20), t.materialId2, 0, 19));
                GUI.Label(new Rect(splitPointX.x - 40, Screen.height - splitPointX.y, 100, 30), "id2");
                t.materialId2 = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(splitPointX.x - 40, Screen.height - splitPointX.y + 10, 80, 20), t.materialId2, 0, 19));
                GUI.Label(new Rect(point.x, Screen.height - point.y - 30, 100, 30), "Text");
                t.windowOpen = GUI.HorizontalSlider(new Rect(point.x, Screen.height - point.y - 20, 100, 30), t.windowOpen, 0.0F, 9.0F);
                GUI.Label(new Rect(point.x, Screen.height - point.y - 10, 100, 30), "Text");
                t.materialId1 = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(point.x, Screen.height - point.y, 100, 30), t.materialId1, 20, 29));
                t.gameObject.transform.localPosition = new Vector3(Mathf.CeilToInt(t.gameObject.transform.localPosition.x / 3) * 3, 0, t.gameObject.transform.localPosition.z);
            }

            if (EditorGUI.EndChangeCheck())
            {
                t.UpdateCity();
            }
            GUILayout.EndVertical();
            Handles.EndGUI();

            //  Vector3 position = t.transform.position + Vector3.up * 2f;
            //  float size = 2f;
            //   float pickSize = size * 2f;





            /////////
            if (!editMe)
            {
                if (Event.current.type == EventType.Repaint && configurePrefab)
                {
                    Handles.color = Color.blue;
                    Handles.Label(t.transform.position + Vector3.up * 2,
                                         "Depth: " + t.buildingDepth + "cs units (" + t.buildingDepth * 3 + "m) \n" +
                                         "Width: " + t.buildingWidth + "cs units (" + t.buildingWidth * 3 + "m) \n" +
                                         "Height: " + t.floorNumber + "Floors (" + t.floorNumber * 3 + "m) \n");



                    Vector3 pos = t.transform.position;


                    Vector3[] verts = new Vector3[] {
                                            newRotation * (new Vector3(pos.x + t.buildingWidth *3, pos.y, pos.z+ t.buildingDepth *3)- t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x + t.buildingWidth*3, pos.y, pos.z) -t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x, pos.y,pos.z)-t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x, pos.y, pos.z + t.buildingDepth * 3)-t.gameObject.transform.position) + t.gameObject.transform.position };

                    Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 1, 0.1f), new Color(0, 0, 0, 1));
                    Handles.color = Color.white;
                    float bwdth = t.buildingWidth;
                    t.buildingWidth = Mathf.CeilToInt(bwdth);
                }
            }




            if (Event.current.type == EventType.Repaint && configurePrefab)
            {
                Handles.color = Color.red;
                Vector3 pos = t.transform.position;

                Vector3[] verts = new Vector3[] {
                                            newRotation * (new Vector3(pos.x + t.buildingWidth *3, pos.y + t.prefabFloors *3, pos.z+ t.buildingDepth *3)- t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x + t.buildingWidth*3, pos.y + t.prefabFloors *3, pos.z)- t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x, pos.y + t.prefabFloors*3, pos.z)- t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x, pos.y + t.prefabFloors*3, pos.z + t.buildingDepth * 3)- t.gameObject.transform.position) + t.gameObject.transform.position };

                Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 0, 0, 0.1f), new Color(1, 0, 0, 1));


                // float bwdth = t.buildingWidth;

                // foreach (Vector3 posCube in verts)
                // bwdth = Handles.ScaleValueHandle(t.buildingWidth, posCube, Quaternion.identity, 1, Handles.CubeCap, 1);
                // t.buildingWidth = Mathf.CeilToInt(bwdth);
            }


            {
                if (Event.current.type == EventType.Repaint && configurePrefab)
                {
                    for (int i = 0; i < t.roofOffsetY.Length; i++)
                    {
                        Handles.color = Color.yellow;
                        Handles.lighting = true;
                        //Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                        Vector3 pos = t.transform.position;

                        Vector3[] verts = new Vector3[] {
                                           newRotation * (new Vector3(pos.x + t.buildingWidth *3 + t.roofOffsetX[i].x, pos.y + t.floorNumber *3 + t.roofOffsetY[i], pos.z+ t.buildingDepth *3 + t.roofOffsetX[i].y )- t.gameObject.transform.position) +t.gameObject.transform.position ,
                                           newRotation * (new Vector3(pos.x + t.buildingWidth*3 + t.roofOffsetX[i].x, pos.y + t.floorNumber *3 + t.roofOffsetY[i], pos.z + t.roofOffsetZ[i].y )- t.gameObject.transform.position) + t.gameObject.transform.position ,
                                           newRotation * (new Vector3(pos.x + t.roofOffsetZ[i].x, pos.y + t.floorNumber*3 + t.roofOffsetY[i], pos.z + t.roofOffsetZ[i].y)- t.gameObject.transform.position) + t.gameObject.transform.position ,
                                           newRotation * (new Vector3(pos.x + t.roofOffsetZ[i].x, pos.y + t.floorNumber*3 + t.roofOffsetY[i], pos.z + t.buildingDepth * 3 + t.roofOffsetX[i].y)- t.gameObject.transform.position) + t.gameObject.transform.position };

                        Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 0, 0.1f), new Color(1, 0, 0, 1));
                    }

                    ///DraW advert zones
                    for (int i = 0; i < t.advertOffsetY.Length; i++)
                    {
                        Handles.color = Color.green;
                        Handles.lighting = true;
                        //Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                        Vector3 pos = t.transform.position;

                        Vector3[] verts = new Vector3[] {
                                            newRotation * (new Vector3(pos.x + t.buildingWidth *3 + t.advertOffsetX[i].x, pos.y  + t.advertOffsetY[i] + t.advertOffsetZ[i].y, pos.z + t.advertOffsetX[i].y )- t.gameObject.transform.position) + t.gameObject.transform.position,
                                            newRotation * (new Vector3(pos.x + t.buildingWidth*3 + t.advertOffsetX[i].x, pos.y  + t.advertOffsetY[i], pos.z + t.advertOffsetX[i].y )- t.gameObject.transform.position) + t.gameObject.transform.position,
                                            newRotation * (new Vector3(pos.x + t.advertOffsetZ[i].x, pos.y  + t.advertOffsetY[i], pos.z + t.advertOffsetX[i].y)- t.gameObject.transform.position) + t.gameObject.transform.position,
                                            newRotation * (new Vector3(pos.x + t.advertOffsetZ[i].x, pos.y  + t.advertOffsetY[i]  + t.advertOffsetZ[i].y, pos.z + t.advertOffsetX[i].y)- t.gameObject.transform.position) + t.gameObject.transform.position};

                        Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 0, 0.1f), new Color(1, 0, 0, 1));
                    }
                    //    float bwdth = t.buildingWidth;

                    // foreach (Vector3 posCube in verts)
                    // bwdth = Handles.ScaleValueHandle(t.buildingWidth, posCube, Quaternion.identity, 1, Handles.CubeCap, 1);
                    //     t.buildingWidth = Mathf.CeilToInt(bwdth);
                }
            }


            if (configurePrefab)
            {

            }
            float size = HandleUtility.GetHandleSize(point);
            EditorGUI.BeginChangeCheck();
            Handles.color = new Color(1, 0, 0, 1);
            float bdepth = Handles.ScaleValueHandle(t.buildingWidth + 0f, (t.transform.position) + newRotation * ((new Vector3(t.buildingWidth * 3, 0, t.buildingDepth * 1.5f) - t.gameObject.transform.position) + t.gameObject.transform.position), t.transform.rotation * Quaternion.LookRotation(Vector3.right), 10, Handles.CircleHandleCap, 3);
            //   float bdepth = t.buildingWidth;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Width Slider");
                t.buildingWidth = Mathf.FloorToInt(bdepth);
                t.UpdateCity();
            }


            EditorGUI.BeginChangeCheck();
            Handles.color = new Color(0, 1, 0, 1);
            float height = Handles.ScaleValueHandle(t.floorNumber + 0f, (t.transform.position) + newRotation * ((new Vector3(t.buildingWidth * 1.5f, t.floorNumber * 3, t.buildingDepth * 1.5f) - t.gameObject.transform.position) + t.gameObject.transform.position), t.transform.rotation * Quaternion.LookRotation(Vector3.up), 10, Handles.CircleHandleCap, 3);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Floor Slider");
                t.floorNumber = Mathf.FloorToInt(height);
                t.UpdateCity();
            }

            EditorGUI.BeginChangeCheck();
            Handles.color = new Color(0, 1, 1, 1);
            float width = Handles.ScaleValueHandle(t.buildingDepth + 0f, (t.transform.position) + newRotation * ((new Vector3(t.buildingWidth * 1.5f, 0, t.buildingDepth * 3) - t.gameObject.transform.position) + t.gameObject.transform.position), t.transform.rotation * Quaternion.LookRotation(Vector3.forward), 10, Handles.CircleHandleCap, 3);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Depth Slider");
                t.buildingDepth = Mathf.FloorToInt(width);
                t.UpdateCity();
            }
            if (Event.current.type == EventType.Repaint && !configurePrefab)
            {
                Vector3 pos = t.transform.position;
                Vector3[] verts = new Vector3[] {
                                            newRotation * (new Vector3(pos.x + t.buildingWidth *3, pos.y, pos.z+ t.buildingDepth *3)- t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x + t.buildingWidth*3, pos.y, pos.z) -t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x, pos.y,pos.z)-t.gameObject.transform.position) + t.gameObject.transform.position ,
                                            newRotation * (new Vector3(pos.x, pos.y, pos.z + t.buildingDepth * 3)-t.gameObject.transform.position) + t.gameObject.transform.position };
                Handles.color = new Color(1, 1, 1, 0.3f);
                Handles.DrawSolidRectangleWithOutline(verts, new Color(0, 0, 0, 0), new Color(1, 1, 1, 1));
            }


        }
        void SplitX()
        {
            BuildingModifier bm = (BuildingModifier)target;
            // int oldBdepth = bm.buildingDepth;
            int oldBwidth = bm.buildingWidth;
            bm.buildingWidth = Mathf.FloorToInt(bm.buildingWidth / 2);
            GameObject newBuilding = Instantiate(bm.cityRandomizerParent.prefabs[Random.Range(0, bm.cityRandomizerParent.prefabs.Length)], bm.transform.position, bm.transform.rotation);
            newBuilding.transform.parent = bm.cityRandomizerParent.GetComponent<CityRandomizer>().streets.transform;
            newBuilding.transform.name = bm.gameObject.transform.name + "_split_1";
            newBuilding.transform.position = bm.gameObject.transform.position;
            newBuilding.transform.position = new Vector3(bm.gameObject.transform.position.x + bm.buildingWidth * 3f, bm.gameObject.transform.position.y, bm.gameObject.transform.position.z);
            BuildingModifier newBuildingModifier = newBuilding.GetComponent<BuildingModifier>();
            newBuildingModifier.buildingWidth = oldBwidth - bm.buildingWidth;
            newBuildingModifier.buildingDepth = bm.buildingDepth;
            newBuildingModifier.floorNumber = Random.Range(bm.floorNumber - 3, bm.floorNumber + 3);
            newBuildingModifier.cityRandomizerParent = bm.cityRandomizerParent;
            newBuildingModifier.AwakeCity();
            bm.AwakeCity();
        }
        void SplitZ()
        {
            BuildingModifier bm = (BuildingModifier)target;
            int oldBdepth = bm.buildingDepth;
            //  int oldBwidth = bm.buildingWidth;
            bm.buildingDepth = Mathf.FloorToInt(bm.buildingDepth / 2);
            GameObject newBuilding = Instantiate(bm.cityRandomizerParent.prefabs[Random.Range(0, bm.cityRandomizerParent.prefabs.Length)], bm.transform.position, bm.transform.rotation);
            newBuilding.transform.parent = bm.cityRandomizerParent.GetComponent<CityRandomizer>().streets.transform;
            newBuilding.transform.name = bm.gameObject.transform.name + "_split_1";
            newBuilding.transform.position = bm.gameObject.transform.position;
            newBuilding.transform.position = new Vector3(bm.gameObject.transform.position.x, bm.gameObject.transform.position.y, bm.gameObject.transform.position.z + bm.buildingDepth * 3f);
            BuildingModifier newBuildingModifier = newBuilding.GetComponent<BuildingModifier>();
            newBuildingModifier.buildingDepth = oldBdepth - bm.buildingDepth;
            newBuildingModifier.buildingWidth = bm.buildingWidth;
            newBuildingModifier.floorNumber = Random.Range(bm.floorNumber - 3, bm.floorNumber + 3);
            newBuildingModifier.cityRandomizerParent = bm.cityRandomizerParent;
            newBuildingModifier.AwakeCity();
            bm.AwakeCity();
        }

        //void OnDestroy()
        //{
        //    if (Input.GetKey(KeyCode.Delete))
        //    {
        //        BuildingModifier bm = (BuildingModifier)target;
        //        bm.OnDestroy();
        //    }
        //}
        }
}