using Michsky.UI.ModernUIPack;
using ProjectD;

using UnityEngine;


public class SkillCDUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        var obj = player.GetComponent<CharacterData>();
        this.GetComponent<ProgressBar>().currentPercent = (obj.m_SkillAttackCoolDown / obj.skillCD) * 100;


        Debug.Log ("ßﬁØ‡CD : " +  this.GetComponent<ProgressBar>().currentPercent);
    }
}
