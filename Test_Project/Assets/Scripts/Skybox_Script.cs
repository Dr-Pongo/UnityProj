using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityStandardAssets.CrossPlatformInput;

public class Skybox_Script : MonoBehaviour
{

    SerialPort mySPort = new SerialPort("COM4", 9600);

    public Material skyDefault;
    public Material skyOne;

    // Use this for initialization
    void Start()
    {
        mySPort.Open();
        RenderSettings.skybox = skyDefault;
    }

    // Update is called once per frame
    void Update()
    {
        string dataFromArduinoString = mySPort.ReadLine();
        controlObjects(dataFromArduinoString);
    }

    void controlObjects(string switchData)
    {
        switch (switchData)
        {
            case "OnOff1val - ON":
                RenderSettings.skybox = skyOne;
                break;
            default:
                RenderSettings.skybox = skyDefault;
                break;
        }
    }
}
