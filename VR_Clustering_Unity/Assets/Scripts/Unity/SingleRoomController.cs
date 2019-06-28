using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class SingleRoomController : MonoBehaviour
{
    enum STATE { WAITING, ENTERED, DISPLAYING, PAUSE, PLACEMENT, LEAVING };
    STATE currentState = STATE.WAITING;

    public int roomNumber;
    public Button ButtonToEnter, ButtonToCalibrate, ButtonToFinish;
    public TextMeshPro FrontText;
    public GameObject InstructionText;
    public GameObject PlacementBoard, PlacementBoardFill;
    public GameObject TeleportSpot;

    float startDisplayTime;
    float startPauseTime;
    float displayLength = 4;
    float pauseLength = 1;
    // Start is called before the first frame update
    void Start()
    {
        SetToAvailable();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == STATE.DISPLAYING)
        {
            if(Time.time > startDisplayTime + displayLength)
            {
                PlacementBoard.SetActive(false);
                PlacementBoardFill.SetActive(false);
                currentState = STATE.PAUSE;
                startPauseTime = Time.time;
            }
        }
        else if(currentState == STATE.PAUSE)
        {
            if(Time.time > startPauseTime + pauseLength)
            {
                PlacementBoard.SetActive(true);
                PlacementBoardFill.SetActive(true);
                for(int i = 0; i < PlacementBoard.transform.childCount; i++)
                {                    
                    PlacementBoard.transform.GetChild(i).transform.localPosition = new Vector3(
                        Mathf.Lerp(-0.5f, 0.5f, (float)(i+1) / (float)(PlacementBoard.transform.childCount+1)),
                        -0.5f, PlacementBoard.transform.GetChild(i).transform.localPosition.z);
                }
                ButtonToFinish.transform.parent.gameObject.SetActive(true);
                EnableGrabInteractionOnObjects();
                currentState = STATE.PLACEMENT;
            }
        }
    }

    void TakeOwnershipOfRoomItems()
    {
        print("Taking ownership of all room items");
        ButtonToEnter.gameObject.GetComponent<PhotonView>().RequestOwnership();
        ButtonToCalibrate.gameObject.GetComponent<PhotonView>().RequestOwnership();
        ButtonToFinish.transform.parent.gameObject.GetComponent<PhotonView>().RequestOwnership();
        ButtonToFinish.gameObject.GetComponent<PhotonView>().RequestOwnership();
        PlacementBoard.GetComponent<PhotonView>().RequestOwnership();
        PlacementBoardFill.GetComponent<PhotonView>().RequestOwnership();
        FrontText.GetComponent<PhotonView>().RequestOwnership();
        InstructionText.GetComponent<PhotonView>().RequestOwnership();
        for (int i = 0; i < PlacementBoard.transform.childCount; i++)
        {
            PlacementBoard.transform.GetChild(i).GetComponent<PhotonView>().RequestOwnership();
        }
    }
    void SetToOccupied()
    {
        print("Set to occupied");
        FrontText.text = "Occupied";
        FrontText.color = Color.red;
        ButtonToEnter.interactable = false;
    }
    void SetToAvailable()
    {
        print("Set to available");
        ButtonToEnter.interactable = true;
        ButtonToCalibrate.gameObject.SetActive(true);
        FrontText.text = "Available";
        FrontText.color = Color.green;
        InstructionText.SetActive(true);
        ButtonToFinish.transform.parent.gameObject.SetActive(false);
    }

    public void FinishButtonClicked()
    {
        print("Finish clicked");
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = Vector3.zero;
        if (roomNumber == 1)
        {
            StudySceneManager.instance.OnRoom1Complete();
        }
        if (roomNumber == 2)
        {
            StudySceneManager.instance.OnRoom2Complete();
        }
        if (roomNumber == 3)
        {
            StudySceneManager.instance.OnRoom3Complete();
        }

        print("Calling RPC for NetworkRoomExited");
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("NetworkRoomExited", RpcTarget.All);
        currentState = STATE.WAITING;
    }

    public void CalibrateButtonClicked()
    {
        print("Calibrate button clicked");
        GameObject player = GameObject.FindWithTag("Player");
        float height = player.transform.GetChild(2).position.y * 0.8f;
        float z = player.transform.GetChild(2).position.z + 0.6f;
        float x = player.transform.GetChild(2).position.x;
        PlacementBoard.transform.position = new Vector3(x, height, z);
        PlacementBoard.SetActive(true);
        PlacementBoardFill.transform.position = new Vector3(x, height, z);
        PlacementBoardFill.SetActive(true);
        startDisplayTime = Time.time;
        displayLength = PlacementBoard.transform.childCount;
        ButtonToFinish.transform.parent.position = PlacementBoard.transform.position + new Vector3(0, -0.4f, 0);
        DisableGrabInteractionOnObjects();
        ButtonToCalibrate.gameObject.SetActive(false);
        InstructionText.gameObject.SetActive(false);
        currentState = STATE.DISPLAYING;
    }

    public void EnterButtonClicked()
    {
        print("Enter button clicked");
        if(currentState == STATE.WAITING)
        {
            print("Entering room");
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position += TeleportSpot.transform.position - player.transform.position;
            currentState = STATE.ENTERED;
            InstructionText.SetActive(true);
            PlacementBoard.SetActive(false);
            PlacementBoardFill.SetActive(false);
            ButtonToCalibrate.gameObject.SetActive(true);
            print("Calling RPC for NetworkRoomEntered");
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("NetworkRoomEntered", RpcTarget.All);
        }
    }
    
    void DisableGrabInteractionOnObjects()
    {
        print("Disabling grab mechanic on all room game objects");
        for(int i = 0; i < PlacementBoard.transform.childCount; i++)
        {
            PlacementBoard.transform.GetChild(i).GetComponent<VRTK.VRTK_InteractableObject>().isGrabbable = false;
        }
    }
    void EnableGrabInteractionOnObjects()
    {
        print("Enable grab mechanic on all room game objects");
        for (int i = 0; i < PlacementBoard.transform.childCount; i++)
        {
            PlacementBoard.transform.GetChild(i).GetComponent<VRTK.VRTK_InteractableObject>().isGrabbable = true;
        }
    }
    [PunRPC]
    void NetworkRoomEntered()
    {
        print("NetworkRoomEntered called");
        SetToOccupied();
    }
    [PunRPC]
    void NetworkRoomExited()
    {
        print("NetworkRoomExited called");
        SetToAvailable();
    }
}
