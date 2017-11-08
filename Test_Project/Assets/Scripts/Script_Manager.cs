
/***************************************************************************
 * Script_Manager
 * Senior Project 2017 CAE
 * Purpose: Receives input from Arduino and sends via serial data to Unity,
 *          Enables any intended effects.                                  
 **************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

public class Script_Manager : MonoBehaviour {

    static string[] portname = System.IO.Ports.SerialPort.GetPortNames();
    SerialPort mySPort = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
    public GameObject Weather_Script;
    public GameObject HeadLights_Script;
    public GameObject SkyBox_Script;
    public GameObject Other_Script;

    public GameObject toggle_led;
    public GameObject rotaryButton_led;
    public GameObject button_led;
    public GameObject switch_led;
    public GameObject coverSwitch_led;

    public bool toggleState = false;
    public bool rotaryButtonState = false;
    public bool buttonState = false;
    public bool switchState = false;
    public bool coverSwitchState = false;
    public bool hallSensorState = false;

    // Use this for initialization
    void Start () {
        if (!mySPort.IsOpen)
        {
            mySPort.Open();
            mySPort.ReadBufferSize = 4096;
            mySPort.ParityReplace = 63;
            mySPort.WriteBufferSize = 2048;
            mySPort.WriteTimeout = -1;
            mySPort.DtrEnable = false;
            mySPort.RtsEnable = false;
            mySPort.ReadTimeout = -1;
            UnityEngine.Debug.Log("Port " + portname[0] + " has been opened");
        }
        else
        {
            UnityEngine.Debug.Log("Port is already open");
        }
    }

    public void ReadFromPort()
    {
        string r = mySPort.ReadLine();
        r = r.Replace("\r", "");

        Toggle_Check(r);
        Switch_Check(r);
        Cover_Switch_Check(r);
        //Hall_Sensor_Check(r);
        

    }

    public void Toggle_Check(string r)
    {
        if (toggleState == false && r.Equals("Tog ON"))
        {
            Weather_Script.SetActive(true);
            toggle_led.SetActive(true);
            toggleState = true;
            UnityEngine.Debug.Log(r);
        }
        else if (toggleState == true && r.Equals("Tog OFF"))
        {
            Weather_Script.SetActive(false);
            toggle_led.SetActive(false);
            toggleState = false;
            UnityEngine.Debug.Log(r);
        }
    }

    public void Rotary_Button_Check(string r)
    {

    }

    public void Button_Check(string r)
    {
        if (buttonState == false && r.Equals("Push ON"))
        {
            buttonState = true;
            UnityEngine.Debug.Log(r);
        }
        else if (buttonState == true && r.Equals("Push OFF"))
        {
            buttonState = false;
            UnityEngine.Debug.Log(r);
        }
    }

    public void Switch_Check(string r)
    {
        if (switchState == false && r.Equals("Switch ON"))
        {
            SkyBox_Script.SetActive(true);
            coverSwitch_led.SetActive(true);
            switchState = true;
            UnityEngine.Debug.Log(r);
        }
        else if (switchState == true && r.Equals("Switch OFF"))
        {
            SkyBox_Script.SetActive(false);
            coverSwitch_led.SetActive(false);
            switchState = false;
            UnityEngine.Debug.Log(r);
        }
    }

    public void Cover_Switch_Check(string r)
    {
        if (coverSwitchState == false && r.Equals("Cover ON"))
        {
            Other_Script.SetActive(true);
            coverSwitch_led.SetActive(true);
            coverSwitchState = true;
            UnityEngine.Debug.Log(r);
        }
        else if (coverSwitchState == true && r.Equals("Cover OFF"))
        {
            Other_Script.SetActive(false);
            coverSwitch_led.SetActive(false);
            coverSwitchState = false;
            UnityEngine.Debug.Log(r);
        }
    }

    public void Hall_Sensor_Check(string r)
    {
        if (hallSensorState == false && r.Equals("Cover DOWN"))
        {
            hallSensorState = true;
            UnityEngine.Debug.Log(r);
        }
        else if (hallSensorState == true && r.Equals("Cover UP"))
        {
            hallSensorState = false;
            UnityEngine.Debug.Log(r);
        }
    }


    // Update is called once per frame
    void Update () {

        string dataFromArduinoString = mySPort.ReadLine();
        //controlObjects(dataFromArduinoString);
        UnityEngine.Debug.Log("Arduino is reading the following line: " + dataFromArduinoString);

        ReadFromPort();


    }

    void controlObjects(string switchData)
    {
        switch (switchData)
        {
            case "onOffToggle - ON":
               Weather_Script.SetActive(true);
                //SkyBox_Script.SetActive(true);
                UnityEngine.Debug.Log("Weather Script to Rain is on");
                break;
            default:
                Weather_Script.SetActive(false);
                UnityEngine.Debug.Log("Weather Script to Rain is off");
                //SkyBox_Script.SetActive(false);
                //HeadLights_Script.SetActive(false);
                //Other_Script.SetActive(false); 
                break;
        }
    }
}
