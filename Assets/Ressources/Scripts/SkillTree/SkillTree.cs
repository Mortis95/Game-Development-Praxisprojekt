using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
   public static SkillTree skillTree;

   private void Awake() => skillTree=this;
   
   public int[] SkillLevels;
   public int[] SkillCaps;
   public string[] SkillNames;
   public string[] SkillDescr;

   public List<Skill> SkillList;
   public GameObject SkillHolder;

   /*public List<Skill> LevelupSkillList;
   public GameObject LevelupSkillHolder;*/

   public List<GameObject> ConnectorList;
   public GameObject ConnectorHolder;

   public double SkillPoints;
   public double LevelupSkillpoints;

   private void Start()
   {
       DontDestroyOnLoad(gameObject);  //damit skilltree in neuer szene erhalten bleibt
       SkillPoints=0; //testwert

       SkillLevels= new int[13];

       SkillCaps = new[] {1,5,1,1,1,5,1,1,1,5,1,1,1}; //Vorläufige skillcaps

       SkillNames = new[] 
       {
        "1",
       "+ Dex",
       "FeuerPfeil",
       "Wasserpfeilhagel",
       "Scharfschuss",
       "+STR",
       "Wasserhieb",
       "Elektrowirbel",
       "Rage",
       "+INT",
       "Feuerball",
       "Wasserfläche",
       "Sturmkette"
       };

       SkillDescr= new[]{
           "Desc 1",
           "Desc 2",
           "Desc 3",
           "Desc 3",  
           "Desc 3",  
           "Desc 3",  
           "Desc 3",  
           "Desc 3",  
           "Desc 3",  
           "Desc 3",  
           "Desc 3",  
           "Desc 3",  
           "Desc 3"                      
       };

        foreach (var skill in SkillHolder.GetComponentsInChildren<Skill>())  //Allen Skillsaus dem Unityfenster werden in die Skillliste eingefügt
        {       
            skill.isNormal=true;
            SkillList.Add(skill); 
            //Debug.Log("Skill hinzugefügt:");
        }   
        
        
        foreach (var connector in ConnectorHolder.GetComponentsInChildren<RectTransform>())//Das gleiche für die connectors aus dem Unity fenster
        {    
            ConnectorList.Add(connector.gameObject);
            //Debug.Log(connector);
            }   

        for ( var i = 0; i < SkillList.Count; i++) SkillList[i].id = i;                     //den skills werden IDs vergeben 


        SkillList[0].ConnectedSkills =new [] {1,5,9}; //nachfolger werden festgelegt
        //Bogenklassen
        SkillList[1].ConnectedSkills =new [] {2}; 
        SkillList[2].ConnectedSkills =new [] {3};
        SkillList[3].ConnectedSkills =new [] {4};
        //Melee Klassen
        SkillList[5].ConnectedSkills =new [] {6};
        SkillList[6].ConnectedSkills =new [] {7};
        SkillList[7].ConnectedSkills =new [] {8};
        //Mage Klassen
        SkillList[9].ConnectedSkills =new [] {10};
        SkillList[10].ConnectedSkills =new [] {11};
        SkillList[11].ConnectedSkills =new [] {12};     

        UpdateAllSkillUI();   
  }

  public void UpdateAllSkillUI()    //für jeden skill wird das UI geupdatet
  {
      foreach( var skill in SkillList) skill.UpdateUI();
      //foreach( var skill in LevelupSkillList) skill.UpdateUI();

  }
}
