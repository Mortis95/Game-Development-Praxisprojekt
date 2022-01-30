using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerMeleeAttack : MonoBehaviour{

    public int damage;
    private bool hitOnce;

    public static void createAttack(Transform transform, Direction direction, int damage){
        GameObject berserkerMeleeAttackPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Bosses/Berserker/BerserkerMeleeAttackPrefab");
        GameObject berserkerMeleeAttack = Instantiate(berserkerMeleeAttackPrefab, transform.position, transform.rotation);
        Vector3 offset = Vector3.zero;
        switch(direction){
            case Direction.Up:
                offset = Vector3.up;
                break;
            case Direction.Down:
                offset = Vector3.down;
                break;
            case Direction.Left:
                offset = Vector3.left;
                break;
            case Direction.Right:
                offset = Vector3.right;
                break;
        }

        berserkerMeleeAttack.transform.position += offset * 2;

        BerserkerMeleeAttack bmaScript = berserkerMeleeAttack.GetComponent<BerserkerMeleeAttack>();
        bmaScript.damage = damage;

        berserkerMeleeAttack.SetActive(true);
        Debug.Log("Berserker Melee Attack activated!");
    }
    // Start is called before the first frame update
    void Start(){
        hitOnce = false;
        Destroy(gameObject, 0.75f);
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
