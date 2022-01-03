using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupController : MonoBehaviour
{
    public Color32 normalHitColor;
    public Color32 fireHitColor;
    public Color32 waterHitColor;
    public Color32 electricHitColor;
    public int baseFontSize;
    public int critFontSize;
    public int weakHitFontSize;
    public float secondsToLive;

    public static DamagePopupController create(GameObject damagePopupPrefab, Transform position, int dmg, DamageType dmgType, bool isCrit, bool isWeak){
        //Setup the GameObject
        GameObject damagePopup = Instantiate(damagePopupPrefab,position);
        DamagePopupController dpcontrol = damagePopup.GetComponent<DamagePopupController>();
        damagePopup.name = "DamagePopup" + dmgType.ToString() + dmg;
        
        //Setup the popup
        dpcontrol.setup(dmg, dmgType, isCrit, isWeak);

        return dpcontrol;
    }

    private TextMeshPro text;
    private Transform transform;


    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0,0.01f,0);
    }

    public void setup(int dmg, DamageType dmgType, bool isCrit, bool isWeak){
        //Find and setup the TextMeshPro in current object
        text = gameObject.GetComponent<TextMeshPro>();
        transform = gameObject.GetComponent<Transform>();

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

        //Destroy the GameObject after set amount of seconds to live (set in Prefab Inspector)
        Destroy(gameObject, secondsToLive);
    }
}
