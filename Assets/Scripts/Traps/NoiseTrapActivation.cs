using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NoiseTrapActivation : BaseTrapClass
{
    public OffMeshLink offLink;
    public Transform startLink;
    public Transform endLink;
    public Transform floor;
    public LayerMask obstacleMask;
    public Color activeColor = new Color(1f, 0f, 0f, 1f);
    public Color inactiveColor = new Color(1f, 0.843f, 0f, 1f);

    private GameObject ring;

    //public Transform myTransform;

    //private Vector3 ogPosition = new Vector3();
    //private Quaternion ogRotation = new Quaternion();

    //private bool justStarted = true;

    //private void FixedUpdate()
    //{
    //    if (justStarted)
    //    {
    //        ogPosition.Set(myTransform.position.x, myTransform.position.y, myTransform.position.z);
    //        ogRotation.Set(myTransform.rotation.x, myTransform.rotation.y, myTransform.rotation.z, myTransform.rotation.w);
    //        //transform.SetPositionAndRotation(ogPosition, ogRotation);
    //        justStarted = false;
    //    }
    //}

    public void respawnMe()
    {
        isTriggered = false;
        transform.parent.GetComponent<Renderer>().material.color = inactiveColor;
        invisible();
        gameObject.GetComponentInParent<Respawn>().respawnMe();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set name of trap to Noise
        trapName = "Noise";
        isTriggered = false;
        transform.parent.GetComponent<Renderer>().material.color = inactiveColor;

        GameObject parent = this.transform.parent.gameObject;
        //GameObject parent = this.gameObject.GetComponentInParent<GameObject>();
        GameObject particle = parent.transform.GetChild(1).gameObject;
        ring = particle.transform.GetChild(1).gameObject;

        invisible();

        ring.GetComponent<ParticleSystem>().startColor = Color.yellow;
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            StateMachine_Robust SM = other.gameObject.GetComponent<StateMachine_Robust>();
            //Debug.Log("Enemy Inside sphere");
            if(!isTriggered && SM.conscious && SM.alive){
                // Debug.Log(transform.position);
               if (!walledOff(transform.position, other.transform.position))
               {
                    offLink.enabled = true;
                    startLink.SetPositionAndRotation(new Vector3(other.transform.position.x, floor.position.y,
                    other.transform.position.z), other.transform.rotation);
                    endLink.SetPositionAndRotation(new Vector3(transform.position.x, floor.position.y,
                    transform.position.z), transform.rotation);
                    offLink.startTransform = startLink;
                    offLink.endTransform = endLink;
                    offLink.GetComponent<workingOffLinkScript>().enabled = true;
               }


                other.GetComponent<StateMachine_Robust>().getNoise(transform.position);
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                //Debug.Log("getNoise called");
                isTriggered = true;
                //data.trapActiveOrder.Add("noiseTrap-1");
                data.noiseTrap.Add(1);

                ring.GetComponent<ParticleSystem>().startColor = Color.white;
                ring.GetComponent<ParticleSystem>().Clear();
                // this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                // this.GetComponent<Renderer>().material.color = Color.grey;
            }
            else
            {
                transform.parent.GetComponent<Renderer>().material.color = inactiveColor;
            }
        }
    }

    public void visible()
    {
        transform.GetComponent<MeshRenderer>().enabled = true;
        transform.GetComponent<SphereCollider>().enabled = true;
        transform.parent.GetComponent<Renderer>().material.color = activeColor;

        ring.GetComponent<ParticleSystem>().startColor = Color.yellow;
    }

    public void invisible()
    {
        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<SphereCollider>().enabled = false;
        ring.GetComponent<ParticleSystem>().startColor = Color.white;
        ring.GetComponent<ParticleSystem>().Clear();
    }

    private bool walledOff(Vector3 firstPoint, Vector3 secondPoint)
    {
        Vector3 direction = (secondPoint - firstPoint);
        float maxDist = direction.magnitude;
        direction.Normalize();

        return Physics.Raycast(firstPoint, direction, maxDist, obstacleMask);
    }
}
