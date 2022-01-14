using UnityEngine;

public class SkillTree : MonoBehaviour{

    #region Singleton
    private static SkillTree instance;
    public static SkillTree getInstance(){
        return instance;
    }
    #endregion
    public Player pl;
    private int skillPointsSpent;
    //Hilfsklasse um das jeweilige Event abzufeuern, sowie um zu zählen wie oft ein Skill bereits gelernt wurde. Die UI benötigt diese Information.
    private SkillTreeNode[] skillTreeNodes;

    #region IndexSpaghetti
    
    //IMPORTANT: This Index determines what object in the skillTreeEffects-Array the user has selected. It is IMPORTANT that the order of the objects in that array fit the SkillTree.
    //This means, we have the following "Index : Skill" - Mapping, which must always stay the same across this code!
    //Therefore, please do NOT change the order of the Objects in this array!
    //Is there a better way to do all of this? Oh absolutely. I just can't think of it right now.
    //This should theoretically not pose a problem, since our SkillTree is static and never expanding.
    //0  : Scharfschuss
    //1  : Rage
    //2  : Kettenblitz
    //3  : WasserPfeilHagel
    //4  : Elektrowirbel
    //5  : Wasserflaeche
    //6  : Feuerpfeil
    //7  : Wasserhieb
    //8  : Feuerball
    //9  : RangerStats
    //10 : WarriorStats
    //11 : SorcererStats
    
    public int selectedSkillTreeNodeIndex;
    public int skillTreeNodesPerRow;
    public int skillTreeNodesPerColumn;
    public int skillTreeCount;
    #endregion

    #region DelegateCallbackInvokePattern
    //This delegate will inform all subscribers when a Node changes on  the SkillTree, i.e. when the player levels a SkillTreeNode
    public delegate void OnSkillTreeChanged();
    public OnSkillTreeChanged onSkillTreeChangedCallback;
    private void skillTreeChangedCallback(){
        if (onSkillTreeChangedCallback != null) {
            onSkillTreeChangedCallback.Invoke();
        }
    }

    //This delegate will inform all subscribes when the selection on the SkillTree changes, i.e. when the player uses WASD-Keys to navigates the SkillTree.
    //Nodes do not need to be rendered anew, since only the selection changed.
    public delegate void OnSkillTreeSelectionChanged(int i);
    public OnSkillTreeSelectionChanged onSkillTreeSelectionChanged;
    private void selectionChangedCallback(){
        if (onSkillTreeSelectionChanged != null){
            onSkillTreeSelectionChanged.Invoke(selectedSkillTreeNodeIndex);
        }
    }

    #endregion

    public SkillTreeUI skillTreeUI;

    private void Awake(){
        if(instance != null){
            Debug.LogWarning("A second SkillTree tried to initialize! This is an issue! Something went wrong. Please check the game Hierarchy for duplicate SkillTree Scripts. This instance will now terminate.");
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
            skillPointsSpent = 0;
            selectedSkillTreeNodeIndex = 10;
            skillTreeNodesPerRow = 3;
            skillTreeNodesPerColumn = 4;
            skillTreeCount = skillTreeNodesPerRow * skillTreeNodesPerColumn;
        }
    }

    #region CreatingAllNodesFromCode 
    //Since the Unity-Inspector can sometimes mess up when creating many data objects, it is preferable to create all Nodes from Code instead. 
    //Here no progress gets lost in this admittedly tedious task.
    void Start(){
        //Set up empty array
        skillTreeNodes = new SkillTreeNode[skillTreeCount];

        //Setup variables that help with Assignment
        string description;
        SkillTreeNodeType[] preReqs;

        //Create Node with Index 0  : Scharfschuss
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.WasserPfeilHagel, SkillTreeNodeType.Feuerpfeil, SkillTreeNodeType.RangerStats};
        skillTreeNodes[0] = new SkillTreeNode("Scharfschuss", description, SkillTreeNodeType.Scharfschuss, unlockScharfschuss, 1, 5, preReqs);
        
