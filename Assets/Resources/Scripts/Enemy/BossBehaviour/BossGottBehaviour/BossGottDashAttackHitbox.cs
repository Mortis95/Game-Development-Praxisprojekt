using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGottDashAttackHitbox : MonoBehaviour{

    public int damage;
    private bool hitOnce;

    public static void createAttack(Transform transform, Direction direction, int damage){
        GameObject bgdaPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Bosses/BossGottAttacks/BossGottDashAttackHitboxPrefab");
        GameObject bgda = Instantiate(bgdaPrefab, transform.position, transform.rotation);

        BossGottMeleeAttack bgdaScript = bgda.GetComponent<BossGottMeleeAttack>();
        bgdaScript.damage = damage;

        bgda.SetActive(true);
    }
    // Start is called before the first frame update
    void Start(){
        hitOnce = false;
        Destroy(gameObject, 2f);
    }


    void OnTriggerEnter2D(Collider2D col){
        if(hitOnce){return;}    //Ignore double hits, blast you LayerSorter!!
        GameObject other = col.gameObject;
        if(other != null && other.tag == "Player"){
            hitOnce = true;
            Player pl = Player.getInstance();
            pl.takeDamage(damage);
            pl.getKnockedBack(transform.position, damage);
    }
  }
}
