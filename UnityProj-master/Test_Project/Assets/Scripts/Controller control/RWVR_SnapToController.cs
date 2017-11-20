using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWVR_SnapToController : RWVR_InteractionObject
{
    public bool hideControllerModel;
    public Vector3 snapPositionOffset;
    public Vector3 snapRotationOffset;

    private Rigidbody rb;

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void ConnectToController(RWVR_InteractionController controller)
    {
        cachedTransform.SetParent(controller.transform);

        cachedTransform.rotation = controller.transform.rotation;
        cachedTransform.Rotate(snapRotationOffset);
        cachedTransform.position = controller.snapColliderOrigin.position;
        cachedTransform.Translate(snapPositionOffset, Space.Self);

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void ReleaseFromController(RWVR_InteractionController controller)
    {
        cachedTransform.SetParent(null);

        rb.useGravity = true;
        rb.isKinematic = false;

        rb.velocity = controller.velocity;
        rb.angularVelocity = controller.angularVelocity;
    }

    public override void OnTriggerWasPressed(RWVR_InteractionController controller)
    {
        base.OnTriggerWasPressed(controller);

        if (hideControllerModel)
        {
            controller.HideControllerModel();
        }

        ConnectToController(controller);
    }

    public override void OnTriggerWasReleased(RWVR_InteractionController controller)
    {
        base.OnTriggerWasReleased(controller);

        if (hideControllerModel)
        {
            controller.ShowControllerModel();
        }

        ReleaseFromController(controller);
    }

}
