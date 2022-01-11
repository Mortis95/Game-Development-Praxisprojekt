using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance {get; private set;}
    //Sprites für die items aus der enum in klasse item
    
    public Sprite HealPotionNormal;
    public Sprite HealPotionGroß;
    public Sprite  ManaTrank;
    public Sprite ManaTrankGroß;
    public Sprite MettBrot;
    public Sprite Blätterwasser;
    public Sprite LederRüstung;
    public Sprite KettenRüstung;
    public Sprite PlattenstahlRüstung;
    public Sprite HolzSchild;
    public Sprite EisenSchild;
    public Sprite StahlSchild;
    public Sprite BeginnerBogen;
    public Sprite JägerBogen;
    public Sprite Akolythenstab;
    public Sprite Elementarstab;
    public Sprite MeisterStab;
    public Sprite Schwert;
    public Sprite KampfAxt;
    public Sprite StreitKolben;

    public Transform pfItemWorld;



    private void Awake()
    {
        Instance=this;
    }
}
