using UnityEngine;

public class SkillTree : MonoBehaviour{

    #region Singleton
    private static SkillTree instance;
    public static SkillTree getInstance(){
        return instance;
    }
    #endregion
    private Player pl;
    //Hilfsklasse um das jeweilige Event abzufeuern, sowie um zu zählen wie oft ein Skill bereits gelernt wurde. Die UI benötigt diese Information.
    public SkillTreeEffect[] skillTreeEffects;

    #region IndexSpaghetti
    
    //IMPORTANT: This Index determines what object in the skillTreeEffects-Array the user has selected. It is IMPORTANT that the order of the objects in that array fit the SkillTree.
    //This means, we have the following "Index : Skill" - Mapping, which must always stay the same across this code!
    //Therefore, please do NOT change the order of the Objects in this array!
    //Is there a better way to do all of this? Oh absolutely. I just can't think of it right now.
    //This should not pose a problem, since our SkillTree is static and never expanding.
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
    
    public int selectedSkillTreeEffectIndex;
    public int skillTreeSlotsPerRow;
    public int skillTreeSlotsPerColumn;
    public int skillTreeCount;
    #endregion

    #region DelegateCallbackInvokePattern
    public delegate void OnSkillTreeChanged();
    public OnSkillTreeChanged onSkillTreeChangedCallback;
    private void invokeCallback(){
        if (onSkillTreeChangedCallback != null) {
            onSkillTreeChangedCallback.Invoke();
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
            pl = Player.getInstance();
            selectedSkillTreeEffectIndex = 10;
            skillTreeSlotsPerRow = 3;
            skillTreeSlotsPerColumn = 4;
            skillTreeCount = skillTreeSlotsPerRow * skillTreeSlotsPerColumn;
        }
    }

    void Update(){
        if(skillTreeUI.getVisibility()){processInput();}
    }

    #region PlayerInputForSkillTreeEffects
    void processInput(){
        bool changed = false;
        if(Input.GetKeyDown(KeyCode.E)){changed = tryToSkill(selectedSkillTreeEffectIndex);}
        if(Input.GetKeyDown(KeyCode.W)){selectedSkillTreeEffectIndex = betterModulo(selectedSkillTreeEffectIndex - skillTreeSlotsPerRow, skillTreeCount);  changed=true;}
        if(Input.GetKeyDown(KeyCode.S)){selectedSkillTreeEffectIndex = betterModulo(selectedSkillTreeEffectIndex + skillTreeSlotsPerRow, skillTreeCount);  changed=true;}
        if(Input.GetKeyDown(KeyCode.A)){selectedSkillTreeEffectIndex = betterModulo(selectedSkillTreeEffectIndex - 1,                    skillTreeCount);  changed=true;}
        if(Input.GetKeyDown(KeyCode.D)){selectedSkillTreeEffectIndex = betterModulo(selectedSkillTreeEffectIndex + 1,                    skillTreeCount);  changed=true;}

        if(changed){invokeCallback();}
    }

    //Complex function that checks 
    private bool tryToSkill(int index){
        if(pl.currentSkillpoints <= 0){return false;}
        if(!skillTreeEffects[index].checkAllRequirements()){return false;}
        skillTreeEffects[index].executeEvent();
        return true;
    }
    #endregion
    

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
}
