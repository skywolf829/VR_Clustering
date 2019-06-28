using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class MovementRestrictionScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.x < -0.5)
        {
            transform.localPosition = new Vector3(-0.5f, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.x > 0.5)
        {
            transform.localPosition = new Vector3(0.5f, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.y < -0.5)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f, transform.localPosition.z);
        }
        if (transform.localPosition.y > 0.5)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0.5f, transform.localPosition.z);
        }
    }
    [PunRPC]
    public void AssignParent(string t)
    {
        transform.SetParent(GameObject.Find(t).transform);
    }
}
