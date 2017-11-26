using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick_to_Objects_Acript : RWVR_InteractionObject {

    public bool hidecontrollerMod;
    public Vector3 posOffset;
    public Vector3 rotateOffSet;
    private Rigidbody RB;
    //public GameObject Chair_Object;
    //public GameObject Panel_Object;
    //public SteamVR_TrackedController leftController;
    //public SteamVR_TrackedController rightController;
    //public bool triggerPressed = false;

    // Use this for initialization
    //void Start () {
    //    rightController.TriggerClicked += TriggerClicked;
    //    leftController.TriggerClicked += TriggerClicked;
    //}

    //void TriggerClicked(object sender, ClickedEventArgs e)
    //{
    //    triggerPressed = true;
    //}

    public override void Awake()
    {
        base.Awake();
        RB = GetComponent<Rigidbody>();
    }

    private void connecttoController(RWVR_InteractionController controller)
    {
        cachedTransform.SetParent(controller.transform);

        cachedTransform.rotation = controller.transform.rotation;
        cachedTransform.Rotate(rotateOffSet);
        cachedTransform.position = controller.snapColliderOrigin.position;
        cachedTransform.Translate(posOffset, Space.Self);

        RB.useGravity = false;
        RB.isKinematic = true;
    }

    public override void OnTriggerWasPressed(RWVR_InteractionController controller)
    {
        base.OnTriggerWasPressed(controller);

        connecttoController(controller);
    }

    /*
     * Release must be implemented in another way. Releasing should be on a controller with another trigger pull.
     * */

    //public override void OnTriggerWasReleased(RWVR_InteractionController controller) // 1
    //{
    //    base.OnTriggerWasReleased(controller); // 2

    //    //if (hideControllerModel) // 3
    //    //{
    //    //    controller.ShowControllerModel();
    //    //}

    //    ReleaseFromController(controller); // 4
    //}


    //private void ReleaseFromController(RWVR_InteractionController controller) // 1
    //{
    //    cachedTransform.SetParent(null); // 2

    //    RB.useGravity = true; // 3
    //    RB.isKinematic = false;

    //    RB.velocity = controller.velocity; // 4
    //    RB.angularVelocity = controller.angularVelocity;
    //}

}
