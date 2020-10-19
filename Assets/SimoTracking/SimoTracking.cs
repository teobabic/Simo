using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public struct UserBody
{
    public GameObject hand;
    public GameObject head;
    public GameObject eyeLeft;
    public GameObject eyeRight;
    public GameObject body;
}

public class SimoTracking : MonoBehaviour
{
    [Header("ARKit components")]
    public ARFaceManager arFaceManager;
    public ARSessionOrigin ARSessionOrigin;

    [Header("AR objects")]
    public UserBody userBodyARKitRaw;
    public GameObject worldReference;

    public GameObject eyeLeftChild;
    public GameObject eyeRightChild;

    // local variables used for body approximation
    private Vector3 _prevHeadPos = Vector3.zero;
    private Vector3 _prevHandPos = Vector3.zero;
    private float _minMovementToMoveBody = 0.004f; // in meters (m)
    private float _nextActionTime = 0.0f;
    private float _period = 0.02f; // in seconds (s)

    [Header("Settings")]
    public Slider bodyApproximationThreshold;
    public float bodyYoffsetInMeters; // in meters (m)

    // gui
    [Header("GUI")]
    public TMP_Text debugLabelApprox;

    // other private variables
    private int skipFirstFewFrames = 0;

    void Update()
    {
        if (ARSession.state == ARSessionState.SessionTracking)
        {
            // AR pose of the device-holding hand
            userBodyARKitRaw.hand.transform.position = ARSessionOrigin.camera.transform.position;
            userBodyARKitRaw.hand.transform.rotation = ARSessionOrigin.camera.transform.rotation;

            // AR head and eye pose
            foreach (ARFace arFace in arFaceManager.trackables)
            {
                userBodyARKitRaw.head.transform.position = arFace.transform.position;
                userBodyARKitRaw.head.transform.rotation = arFace.transform.rotation;

                userBodyARKitRaw.eyeLeft.transform.position = arFace.leftEye.position;
                userBodyARKitRaw.eyeLeft.transform.rotation = arFace.leftEye.rotation;

                userBodyARKitRaw.eyeRight.transform.position = arFace.rightEye.position;
                userBodyARKitRaw.eyeRight.transform.rotation = arFace.rightEye.rotation;

                // eyes - this will transform LeftEye or RightEye rotation in the scene to Head's local space -> https://answers.unity.com/questions/275565/what-is-the-rotation-equivalent-of-inversetransfor.html
                Quaternion sceneLeftEyeLocalRotation = Quaternion.Inverse(userBodyARKitRaw.head.transform.rotation) * userBodyARKitRaw.eyeLeft.transform.rotation;
                Quaternion sceneRightEyeLocalRotation = Quaternion.Inverse(userBodyARKitRaw.head.transform.rotation) * userBodyARKitRaw.eyeRight.transform.rotation;

                // eyes - set eyes rotation, and change few axes, since: "ARKit uses a left-handed transform for the ARFaceAnchor,
                // but the eye transforms continue to use a right-handed transform." - Unity Docs
                Vector3 sceneLeftEyeRotation = new Vector3(-sceneLeftEyeLocalRotation.eulerAngles.x, -sceneLeftEyeLocalRotation.eulerAngles.y, sceneLeftEyeLocalRotation.eulerAngles.z);
                Vector3 sceneRightEyeRotation = new Vector3(-sceneRightEyeLocalRotation.eulerAngles.x, -sceneRightEyeLocalRotation.eulerAngles.y, sceneRightEyeLocalRotation.eulerAngles.z);

                eyeLeftChild.transform.localRotation = Quaternion.Euler(sceneLeftEyeRotation);
                eyeRightChild.transform.localRotation = Quaternion.Euler(sceneRightEyeRotation);

                Vector3 deltaLeftEyePositionRelToHead = userBodyARKitRaw.head.transform.InverseTransformPoint(userBodyARKitRaw.eyeLeft.transform.position);
                Vector3 deltaRightEyePositionRelToHead = userBodyARKitRaw.head.transform.InverseTransformPoint(userBodyARKitRaw.eyeRight.transform.position);

                // eyes - set eyes position, and change few axes to transform the eyes from right- to left-handed transform
                eyeLeftChild.transform.localPosition = new Vector3(-deltaLeftEyePositionRelToHead.x, deltaLeftEyePositionRelToHead.y, -deltaLeftEyePositionRelToHead.z);
                eyeRightChild.transform.localPosition = new Vector3(-deltaRightEyePositionRelToHead.x, deltaRightEyePositionRelToHead.y, -deltaRightEyePositionRelToHead.z);
            }

            // body approximation
            BodyApproximation(userBodyARKitRaw.hand.transform.position, userBodyARKitRaw.head.transform.position, userBodyARKitRaw.head.transform.eulerAngles);
        }
    }

    private void BodyApproximation(Vector3 ARKitHandPosition, Vector3 ARKitHeadPosition, Vector3 ARKitHeadRotation)
    {
        if (Time.time > _nextActionTime)
        {
            _nextActionTime += _period;

            _minMovementToMoveBody = bodyApproximationThreshold.value;
            debugLabelApprox.text = "f: " + _minMovementToMoveBody.ToString("F3");

            Vector3 bodyPosition = Vector3.zero;
            Quaternion bodyRotation = Quaternion.identity;

            float difPhone = Mathf.Abs(Vector3.Distance(ARKitHandPosition, _prevHandPos));
            float difFace = Mathf.Abs(Vector3.Distance(ARKitHeadPosition, _prevHeadPos));

            bool isPhoneMoving = difPhone > _minMovementToMoveBody;
            bool isFaceMoving = difFace > _minMovementToMoveBody;

            if ((isPhoneMoving && isFaceMoving) || skipFirstFewFrames < 100)
            {
                // body position
                bodyPosition = ARKitHeadPosition + new Vector3(0, -bodyYoffsetInMeters, 0);
                userBodyARKitRaw.body.transform.position = bodyPosition;

                // body rotation
                // body rotation only gets updated with body movement, therefor in this if statement, otherwise the body always rotates with head rotation (put after this if for testing).
                bodyRotation.eulerAngles = new Vector3(bodyRotation.eulerAngles.x, ARKitHeadRotation.y, bodyRotation.eulerAngles.z);
                userBodyARKitRaw.body.transform.rotation = bodyRotation;
            }
            skipFirstFewFrames++;

            _prevHandPos = ARKitHandPosition;
            _prevHeadPos = ARKitHeadPosition;
        }
    }

    private Quaternion ConvertRightHandedToLeftHandedQuaternion(Quaternion rightHandedQuaternion)
    {
        return new Quaternion(
            rightHandedQuaternion.x,
            rightHandedQuaternion.y,
            rightHandedQuaternion.z,
            rightHandedQuaternion.w);
    }

    private Quaternion ConvertRightHandedToLeftHandedQuaternion2(Quaternion rightHandedQuaternion)
    {
        return new Quaternion(-rightHandedQuaternion.x,
            rightHandedQuaternion.y,
            rightHandedQuaternion.z,
            rightHandedQuaternion.w);
    }
}