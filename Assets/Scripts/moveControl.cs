using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class moveControl : MonoBehaviour
{
    //NavMeshAgent agent;

    public float speed = .01f;
    // Update is called once per frame

    private void Start()
    {
       // agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");
        
        Vector3 moverDirection = new Vector3(xDirection, 0.0f, zDirection);

       // transform.rotation = Quaternion.LookRotation(agent.velocity, Vector3.up);

        transform.position += moverDirection * speed ;
    }
}
