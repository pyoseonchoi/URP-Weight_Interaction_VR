using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class XRHandPalmCollider : MonoBehaviour
{
    public bool isLeft = true;
    XRHandSubsystem hands;
    Rigidbody rb; BoxCollider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true; rb.useGravity = false;
        col = GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();
        if (col.size == Vector3.zero) { col.size = new Vector3(0.085f, 0.02f, 0.09f); col.center = new Vector3(0, 0, 0.04f); }
    }

    void OnEnable()
    {
        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if (loader != null) hands = loader.GetLoadedSubsystem<XRHandSubsystem>();
    }

    void LateUpdate()
    {
        if (hands == null || !hands.running) return;
        var hand = isLeft ? hands.leftHand : hands.rightHand;
        if (!hand.isTracked) return;

        var palm = hand.GetJoint(XRHandJointID.Palm);
        if (palm.TryGetPose(out Pose p))
            transform.SetPositionAndRotation(p.position, p.rotation);
    }
}
