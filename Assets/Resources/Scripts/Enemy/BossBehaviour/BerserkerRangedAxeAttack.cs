using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerRangedAxeAttack : MonoBehaviour{
    public Vector2 direction;
    public float shotSpeed;
    public float timeToLiveSeconds;
    public float rotationSpeed;
    public int damage;
    public LayerMask lm;

    public static void createAttack(Transform transform, Vector2 direction, int damage, float timeToLive){
        GameObject braaPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Bosses/BerserkerAttackPrefabs/BerserkerRangedAxeAttackPrefab");
        GameObject braa = Instantiate(braaPrefab, transform.position, transform.rotation);
        BerserkerRangedAxeAttack braaScript = braa.GetComponent<BerserkerRangedAxeAttack>();
        braaScript.direction = direction;
        braaScript.damage = damage;
        braaScript.timeToLiveSeconds = timeToLive;
        braaScript.rotationSpeed = 10f;
        braa.SetActive(true);
    }
    
    void Start(){
        lm = LayerMask.GetMask("Blocking");
        Destroy(gameObject, timeToLiveSeconds);
    }

    void Update(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, lm);
        if(hit){direction = Vector2.Reflect(direction, hit.normal);} //We have bouncy Axes now. This is the future!!
    }

    void FixedUpdate(){
        transform.position += (Vector3) direction * shotSpeed * Time.fixedDeltaTime;
        transform.Rotate(0.0f,0.0f,rotationSpeed);
    }

    void OnTriggerEnter2D(Collider2D col){
        GameObject other = col.gameObject;
        Debug.Log("Collision with:" + col.name);
        if(other != null && !other.name.Equals("LayerSorter")){return;}
        if(other != null && other.tag == "Player"){
            Player playerScript = Player.getInstance();
            playerScript.takeDamage(damage);
            playerScript.getKnockedBack(transform.position,damage);
        }
        if(other != null && other.tag == "Enemy"){
            return; //Ignore hits on other enemies, or on !yourself!
        }
        Destroy(gameObject);
    }
}
