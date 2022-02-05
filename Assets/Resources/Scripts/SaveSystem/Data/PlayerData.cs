using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int exp;
    public int level;
    public int currentHealthPoints;
    public int maxHealthPoints;
    public int currentMagicPoints;
    public int maxMagicPoints;
    public int attack;
    public int defense;
    public int strength;
    public int dexterity;
    public int intelligence;
    public float[] position;

    public bool FeuerpfeilLearned;
    public bool WasserpfeilhagelLearned;
    public bool ScharfschussLearned;
    public bool WasserhiebLearned;
    public bool ElektrowirbelLearned;
    public bool RageLearned;
    public bool FeuerballLearned;
    public bool WasserflaecheLearned;
    public bool KettenblitzLearned;

    public PlayerData(Player player)
    {
        exp = player.experiencePoints;
        level = player.currentLevel;
        currentHealthPoints = player.currentHealthPoints;
        maxHealthPoints = player.maxHealthPoints;
        currentMagicPoints = player.currentMagicPoints;
        maxMagicPoints = player.maxMagicPoints;
        attack = player.getAttack();
        defense = player.getDefense();
        strength = player.getStrength();
        dexterity = player.getDexterity();
        intelligence = player.getIntelligence();

        position = new float[]{ player.transform.position.x, player.transform.position.y, player.transform.position.x, player.transform.position.z };

        FeuerpfeilLearned = player.FeuerpfeilLearned;
        WasserpfeilhagelLearned = player.WasserpfeilhagelLearned;
        ScharfschussLearned = player.ScharfschussLearned;
        WasserhiebLearned = player.WasserhiebLearned;
        ElektrowirbelLearned = player.ElektrowirbelLearned;
        RageLearned = player.RageLearned;
        FeuerballLearned = player.FeuerballLearned;
        WasserflaecheLearned = player.WasserflaecheLearned;
        KettenblitzLearned = player.KettenblitzLearned;

    }
}
