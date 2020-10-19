using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBodyMapping : MonoBehaviour
{
    public GameObject sceneBody;
    public GameObject sceneHand;
    public GameObject sceneHead;
    public GameObject humanoidHandTarget;
    public GameObject humanoidPhoneTarget;
    public GameObject humanoidHeadTarget;
    public GameObject humanoidBody;
    public Vector3 humanoidHandOffset;
    public Vector3 humanoidHeadOffset;
    public float yOffsetOfTheHumanoidModel;

    void Update()
    {
        humanoidBody.transform.position = sceneBody.transform.position + new Vector3(0, yOffsetOfTheHumanoidModel, 0);
        humanoidBody.transform.rotation = sceneBody.transform.rotation;

        humanoidHeadTarget.transform.position = sceneHead.transform.position;
        humanoidHeadTarget.transform.Translate(humanoidHeadOffset, Space.Self);
        humanoidHeadTarget.transform.rotation = sceneHead.transform.rotation;
    }
}
