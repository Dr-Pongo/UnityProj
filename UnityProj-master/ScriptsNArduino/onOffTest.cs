using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityStandardAssets.CrossPlatformInput;

public class onOffTest : MonoBehaviour {

    SerialPort mySPort2 = new SerialPort("COM3", 9600);
    public GameObject switchObject;

    public float offX = 0.53f, offY = 0.24f, offZ = .53f;
    public float onX = .8f, onY = .5f, onZ = 1f;

    // Use this for initialization
    void Start () {

        mySPort2.Open();

    }
	
	// Update is called once per frame
	void Update ()
    {
        int dataFromArduinoString = int.Parse(mySPort2.ReadLine());
        //int dFAI = int.Parse(dataFromArduinoString);
        print("");
        print("The Serial Port Reads: ");
        print("- " + dataFromArduinoString);
        print("");
        //print(dFAI);
        //controlObjects(dFAI);

    }

    //
    void controlObjects(int buttonStatus)
    {

        switch (buttonStatus)
        {
            case 0:
                switchObject.gameObject.transform.localScale = new Vector3(offX, offY, offZ);

                break;
            case 1:
                switchObject.gameObject.transform.localScale = new Vector3(onX, onY, onZ);

                if (Input.GetButtonDown("Fire1"))
                {
                    // runs this code when the fire button is pressed down
                    print("Pressed left click.");
                    var list = switchObject.GetComponent<Animation>();
                    switchObject.GetComponent<Animation>().Play("SmallButtonPress");  // this will play the default animation on this object
                }

                break;
        }

    }
}
