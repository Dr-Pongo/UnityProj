using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWVR_InteractionController : MonoBehaviour {
    public Transform snapColliderOrigin; // 1
    public GameObject ControllerModel; // 2

    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public Vector3 angularVelocity;
    private RWVR_InteractionObject objectBeingInteractedWith; // 5

    private SteamVR_TrackedObject trackedObj; // 6

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public RWVR_InteractionObject InteractionObject
    {
        get { return objectBeingInteractedWith; }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void CheckForInteractionObject()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(snapColliderOrigin.position, snapColliderOrigin.lossyScale.x / 2f);

        foreach (Collider overlappedCollider in overlappedColliders)
        {
            if (overlappedCollider.CompareTag("InteractionObject") && overlappedCollider.GetComponent<RWVR_InteractionObject>().IsFree())
            {
                objectBeingInteractedWith = overlappedCollider.GetComponent<RWVR_InteractionObject>();
                objectBeingInteractedWith.OnTriggerWasPressed(this);
                return;
            }
        }
    }

    void Update()
    {
        if (Controller.GetHairTriggerDown())
        {
            CheckForInteractionObject();
        }

        if (Controller.GetHairTrigger())
        {
            if (objectBeingInteractedWith)
            {
                objectBeingInteractedWith.OnTriggerIsBeingPressed(this);
            }
        }

        if (Controller.GetHairTriggerUp())
        {
            if (objectBeingInteractedWith)
            {
                objectBeingInteractedWith.OnTriggerWasReleased(this);
                objectBeingInteractedWith = null;
            }
        }
    }

    private void UpdateVelocity()
    {
        velocity = Controller.velocity;
        angularVelocity = Controller.angularVelocity;
    }

    void FixedUpdate()
    {
        UpdateVelocity();
    }

    public void HideControllerModel()
    {
        ControllerModel.SetActive(false);
    }

    public void ShowControllerModel()
    {
        ControllerModel.SetActive(true);
    }

    public void Vibrate(ushort strength)
    {
        Controller.TriggerHapticPulse(strength);
    }

    public void SwitchInteractionObjectTo(RWVR_InteractionObject interactionObject)
    {
        objectBeingInteractedWith = interactionObject;
        objectBeingInteractedWith.OnTriggerWasPressed(this);
    }
}
