using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackController : MonoBehaviour{
    public Sprite sprite;
    public Vector2 direction;
    public float shotSpeed;
    public float timeToLiveSeconds;
    public int damage;
    
    public void setParameters(Vector2 direction, float shotSpeed, float timeToLiveSeconds, int damage){
        this.direction = direction;
        this.shotSpeed = shotSpeed;
        this.timeToLiveSeconds = timeToLiveSeconds;
        this.damage = damage;
    }
    public void setParameters(Sprite sprite, Vector2 direction, float shotSpeed, float timeToLiveSeconds, int damage){
        this.sprite = sprite;
        this.direction = direction;
        this.shotSpeed = shotSpeed;
        this.timeToLiveSeconds = timeToLiveSeconds;
        this.damage = damage;
    }
    void Awake(){
        if(sprite != null){
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        direction.Normalize();
        Destroy(gameObject, timeToLiveSeconds);
    }

    void Start(){
        transform.up = new Vector3(direction.x,direction.y,0);
    }

    void FixedUpdate(){
        transform.position += (Vector3) direction * shotSpeed * Time.fixedDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D col){
        GameObject other = col.gameObject;
        Debug.Log("Collision with:" + col.name);
        if(other != null && other.tag == "Player"){
            Player playerScript = Player.getInstance();
            playerScript.takeDamage(damage);
        }
        if(other != null && other.tag == "Enemy"){
            return; //Ignore hits on other enemies, or on !yourself!
        }
        Destroy(gameObject);
    }
}
