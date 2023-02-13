using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody pbody;
    public float speed = 15f;

    private void Start()
    {
        data.startTime = System.DateTime.Now;
    }
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
        
    }

    public IEnumerator playerDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        data.playerDeath = "yes";
        data.endTime = System.DateTime.Now;
        data.gameCompleted = true;
        Debug.Log(data.gameCompleted);
        data.levelName = SceneManager.GetActiveScene().name;
        Debug.Log(data.levelName);
        data.checkGameCompleted(data.gameCompleted);
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
