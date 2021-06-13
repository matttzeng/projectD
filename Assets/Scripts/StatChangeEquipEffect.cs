using System.Collections;
using System.Collections.Generic;
using System.Text;
using ProjectD;
using UnityEngine;

public class StatChangeEquipEffect : EquipmentItem.EquippedEffect
{
    public StatSystem.StatModifier Modifier;
    public string ss;
    
    public override void Equipped(CharacterData user)
    {
        //user.Stats.AddModifier(Modifier);
        //string ss = JsonUtility.ToJson(Modifier);
        //Debug.Log(ss);
        if (ss == "")
        {
            //Debug.Log("裝備的武器有json: " + Modifier.jsonString);
            //ss = JsonUtility.ToJson(Modifier);
            user.Stats.AddModifier(Modifier);
            //Modifier.Stats = JsonUtility.FromJson<StatSystem.StatModifier.Stats>(Modifier.jsonString);
            //ss = JsonUtility.ToJson(Modifier);
            //Debug.Log("ss: " + ss);
        }
        //var w = JsonUtility.FromJson<StatSystem.StatModifier>(ss);
        //Modifier = w as StatSystem.StatModifier;
        else
        {
            //var w = JsonUtility.FromJson<StatSystem.StatModifier>(ss);
            //Modifier = w as StatSystem.StatModifier;
            user.Stats.AddModifier(Modifier);
            //ss = JsonUtility.ToJson(Modifier);
        }
    }

    public override void Removed(CharacterData user)
    {
        user.Stats.RemoveModifier(Modifier);
        //Modifier.jsonString = JsonUtility.ToJson(Modifier.Stats);
        //Debug.Log("脫下裝備的素質: " + Modifier.jsonString);
    }

    public override string GetDescription()
    {
        string desc = base.GetDescription() + "\n";

        string unit = Modifier.ModifierMode == StatSystem.StatModifier.Mode.Percentage ? "%" : "";

        if (Modifier.Stats.strength != 0)
            desc += $"Str : {Modifier.Stats.strength:+0;-#}{unit}\n"; //format specifier to force the + sign to appear
        if (Modifier.Stats.agility != 0)
            desc += $"Agi : {Modifier.Stats.agility:+0;-#}{unit}\n";
        if (Modifier.Stats.defense != 0)
            desc += $"Def : {Modifier.Stats.defense:+0;-#}{unit}\n";
        if (Modifier.Stats.health != 0)
            desc += $"HP : {Modifier.Stats.health:+0;-#}{unit}\n";
        if (Modifier.Stats.attack != 0)
            desc += $"Atk : {Modifier.Stats.attack:+0;-#}{unit}\n";
        if (Modifier.Stats.skill != 0)
            desc += $"Skill : {Modifier.Stats.skill:+0;-#}{unit}\n";
        if (Modifier.Stats.moveSpeed != 0)
            desc += $"MoveSpeed : {Modifier.Stats.moveSpeed:+0;-#}{unit}\n";
        if (Modifier.Stats.attackSpeed != 0)
            desc += $"AtkSpeed : {Modifier.Stats.attackSpeed:+0;-#}{unit}\n";
        if (Modifier.Stats.attackRange != 0)
            desc += $"AtkRange : {Modifier.Stats.attackRange:+0;-#}{unit}\n";
        if (Modifier.Stats.skillRange != 0)
            desc += $"SkillRange : {Modifier.Stats.skillRange:+0;-#}{unit}\n";
        if (Modifier.Stats.crit != 0)
            desc += $"Crit : {Modifier.Stats.crit:+0;-#}{unit}\n";



        return desc;
    }
}
