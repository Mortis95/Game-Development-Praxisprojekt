using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourBerserker : MonoBehaviour, EnemyBehaviour {




    #region EnemyBehaviourInterface
    public void findTarget(){return;}
    public void onDeath(){return;}
    public void getKnockedBack(Vector2 origin, float knockBackForce){return;}
    #endregion

}
