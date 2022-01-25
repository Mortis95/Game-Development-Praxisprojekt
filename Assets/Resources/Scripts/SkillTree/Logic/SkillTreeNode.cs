using UnityEngine;
using UnityEngine.Events;

public class SkillTreeNode {
    //Info to be shown in the UI
    public string name;
    public string description;

    //The NodeType this Node has and can be found by.
    public SkillTreeNodeType nodeType;
    
    //The function to be executed when the node is leveled and executeEvent() is called.
    public delegate void skillNodeDelegate();
    public skillNodeDelegate funcToExecute;
    //Stat changes that leveling this Node will have
    public int bonusAttack;
    public int bonusDefense;
    public int bonusStrength;
    public int bonusDexterity;
    public int bonusIntelligence;

    //How many times the Event has been executed so far.
    public int currentLevel;
    //How many times the Event is maximum allowed to be executed.
    public int maxLevel;
    //Player must be at least this Level to level this Node 
    public int minimumLevelRequirement;
    //Other Nodes the Player must have at least 1 point in before leveling this Node
    public SkillTreeNodeType[] prerequisiteNodes;

    //Constructor for creating a new SkillTreeNode
    public SkillTreeNode(string name, string description, SkillTreeNodeType nodeType, skillNodeDelegate funcToExecute, int maxLevel, int minLevelReq, SkillTreeNodeType[] prereqNodes, int[] bonusStats){
        this.name = name;
        this.description = description;
        this.nodeType = nodeType;
        this.funcToExecute = funcToExecute;
        this.maxLevel = maxLevel;
        this.minimumLevelRequirement = minLevelReq;
        this.prerequisiteNodes = prereqNodes;
        this.currentLevel = 0;
        this.bonusAttack       = bonusStats[0];
        this.bonusDefense      = bonusStats[1];
        this.bonusStrength     = bonusStats[2];
        this.bonusDexterity    = bonusStats[3];
        this.bonusIntelligence = bonusStats[4];
    }
    public bool checkAllRequirements(){
        //Wurde diese Node schon zu oft geskillt? Wenn ja, dann return false.
        if(currentLevel >= maxLevel){return false;}

        //Hat der Spieler auch das minimale Level erreicht um diesen Node zu skillen? Wenn nein, dann return false.
        int playerLevel = Player.getInstance().currentLevel;
        if(playerLevel < minimumLevelRequirement){return false;}

        //Versucht der Spieler gerade eine Wertänderung auf einem Level zu kaufen welches ihn übersteigt? Wenn ja, dann return false.
        if(playerLevel < currentLevel + 1){return false;}

        //Hat der Spieler auch schon die Prerequisite Skills mindestens einmal geskillt? Wenn nein, dann return false.
        for (int i = 0; i < prerequisiteNodes.Length; i++){
            SkillTreeNodeType prerequisiteNodeType = prerequisiteNodes[i];
            SkillTreeNode preReq = SkillTree.getInstance().getNodeByType(prerequisiteNodeType);  
            if(preReq == null){Debug.LogWarning("Can't find Node with type:" + prerequisiteNodeType);return false;} 
            if (preReq.currentLevel < 1){
                return false;
            }
        }

        //Ansonsten können wir skillen.
        return true;
    }

    public bool isNotSkilled(){return currentLevel == 0;}
    public bool isPartiallySkilled(){return (currentLevel > 0 && currentLevel < maxLevel);}
    public bool isFullySkilled(){return currentLevel == maxLevel;}


    public void levelNode(){
        currentLevel++;
        Debug.Log("Node: " + name + " is now Level: " + currentLevel);
        SkillTree.getInstance().addPlayerStats(bonusAttack,bonusDefense,bonusStrength,bonusDexterity,bonusIntelligence);
        if(funcToExecute != null){funcToExecute.Invoke();}
    }

    public string getStatsAsFormattedString(){
        string stats = "";
        if(bonusAttack       != 0){stats += "ATK : " + bonusAttack    + "\r\n";}
        if(bonusDefense      != 0){stats += "DEF : " + bonusDefense   + "\r\n";}
        if(bonusStrength     != 0){stats += "STR : " + bonusStrength  + "\r\n";}
        if(bonusDexterity    != 0){stats += "DEX : " + bonusDexterity + "\r\n";}
        if(bonusIntelligence != 0){stats += "INT : " + bonusIntelligence;}
        return stats;
    }

    public int getCurrentBonusAttack(){
        return currentLevel * bonusAttack;
    } 

    public int getCurrentBonusDefense(){
        return currentLevel * bonusDefense;
    }

    public int getCurrentBonusStrength(){
        return currentLevel * bonusStrength;
    }

    public int getCurrentBonusDexterity(){
        return currentLevel * bonusDexterity;
    }

    public int getCurrentBonusIntelligence(){
        return currentLevel * bonusIntelligence;
    }
}
