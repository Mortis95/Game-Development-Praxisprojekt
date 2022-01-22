using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNodeController : MonoBehaviour{
    public bool nodeHasBottomConnector;
    public NodeElement nodeStyle;
    private Image hoverEffectImage;
    private Sprite hoverEffectSprite;
    private Image nodeBottomConnectorImage;
    private Sprite nodeBottomConnectorUnskilled;
    private Sprite nodeBottomConnectorSkilled;
    private Image nodeSlotImage;
    private Sprite nodeSlotNotSkilled;
    private Sprite nodeSlotPartiallySkilled;
    private Sprite nodeSlotFullySkilled;
    private Image nodeIconImage;
    
    //These need to be public, the class cannot possibly know what Skill it is supposed to represent without a Skill-Class.
    public Sprite nodeIconNotSkilled;
    public Sprite nodeIconSkilled;

    private void Awake(){
        //Get Images
        getImageReferences();

        //Load correct Asset - Sprites
        loadAssets();

        //Set everything to default state
        hoverEffectImage.enabled = false;

        //Some Nodes might not have a Bottom Connector Sprite set, as they are the root of the skilltree.
        if(nodeHasBottomConnector){nodeBottomConnectorImage.sprite = nodeBottomConnectorUnskilled;}
        else{nodeBottomConnectorImage.enabled = false;}

        //Set the rest to NotSkilled as default state
        nodeSlotImage.sprite = nodeSlotNotSkilled;
        nodeIconImage.sprite = nodeIconNotSkilled;
    }

    private void getImageReferences(){
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        hoverEffectImage = images[0];
        nodeBottomConnectorImage = images[1];
        nodeSlotImage = images[2];
        nodeIconImage = images[3];
    }
    private void loadAssets(){
        //Load Hover Effect Sprite
        hoverEffectSprite = Resources.Load<Sprite>("Sprites/SkillTreeSprites/hovereffect");

        //Load BottomConnector Sprite if Node has Bottom Connector
        if(nodeHasBottomConnector){
            nodeBottomConnectorUnskilled = Resources.Load<Sprite>("Sprites/SkillTreeSprites/unlearnd");
            nodeBottomConnectorSkilled = Resources.Load<Sprite>("Sprites/SkillTreeSprites/learned");
        }

        //Load the correct style for the Node
        
        Sprite notLearned = Resources.Load<Sprite>("Sprites/SkillTreeSprites/2");
        Sprite partiallyLearned = Resources.Load<Sprite>("Sprites/SkillTreeSprites/3");
        Sprite fullyLearned = null;
        
        switch(nodeStyle){
            case NodeElement.Normal:
                fullyLearned = Resources.Load<Sprite>("Sprites/SkillTreeSprites/1");
                break;
            case NodeElement.Blitz:
                fullyLearned = Resources.Load<Sprite>("Sprites/SkillTreeSprites/6");
                break;
            case NodeElement.Feuer:
                fullyLearned = Resources.Load<Sprite>("Sprites/SkillTreeSprites/4");
                break;
            case NodeElement.Wasser:
                fullyLearned = Resources.Load<Sprite>("Sprites/SkillTreeSprites/5");
                break;
        }
        nodeSlotNotSkilled = notLearned;
        nodeSlotPartiallySkilled = partiallyLearned;
        nodeSlotFullySkilled = fullyLearned;

    }

    private void setNotSkilled(){
        if(nodeHasBottomConnector){nodeBottomConnectorImage.sprite = nodeBottomConnectorUnskilled;}
        nodeSlotImage.sprite = nodeSlotNotSkilled;
        nodeIconImage.sprite = nodeIconNotSkilled;
    }

    private void setPartiallySkilled(){
        if(nodeHasBottomConnector){nodeBottomConnectorImage.sprite = nodeBottomConnectorSkilled;}
        nodeSlotImage.sprite = nodeSlotPartiallySkilled;
        nodeIconImage.sprite = nodeIconSkilled;
    }

    private void setFullySkilled(){
        if(nodeHasBottomConnector){nodeBottomConnectorImage.sprite = nodeBottomConnectorSkilled;}
        nodeSlotImage.sprite = nodeSlotFullySkilled;
        nodeIconImage.sprite = nodeIconSkilled;
    }

    public void setNodeUI(SkillTreeNode stn){
        if(stn.isNotSkilled()){
            setNotSkilled();
        } else if(stn.isPartiallySkilled()){
            setPartiallySkilled();
        } else if(stn.isFullySkilled()){
            setFullySkilled();
        } else {
            Debug.LogWarning("Something went seriously wrong! Node is neither not skilled, nor partially skilled, nor fully skilled?? Please investigate Node: " + stn.name + " , Current Level: " + stn.currentLevel + " , Max Node Level: " + stn.maxLevel);
        }
    }

    public void beSelected(){
        hoverEffectImage.enabled = true;
    }
    public void beUnselected(){
        hoverEffectImage.enabled = false;
    }
}

public enum NodeElement{
    Normal,
    Blitz,
    Feuer,
    Wasser
}