using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static SkillTree;
using UnityEngine.UI;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{   
    //Id des skills
    public int id;
    //Texte innerhalb des Trees
    public TMP_Text TitleText;
    public TMP_Text DescText;
    //Nachfolger Array
    public int[] ConnectedSkills;
    //Effekt der durch buy ausgelöst werden soll
    public UnityEvent effect;
    public bool isNormal; //LevelupSkills


    public void Start()
    {
        DontDestroyOnLoad(gameObject);  //damit skills in neuer szene erhalten bleiben
    }

    public void UpdateUI()
    {   //die Textfelder im SKilltree werden manipuliert
        TitleText.text=$"{skillTree.SkillLevels[id]}/{skillTree.SkillCaps[id]}\n{skillTree.SkillNames[id]}";
        DescText.text= $"{skillTree.SkillDescr[id]}\nCost:{skillTree.SkillPoints}/1 SP";

        GetComponent<Image>().color = skillTree.SkillLevels[id] >= skillTree.SkillCaps[id] ? Color.yellow   //die "blätter" des Trees werden je nach bedarf gefärbt
            : skillTree.SkillPoints >= 1 ? Color.green : Color.white;


        foreach(var connectedSkill in ConnectedSkills)      //die  erreichbaren Nodes werden eingefärbt
        {
            skillTree.SkillList[connectedSkill].gameObject.SetActive(skillTree.SkillLevels[id] > 0);
            skillTree.ConnectorList[connectedSkill].gameObject.SetActive(skillTree.SkillLevels[id] > 0);
        }            
    }

    public void Buy()       //selbsterklärend
    {   
      
        if(skillTree.SkillPoints < 1 || skillTree.SkillLevels[id] >= skillTree.SkillCaps[id] ) return;
        skillTree.SkillPoints -=1;
        skillTree.SkillLevels[id] ++;
        skillTree.UpdateAllSkillUI();

        effect.Invoke();
       
    }
}
