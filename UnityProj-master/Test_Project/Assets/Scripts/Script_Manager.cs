
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
using System;
using System.Linq;

public class Script_Manager : MonoBehaviour
{

    static string[] portname = System.IO.Ports.SerialPort.GetPortNames();
    SerialPort mySPort;
    public GameObject Rain_Script;
    public GameObject HeadLights_Script;
    public GameObject SkyBox_Script;
    public GameObject Lightning_Script;
    public GameObject Hail_Script;

    public GameObject toggle;
    public GameObject rotaryButton;
    public GameObject button;
    public GameObject switchToggle;
    public GameObject coverSwitch;

    public GameObject toggle_led;
    public GameObject rotaryButton_led;
    public GameObject button_led;
    public GameObject switch_led;
    public GameObject coverSwitch_led;

    public Thread t;
    public BitArray bits;
    public int[] states = { 0, 0, 0, 0, 0, 0, 0, 0 };
    public byte[] data;

    public bool rotaryAState = false;
    public bool rotaryBState = false;
    public bool rotaryCState = false;
    public int rotaryState = 0;

    public bool toggleState = false;
    public bool buttonState = false;
    public bool switchState = false;
    public bool coverSwitchState = false;
    public bool hallSensorState = false;

    // Use this for initialization
    public void Start()
    {
        mySPort = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);

        if (!mySPort.IsOpen)
        {
            mySPort.Open();
            //mySPort.ReadBufferSize = 4096;
            //mySPort.ParityReplace = 63;
            //mySPort.WriteBufferSize = 2048;
            //mySPort.WriteTimeout = -1;
            //mySPort.DtrEnable = false;
            //mySPort.RtsEnable = false;
            //mySPort.ReadTimeout = -1;


            UnityEngine.Debug.Log("Port " + portname[0] + " has been opened");

            mySPort.Write("r");
        }
        else
        {
            UnityEngine.Debug.Log("Port is already open");
        }

        t = new Thread(() =>
        {
            while (true)
            {
                ReadFromPort();
            }

        });

        t.Start();
    }

    public void ReadFromPort()
    {
        int data = Convert.ToInt16(mySPort.ReadLine());
        bits = new BitArray(new byte[] { (byte)data });
        states = bits.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray();
    }

    public void SetStates()
    {
        rotaryAState = (states[0] == 1) ? true : false;
        rotaryBState = (states[1] == 1) ? true : false;
        rotaryCState = (states[2] == 1) ? true : false;
        rotaryState = setRotaryValue();

        toggleState = (states[3] == 1) ? true : false;
        buttonState = (states[4] == 1) ? true : false;
        switchState = (states[5] == 1) ? true : false;
        hallSensorState = (states[6] == 1) ? true : false;
        coverSwitchState = (states[7] == 1) ? true : false;

        scriptManager_Overseer();
    }

    public void scriptManager_Overseer()
    {
        Toggle_Check();
        Rotary_Button_Check();
        Button_Check();
        Switch_Check();
        Cover_Switch_Check();
        Hall_Sensor_Check();
    }

    public int setRotaryValue()
    {
        if (!rotaryAState && !rotaryBState && !rotaryCState)
            return 0;
        else if (rotaryAState && !rotaryBState && !rotaryCState)
            return 1;
        else if (!rotaryAState && rotaryBState && !rotaryCState)
            return 2;
        else if (rotaryAState && rotaryBState && !rotaryCState)
            return 3;
        else if (!rotaryAState && !rotaryBState && rotaryCState)
            return 4;
        else if (rotaryAState && !rotaryBState && rotaryCState)
            return 5;
        else if (!rotaryAState && rotaryBState && rotaryCState)
            return 6;
        else
            return 7;
    }

    public void Toggle_Check()
    {
        if (toggleState == true)
        {
            Rain_Script.SetActive(true);
            toggle_led.SetActive(true);
            UnityEngine.Debug.Log("Toggle State is True");
        }
        else
        {
            Rain_Script.SetActive(false);
            toggle_led.SetActive(false);
            UnityEngine.Debug.Log("Toggle State is false");
        }
    }

    public void Rotary_Button_Check()
    {
        //switch (rotaryState)
        //{
        //    case 0:
        //        Shader led_shader = rotaryButton_led.GetComponent<Shader>();
        //        break;
        //    case 1:
        //        break;
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //}
    }

    public void Button_Check()
    {
        

        if (buttonState == true)
        {
            var animationObj = button.GetComponent<Animation>();

            button_led.SetActive(true);
            Hail_Script.SetActive(true);
            animationObj.wrapMode = WrapMode.Once;
            animationObj.Play("SmallButtonPress");
            UnityEngine.Debug.Log("Button State is True");
        }
        else
        {
            button_led.SetActive(false);
            Hail_Script.SetActive(false);
            UnityEngine.Debug.Log("Button State is False");
        }
    }

    public void Switch_Check()
    {
        if (switchState == true)
        {
            SkyBox_Script.SetActive(true);
            switch_led.SetActive(true);
            UnityEngine.Debug.Log("Switch State is True");
        }
        else 
        {
            SkyBox_Script.SetActive(false);
            switch_led.SetActive(false);
            UnityEngine.Debug.Log("Switch State is False");
        }
    }

    public void Cover_Switch_Check()
    {
        if (coverSwitchState == true)
        {
            coverSwitch_led.SetActive(true);
            Lightning_Script.SetActive(true);
            UnityEngine.Debug.Log("Cover Switch State is True");
        }
        else
        {
            coverSwitch_led.SetActive(false);
            Lightning_Script.SetActive(false);
            UnityEngine.Debug.Log("Cover Switch State is False");
        }
    }

    public void Hall_Sensor_Check()
    {
        if (hallSensorState == true)
        {
            UnityEngine.Debug.Log("Hall Sensor State is True");
        }
        else if (hallSensorState == false)
        {
            UnityEngine.Debug.Log("Hall Sensor State is False");
        }
    }


    // Update is called once per frame
    void Update()
    {

        //string dataFromArduinoString = mySPort.ReadByte().ToString();
        //controlObjects(dataFromArduinoString);
        //UnityEngine.Debug.Log("Arduino is reading the following line: " + dataFromArduinoString);

        SetStates();


    }
}
