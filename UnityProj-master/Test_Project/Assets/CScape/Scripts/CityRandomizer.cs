#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;
using CScape;
//using UnityEditor;

namespace CScape
{

    //  [ExecuteInEditMode]
    public class CityRandomizer : MonoBehaviour
    {
        public bool randomize = false;
        public bool updateColliders = false;
        public bool createRooftopDetails = false;
        public bool createStreets = false;
        public bool createStreetDetails = false;
        public bool deactivateModifiers = false;
        public bool activateModifiers = true;
        public int minFloors = 5;
        public int maxFloors = 50;
        public int minDepth = 2;
        public int maxDepth = 10;
        public int minWidth = 2;
        public int maxWidth = 30;
        public int minMatIndex;
        public int maxMatIndex;
        public int minMatIndex1;
        public int maxMatIndex1;
        public int minMatIndex2;
        public int maxMatIndex2;
        public int minMatIndex4;
        public int maxMatIndex4;
        public float minWindowOpen = 0.5f;
        public float maxWindowOpen = 100f;
        public float patern;
        public GameObject[] prefabs;
        public GameObject[] rooftopPrefabs;
        public GameObject[] buildPrefabs;
        public Color[] nightColors;
        public GameObject[] streetDetailPrefabs;
        public GameObject[] streetLightsPrefabs;
        public GameObject[] streetFoliagePrefabs;
        //  public GameObject[] streetDetailsPrefabs;
        public GameObject fbStreet;
        public GameObject lrStreet;
        public GameObject crossroad;
        public bool width;
        public bool height;
        public bool depth;
        public bool openWindow;
        public bool rndPatternHorizontal;
        public bool faccadeStyles;
        public bool rndColor;
        public bool useMeshCollider;
        public bool useConvexCollider;
        public bool generate = false;
        public bool delete = false;
        public int blockDistances;
        public int maxBlockDistances;
        public int randomSeed;

        public float cityCenterRadius = 200f;
        public int numberOfBuildingsX = 20;
        public int numberOfBuildingsZ = 20;
        public int streetSizeX = 2;
        public int streetSizeZ = 2;
        public int sidewalkSizeX = 2;
        public int sidewalkSizeZ = 2;
        public int streetSizeXmax = 2;
        public int streetSizeZmax = 2;
        public int sidewalkSizeXmax = 2;
        public int sidewalkSizeZmax = 2;
        public GameObject buildings;
        public GameObject rooftopDetails;
        public GameObject streets;
        public GameObject foliage;
        public GameObject streetDetails;
        public GameObject streetLights;
        public GameObject adverts;
        public GameObject balcony;

        public AnimationCurve cityCurve;
        //public int[] streetRuleX;
        // public int[] streetRuleZ;

        public GameObject cityCenterObject;
        public Vector4[] cityRuleArrayX;
        public Vector4[] cityRuleArrayZ;
        float cityCenterWeightX;
        float cityCenterWeightZ;
        public int folliageThreshold;
        public bool subdivide = true;
        public int sectionDivisionMax;
        public int sectionDivisionMin;
        public bool useRivers = true;
        public int riverPosition = 5;
        public GameObject river;
        int citySize;
        public bool usePOM = true;



        public void UpdateCity()
        {
            if (cityCenterObject == null) cityCenterObject = gameObject;
            // randomize = true;

            if (generate)
            {
                Generate();
            }




            if (delete)
            {
                Deletesolution();
            }



            if (randomize)
            {
                Randomize();
            }







        }

