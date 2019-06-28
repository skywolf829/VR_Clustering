using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class StudySceneManager : MonoBehaviour
{
    public static StudySceneManager instance;

    public GameObject Room1Board, Room2Board, Room3Board;
    public List<GameObject> gamePrefabs = new List<GameObject>();
    public float widthOfGameObjects, localWidthOfGameObjects;

    List<Vector2> currentRoom1Configuration, currentRoom2Configuration, currentRoom3Configuration;

    private void Awake()
    {
        if(StudySceneManager.instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void StartRoomManager()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        currentRoom1Configuration = Generate2C2();
        for(int i = 0; i < currentRoom1Configuration.Count; i++)
        {
            GameObject gamePiece = PhotonNetwork.Instantiate(gamePrefabs[i].name, Vector3.zero, Quaternion.identity);
            gamePiece.transform.SetParent(Room1Board.transform);
            gamePiece.transform.localPosition = Vector3.zero;
            gamePiece.transform.localPosition = new Vector3(currentRoom1Configuration[i].x, currentRoom1Configuration[i].y, 0);
        }
        //Room1Board.SetActive(false);
       
        currentRoom2Configuration = Generate2C2();
        for (int i = 0; i < currentRoom2Configuration.Count; i++)
        {
            GameObject gamePiece = PhotonNetwork.Instantiate(gamePrefabs[i].name, Vector3.zero, Quaternion.identity);
            gamePiece.transform.SetParent(Room2Board.transform);
            gamePiece.transform.localPosition = Vector3.zero;
            gamePiece.transform.localPosition = new Vector3(currentRoom2Configuration[i].x, currentRoom2Configuration[i].y, 0);
        }
        //Room2Board.SetActive(false);

        currentRoom3Configuration = Generate2C2();
        for (int i = 0; i < currentRoom3Configuration.Count; i++)
        {
            GameObject gamePiece = PhotonNetwork.Instantiate(gamePrefabs[i].name, Vector3.zero, Quaternion.identity);
            gamePiece.transform.SetParent(Room3Board.transform);
            gamePiece.transform.localPosition = Vector3.zero;
            gamePiece.transform.localPosition = new Vector3(currentRoom3Configuration[i].x, currentRoom3Configuration[i].y, 0);
        }
        //Room3Board.SetActive(false);


        LogRoom1Config();
        LogRoom2Config();
        LogRoom3Config();
    }
    void LogRoom1Config()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        string toWrite = "[";
        for (int i = 0; i < currentRoom1Configuration.Count; i++)
        {
            toWrite = toWrite + "(" + currentRoom1Configuration[i].x + "," + currentRoom1Configuration[i].y + ")";
        }
        toWrite = toWrite + "]" + System.Environment.NewLine;
        if (!File.Exists("Assets/Resources/Room1Configs.txt"))
        {
            File.Create("Assets/Resources/Room1Configs.txt").Dispose();
        }
        File.AppendAllText("Assets/Resources/Room1Configs.txt", toWrite);
    }
    void LogRoom2Config()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        string toWrite = "[";
        for (int i = 0; i < currentRoom2Configuration.Count; i++)
        {
            toWrite = toWrite + "(" + currentRoom2Configuration[i].x + "," + currentRoom2Configuration[i].y + ")";
        }
        toWrite = toWrite + "]" + System.Environment.NewLine;
        if (!File.Exists("Assets/Resources/Room2Configs.txt"))
        {
            File.Create("Assets/Resources/Room2Configs.txt").Dispose();
        }
        File.AppendAllText("Assets/Resources/Room2Configs.txt", toWrite);
    }
    void LogRoom3Config()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        string toWrite = "[";
        for (int i = 0; i < currentRoom3Configuration.Count; i++)
        {
            toWrite = toWrite + "(" + currentRoom3Configuration[i].x + "," + currentRoom3Configuration[i].y + ")";
        }
        toWrite = toWrite + "]" + System.Environment.NewLine;
        if (!File.Exists("Assets/Resources/Room3Configs.txt"))
        {
            File.Create("Assets/Resources/Room3Configs.txt").Dispose();
        }
        File.AppendAllText("Assets/Resources/Room3Configs.txt", toWrite);
    }
    void OnApplicationQuit()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (File.Exists("Assets/Resources/Room1Configs.txt"))
        {
            File.AppendAllText("Assets/Resources/Room1Configs.txt", "END" + System.Environment.NewLine);
        }
        if (File.Exists("Assets/Resources/Room2Configs.txt"))
        {
            File.AppendAllText("Assets/Resources/Room2Configs.txt", "END" + System.Environment.NewLine);
        }
        if (File.Exists("Assets/Resources/Room3Configs.txt"))
        {
            File.AppendAllText("Assets/Resources/Room3Configs.txt", "END"+System.Environment.NewLine);
        }
    }
    private Vector2 Get2DIsotropicGaussian(Vector2 u)
    {
        float o = widthOfGameObjects * 3;
        float u1 = Random.Range(0f, 1f);
        float u2 = Random.Range(0f, 1f);
        float randomStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        float randomNormalX = u.x + o * randomStdNormal;
        randomNormalX = Mathf.Min(Mathf.Max(randomNormalX, -0.5f), 0.5f);
        u1 = Random.Range(0f, 1f);
        u2 = Random.Range(0f, 1f);
        randomStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        float randomNormalY = u.y + o * randomStdNormal;
        randomNormalY = Mathf.Min(Mathf.Max(randomNormalY, -0.5f), 0.5f);
        return new Vector2(randomNormalX, randomNormalY);
    }
    private bool CheckOverlap(Vector2 a, Vector2 b)
    {
        //return false;
        return Vector2.Distance(a, b) < 2* Mathf.Sqrt(2) * localWidthOfGameObjects;
    }
    private List<Vector2> Generate2C2()
    {
        float x1, x2, y1, y2;
        x1 = Random.Range(-0.5f, 0.5f);
        y1 = Random.Range(-0.5f, 0.5f);
        x2 = Random.Range(-0.5f, 0.5f);
        y2 = Random.Range(-0.5f, 0.5f);

        List<Vector2> objectPositions = new List<Vector2>();
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x1, y1)));
        Vector2 testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while(CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x2, y2)));
        testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        while (CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        }
        objectPositions.Add(testPos);
        return objectPositions;
    }
    private List<Vector2> Generate4C1()
    {
        float x1, x2, x3, x4, y1, y2, y3, y4;
        x1 = Random.Range(-0.5f, 0.5f);
        y1 = Random.Range(-0.5f, 0.5f);
        x2 = Random.Range(-0.5f, 0.5f);
        y2 = Random.Range(-0.5f, 0.5f);
        x3 = Random.Range(-0.5f, 0.5f);
        y3 = Random.Range(-0.5f, 0.5f);
        x4 = Random.Range(-0.5f, 0.5f);
        y4 = Random.Range(-0.5f, 0.5f);

        List<Vector2> objectPositions = new List<Vector2>();
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x1, y1)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x2, y2)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x3, y3)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x4, y4)));
        return objectPositions;
    }
    private List<Vector2> Generate4C2()
    {
        float x1, x2, x3, x4, y1, y2, y3, y4;
        x1 = Random.Range(-0.5f, 0.5f);
        y1 = Random.Range(-0.5f, 0.5f);
        x2 = Random.Range(-0.5f, 0.5f);
        y2 = Random.Range(-0.5f, 0.5f);
        x3 = Random.Range(-0.5f, 0.5f);
        y3 = Random.Range(-0.5f, 0.5f);
        x4 = Random.Range(-0.5f, 0.5f);
        y4 = Random.Range(-0.5f, 0.5f);
        List<Vector2> objectPositions = new List<Vector2>();

        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x1, y1)));
        Vector2 testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);



        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x2, y2)));
        testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        while (CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        }
        objectPositions.Add(testPos);



        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x3, y3)));
        testPos = Get2DIsotropicGaussian(new Vector2(x3, y3));
        while (CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x3, y3));
        }
        objectPositions.Add(testPos);



        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x4, y4)));
        testPos = Get2DIsotropicGaussian(new Vector2(x4, y4));
        while (CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x4, y4));
        }
        objectPositions.Add(testPos);
        return objectPositions;
    }
    private List<Vector2> Generate1C4()
    {
        float x1, y1;
        x1 = Random.Range(-0.5f, 0.5f);
        y1 = Random.Range(-0.5f, 0.5f);

        List<Vector2> objectPositions = new List<Vector2>();
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x1, y1)));
        Vector2 testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]) ||
            CheckOverlap(testPos, objectPositions[2]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        return objectPositions;
    }
    private List<Vector2> Generate1C8()
    {
        float x1, y1;
        x1 = Random.Range(-0.5f, 0.5f);
        y1 = Random.Range(-0.5f, 0.5f);

        List<Vector2> objectPositions = new List<Vector2>();
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x1, y1)));
        Vector2 testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]) ||
            CheckOverlap(testPos, objectPositions[2]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]) ||
            CheckOverlap(testPos, objectPositions[2]) ||
            CheckOverlap(testPos, objectPositions[3]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]) ||
            CheckOverlap(testPos, objectPositions[2]) ||
            CheckOverlap(testPos, objectPositions[3]) ||
            CheckOverlap(testPos, objectPositions[4]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]) ||
            CheckOverlap(testPos, objectPositions[2]) ||
            CheckOverlap(testPos, objectPositions[3]) ||
            CheckOverlap(testPos, objectPositions[4]) ||
            CheckOverlap(testPos, objectPositions[5]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]) ||
            CheckOverlap(testPos, objectPositions[2]) ||
            CheckOverlap(testPos, objectPositions[3]) ||
            CheckOverlap(testPos, objectPositions[4]) ||
            CheckOverlap(testPos, objectPositions[5]) ||
            CheckOverlap(testPos, objectPositions[6]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        return objectPositions;
    }
    private List<Vector2> Generate8C1()
    {
        float x1, x2, x3, x4, x5, x6, x7, x8, y1, y2, y3, y4, y5, y6, y7, y8;
        x1 = Random.Range(-0.5f, 0.5f);
        y1 = Random.Range(-0.5f, 0.5f);
        x2 = Random.Range(-0.5f, 0.5f);
        y2 = Random.Range(-0.5f, 0.5f);
        x3 = Random.Range(-0.5f, 0.5f);
        y3 = Random.Range(-0.5f, 0.5f);
        x4 = Random.Range(-0.5f, 0.5f);
        y4 = Random.Range(-0.5f, 0.5f);
        x5 = Random.Range(-0.5f, 0.5f);
        y5 = Random.Range(-0.5f, 0.5f);
        x6 = Random.Range(-0.5f, 0.5f);
        y6 = Random.Range(-0.5f, 0.5f);
        x7 = Random.Range(-0.5f, 0.5f);
        y7 = Random.Range(-0.5f, 0.5f);
        x8 = Random.Range(-0.5f, 0.5f);
        y8 = Random.Range(-0.5f, 0.5f);

        List<Vector2> objectPositions = new List<Vector2>();
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x1, y1)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x2, y2)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x3, y3)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x4, y4)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x5, y5)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x6, y6)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x7, y7)));
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x8, y8)));
        return objectPositions;
    }
    private List<Vector2> Generate2C4()
    {
        float x1, x2, y1, y2;
        x1 = Random.Range(-0.5f, 0.5f);
        y1 = Random.Range(-0.5f, 0.5f);
        x2 = Random.Range(-0.5f, 0.5f);
        y2 = Random.Range(-0.5f, 0.5f);

        List<Vector2> objectPositions = new List<Vector2>();
        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x1, y1)));
        Vector2 testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[objectPositions.Count - 1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        while (CheckOverlap(testPos, objectPositions[0]) ||
            CheckOverlap(testPos, objectPositions[1]) ||
            CheckOverlap(testPos, objectPositions[2]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);




        objectPositions.Add(Get2DIsotropicGaussian(new Vector2(x2, y2)));
        testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        while (CheckOverlap(testPos, objectPositions[4]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        while (CheckOverlap(testPos, objectPositions[4]) ||
            CheckOverlap(testPos, objectPositions[5]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        }
        objectPositions.Add(testPos);

        testPos = Get2DIsotropicGaussian(new Vector2(x2, y2));
        while (CheckOverlap(testPos, objectPositions[4]) ||
            CheckOverlap(testPos, objectPositions[5]) ||
            CheckOverlap(testPos, objectPositions[6]))
        {
            testPos = Get2DIsotropicGaussian(new Vector2(x1, y1));
        }
        objectPositions.Add(testPos);
        return objectPositions;
    }

    public void OnRoom1Complete()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        for (int i = 0; i < Room1Board.transform.childCount; i++)
        {
            currentRoom1Configuration[i] = new Vector2(Room1Board.transform.GetChild(i).localPosition.x, Room1Board.transform.GetChild(i).localPosition.y);
        }
        LogRoom1Config();
    }
    public void OnRoom2Complete()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        for (int i = 0; i < Room2Board.transform.childCount; i++)
        {
            currentRoom2Configuration[i] = new Vector2(Room2Board.transform.GetChild(i).localPosition.x, Room2Board.transform.GetChild(i).localPosition.y);
        }
        LogRoom2Config();
    }
    public void OnRoom3Complete()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        for (int i = 0; i < Room3Board.transform.childCount; i++)
        {
            currentRoom3Configuration[i] = new Vector2(Room3Board.transform.GetChild(i).localPosition.x, Room3Board.transform.GetChild(i).localPosition.y);
        }
        LogRoom3Config();
    }
}
