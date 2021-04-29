using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1.5f;
    public Transform  interactionTransform;
    bool isFocus = false;
    public Transform playerObj;

    bool hasInteracted = false;

    public virtual void Interact()
    {
        

        //this method is meant to be overwritten
        Debug.Log("Interaction with " + transform.name);    
    
    }

    private void Update()
    {

        float distance = Vector3.Distance(playerObj.position, transform.position);
        if (distance <= radius)
        {
            Interact();
            hasInteracted = true;
        }


        
    
    }
    /*
    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
    }
    */
    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;
           
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
