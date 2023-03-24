using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 ogPosition = new Vector3();
    private Quaternion ogRotation = new Quaternion();

    private void Start()
    {
        ogPosition = transform.position;
        ogRotation = transform.rotation;
    }
    public void respawnMe()
    {
        transform.SetPositionAndRotation(ogPosition, ogRotation);
    }
}
