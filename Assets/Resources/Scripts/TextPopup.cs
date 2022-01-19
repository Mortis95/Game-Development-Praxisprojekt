using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    public Color32 normalHitColor;
    public Color32 fireHitColor;
    public Color32 waterHitColor;
    public Color32 electricHitColor;
    public int baseFontSize;
    public int critFontSize;
    public int weakHitFontSize;
    public float secondsToLive;

    public static TextPopup createEnemyDamagePopup(Transform tr, int dmg, DamageType dmgType, bool isCrit, bool isWeak){
        //Setup the GameObject
        GameObject damagePopupPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popuptext"), tr.position, tr.rotation);
        TextPopup dpcontrol = damagePopupPrefab.GetComponent<TextPopup>();
        damagePopupPrefab.name = "DamagePopup" + dmgType.ToString() + dmg;
        
        //Setup the popup
        dpcontrol.setupEnemyDamagePopup(dmg, dmgType, isCrit, isWeak);

        return dpcontrol;
    }
    public static TextPopup createPlayerDamagePopup(Transform tr, int dmg){
        //Setup the GameObject
        GameObject damagePopupPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popuptext"), tr.position, tr.rotation);
        TextPopup textControl = damagePopupPrefab.GetComponent<TextPopup>();
        textControl.name = "Player DamagePopup + " + dmg;

        //Setup the Popup
        textControl.setupPlayerDamagePopup(dmg);

        return textControl;
    }
    public static TextPopup createPlayerHealPopup(Transform tr, int heal){
        //Setup the GameObject
        GameObject healPopupPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popuptext"), tr.position, tr.rotation);
        TextPopup textControl = healPopupPrefab.GetComponent<TextPopup>();
        textControl.name = "Player HealPopup + " + heal;

        //Setup the Popup
        textControl.setupPlayerHealPopup(heal);

        return textControl;
    }
    public static TextPopup createPlayerNotificationPopup(Transform tr, string message, Color color){
        //Setup the GameObject
        GameObject textPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Popuptext"), tr.position, tr.rotation);
        TextPopup textControl = textPrefab.GetComponent<TextPopup>();
        textControl.name = "Player Notification: " + message + " (" + color.ToString() +")";

        //Setup the Popup
        textControl.setupPlayerNotificationPopup(message, color);

        return textControl;
    }
    private TextMeshPro text;
    private Rigidbody2D rb;
    private float baseY;
    private bool setupFinished;

    public void setupPlayerNotificationPopup(string s, Color c){
        setupFinished = false;

        //Find and setup the TextMeshPro in current object
        text = gameObject.GetComponent<TextMeshPro>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        //Set damage Text and Font-Size and Color
        text.SetText(s);
        text.fontSize = baseFontSize;
        text.faceColor = c;

        //StartCoroutine
        StartCoroutine(playerTextPopupAnimation());

        //Destroy gameObject after some time
        Destroy(gameObject, secondsToLive);
        setupFinished = true;
    }
    public void setupPlayerHealPopup(int heal){
        setupFinished = false;

        //Find and setup the TextMeshPro in current object
        text = gameObject.GetComponent<TextMeshPro>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        //Set damage Text and Font-Size and Color
        text.SetText(heal.ToString());
        text.fontSize = baseFontSize;
        text.faceColor = Color.green;

        //StartCoroutine
        StartCoroutine(playerTextPopupAnimation());

        //Destroy gameObject after some time
        Destroy(gameObject, secondsToLive);
        setupFinished = true;
    }
    IEnumerator playerTextPopupAnimation(){
        while(!setupFinished){
            yield return new WaitForSeconds(0.05f);
        }
        float baseX = rb.position.x;
        float baseY = rb.position.y;
        float angleHelp = 0f;   //Warning spontaneous spaghetti code
        float heightHelp = 0f;
        //Debug.Log("" + angleHelp + "," + Mathf.Sin(angleHelp));

        while(true){
            Vector2 newPos = new Vector2(baseX + Mathf.Sin(angleHelp), baseY + heightHelp);
            rb.position = newPos;
            angleHelp += 10 *  Time.fixedDeltaTime;
            heightHelp += 10 * Time.fixedDeltaTime;
            angleHelp = angleHelp % 360;
            yield return new WaitForFixedUpdate();
        }
    }
    public void setupPlayerDamagePopup(int dmg){
        setupFinished = false;

        //Find and setup the TextMeshPro in current object
        text = gameObject.GetComponent<TextMeshPro>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        //Set damage Text and Font-Size and Color
        text.SetText(dmg.ToString());
        text.fontSize = baseFontSize;
        text.faceColor = Color.red;

        //StartCoroutine
        StartCoroutine(playerTextPopupAnimation());

        //Destroy gameObject after some time
        Destroy(gameObject, secondsToLive);
        setupFinished = true;
    }
    IEnumerator enemyDamagePopupAnimation(){
        while(!setupFinished){
            yield return new WaitForSeconds(0.05f);
        }
        rb.velocity = new Vector2(Random.Range(-10f,10f), 20);

        while(true){
            if(rb.position.y < baseY){
                rb.position = new Vector2(rb.position.x,baseY);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y));
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public void setupEnemyDamagePopup(int dmg, DamageType dmgType, bool isCrit, bool isWeak){
        setupFinished = false;
        //Find and setup the TextMeshPro in current object
        text = gameObject.GetComponent<TextMeshPro>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        //Remember base height for calculations
        baseY = transform.position.y;


        //Setup damage text and Font-Size to display
        string s = dmg.ToString();
        if(isCrit){
            s += "!";
            text.fontSize = critFontSize;
        } else if (isWeak){
            s += "...";
            text.fontSize = weakHitFontSize;
        } else {
            text.fontSize = baseFontSize;
        }
        text.SetText(s);

        //Setup color of damage text
        switch(dmgType){
            case DamageType.Normal:
                text.faceColor = normalHitColor;
                break;
            case DamageType.Feuer:
                text.faceColor = fireHitColor;
                break;
            case DamageType.Wasser:
                text.faceColor = waterHitColor;
                break;
            case DamageType.Blitz:
                text.faceColor = electricHitColor;
                break;
        }

        //Start Coroutine
        StartCoroutine(enemyDamagePopupAnimation());

        //Destroy the GameObject after set amount of seconds to live (set in Prefab Inspector)
        Destroy(gameObject, secondsToLive);
        setupFinished = true;
    }
}
