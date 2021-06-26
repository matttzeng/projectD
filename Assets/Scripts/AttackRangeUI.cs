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





    }

    public  void ShowRangeUI()
    {

        float attackRange = GetComponent<CharacterData>().Stats.stats.attackRange;
        Debug.Log("§ðÀ»¶ZÂ÷" + attackRange);

        rend.material.SetFloat("_scale", attackRange * -0.07f-0.06f);
        Debug.Log(attackRange);


        rangeUI.SetActive(true);

        StartCoroutine(CloseAfetrSeconds(2, rangeUI));

        IEnumerator CloseAfetrSeconds(int seconds,GameObject obj)
        {
            yield return new WaitForSeconds(seconds);
            obj.SetActive(false);
        }

    }

    /*
   public  IEnumerator ShowRangeUI()
    {
        
        if (rangeUIisShow == true)
            yield break;
      
        rangeUIisShow = true;
        Debug.Log("¶}±Ò§ðÀ»½d³òUI");
        rangeUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        rangeUI.SetActive(false);
       

        rangeUIisShow = false;
    }
    */
}
