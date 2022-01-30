using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerRangedKnifeAttack : MonoBehaviour{
    public Vector2 direction;
    public float shotSpeed;
    public float timeToLiveSeconds;
    public int damage;
    public bool hitOnce;

    public static void createAttack(Transform transform, Vector2 direction, int damage){
        GameObject brkaPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Bosses/Berserker/BerserkerRangedKnifeAttackPrefab");
        GameObject brka = Instantiate(brkaPrefab, transform.position, transform.rotation);
        BerserkerRangedKnifeAttack brkaScript = brka.GetComponent<BerserkerRangedKnifeAttack>();
        brkaScript.direction = direction;
        brkaScript.damage = damage;
        brkaScript.timeToLiveSeconds = 5f;
        brka.SetActive(true);
    }
    
    void Start(){
        hitOnce = false;
        transform.up = new Vector3(direction.x,direction.y,0);
        Destroy(gameObject, timeToLiveSeconds);
    }

    void FixedUpdate(){
        transform.position += (Vector3) direction * shotSpeed * Time.fixedDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(hitOnce){return;}    //Ignore double hits
        GameObject other = col.gameObject;
        Debug.Log("Collision with:" + col.name);
        if(other != null && other.tag == "Player"){
            hitOnce = true;
            Player playerScript = Player.getInstance();
            playerScript.takeDamage(damage);
        }
        if(other != null && other.tag == "Enemy"){
            return; //Ignore hits on other enemies, or on !yourself!
        }
        Destroy(gameObject);
    }
}
