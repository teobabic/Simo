using TMPro;
using UnityEngine;

public class TrackingView : MonoBehaviour
{
    [Header("AR objects")]

    public SimoTracking simoTracking;
    private UserBody userMotionARKitRaw;

    private GameObject ARKitWorldReference;

    [Header("GUI")]
    public TMP_Text debugLabel;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        userMotionARKitRaw = simoTracking.GetComponent<SimoTracking>().userBodyARKitRaw;
        ARKitWorldReference = simoTracking.GetComponent<SimoTracking>().worldReference;
    }

    void Update()
    {
        #if UNITY_EDITOR
        //return;
        #endif
        
        string debugText = "Tracking info: \n";

        // hand - calculate and set the relative distances of the hand to the reference image
        Vector3 handRelativeToAnchorPos = ARKitWorldReference.transform.InverseTransformPoint(userMotionARKitRaw.hand.transform.position);
        Quaternion handRelativeToAnchorRot = Quaternion.Inverse(ARKitWorldReference.transform.rotation) * userMotionARKitRaw.hand.transform.rotation;

        debugText +=
            "Hand pose relative to anchor: \n" +
            "Position: " + GetStringFromVector(handRelativeToAnchorPos) + " \n" +
            "Rotation: " + GetStringFromEulerAngles(handRelativeToAnchorRot.eulerAngles) + " \n" +
            " \n";

        // head - calculate and set the relative distances of the head to the reference image
        Vector3 headRelativeToAnchorPos  = userMotionARKitRaw.hand.transform.InverseTransformPoint(userMotionARKitRaw.head.transform.position);
        Quaternion headRelativeToAnchorRot = Quaternion.Inverse(userMotionARKitRaw.hand.transform.rotation) * userMotionARKitRaw.head.transform.rotation;

        debugText +=
            "Head pose relative to hand: \n" +
            "Position: " + GetStringFromVector(headRelativeToAnchorPos) + " \n" +
            "Rotation: " + GetStringFromEulerAngles(headRelativeToAnchorRot.eulerAngles) + " \n" +
            " \n";

        // body
        Vector3 bodyRelativeToAnchorPos = ARKitWorldReference.transform.InverseTransformPoint(userMotionARKitRaw.body.transform.position);
        Quaternion bodyRelativeToAnchorRot = Quaternion.Inverse(ARKitWorldReference.transform.rotation) * userMotionARKitRaw.body.transform.rotation;

        debugText +=
            "Body pose relative to anchor: \n" +
            "Position: " + GetStringFromVector(bodyRelativeToAnchorPos) + " \n" +
            "Rotation: " + GetStringFromEulerAngles(bodyRelativeToAnchorRot.eulerAngles) + " \n" +
            " \n";

        debugLabel.text = debugText;
    }

    private string GetStringFromVector(Vector3 v)
    {
        string vectorAsString = "";
        v = v * 100;

        vectorAsString = v.x.ToString("F1") + ", " +
            v.y.ToString("F1") + ", " +
            v.z.ToString("F1") + " cm";

        return vectorAsString;
    }

    private string GetStringFromEulerAngles(Vector3 e)
    {
        string eulerAsString = "";

        eulerAsString = e.x.ToString("F0") + ", " +
            e.y.ToString("F0") + ", " +
            e.z.ToString("F0") + " °";

        return eulerAsString;
    }
}