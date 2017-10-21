using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityStandardAssets.CrossPlatformInput;

public class Skybox_Script : MonoBehaviour
{

    SerialPort mySPort = new SerialPort("COM3", 9600);

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
        int dataFromArduinoString = int.Parse(mySPort.ReadLine());
        controlObjects(dataFromArduinoString);
    }

    void controlObjects(int switchData)
    {
        switch (switchData)
        {
            case 0:
                RenderSettings.skybox = skyOne;
                break;
            default:
                RenderSettings.skybox = skyDefault;
                break;
        }
    }
}
