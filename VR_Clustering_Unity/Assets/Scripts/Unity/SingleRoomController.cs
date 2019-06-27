using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SingleRoomController : MonoBehaviour
{
    enum STATE { WAITING, ENTERED, DISPLAYING, PAUSE, PLACEMENT, LEAVING };
    STATE currentState = STATE.WAITING;

    public int roomNumber;
    public Button ButtonToEnter, ButtonToCalibrate, ButtonToFinish;
    public TextMeshPro FrontText;
    public GameObject InstructionText;
    public GameObject PlacementBoard;
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
                currentState = STATE.PAUSE;
                startPauseTime = Time.time;
            }
        }
        else if(currentState == STATE.PAUSE)
        {
            if(Time.time > startPauseTime + pauseLength)
            {
                PlacementBoard.SetActive(true);
                for(int i = 0; i < PlacementBoard.transform.childCount; i++)
                {
                    PlacementBoard.transform.GetChild(i).transform.localPosition = new Vector3(
                        Mathf.Lerp(-0.5f, 0.5f, (float)(i+1) / (float)(PlacementBoard.transform.childCount+1)),
                        -0.5f, PlacementBoard.transform.GetChild(i).transform.localPosition.z);
                }
                ButtonToFinish.gameObject.SetActive(true);
                EnableGrabInteractionOnObjects();
                currentState = STATE.PLACEMENT;
            }
        }
    }

    void SetToOccupied()
    {
        FrontText.text = "Occupied";
        FrontText.color = Color.red;
        ButtonToEnter.interactable = false;
    }
    void SetToAvailable()
    {
        ButtonToEnter.interactable = true;
        ButtonToCalibrate.gameObject.SetActive(true);
        FrontText.text = "Available";
        FrontText.color = Color.green;
        InstructionText.SetActive(true);
        PlacementBoard.SetActive(false);
        ButtonToFinish.gameObject.SetActive(false);
    }

    public void FinishButtonClicked()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = Vector3.zero;
        if (roomNumber == 1)
        {
            RoomManager.instance.OnRoom1Complete();
        }
        if (roomNumber == 2)
        {
            RoomManager.instance.OnRoom2Complete();
        }
        if (roomNumber == 3)
        {
            RoomManager.instance.OnRoom3Complete();
        }
        SetToAvailable();
        currentState = STATE.WAITING;
    }

    public void CalibrateButtonClicked()
    {
        GameObject player = GameObject.FindWithTag("Player");
        float height = player.transform.GetChild(2).position.y * 0.8f;
        float z = player.transform.GetChild(2).position.z + 0.5f;
        float x = player.transform.GetChild(2).position.x;
        PlacementBoard.transform.position = new Vector3(x, height, z);
        PlacementBoard.SetActive(true);
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
        if(currentState == STATE.WAITING)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position += TeleportSpot.transform.position - player.transform.position;
            currentState = STATE.ENTERED;
            InstructionText.SetActive(true);
            ButtonToCalibrate.gameObject.SetActive(true);
            SetToOccupied();
        }
    }
    
    void DisableGrabInteractionOnObjects()
    {
        for(int i = 0; i < PlacementBoard.transform.childCount; i++)
        {
            PlacementBoard.transform.GetChild(i).GetComponent<VRTK.VRTK_InteractableObject>().isGrabbable = false;
        }
    }
    void EnableGrabInteractionOnObjects()
    {
        for (int i = 0; i < PlacementBoard.transform.childCount; i++)
        {
            PlacementBoard.transform.GetChild(i).GetComponent<VRTK.VRTK_InteractableObject>().isGrabbable = true;
        }
    }
}
