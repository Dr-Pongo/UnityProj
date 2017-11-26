using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera_Pass_Script : MonoBehaviour {

    public GameObject TrackedCamera;
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;
    bool menuPressed;


    // Use this for initialization
    void Start () {
        TrackedCamera.SetActive(false);

        rightController.PadClicked += CtrlClicked;
        leftController.PadClicked += CtrlClicked;
    }
	
	// Update is called once per frame
	void Update () {
		if(menuPressed)
        {
            if (TrackedCamera.activeInHierarchy)
                TrackedCamera.SetActive(false);
            else
                TrackedCamera.SetActive(true);

            menuPressed = false;
        }
	}

    void CtrlClicked(object sender, ClickedEventArgs e)
    {
        menuPressed = true;
    }
}
