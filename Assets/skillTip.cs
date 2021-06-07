using ProjectD;
using ProjectDInternal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class skillTip : MonoBehaviour
{
    public int skillNum;
    public TMP_Text skillText;
    public TMP_Text skillDescription;
    

    

    void Update()

    {
        Skill skillUsed = GameObject.Find("Character").GetComponent<CharacterData>().Skill[skillNum];

        skillText.text = skillUsed.ItemName;

        skillDescription.text = skillUsed.GetDescription();
    }
}
