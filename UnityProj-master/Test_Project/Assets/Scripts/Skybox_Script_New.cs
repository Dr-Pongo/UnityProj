using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Skybox_Script_New : MonoBehaviour
{
    public Material skyDefault;
    public Material skyOne;

    // Use this for initialization
    void Start()
    {
        RenderSettings.skybox = skyDefault;
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox = skyOne;
    }
}
