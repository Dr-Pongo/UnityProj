using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Making sure there is a controller object
[RequireComponent(typeof(Controller))]

public class Hand : MonoBehaviour
{

    GameObject heldObject;
    Controller controller;

    Rigidbody simulator;

	// Use this for initialization
	void Start ()
    {
        // Allows the object to be throwable
        simulator = new GameObject().AddComponent<Rigidbody>();
        simulator.name = "simulator";
        simulator.transform.parent = transform.parent;

        // Setting controller object
        controller = GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Making sure there is not already a held object
		if(heldObject)
        {
            // Velocity for when object is let go of (50 is arbitrary, but "works well")
            simulator.velocity = (transform.position - simulator.position) * 50f;

            // Is the controller trigger pressed
            if (controller.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) // annoyingly long trigger name
            {
                heldObject.transform.parent = null;
                heldObject.GetComponent<Rigidbody>().isKinematic = false; // Has gravity
                heldObject.GetComponent<Rigidbody>().velocity = simulator.velocity; // object will be thrown
                heldObject = null;
            }
        }
        else
        {
            // Is the controller trigger pressed
            if(controller.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) // annoyingly long trigger name
            {
                // array of colliders that are within the area (0.1 radius) of the controller
                Collider[] cols = Physics.OverlapSphere(transform.position, 0.1f);

                // see if all of these are ready to be held
                foreach(Collider col in cols)
                {
                    // Make sure it's not being held by anything else && check to see if it can be held
                    if(heldObject == null && col.GetComponent<HeldObject>() && col.GetComponent<HeldObject>().parent == null)
                    {
                        // seting object to be held
                        heldObject = col.gameObject;

                        // Sets object to controller to follow around
                        heldObject.transform.parent = transform;
                        
                        heldObject.transform.localPosition = Vector3.zero;
                        heldObject.transform.localRotation = Quaternion.identity;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;

                        // Set parent of held object
                        heldObject.GetComponent<HeldObject>().parent = controller;

                    }
                }
            }
        }
	}
}
