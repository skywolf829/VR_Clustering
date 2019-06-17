using Valve.Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject board, shapeBoard, fingerObject;
    public TextAsset positionsToLoad;

    private enum STATE { WAITING_TO_START, DISPLAYING_INFO, USER_INPUT, FINISHED};

    private STATE state = STATE.WAITING_TO_START;

    private List<Dictionary<string, string>> objectsAndPositions;
    private List<GameObject> shapes;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case STATE.WAITING_TO_START:

                break;
            case STATE.DISPLAYING_INFO:

                break;
            case STATE.USER_INPUT:

                break;
            case STATE.FINISHED:

                break;
            default:
                break;
        }
    }
    public void LoadJson()
    {
        string jsonText = positionsToLoad.text;
        objectsAndPositions = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonText);        
    }
    public void PopulateLoadedObjects()
    {
        for(int i = 0; i < objectsAndPositions.Count; i++)
        {
            if(objectsAndPositions[i]["shape"] == "Square")
            {

            }
            else if (objectsAndPositions[i]["shape"] == "Circle")
            {

            }
            else if (objectsAndPositions[i]["shape"] == "Triangle")
            {

            }
            else if (objectsAndPositions[i]["shape"] == "")
            {

            }
            else if (objectsAndPositions[i]["shape"] == "")
            {

            }
            else if (objectsAndPositions[i]["shape"] == "")
            {

            }
            else if (objectsAndPositions[i]["shape"] == "")
            {

            }
            else if (objectsAndPositions[i]["shape"] == "")
            {

            }
        }
    }
}
