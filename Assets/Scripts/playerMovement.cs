using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody pbody;
    public float movementSpeed = 15f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Debug.Log(vertical);

        Vector3 mag = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 velocity = mag * movementSpeed * Time.deltaTime;

        pbody.MovePosition(transform.position + velocity);
        
    }
}
