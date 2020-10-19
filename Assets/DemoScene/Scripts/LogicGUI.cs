using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicGUI : MonoBehaviour
{
    public List<GameObject> allCameras = new List<GameObject>();
    public GameObject userFigureHumanoid;
    public GameObject userFigureHumanoidMesh;
    public Material userFigureHumanoidMaterialDefault;
    public Material userFigureHumanoidMaterialTrasparent;

    public GameObject room;
    public List<GameObject> cameraFOVs = new List<GameObject>();
    public List<GameObject> boxes = new List<GameObject>();
    public List<GameObject> eyes = new List<GameObject>();
    public PlaceOnPlane placeOnPlane;
    public Button buttonChangeView;
    public Button buttonChangeCamera;
    public GameObject labelInstructionArReferenceImage;
    public GameObject touchCameraControlInstructions;

    public GameObject headPointingLine;
    private int currentCameraIndex = -1;
    private int currentUserFigureIndex = -1;

    public TMP_Text labelCurrentCameraView;
    public TMP_Text labelCurrentUserView;

    // debug
    public GameObject debugInfo;

    private void ShowSceneViewButtons(bool show)
    {
        buttonChangeCamera.gameObject.SetActive(show);
        buttonChangeView.gameObject.SetActive(show);
        labelCurrentCameraView.gameObject.SetActive(show);
        labelCurrentUserView.gameObject.SetActive(show);
    }

    private void Start()
    {
        ShowSceneViewButtons(false);
        SwitchCamera();
        SwitchUserFigure();
        Invoke("HideInsteructionsAfterTime", 5f);
    }

    private void HideInsteructionsAfterTime()
    {
        labelInstructionArReferenceImage.SetActive(false);
    }

    private void Update()
    {
        if (placeOnPlane.isAnchorSet() && !buttonChangeView.gameObject.activeSelf)
        {
            ShowSceneViewButtons(true);
        }
    }

    public void SwitchCamera()
    {
        currentCameraIndex++;
        if (currentCameraIndex >= allCameras.Count)
        {
            currentCameraIndex = 0;
        }

        // hide all
        foreach (GameObject cam in allCameras)
        {
            if (cam == allCameras[0]) // do not turn the AR camera off (trackign stops)
            {
                cam.GetComponentInChildren<Camera>().enabled = false; // AR camera
            }
            else
            {
                cam.SetActive(false); // all other scene cams
            }
        }

        // show next camera
        allCameras[currentCameraIndex].SetActive(true);
        allCameras[currentCameraIndex].GetComponentInChildren<Camera>().enabled = true;
        touchCameraControlInstructions.SetActive(false);

        if (currentCameraIndex == 0)
        {
            room.SetActive(false);
            labelCurrentCameraView.text = "AR";
        }
        else if (currentCameraIndex == 1)
        {
            room.SetActive(true);
            touchCameraControlInstructions.SetActive(true);
            labelCurrentCameraView.text = "Touch Control";
        }
    }

    public void SwitchUserFigure()
    {
        currentUserFigureIndex++;
        if (currentUserFigureIndex >= 5)
        {
            currentUserFigureIndex = 0;
        }

        userFigureHumanoid.SetActive(false);
        headPointingLine.SetActive(false);
        TurnOnOffGameObjects(cameraFOVs, false);
        TurnOnOffGameObjects(boxes, false);
        TurnOnOffGameObjects(eyes, false);
        userFigureHumanoidMesh.GetComponent<SkinnedMeshRenderer>().material = userFigureHumanoidMaterialTrasparent;

        if (currentUserFigureIndex == 0)
        {
            TurnOnOffGameObjects(boxes, true);
            labelCurrentUserView.text = "Boxes";
        }
        else if (currentUserFigureIndex == 1)
        {
            userFigureHumanoid.SetActive(true);
            userFigureHumanoidMesh.GetComponent<SkinnedMeshRenderer>().material = userFigureHumanoidMaterialDefault;
            labelCurrentUserView.text = "Humanoid";
        }
        else if (currentUserFigureIndex == 2)
        {
            userFigureHumanoid.SetActive(true);
            TurnOnOffGameObjects(cameraFOVs, true);
            labelCurrentUserView.text = "Tracking Range";
        }
        else if (currentUserFigureIndex == 3)
        {
            userFigureHumanoid.SetActive(true);
            headPointingLine.SetActive(true);
            labelCurrentUserView.text = "Head Pointing";
        }
        else if (currentUserFigureIndex == 4)
        {
            userFigureHumanoid.SetActive(true);
            TurnOnOffGameObjects(eyes, true);
            labelCurrentUserView.text = "Gaze Pointing";
        }
    }

    private void TurnOnOffGameObjects(List<GameObject> objects, bool onOrOff)
    {
        foreach (GameObject o in objects)
        {
            o.SetActive(onOrOff);
        }
    }

    public void ShowHideDebugInfo()
    {
        if (debugInfo.activeSelf)
        {
            debugInfo.SetActive(false);
        }
        else
        {
            debugInfo.SetActive(true);
        }
    }
}