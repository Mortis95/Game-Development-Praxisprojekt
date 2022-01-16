using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeUI : MonoBehaviour{
    public Animator animator;
    private SkillTreeNodeController[] skillTreeNodesUI;
    public SkillTree skillTree;
    private bool isVisible;

    public GameObject skillPointsLayout;
    private TextMeshProUGUI skillPointsText;
    public GameObject leftDescriptionLayout;
    private TextMeshProUGUI totalBonusStatsText;
    public GameObject rightDescriptionLayout;
    private TextMeshProUGUI skillNodeNameText;
    private TextMeshProUGUI skillNodeRequirementText;
    private TextMeshProUGUI skillNodeDescriptionText;
    private TextMeshProUGUI skillNodeBonusStatsText;

    private void Awake(){
        isVisible = false;
        skillTree.onSkillTreeChangedCallback += updateSkillTreeUI;
        skillTree.onSkillTreeSelectionChanged += updateSelectionUI;
        
        //Get all important UI Elements
        skillTreeNodesUI = gameObject.GetComponentsInChildren<SkillTreeNodeController>();
    
        //Get SkillPointsText
        skillPointsText = skillPointsLayout.GetComponentInChildren<TextMeshProUGUI>();
    
        //Get TotalBonusStatsText
        totalBonusStatsText = leftDescriptionLayout.GetComponentsInChildren<TextMeshProUGUI>()[1];
    
        //Get many important Text Elements from the right description
        TextMeshProUGUI[] rightTexts = rightDescriptionLayout.GetComponentsInChildren<TextMeshProUGUI>();
        skillNodeNameText = rightTexts[0];
        skillNodeRequirementText = rightTexts[1];
        skillNodeDescriptionText = rightTexts[2];
        skillNodeBonusStatsText = rightTexts[4]; //5th Text in this GameObject Group, Skip out on that one static "Bonus Attribute" Text. We don't need to change that.
    }

    void Start(){
        updateSkillTreeUI();
        updateSelectionUI(10);  //Default selected Node will be bottom middle (RangerStats)
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.T)){
            switchVisibility();
        }
    }

    private void switchVisibility(){
        isVisible = !isVisible;
        animator.SetBool("isVisible",isVisible);
    }
    public bool getVisibility(){
        return isVisible;
    }
    private void updateSkillTreeUI(){
        SkillTreeNode[] nodes = skillTree.getSkillTreeNodes();
        for(int i = 0; i < nodes.Length; i++){
            skillTreeNodesUI[i].setNodeUI(nodes[i]);
        }

        updateAllLeftTexts();
    }
    
    private void updateSelectionUI(int selectedIndex){
        //Set all UI Nodes to selected except the selected one
        for(int i = 0; i < skillTreeNodesUI.Length; i++){
            if(i == selectedIndex){skillTreeNodesUI[i].beSelected();}
            else{skillTreeNodesUI[i].beUnselected();}
        }

        //Set Information about selected Node

        //Set Name of selected Node    
        SkillTreeNode selectedNode = skillTree.getSkillTreeNodes()[selectedIndex];
        skillNodeNameText.SetText(selectedNode.name);
        
        //Set Levelrequirement of selected Node
        //If it is fully skilled, set Text blank
        if(selectedNode.isFullySkilled()){skillNodeRequirementText.SetText("");}
        //Else set Text to required Level.
        //If Player has reached that level, the text will be blue.
        //If Player has not reached that level, the text will be red.
        else{
            int nodeRequiredLevel = Mathf.Max(selectedNode.minimumLevelRequirement,selectedNode.currentLevel+1);
            skillNodeRequirementText.SetText("Level " + nodeRequiredLevel + " benötigt");
            if(nodeRequiredLevel > Player.getInstance().currentLevel){
                skillNodeRequirementText.color = new Color32(255,0,0,255);
            } else {
                skillNodeRequirementText.color = new Color32(0,255,255,255);
            }
        }

        //Set Description of selected Node
        skillNodeDescriptionText.SetText(selectedNode.description);

        //Set Bonus Attributes of selected Node
        skillNodeBonusStatsText.SetText(selectedNode.getStatsAsFormattedString());
    }

    private void updateAllLeftTexts(){
        //Set SkillPoint Text
        int playerSkillpoints = Player.getInstance().currentSkillpoints;
        int spentSkillpoints = skillTree.getSpentSkillPoints();

        skillPointsText.SetText("Skillpoints:\r\n" + playerSkillpoints + "/" + (playerSkillpoints + spentSkillpoints));
    
        //Set Total Bonus Attributes
        totalBonusStatsText.SetText(skillTree.getTotalBonusStats());
        

    }
}
