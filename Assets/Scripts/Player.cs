using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            PlayerStats.PlayHP -= 1;
            
            Destroy(collision.gameObject);
        }
    }

}



