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

    public PlayerData(Player player)
    {
        exp = player.exp;
        level = player.level;
        currentHealthPoints = player.currentHealthPoints;
        maxHealthPoints = player.maxHealthPoints;
        currentHealthPoints = player.currentHealthPoints;
        maxMagicPoints = player.maxMagicPoints;
        attack = player.attack;
        defense = player.defense;
        strength = player.strength;
        dexterity = player.dexterity;
        intelligence = player.intelligence;

        position = new float[]{ player.transform.position.x, player.transform.position.y, player.transform.position.x, player.transform.position.z };

    }
}
