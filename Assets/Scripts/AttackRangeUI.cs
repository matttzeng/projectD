using ProjectD;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeUI : MonoBehaviour
{
    public GameObject rangeUI;
    Renderer rend;
    public bool rangeUIisShow;
    // Update is called once per frame

    void Start()
    {
        //RangeUI.GetComponent<reander>
       rend =rangeUI. GetComponent<Renderer>();

    }


    public void SetAttakRangeUI()
    {


            float attackRange = GetComponent<CharacterData>().Stats.stats.attackRange;
            Debug.Log("§ðÀ»¶ZÂ÷" + attackRange);

            rend.material.SetFloat("_scale", attackRange * 0.13f);
        



    }

    
   public  IEnumerator ShowRangeUI(float showTime)
    {
        if (rangeUIisShow == true)
            yield break;
      
        rangeUIisShow = true;
        Debug.Log("¶}±Ò§ðÀ»½d³òUI");
        rangeUI.SetActive(true);
        yield return new WaitForSeconds(showTime);
        rangeUI.SetActive(false);
        yield return new WaitForSeconds(showTime * 3f);

        rangeUIisShow = false;
    }
}
