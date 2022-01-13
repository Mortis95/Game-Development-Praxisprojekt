using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour{
    public GameObject skillTreeUIParent;
    public SkillTree skillTree;
    private bool isVisible;
    private void Awake(){
        isVisible = false;
        skillTree.onSkillTreeChangedCallback += updateSkillTreeUI;
    }


    void Update(){
        if(Input.GetKeyDown(KeyCode.T)){
            switchVisibility();
        }
    }

    private void switchVisibility(){
        isVisible = !isVisible;
        if(isVisible){
            //Put SkillTree in the middle of the screen and be visible
            transform.position = skillTreeUIParent.transform.position;
        } else {
            //Put it VERY FAR offscreen, like SUPER FAR, even the BIGGEST BROADEST Monitor shouldn't be able to see that thing
            transform.position = new Vector3(5000,5000,0);
        }
    }
    public bool getVisibility(){
        return isVisible;
    }
    private void updateSkillTreeUI(){

    }
}