        void Deletesolution()
        {
            foreach (Transform go in buildings.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }

            foreach (Transform go in streets.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }

            foreach (Transform go in rooftopDetails.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }

            foreach (Transform go in streetDetails.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
            foreach (Transform go in foliage.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
            foreach (Transform go in adverts.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
            foreach (Transform go in streetLights.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }

            foreach (Transform go in balcony.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }

            delete = false;
        }

        public void DeleteFolliage()
        {
            foreach (Transform go in foliage.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
        }

        public void DeleteStreets()
        {
            foreach (Transform go in streets.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
        }

        public void DeleteStreeetDetails()
        {
            foreach (Transform go in streetDetails.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
        }

        public void DeleteBuildings()
        {
            foreach (Transform go in buildings.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
        }

        public void DeleteLights()
        {
            foreach (Transform go in streetLights.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
        }

        public void DeleteAdverts()
        {
            foreach (Transform go in adverts.transform.Cast<Transform>().Reverse())
            {
                DestroyImmediate(go.gameObject);
            }
        }


        public void Generate()
        {
            Deletesolution();
        //    Random.InitState(randomSeed);


            cityCenterWeightX = 0;
            cityCenterWeightZ = 0;

            ///Generate Random Rules            
            cityRuleArrayX = new Vector4[numberOfBuildingsX];
            cityRuleArrayZ = new Vector4[numberOfBuildingsZ];
            //GenerateDivisionRuleArray
            var sectionDivision = new Vector4[cityRuleArrayX.Length * cityRuleArrayZ.Length];

            for (int i = 0; i < cityRuleArrayX.Length; i++)
            {
                // Blocks Size
                cityRuleArrayX[i].x = Mathf.Ceil(Random.Range(blockDistances, maxBlockDistances));
                // World Position (in CS units)
                if (i == 0) cityRuleArrayX[i].y = 0;
                else cityRuleArrayX[i].y = cityRuleArrayX[i - 1].y + cityRuleArrayX[i - 1].x;
                //SidawalkSize
                cityRuleArrayX[i].z = Mathf.Ceil(Random.Range(sidewalkSizeX, sidewalkSizeXmax));
                //lane Number
                cityRuleArrayX[i].w = Mathf.Ceil(Random.Range(streetSizeX, streetSizeXmax));
                // Debug.Log(cityRuleArrayX[i].w);


            }
            for (int i = 0; i < cityRuleArrayZ.Length; i++)
            {
                cityRuleArrayZ[i].x = Mathf.Ceil(Random.Range(blockDistances, maxBlockDistances));
                if (i == 0) cityRuleArrayZ[i].y = 0;
                else cityRuleArrayZ[i].y = cityRuleArrayZ[i - 1].y + cityRuleArrayZ[i - 1].x;

                cityRuleArrayZ[i].z = Mathf.Ceil(Random.Range(sidewalkSizeZ, sidewalkSizeZmax));
                cityRuleArrayZ[i].w = Mathf.Ceil(Random.Range(streetSizeZ, streetSizeZmax));


            }
            int size = 0;
            for (int i = 0; i < cityRuleArrayZ.Length; i++)
            {
               size = size + Mathf.FloorToInt (cityRuleArrayZ[i].x);
            }
            citySize = size;
                CityRandomizer cRandomizerThis = GetComponent<CityRandomizer>();
            int iteration = 0;
            for (int i = 0; i < numberOfBuildingsX; i++)
            {


                for (int j = 0; j < numberOfBuildingsZ; j++)
                {
                    iteration = iteration + 1;
                    Random.InitState(randomSeed + iteration);

                    ///divideBlockSections
                    sectionDivision[i * j] = new Vector4(Random.Range(sectionDivisionMin, sectionDivisionMax), Random.Range(sectionDivisionMin, sectionDivisionMax), 0, 0);

                    ///check if our object fits tile
                    int iteration2 = 0;
                    if (subdivide)
                    {

                        int subsX = Mathf.CeilToInt(sectionDivision[j * i].x);
                        int subsZ = Mathf.CeilToInt(sectionDivision[j * i].y);

                        for (int subi = 0; subi < subsX; subi++)
                        {
                            for (int subj = 0; subj < subsZ; subj++)
                            {
                                iteration2 = iteration2 + 1;
                                Random.InitState(randomSeed + iteration2 + iteration);

                                if (subi == 0 || subi == subsX - 1 || subj == 0 || subj == subsZ - 1)
                                {

                                    bool validate = false;
                                    GameObject prefabToInstantiate = null;
                                    int iterations = 0;
                                    bool start = true;
                                    int prefabChoice = Random.Range(0, prefabs.Length - 1);
                                    
                                
                                    while (!validate && iterations < prefabs.Length)
                                    {
                                        if (start == true)
                                        {
                                            prefabToInstantiate = prefabs[prefabChoice];
                                            start = false;
                                        }
                                        else
                                        {
                                            if (prefabChoice < prefabs.Length)
                                                prefabChoice++;
                                            else prefabChoice = 0;

                                            prefabToInstantiate = prefabs[prefabChoice];
                                        }

                                        BuildingModifier buildingModifierx = prefabToInstantiate.GetComponent(typeof(BuildingModifier)) as BuildingModifier;
 


                                        if (subj != subsZ - 1 && subj != 0)
                                        {
                                            if (buildingModifierx.prefabWidth < (Mathf.FloorToInt((cityRuleArrayX[i].x - (((cityRuleArrayX[i].z) * 2) + cityRuleArrayX[i].w)) / subsX)) 
                                                || buildingModifierx.prefabWidth < Mathf.FloorToInt((cityRuleArrayZ[j].x - (((cityRuleArrayZ[j].z) * 2) + cityRuleArrayZ[j].w)) / subsZ)) validate = true;
                                        }

                                        else if (subj != subsZ - 1 && subj != 0 && subi == 0)
                                        {
                                            if (buildingModifierx.prefabWidth < (Mathf.FloorToInt((cityRuleArrayX[i].x - (((cityRuleArrayX[i].z) * 2) + cityRuleArrayX[i].w)) / subsX))
                                                || buildingModifierx.prefabWidth < Mathf.FloorToInt((cityRuleArrayZ[j].x - (((cityRuleArrayZ[j].z) * 2) + cityRuleArrayZ[j].w)) / subsZ)) validate = true;
                                        }

                                        else if (buildingModifierx.prefabDepth < (Mathf.FloorToInt((cityRuleArrayX[i].x - (((cityRuleArrayX[i].z) * 2) + cityRuleArrayX[i].w)) / subsX)) || buildingModifierx.prefabWidth < Mathf.FloorToInt((cityRuleArrayZ[j].x - (((cityRuleArrayZ[j].z) * 2) + cityRuleArrayZ[j].w)) / subsZ)) validate = true;

                                        iterations++;
                                    }
                                    if (iterations == prefabs.Length) prefabToInstantiate = prefabs[0];

                                    if (useRivers && riverPosition != i)
                                    {
                                        GameObject cloneH = Instantiate(prefabToInstantiate, new Vector3(((cityRuleArrayX[i].y * 3f) + (cityRuleArrayX[i].z * 3f)) + (3f * subi * (Mathf.FloorToInt((cityRuleArrayX[i].x - (((cityRuleArrayX[i].z) * 2) + cityRuleArrayX[i].w)) / subsX))), 0, ((cityRuleArrayZ[j].y * 3f) + (cityRuleArrayZ[j].z * 3f)) + (3f * subj * (Mathf.FloorToInt((cityRuleArrayZ[j].x - (((cityRuleArrayZ[j].z) * 2) + cityRuleArrayZ[j].w)) / subsZ)))), transform.rotation) as GameObject;
                                        cloneH.transform.parent = buildings.transform;
                                        cloneH.transform.name = "Section_" + (j + numberOfBuildingsZ * (i)) + "_Building_" + ((subi + 1) * (subj + 1));

                                        cloneH.transform.localPosition = cloneH.transform.position - new Vector3(cityCenterWeightX, 0, cityCenterWeightZ);
                                        BuildingModifier buildingModifier = cloneH.GetComponent(typeof(BuildingModifier)) as BuildingModifier;
                                        CSRooftops csRooftopsModifier = cloneH.GetComponent(typeof(CSRooftops)) as CSRooftops;
                                        CSAdvertising csAdverts = cloneH.GetComponent(typeof(CSAdvertising)) as CSAdvertising;
                                        buildingModifier.buildingWidth = Mathf.FloorToInt((cityRuleArrayX[i].x - (((cityRuleArrayX[i].z) * 2) + cityRuleArrayX[i].w)) / subsX);
                                        buildingModifier.buildingDepth = Mathf.FloorToInt((cityRuleArrayZ[j].x - (((cityRuleArrayZ[j].z) * 2) + cityRuleArrayZ[j].w)) / subsZ);
                                        buildingModifier.cityRandomizerParent = cRandomizerThis;

                                        if (subj == subsZ - 1)
                                        {
                                            cloneH.transform.Rotate(0, 180, 0);
                                            cloneH.transform.Translate(buildingModifier.buildingWidth * -3, 0, buildingModifier.buildingDepth * -3);

                                        }

                                        if (subj != subsZ - 1 && subj != 0)
                                        {
                                            cloneH.transform.Rotate(0, -90, 0);
                                            int bd = buildingModifier.buildingDepth;
                                            buildingModifier.buildingDepth = buildingModifier.buildingWidth;
                                            buildingModifier.buildingWidth = bd;
                                            cloneH.transform.Translate(0, 0, buildingModifier.buildingDepth * -3);

                                        }

                                        if (subj != subsZ - 1 && subj != 0 && subi == 0)
                                        {
                                            cloneH.transform.Rotate(0, -180, 0);
                                       //     int bd = buildingModifier.buildingDepth;
                                       //     buildingModifier.buildingDepth = buildingModifier.buildingWidth;
                                       //     buildingModifier.buildingWidth = bd;
                                            cloneH.transform.Translate(buildingModifier.buildingWidth * -3, 0, buildingModifier.buildingDepth * -3);

                                        }


                                        if (csRooftopsModifier != null)
                                        {
                                            csRooftopsModifier.randomSeed = Random.Range(0, 1000000);
                                            csRooftopsModifier.lodDistance = 0.18f;
                                            csRooftopsModifier.instancesX = 300;
                                        }
                                        if (csAdverts != null)
                                        {
                                            csAdverts.randomSeed = Random.Range(0, 1000000);
                                        }


                                        buildingModifier.AwakeCity();
                                        buildingModifier.UpdateCity();
                                    }
                                    
                                }


                            }


                        }
                    }
                }

            }

            Randomize();
            generate = false;
        }

        public void GenerateDetails()
        {

            foreach (Transform go in streets.transform.Cast<Transform>().Reverse())
            {
                StreetModifier streetMod = go.GetComponent(typeof(StreetModifier)) as StreetModifier;
                if (streetMod != null) {
                    if (streetMod.streetType != StreetModifier.CScapeStreetType.River)
                    {
                        GameObject cloneH = Instantiate(streetDetailPrefabs[Random.Range(0, streetDetailPrefabs.Length)], go.transform.position, transform.rotation) as GameObject;
                        cloneH.transform.parent = streetDetails.transform;
                        cloneH.transform.name = go.name + "Street_";
                        CSInstantiator streetDetailsModifier = cloneH.GetComponent(typeof(CSInstantiator)) as CSInstantiator;
                        cloneH.SetActive(true);

                        streetDetailsModifier.depth = streetMod.sectionWidth;
                        streetDetailsModifier.width = streetMod.sectionDepth;
                        streetDetailsModifier.AwakeMe();


                    }
                }
            }
        }

        public void GenerateLights()
        {
            for (int i = 0; i < numberOfBuildingsX; i++)
            {

                for (int j = 0; j < numberOfBuildingsZ; j++)
                {

                    GameObject cloneH = Instantiate(streetLightsPrefabs[Random.Range(0, streetLightsPrefabs.Length)], new Vector3(cityRuleArrayX[i].y * 3f, 0, cityRuleArrayZ[j].y * 3f), transform.rotation) as GameObject;
                    cloneH.transform.parent = streetLights.transform;
                    cloneH.transform.name = "Street_" + j + "_" + i;
                    cloneH.transform.localPosition = cloneH.transform.position - new Vector3(cityCenterWeightX, 0, cityCenterWeightZ);
                    CSInstantiatorLights streetLightsModifier = cloneH.GetComponent(typeof(CSInstantiatorLights)) as CSInstantiatorLights;

                    streetLightsModifier.depth = Mathf.RoundToInt(cityRuleArrayX[i].x - (cityRuleArrayX[i].w));
                    streetLightsModifier.width = Mathf.RoundToInt(cityRuleArrayZ[j].x - (cityRuleArrayZ[j].w));
                    streetLightsModifier.Awake();
                    streetLightsModifier.UpdateElements();



                }
            }
        }

        public void GenerateFolliage()
        {
            for (int i = 0; i < numberOfBuildingsX; i++)
            {

                for (int j = 0; j < numberOfBuildingsZ; j++)
                {
                    if (cityRuleArrayX[i].z >= folliageThreshold && cityRuleArrayZ[j].z >= folliageThreshold)
                    {
                        if (useRivers && riverPosition != i)
                        {
                            GameObject cloneH = Instantiate(streetFoliagePrefabs[Random.Range(0, streetFoliagePrefabs.Length)], new Vector3(cityRuleArrayX[i].y * 3f, 0, cityRuleArrayZ[j].y * 3f), transform.rotation) as GameObject;
                            cloneH.transform.parent = foliage.transform;
                            cloneH.transform.name = "Street_" + j + "_" + i;
                            cloneH.transform.localPosition = cloneH.transform.position - new Vector3(cityCenterWeightX, 0, cityCenterWeightZ);
                            CSFoliageInstantiator streetDetailsModifier = cloneH.GetComponent(typeof(CSFoliageInstantiator)) as CSFoliageInstantiator;
                            streetDetailsModifier.depth = Mathf.RoundToInt(cityRuleArrayX[i].x - (cityRuleArrayX[i].w));
                            streetDetailsModifier.width = Mathf.RoundToInt(cityRuleArrayZ[j].x - (cityRuleArrayZ[j].w));
                            streetDetailsModifier.Awake();
                            streetDetailsModifier.UpdateElements();
                        }
                    }
                }
            }
        }

        public void GenerateStreets()
        {
            Random.InitState(randomSeed);
            
            for (int i = 0; i < numberOfBuildingsX; i++)
            {

                for (int j = 0; j < numberOfBuildingsZ; j++)
                {

                    GameObject cloneH;
                    if (useRivers && riverPosition == i) { 
                        cloneH = Instantiate(buildPrefabs[0], new Vector3(cityRuleArrayX[i].y * 3f, 0, cityRuleArrayZ[j].y * 3f), transform.rotation) as GameObject;
                        if (j == 0)
                        {
                            GameObject nordlake = Instantiate(river, new Vector3(cityRuleArrayX[i].y * 3f, 0, cityRuleArrayZ[j].y * 3f), Quaternion.identity) as GameObject;
                            nordlake.transform.position = (nordlake.transform.position + gameObject.transform.position);
                            nordlake.transform.localScale = new Vector3(Mathf.RoundToInt(cityRuleArrayX[i].x), 1, citySize);
                            nordlake.transform.name = "NordLakeRiver_" + j + "_" + i;
                            nordlake.transform.parent = streets.transform;
                            ReflectionProbe rProbe = nordlake.transform.Find("Reflection Probe").GetComponent<ReflectionProbe>();
                            rProbe.size = new Vector3(cityRuleArrayX[i].x * 3, rProbe.size.y, nordlake.transform.localScale.z * 3);
                        }
                    }
                    else
                    {
                        cloneH = Instantiate(buildPrefabs[1], new Vector3(cityRuleArrayX[i].y * 3f, 0, cityRuleArrayZ[j].y * 3f), transform.rotation) as GameObject;
                        
                    }
                    cloneH.transform.parent = streets.transform;
                    cloneH.transform.name = "Street_" + j + "_" + i;
                    //       clone.transform.localPosition = clone.transform.position;
                    cloneH.transform.localPosition = cloneH.transform.position - new Vector3(cityCenterWeightX, 0, cityCenterWeightZ);
                    
                    StreetModifier streetModifier = cloneH.GetComponent(typeof(StreetModifier)) as StreetModifier;
                    streetModifier.blockWidth = Mathf.RoundToInt(cityRuleArrayX[i].x);
                    streetModifier.blockDepth = Mathf.RoundToInt(cityRuleArrayZ[j].x);
                    streetModifier.sectionWidth = Mathf.RoundToInt(cityRuleArrayX[i].x - cityRuleArrayX[i].w);
                    streetModifier.sectionDepth = Mathf.RoundToInt(cityRuleArrayZ[j].x - cityRuleArrayZ[j].w);
                    streetModifier.sidewalkID = Mathf.RoundToInt(Random.Range(0f, 6f));
                    //  Debug.Log(streetModifier.sectionWidth + " street Size " + cityRuleArrayX[i].w + "Block width is: " + cityRuleArrayX[i].x);
                    streetModifier.AwakeCity();
                    streetModifier.UpdateCity();

                }
            }
        }

        public void Refresh()
        {

            Component[] bmodif = buildings.GetComponentsInChildren<BuildingModifier>();

            foreach (BuildingModifier x in bmodif)
            {
                x.AwakeCity();
                x.UpdateCity();

            }

            Component[] smodif = streets.GetComponentsInChildren<StreetModifier>();
            foreach (StreetModifier x in smodif)
            {
                x.AwakeCity();
                x.UpdateCity();
            }

            Component[] sdmodif = streetDetails.GetComponentsInChildren<CSInstantiator>();
            foreach (CSInstantiator x in sdmodif)
            {
                x.UpdateElements();
            }
        }

        public void UpdateHeights()
        {
           // Random.InitState(randomSeed);
           // float curveVal = cityCurve.Evaluate(0f);

            foreach (Transform go in buildings.transform.Cast<Transform>().Reverse()) 
            {
                BuildingModifier building = go.GetComponent(typeof(BuildingModifier)) as BuildingModifier;
                float dist = Vector3.Distance(cityCenterObject.transform.position, go.position);
         //       int rnd = Random.Range(0, nightColors.Length - 1);
                building.floorNumber = Random.Range(minFloors, maxFloors + Mathf.CeilToInt(cityCurve.Evaluate(dist / (blockDistances * 3f * numberOfBuildingsX / 2)) * 200f));

            }
        }

        public void StripScripts()
        {
            foreach (Transform go in streetDetails.transform.Cast<Transform>().Reverse())
            {
                CSInstantiator csInstantiator = go.GetComponent(typeof(CSInstantiator)) as CSInstantiator;
                DestroyImmediate(csInstantiator);
            }

            foreach (Transform go in streets.transform.Cast<Transform>().Reverse())
            {
                StreetModifier csInstantiator = go.GetComponent(typeof(StreetModifier)) as StreetModifier;
                DestroyImmediate(csInstantiator);
            }

            foreach (Transform go in streetLights.transform.Cast<Transform>().Reverse())
            {
                CSInstantiatorLights csInstantiator = go.GetComponent(typeof(CSInstantiatorLights)) as CSInstantiatorLights;
                DestroyImmediate(csInstantiator);
            }

            foreach (Transform go in buildings.transform.Cast<Transform>().Reverse())
            {
                CSAdvertising csInstantiator = go.GetComponent(typeof(CSAdvertising)) as CSAdvertising;
                DestroyImmediate(csInstantiator);
            }

            foreach (Transform go in buildings.transform.Cast<Transform>().Reverse())
            {
                CSRooftops csInstantiator = go.GetComponent(typeof(CSRooftops)) as CSRooftops;
                DestroyImmediate(csInstantiator);
            }

            foreach (Transform go in buildings.transform.Cast<Transform>().Reverse())
            {
                CSArray csInstantiator = go.GetComponent(typeof(CSArray)) as CSArray;
                DestroyImmediate(csInstantiator);
            }

            foreach (Transform go in buildings.transform.Cast<Transform>().Reverse())
            {
                BuildingModifier csInstantiator = go.GetComponent(typeof(BuildingModifier)) as BuildingModifier;
                DestroyImmediate(csInstantiator);
            }



        }


        public void SetColliders()
        {

            foreach (Transform go in buildings.transform.Cast<Transform>().Reverse())
            {
                MeshCollider colliderMesh = go.GetComponent(typeof(MeshCollider)) as MeshCollider;
                DestroyImmediate(colliderMesh);
                MeshCollider col = go.gameObject.AddComponent<MeshCollider>() as MeshCollider;
                if (useConvexCollider) col.convex = true;
                //
            }

            foreach (Transform go in streets.transform.Cast<Transform>().Reverse())
            {
                MeshCollider colliderMesh = go.GetComponent(typeof(MeshCollider)) as MeshCollider;
                DestroyImmediate(colliderMesh);
                MeshCollider col = go.gameObject.AddComponent<MeshCollider>() as MeshCollider;
                if (useConvexCollider) col.convex = true;
                //
            }

            foreach (Transform go in streetDetails.transform.Cast<Transform>().Reverse())
            {
                MeshCollider colliderMesh = go.GetComponent(typeof(MeshCollider)) as MeshCollider;
                BoxCollider collider = go.GetComponent(typeof(BoxCollider)) as BoxCollider;
                DestroyImmediate(collider);
                DestroyImmediate(colliderMesh);
                MeshCollider col = go.gameObject.AddComponent<MeshCollider>() as MeshCollider;
                col.sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
                if (useConvexCollider) col.convex = true;
                //
            }

            foreach (Transform go in rooftopDetails.transform.Cast<Transform>().Reverse())
            {
                MeshCollider colliderMesh = go.GetComponent(typeof(MeshCollider)) as MeshCollider;
                DestroyImmediate(colliderMesh);
                MeshCollider col = go.gameObject.AddComponent<MeshCollider>() as MeshCollider;
                col.sharedMesh = go.GetComponent<MeshFilter>().sharedMesh;
                if (useConvexCollider) col.convex = true;
                //
            }
        }



        public void Randomize()
        {
           Random.InitState(randomSeed);

            BuildingModifier[] buildingModifierArray = buildings.GetComponentsInChildren<BuildingModifier>();
            int init = 0;

            //    foreach (Transform go in buildings.transform.Cast<Transform>().Reverse())
            //{
            //    BuildingModifier building = go.GetComponent(typeof(BuildingModifier)) as BuildingModifier;
            //for (int i = 0; i < buildingModifierArray.Length; i++)
            //{
            foreach (BuildingModifier scripts in buildingModifierArray)
            {
                init = init + 1;
                Random.InitState(randomSeed + init);
              //  Debug.Log(scripts.transform.name + Random.Range(0, 9));
                float dist = Vector3.Distance(cityCenterObject.transform.position, scripts.gameObject.transform.position);
                int rnd = Random.Range(0, nightColors.Length - 1);
                scripts.colorVariation.x = Random.Range(0, 9);
                scripts.colorVariation.y = Mathf.Floor(nightColors[rnd].r * 9f);
                scripts.colorVariation.z = Mathf.Floor(nightColors[rnd].g * 9f);
                scripts.colorVariation.w = Mathf.Floor(nightColors[rnd].b * 9f);

                scripts.colorVariation2.x = Random.Range(0, 9);
                scripts.colorVariation2.y = Random.Range(0, 9);
                scripts.colorVariation2.z = Random.Range(0, 9);
                scripts.colorVariation2.w = Random.Range(2, 9);

                scripts.colorVariation3.x = Random.Range(0, 9);
                scripts.colorVariation3.y = Random.Range(0, 9);
                scripts.colorVariation3.z = Random.Range(0, 9);
                scripts.colorVariation3.w = Random.Range(0, 9);

                scripts.colorVariation4.x = Random.Range(0, 9);
                scripts.colorVariation4.y = Random.Range(0, 9);
                scripts.colorVariation4.z = Random.Range(0, 9);
                scripts.colorVariation4.w = Random.Range(0, 9);
                scripts.lightnessFront = Random.Range(0, 9);
                scripts.lightnessSide = Random.Range(0, 9);
                scripts.colorVariation5.x = Random.Range(0, 9);
                scripts.colorVariation5.y = Random.Range(0, 9);
                scripts.borderCol = Random.Range(0, 9);
                scripts.lightsOnOff = Random.Range(0, 9);



                if (height)
                    scripts.floorNumber = Random.Range(minFloors, maxFloors + Mathf.CeilToInt(cityCurve.Evaluate(dist / (blockDistances * 3f * numberOfBuildingsX / 2)) * 200f));
                //        if (depth)
                //            building.buildingDepth = Random.Range(minDepth, maxDepth);
                //        if (width)
                //            building.buildingWidth = Random.Range(minWidth, maxWidth);
                scripts.uniqueMapping = Random.Range(-160, 160);
                if (faccadeStyles)
                {
                    scripts.colorVariation2.y = Random.Range(0, 9);
                    scripts.colorVariation2.z = Random.Range(0, 9);
                    scripts.materialId1 = Random.Range(minMatIndex, maxMatIndex);
                    scripts.materialId2 = Random.Range(minMatIndex1, maxMatIndex1);
                    scripts.materialId3 = Random.Range(minMatIndex2, maxMatIndex2);
                    scripts.materialId4 = Random.Range(minMatIndex4, maxMatIndex4);
                    scripts.materialId5 = Random.Range(0, 30);
                }
                if (openWindow)
                    scripts.windowOpen = Random.Range(minWindowOpen, maxWindowOpen);
                if (rndPatternHorizontal)
                    scripts.pattern = Random.Range(0f, 1f);
                if (rndColor)
                {
                    scripts.UpdateCity();
                }



                //  buildingModifierArray[i].AwakeCity();
                //  buildingModifierArray[i].UpdateCity();
            }

            CSAdvertising[] advertisings = buildings.GetComponentsInChildren<CSAdvertising>();
            int initAdv = 0;
            foreach (CSAdvertising scripts in advertisings)
            {
                init = init + 1;
                Random.InitState(randomSeed + init);
                scripts.randomSeed = randomSeed + init;
                scripts.instancesX = Random.Range(50, 150);

            }

                Refresh();

        }


    }
}

#endif