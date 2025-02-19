using UnityEngine;

//Every EnemyBehaviour-Script must derive from this interface, i.e. implement the getKnockback() method, so it can be called in EnemyManager when an Enemy is hit.
public interface EnemyBehaviour {
    void getKnockedBack(Vector2 origin, float knockBackForce);
    void findTarget();
    void onDeath();
}
