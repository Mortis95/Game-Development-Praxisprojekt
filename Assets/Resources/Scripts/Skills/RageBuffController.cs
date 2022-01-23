using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBuffController : MonoBehaviour
{

    //Singleton-Pattern, do not let Rage stack.
    public static RageBuffController instance;    

    //Public Stuff
    public float disappearAfterSeconds;
    public float rotationSpeed;
    public Sprite[] sprites;
    
    //Private Stuff, kann Skript sich schon selbst holen
    private float currentAngle;
    private Transform playerTransform;
    private SpriteRenderer spr;
    private float animatonSpeedSeconds;
    
    
    private void Awake(){
        //Check if Singleton
        if(RageBuffController.instance != null){
            instance.refreshBuff();
            Destroy(gameObject);
            return;
        } else {
            RageBuffController.instance = this;
        }

        //Get SpriteRenderer
        spr = gameObject.GetComponent<SpriteRenderer>();

        //Do the actual buff
        Player pl = Player.getInstance();
        pl.setStrength((int)((float) pl.getStrength() * 1.4f));       //Konvert zu Float, multiply mit 1.4, konvert back zu int -> 40% STR Buff

        //Ein paar wichtige Sachen setzen
        currentAngle = 0;
        playerTransform = pl.transform;
        animatonSpeedSeconds = 0.15f;

        //Start Animation Loop
        StartCoroutine(animationLoop());

        //Invoke EndBuff after Seconds
        Invoke("endBuff", disappearAfterSeconds);
    }
    
    //Visual Flame Stuff
    void FixedUpdate(){
        currentAngle += rotationSpeed * Time.fixedDeltaTime;                                                            //Calculate current angle
        currentAngle = currentAngle % 360;                                                                              //Loop after 360 (obviously)
        Vector3 newPos = playerTransform.position + Vector3.up;                                                         //Flame Position should be at just upwards of the player, but still needs to rotate
        transform.position = RotatePointAroundPivot(newPos, playerTransform.position, new Vector3(0,0,currentAngle));   //Rotate the point around the Player around the Z-Axis (The one sticking out of your monitor) by currentAngle
    }

    private void refreshBuff(){
        CancelInvoke("endBuff");
        Invoke("endBuff", disappearAfterSeconds);
    }
    private void endBuff(){
        instance = null;
        Player.getInstance().recalculateStats();        //Make Player recalculate all stats at the end of the Buff, so no wrong Stats are left over.
        Destroy(gameObject);
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles){
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    IEnumerator animationLoop(){
        int currentIndex = 0;
        while(true){
            spr.sprite = sprites[currentIndex];
            currentIndex++;
            currentIndex = currentIndex % sprites.Length;
            yield return new WaitForSeconds(animatonSpeedSeconds);
        }
    }
}
