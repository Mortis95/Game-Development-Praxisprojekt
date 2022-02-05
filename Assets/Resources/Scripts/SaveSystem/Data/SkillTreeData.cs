using System;
[System.Serializable]
public class SkillTreeData
{
    public int skillpointsAttack;
    public int skillpointsMagic;
    public int skillpointsRangeAttack;
    
    public SkillTreeData(int skillpointsRangeAttack, int skillpointsAttack, int skillpointsMagic)
    {
        this.skillpointsAttack = skillpointsAttack;
        this.skillpointsMagic = skillpointsMagic;
        this.skillpointsRangeAttack = skillpointsRangeAttack;
    }

}
