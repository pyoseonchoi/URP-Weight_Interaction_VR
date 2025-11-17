using TMPro;
using UnityEngine;

public class HandRotHUD_XRI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Transform hmd;       // XR Origin 안의 Main Camera
    public Transform leftHand;  // UnityXR/Hands/LeftHand
    public Transform rightHand; // UnityXR/Hands/RightHand
    public float logEvery = 0f; // 0이면 로그 안 찍음. 0.5면 0.5초마다 찍음

    void Awake()
    {
        if (!hmd && Camera.main) hmd = Camera.main.transform;
        var root = hmd ? hmd.GetComponentInParent<Transform>() : null;
        if (root)
        {
            if (!leftHand) leftHand = FindDeep(root, "LeftHand");
            if (!rightHand) rightHand = FindDeep(root, "RightHand");
        }
    }

    Transform FindDeep(Transform root, string name)
    {
        foreach (var t in root.GetComponentsInChildren<Transform>(true))
            if (t.name == name) return t;
        return null;
    }

    float acc;
    void LateUpdate()
    {
        if (!text) return;

        string Line(Transform tr, string label)
        {
            if (!tr) return $"{label}: (none)\n";
            var p = tr.position;
            var r = tr.rotation.eulerAngles;
            return $"{label}  P({p.x:0.00},{p.y:0.00},{p.z:0.00})  R({r.x:0},{r.y:0},{r.z:0})\n";
        }

        string msg =
            Line(hmd, "HMD") +
            Line(leftHand, "LeftHand") +
            Line(rightHand, "RightHand");

        text.text = msg;

        if (logEvery > 0f)
        {
            acc += Time.deltaTime;
            if (acc >= logEvery) { acc = 0f; Debug.Log(msg.TrimEnd()); }
        }
    }
}
