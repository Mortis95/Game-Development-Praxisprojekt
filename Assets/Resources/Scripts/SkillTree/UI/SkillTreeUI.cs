using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour{
    public Animator animator;
    private SkillTreeNodeController[] skillTreeNodesUI;
    public SkillTree skillTree;
    private bool isVisible;
    private void Awake(){
        isVisible = false;
        skillTree.onSkillTreeChangedCallback += updateSkillTreeUI;
        skillTree.onSkillTreeSelectionChanged += updateSelectionUI;
        skillTreeNodesUI = gameObject.GetComponentsInChildren<SkillTreeNodeController>();
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
        SkillTreeNode[] nodes = SkillTree.getInstance().getSkillTreeNodes();
        for(int i = 0; i < nodes.Length; i++){
            skillTreeNodesUI[i].setNodeUI(nodes[i]);
        }
    }
    
    private void updateSelectionUI(int selectedIndex){
        for(int i = 0; i < skillTreeNodesUI.Length; i++){
            if(i == selectedIndex){skillTreeNodesUI[i].beSelected();}
            else{skillTreeNodesUI[i].beUnselected();}
        }
    }
}