        //Create Node with Index 1  : Rage
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.Elektrowirbel, SkillTreeNodeType.Wasserhieb, SkillTreeNodeType.WarriorStats};
        skillTreeNodes[1] = new SkillTreeNode("Rage", description, SkillTreeNodeType.Rage, unlockRage, 1, 5, preReqs);
        
        //Create Node with Index 2  : Kettenblitz
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.Wasserflaeche, SkillTreeNodeType.Feuerball, SkillTreeNodeType.SorcererStats};
        skillTreeNodes[2] = new SkillTreeNode("Kettenblitz", description, SkillTreeNodeType.Kettenblitz, unlockKettenblitz, 1, 5, preReqs);

        //Create Node with Index 3  : WasserPfeilHagel
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.Feuerpfeil, SkillTreeNodeType.RangerStats};
        skillTreeNodes[3] = new SkillTreeNode("WasserPfeilHagel",description,SkillTreeNodeType.WasserPfeilHagel,unlockWasserpfeilHagel ,1,4,preReqs);

        //Create Node with Index 4  : Elektrowirbel
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.Wasserhieb, SkillTreeNodeType.WarriorStats};
        skillTreeNodes[4] = new SkillTreeNode("Elektrowirbel",description,SkillTreeNodeType.Elektrowirbel,unlockElektrowirbel ,1,4,preReqs);;

        //Create Node with Index 5  : Wasserflaeche
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.Feuerball,SkillTreeNodeType.SorcererStats};
        skillTreeNodes[5] = new SkillTreeNode("Wasserflaeche",description,SkillTreeNodeType.Wasserflaeche,unlockWasserflaeche ,1,4,preReqs);;

        //Create Node with Index 6  : Feuerpfeil
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.RangerStats};
        skillTreeNodes[6] = new SkillTreeNode("Feuerpfeil",description,SkillTreeNodeType.Feuerpfeil,unlockFeuerpfeil ,1,3,preReqs);;
        
        //Create Node with Index 7  : Wasserhieb
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.WarriorStats};
        skillTreeNodes[7] = new SkillTreeNode("Wasserhieb",description,SkillTreeNodeType.Wasserhieb,unlockWasserHieb ,1,3,preReqs);;
        
        //Create Node with Index 8  : Feuerball
        description = "";
        preReqs = new SkillTreeNodeType[]{SkillTreeNodeType.SorcererStats};
        skillTreeNodes[8] = new SkillTreeNode("Feuerball",description,SkillTreeNodeType.Feuerball, unlockFeuerball,1,3,preReqs);;
        
        //Create Node with Index 9  : RangerStats
        description = "";
        preReqs = new SkillTreeNodeType[]{};
        skillTreeNodes[9] = new SkillTreeNode("RangerStats",description,SkillTreeNodeType.RangerStats,levelRangerStats ,5,2,preReqs);;
        
        //Create Node with Index 10 : WarriorStats
        description = "";
        preReqs = new SkillTreeNodeType[]{};
        skillTreeNodes[10] = new SkillTreeNode("WarriorStats",description,SkillTreeNodeType.WarriorStats, levelWarriorStats,5,2,preReqs);;

        //Create Node with Index 11 : SorcererStats
        description = "";
        preReqs = new SkillTreeNodeType[]{};
        skillTreeNodes[11] = new SkillTreeNode("SorcererStats",description,SkillTreeNodeType.SorcererStats, levelSorcererStats,5,2,preReqs);;
    }
    #endregion

    void Update(){
        if(skillTreeUI.getVisibility()){processInput();}
    }

    #region PlayerInputForSkillTreeEffects
    void processInput(){
        bool selectionChanged = false;
        bool skillTreeChanged = false;
        if(Input.GetKeyDown(KeyCode.E)){skillTreeChanged = tryToSkill(selectedSkillTreeNodeIndex);}
        if(Input.GetKeyDown(KeyCode.W)){selectedSkillTreeNodeIndex = betterModulo(selectedSkillTreeNodeIndex - skillTreeNodesPerRow, skillTreeCount);  selectionChanged=true;}
        if(Input.GetKeyDown(KeyCode.S)){selectedSkillTreeNodeIndex = betterModulo(selectedSkillTreeNodeIndex + skillTreeNodesPerRow, skillTreeCount);  selectionChanged=true;}
        if(Input.GetKeyDown(KeyCode.A)){selectedSkillTreeNodeIndex = betterModulo(selectedSkillTreeNodeIndex - 1,                    skillTreeCount);  selectionChanged=true;}
        if(Input.GetKeyDown(KeyCode.D)){selectedSkillTreeNodeIndex = betterModulo(selectedSkillTreeNodeIndex + 1,                    skillTreeCount);  selectionChanged=true;}

        if(skillTreeChanged){skillTreeChangedCallback();}
        else if(selectionChanged){selectionChangedCallback();}
    }

    //Check if Player has enough SkillPoints to Skill this Node, if yes then ask the Node if the Player meets all the requirements.
    //If yes, then we can skill.
    private bool tryToSkill(int index){
        if(pl.currentSkillpoints <= 0){return false;}
        Debug.Log("Player has enough SkillPoints.");
        if(!skillTreeNodes[index].checkAllRequirements()){return false;}
        Debug.Log("Player meets all requirements!");
        pl.currentSkillpoints -= 1;
        skillPointsSpent += 1;
        skillTreeNodes[index].levelNode();
        return true;
    }
    #endregion
    
    public SkillTreeNode getNodeByType(SkillTreeNodeType desiredType){
        foreach (SkillTreeNode node in skillTreeNodes){
            if(node.nodeType.Equals(desiredType)){
                return node;
            }
        }
        return null;
    }
    #region Unlockables
    //Hardcoded Events, because Unity does not allow for variable assignment or methods with 5 parameters in its event system.
    //Probably not the optimal way to do it, but I can't rework every single system after it's been already implemented. :/
    public void unlockScharfschuss(){pl.ScharfschussLearned         = true;}
    public void unlockRage(){pl.RageLearned                         = true;}
    public void unlockKettenblitz(){pl.KettenblitzLearned           = true;}
    public void unlockWasserpfeilHagel(){pl.WasserpfeilhagelLearned = true;}
    public void unlockElektrowirbel(){pl.ElektrowirbelLearned       = true;}
    public void unlockWasserflaeche(){pl.WasserflaecheLearned       = true;}
    public void unlockFeuerpfeil(){pl.FeuerpfeilLearned             = true;}
    public void unlockWasserHieb(){pl.WasserhiebLearned             = true;}
    public void unlockFeuerball(){pl.FeuerballLearned               = true;}
    public void levelRangerStats(){pl.addPermanentStats(2,2,0,3,0);}
    public void levelWarriorStats(){pl.addPermanentStats(2,2,3,0,0);}
    public void levelSorcererStats(){pl.addPermanentStats(2,2,0,0,3);}

    #endregion

    private int betterModulo(int dividend, int divisor){
        return (dividend % divisor + divisor) % divisor;
    }

    public SkillTreeNode[] getSkillTreeNodes(){
        return skillTreeNodes;
    }
}
