using ProjectD;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeUI : MonoBehaviour
{
    Renderer rend;
    // Update is called once per frame

    void Start()
    {
        rend = GetComponent<Renderer>();

       
    }

    void Update()
    {
       
        
        float attackRange = GetComponentInParent <CharacterData>().Stats.stats.attackRange;
        Debug.Log("§ðÀ»¶ZÂ÷" + attackRange);

        rend.material.SetFloat("_scale", attackRange*0.13f);

    }

    
}
