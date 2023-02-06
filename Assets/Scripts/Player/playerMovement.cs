using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody pbody;
    public float speed = 15f;

    // Update is called once per frame
    void Update()
    {
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        ////Debug.Log(vertical);

        //Vector3 mag = new Vector3(horizontal, 0f, vertical).normalized;

        //Vector3 velocity = mag * movementSpeed * Time.deltaTime;

        //pbody.MovePosition(transform.position + velocity);


        float horVal = Input.GetAxis("Horizontal");
        float vertVal = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horVal * Time.deltaTime * speed, 0, vertVal * Time.deltaTime * speed));
        data.checkGameCompleted(data.gameCompleted);
    }

    public IEnumerator playerDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject, 0.5f);
        data.playerDeath = "yes";
        data.endTime = System.DateTime.Now;
        data.gameCompleted = true;
        Debug.Log(data.gameCompleted);
    }
    //Analytics start: If enenemy catches player, update end time and mark player as dead
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("collideEnemy");
            StartCoroutine(playerDie(0f));
            //data.timeToComplete = System.DateTime.Now;
            //Debug.Log(data.timeToComplete);
        }
    }
}
