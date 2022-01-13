using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SkillTreeEffect {
    public string name;
    //The event to be executed when executeEvent() is called.
    public string description;
    public UnityEvent toExecute;

    //How many times the Event has been executed.
    public int currentAmountofTimes;
    //How many times the Event is maximum allowed to be executed.
    public int maxAmountOfTimes;
    //Player must be at least this Level to skill this Skill 
    public int minimumLevelRequirement;
    //Indices of other Skills the Player must have at least 1 point in
    public int[] otherSkillRequiredIndices;
    private void Awake(){
        currentAmountofTimes = 0;
    }
    public bool checkAllRequirements(){
        //Wurde dieser SkillTreeEffect schon zu oft geskillt? Wenn ja, dann return false.
        if(currentAmountofTimes >= maxAmountOfTimes){return false;}
        

        int playerLevel = Player.getInstance().currentLevel;
        //Hat der Spieler auch das minimale Level erreicht um diesen SkillTreeEffect zu bekommen? Wenn nein, dann return false.
        if(playerLevel < minimumLevelRequirement){return false;}

        //Versucht der Spieler gerade eine Wertänderung auf einem Level zu kaufen welches ihn übersteigt? Wenn ja, dann return false.
        if(playerLevel < currentAmountofTimes + 1){return false;}

        //Hat der Spieler auch schon die vorherigen Skills mindestens einmal geskillt? Wenn nein, dann return false.
        SkillTreeEffect[] sTE = SkillTree.getInstance().skillTreeEffects;
        for (int i = 0; i < otherSkillRequiredIndices.Length; i++){   
            if (sTE[otherSkillRequiredIndices[i]].currentAmountofTimes < 1){
                return false;
            }
        }

        //Ansonsten können wir skillen.
        return true;
    }

    public bool isUnskilled(){return currentAmountofTimes == 0;}
    public bool isPartiallySkilled(){return (currentAmountofTimes != 0 && currentAmountofTimes != maxAmountOfTimes);}
    public bool isMaxedOut(){return currentAmountofTimes == maxAmountOfTimes;}


    public void executeEvent(){
        currentAmountofTimes++;
        toExecute.Invoke();
        
    }
}
