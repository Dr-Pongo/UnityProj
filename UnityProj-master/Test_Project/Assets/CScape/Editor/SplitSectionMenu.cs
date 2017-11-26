using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using CScape;
public class SelectionActiveTransform : ScriptableObject
{
    [MenuItem("Window/Cscape/Layout Tools/Split X ")]
    static void SplitSelectedX()
    {

        foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) { 
            BuildingModifier bm = t.GetComponent<BuildingModifier>();
         //   int oldBdepth = bm.buildingDepth;
            int oldBwidth = bm.buildingWidth;
            bm.buildingWidth = Mathf.FloorToInt(bm.buildingWidth / 2);
            GameObject newBuilding = Instantiate(bm.cityRandomizerParent.prefabs[Random.Range(0, bm.cityRandomizerParent.prefabs.Length)], bm.transform.position, bm.transform.rotation);
            newBuilding.transform.parent = bm.cityRandomizerParent.transform;
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

    }

    [MenuItem("Window/Cscape/Layout Tools/Split X", true)]
    static bool ValidateSplitSelectionX()
    {
        return Selection.activeTransform != null;
    }

    [MenuItem("Window/Cscape/Layout Tools/Split Z ")]
    static void SplitSelectedY()
    {
        BuildingModifier bm = Selection.activeTransform.GetComponent<BuildingModifier>();
        int oldBdepth = bm.buildingDepth;
      //  int oldBwidth = bm.buildingWidth;
        bm.buildingDepth = Mathf.FloorToInt(bm.buildingDepth / 2);
        GameObject newBuilding = Instantiate(bm.cityRandomizerParent.prefabs[Random.Range(0, bm.cityRandomizerParent.prefabs.Length)], bm.transform.position, bm.transform.rotation);
        newBuilding.transform.parent = bm.cityRandomizerParent.transform;
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

    [MenuItem("Window/Cscape/Layout Tools/Split Z", true)]
    static bool ValidateSplitSelectionY()
    {
        return Selection.activeTransform != null;
    }


}