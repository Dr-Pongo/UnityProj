using UnityEngine;
using System.Collections;

public class createwall : MonoBehaviour
{

 bool creating;
 public GameObject start;
 public GameObject end;
 public GameObject WallPrefab;
 GameObject Wall;
 public Camera camera;
 int layerMask = 1 << 13;//13 is the Layer number I set to "IgnoreRaycast" in the Inspector.
 void Start ()
 {
  camera = Camera.main.GetComponent<Camera> ();
  layerMask = ~layerMask;
 }
 void Update ()
 {
  GetInput ();
  if (Wall != null) {
   if (Wall.GetComponent<Renderer> ().material.mainTexture.wrapMode != TextureWrapMode.Repeat) {
    Debug.LogError ("CreateWalls.cs: WallPrefab at Line 16ish must have its material texture set to Repeat");
   }
   Wall.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 ((Wall.transform.localScale.x + Wall.transform.localScale.z * 1) / 1, (Wall.transform.localScale.y + Wall.transform.localScale.z * 1) / 10);
            //I divided by 10 becuase it worked alright with my texture
  }
 }
 void GetInput ()
 {
  if (Input.GetMouseButtonDown (0)) {
   Setstart ();
  } else if (Input.GetMouseButtonUp (0)) {
   SetEnd ();
   GameObject[] CloneToSmall;//was getting small gameObject artifact after by clicking without dragging
   CloneToSmall = GameObject.FindGameObjectsWithTag ("IgnoreRaycast");
   foreach (GameObject go in CloneToSmall) {
    if (go.gameObject.transform.localScale.z <= 0.5) {
     Destroy (go.gameObject);
    }
   }
  } else {
   if (creating) {
    Adjust ();
   }
  }
 }
 void Setstart ()
 {
  creating = true;
  start.transform.position = GetWorldPoint ();
  Wall = Instantiate (WallPrefab, start.transform.position, Quaternion.identity)as GameObject;
  Wall.transform.SetParent (transform);//If this script was added to the camera you may get undesired effects.
  Wall.gameObject.name = "Wall(Clone)";//so the wall can be referenced later if needed
 }
 void SetEnd ()
 {
  creating = false;
  end.transform.position = GetWorldPoint ();
 }
 void Adjust ()
 {
  end.transform.position = GetWorldPoint ();
  AdjustWall ();
 }

 void AdjustWall ()
 {
  start.transform.LookAt (end.transform.position);
  end.transform.LookAt (start.transform.position);
  float dist = Vector3.Distance (start.transform.position, end.transform.position);
  Wall.transform.position = start.transform.position + dist / 2 * start.transform.forward;
  Wall.transform.rotation = start.transform.rotation;
  Wall.transform.localScale = new Vector3 (Wall.transform.localScale.x, Wall.transform.localScale.y, dist / 2);//dist / 2 rather than just dist fixes scale

 }
 Vector3 GetWorldPoint ()
 {
  Ray ray = camera.ScreenPointToRay (Input.mousePosition);
  RaycastHit hit;
  if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
   Vector3 pos = new Vector3 (hit.point.x, 0, hit.point.z);
   return pos;
  }
  Vector3 Startpos = Input.mousePosition;
  return Startpos;//Using Vector3.zero cuased it to glitch look at 0,0,0.
 }
}﻿
