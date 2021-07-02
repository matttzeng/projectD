using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAndDestroy : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    void Update()
    {
        var destroyTime = 5;
        Destroy(gameObject, destroyTime);
    }
}
