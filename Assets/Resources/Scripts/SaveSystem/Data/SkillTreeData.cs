using System;
public class SkillTreeData
{
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
}
