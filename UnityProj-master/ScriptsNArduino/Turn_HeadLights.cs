using UnityEngine;
using Uniduino;

public class Turn_HeadLights : MonoBehaviour {

    public Arduino arduino;
    public int onOff_switch1 = 2;

    public int onOff_switch1_pinValue = 0;

    Light flashlight;
    bool on = false;

    // Use this for initialization
    void Start () {
        arduino = Arduino.global;
        arduino.Setup(ConfigurePins);

        flashlight = GetComponentInChildren<Light>();
        flashlight.enabled = false;

    }
	
	// Update is called once per frame
	void Update () {
        onOff_switch1_pinValue = arduino.digitalRead(onOff_switch1);
        controlObjects(onOff_switch1_pinValue);
    }

    void controlObjects(int switchData)
    {
        switch (switchData)
        {
            case 1:
                flashlight.enabled = true;
                break;
            default:
                flashlight.enabled = false;
                break;
        }
    }

    void ConfigurePins()
    {
        arduino.pinMode(onOff_switch1, PinMode.INPUT);
        arduino.reportDigital((byte)(onOff_switch1 / 8), 1);
    }
}
