using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Controller : MonoBehaviour
{
    [SerializeField] Player player;
    
    //Effekte des Skilltrees
    public void increaseDEX(){
        player.dex+=10;
    }
    public void increaseSTR(){
        player.str+=10;
    }
    public void increaseINT(){
        player.inte+=10;
    }

    public void setElementarpfeil(){
        player.FeuerpfeilLearned = true;
    }
    public void setElementarRegen(){
        player.WasserpfeilhagelLearned = true;
    }
    public void setScharfschuss(){
        player.ScharfschussLearned = true;
    }
    public void setElementarHieb(){
        player.WasserhiebLearned = true;
    }
    public void setElementarWirbel(){
        player.ElektrowirbelLearned = true;
    }
    public void setRage(){
        player.RageLearned = true;
    }
    public void setElementarBall(){
        player.FeuerballLearned = true;
    }
    public void setElementarflaeche(){
        player.WasserflaecheLearned = true;
    }
    public void setSturmkette(){
        player.SturmketteLearned = true;
    }
}
