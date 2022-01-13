using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Singleton
    //Singleton-Pattern
    private static Player instance;

        private void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
        
    }

    public static Player getInstance(){
        return instance;
    }
    #endregion

    //Basic Attributes needed for Movement and Movement Animation
    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 movement;
    public Direction lastFacedDirection;

    public Inventory inventory;
    #region Equipment
    //Kampf Stuff
    public Equipment equipment;
    public Weapon equippedWeapon;
    public Shield equippedShield;
    public Consumable equippedConsumable;
    public Armor equippedArmor;
    #endregion
    
    #region Instantiates
    //Instantiates - Gameobjects needed to instantiate by button presses
    public GameObject meleeAttackPrefab;
    public GameObject rangedAttackPrefab;
    public GameObject magicAttackPrefab;
    public GameObject emptySkill;
    public GameObject Wasserflaeche;
    public GameObject WasserPfeil;
    public GameObject ElektroWirbel;
    public GameObject WasserHieb;
    public GameObject Rage;
    #endregion

    #region CharacterAttributes
    //Character Attributes
    public int experiencePoints;
    public int currentLevel;
    public int currentSkillpoints;
    public int currentHealthPoints;
    public int maxHealthPoints;
    public int currentMagicPoints;
    public int maxMagicPoints;
    
    //Base Stats from the character at that level
    private int baseAttack;
    private int baseDefense;
    private int baseStrength;
    private int baseDexterity;
    private int baseIntelligence;
    //Total Stats for the character, after Equipment Bonus is added.
    private int totalAttack;
    private int totalDefense;
    private int totalStrength;
    private int totalDexterity;
    private int totalIntelligence;

    #endregion

    #region AbilityCostAndVariables
    //Ability Kram - Bools sehen zwar doof aus, sind aber praktisch und peformancetechnisch gesehen das Beste. 
    public Ability equippedAbility;
    public bool FeuerpfeilLearned               = false;
    private int FeuerpfeilMPKost                = 100;
    public bool WasserpfeilhagelLearned         = false;
    private int WasserpfeilhagelMPKost          = 100;
    public bool ScharfschussLearned             = false;
    private int ScharfschussMPKost              = 100;
    public bool WasserhiebLearned               = false;
    private int WasserhiebMPKost                = 20;
    public bool ElektrowirbelLearned            = false;
    private int ElektrowirbelMPKost             = 20;
    public bool RageLearned                     = false;
    private int RageMPKost                      = 10;
    public bool FeuerballLearned                = false;
    private int FeuerballMPKost                 = 100;
    public bool WasserflaecheLearned            = false;
    private int WasserflaecheMPKost             = 100;
    public bool KettenblitzLearned              = false;
    private int KettenblitzMPKost               = 5;
    #endregion
    
    
    public AudioSource audioSource;
    public AudioClip swordSound;
    public AudioClip walkingSound;

    //Levelsystem
    //Vorläufige leveleinteilung, enthalten sind die mengen an nötigen xp
    int[] levelStufen = new int[] { 0,300,700,1200,1800,2400,   
                                    3100,3900,4700,5600,6600,
                                    7700,8900,10200,11600,13100,
                                    14700,16400,1900,2200,23000,
                                    25000,30000,35000,400000 }; 

    
    

    AudioSource attackSound;

    //UI Status Bar
    public StatusBar healthBar;
    public StatusBar manaBar;
    public Text UIcurrentLevel;
                                                                

    void Start()
    {        	
        
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSources[0].clip = walkingSound;
        audioSources[1].clip = swordSound;
        audioSource = audioSources[0];
        attackSound = audioSources[1];

        equipment = Equipment.getInstance();
        equipment.onEquipmentChangedCallback += UpdateEquipment;

    }
   
    public void UpdateEquipment(){
        this.equippedWeapon = equipment.equippedWeapon;
        this.equippedShield = equipment.shieldInHand;
        this.equippedConsumable = equipment.consumableInHand;
        this.equippedArmor = equipment.equippedArmor;
        recalculateStats();
    }

    void Update(){
    processMovement();
    processAttackInput();
    processSkillInput();         
    }

    void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void addExp(int xp)
    {
        experiencePoints += xp;
        checkLevelup();
    }

    public void addHealthPoints(int amount){
        if (currentHealthPoints + amount > maxHealthPoints){
            currentHealthPoints = maxHealthPoints;
        } else {
            currentHealthPoints += amount;
        }

        updateUIStatusBar();
    }

    public void addHealthPointsPercentage(float percentage){
        addHealthPoints((int)((float) maxHealthPoints * percentage));
    }

    public void addMagicPoints(int amount){
        if (currentMagicPoints + amount > maxMagicPoints){
            currentMagicPoints = maxMagicPoints;
        } else {
            currentMagicPoints += amount;
        }

        updateUIStatusBar();
    }
    public void addMagicPointsPercentage(float percentage){
        addHealthPoints((int)((float) maxHealthPoints * percentage));
    }
    void checkLevelup(){
        //TODO
    }

    public void updateUIStatusBar(){
        //Update UI Statusbar
        healthBar.setMaxValue(maxHealthPoints);
        healthBar.setValue(currentHealthPoints);

        manaBar.setMaxValue(maxMagicPoints);
        manaBar.setValue(currentMagicPoints);
    }
    public void recalculateStats(){
        int bonusAttack         = 0;
        int bonusDefense        = 0;
        int bonusStrength       = 0;
        int bonusDexterity      = 0;
        int bonusIntelligence   = 0;
        if(equippedWeapon != null){
            bonusAttack += equippedWeapon.bonusAttack;
            //bonusDefense += equippedWeapon.bonusDefense; //Weapons do not have bonus Defense yet
            bonusStrength += equippedWeapon.bonusStrength;
            bonusDexterity += equippedWeapon.bonusDexterity;
            bonusIntelligence += equippedWeapon.bonusIntelligence;
        }
        if(equippedShield != null){
            //bonusAttack += equippedShield.bonusAttack; //Shields do not have bonusAttack yet
            bonusDefense += equippedShield.bonusDefense;
            bonusStrength += equippedShield.bonusStrength;
            bonusDexterity += equippedShield.bonusDexterity;
            bonusIntelligence += equippedShield.bonusIntelligence;
        }
        if(equippedArmor != null){
            //bonusAttack += equippedArmor.bonusAttack; //Armors do not have bonusAttack yet
            bonusDefense += equippedArmor.bonusDefense;
            bonusStrength += equippedArmor.bonusStrength;
            bonusDexterity += equippedArmor.bonusDexterity;
            bonusIntelligence += equippedArmor.bonusIntelligence;
        }
        totalAttack = baseAttack + bonusAttack;
        totalDefense = baseDefense + bonusDefense;
        totalStrength = baseStrength + bonusStrength;
        totalIntelligence = baseIntelligence + bonusIntelligence;
    }
    public void takeDamage(DamageType damageType, int dmg){
        currentHealthPoints -= dmg;
        //DamagePopupController.create();
    }
    public void processMovement(){
        //Movement
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical"); 
        movement.x = moveX;
        movement.y = moveY;
        movement.Normalize();   //Normalizes the vector to have a magnitude of 1. Heißt im Klartext, unser Spieler läuft Diagonal genauso schnell wie horizontal / vertikal
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Movement sound
        if(moveX != 0 || moveY != 0){
            audioSource.volume = 0.04f;
            if(!audioSource.isPlaying){audioSource.Play();}
        } else{
            audioSource.Stop();
        }

        //Remember last faced direction - In edge cases this prioritizes Right over Left and Up over Down.  ¯\_(ツ)_/¯
        if(moveX > 0){
            lastFacedDirection = Direction.Right;
        } else if(moveX < 0){
            lastFacedDirection = Direction.Left;
        }

        if(moveY > 0){
            lastFacedDirection = Direction.Up;
        } else if(moveY < 0){
            lastFacedDirection = Direction.Down;
        }
    }
    public void processAttackInput(){
        if(Input.GetKeyDown(KeyCode.Space) && equippedWeapon != null){
            switch(equippedWeapon.weaponType){
                case WeaponType.Melee:
                    Instantiate(meleeAttackPrefab, transform.position, transform.rotation);
                    break;
                case WeaponType.Range:
                    Instantiate(rangedAttackPrefab, transform.position, transform.rotation);
                    break;
                case WeaponType.Magic:
                    Instantiate(magicAttackPrefab, transform.position, transform.rotation);
                    break;
            }
        }
    }

    #region UseSkills
    //Skills nutzen
    //Ein Skill wird benutzt indem zuerst geschaut wird welcher Skill ausgerüstet ist und ob die MP dafür reichen
    //Wenn dies erfüllt ist, werden die MP dementsprechend verringert, und ein neues komplett leeres GameObject wird instanziiert.
    //Das leere GameObject wird an der gleichen Stelle erstellt, wo der Spieler sich aktuell befindet.
    //Der Plan ist, diesem leeren GameObject dann das jeweilige Skill-Skript hinzuzufügen. 
    //Das jeweilige Skill-Skript soll das komplette Behaviour vom Skill bestimmen (Lebenszeit, Hitbox, Schaden).
    //Dazu kann in der "private void Awake()" - Methode eine Referenz zum Spieler mithilfe von "Player.getInstance()" erstellt werden.
    //Mithilfe dieser Referenz können dann Spielerattribute (STR, DEX, INT) geholt werden, welche eventuell zur Schadensberechnung wichtig sind.
    //Es kann alternativ auch ein vorher erstelltes Prefab instantiated werden, solange dies vorher der Player-Class bekannt gemacht wurde unter den "Instantiates" oben

    //Notiz: Dieser Code ist nicht hübsch, sollte aber Performancetechnisch kein Problem darstellen.
    //Vermutlich ließe sich das hier besser lösen über ScriptableObjects, wenn man mehr Ahnung von Unity hätte, allerdings haben wir Zeitdruck und müssen fertig werden.
    //Ich bitte euch daher einfach diese suboptimale Lösung zu akzeptieren, weil sie funktioniert.
    public void processSkillInput(){
        if(Input.GetKeyDown(KeyCode.Q)){
            if (equippedAbility == Ability.Feuerpfeil && currentMagicPoints >= FeuerpfeilMPKost){
                currentMagicPoints -= FeuerpfeilMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                /* skill.AddComponent<Feuerpfeil>(); */

            } else if (equippedAbility == Ability.Wasserpfeilhagel && currentMagicPoints >= WasserpfeilhagelMPKost){
                currentMagicPoints -= WasserpfeilhagelMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<WasserPfeile>();

            } else if (equippedAbility == Ability.Scharfschuss && currentMagicPoints >= ScharfschussMPKost){
                currentMagicPoints -= ScharfschussMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                /* skill.AddComponent<Scharfschuss>(); */

            } else if (equippedAbility == Ability.Wasserhieb && currentMagicPoints >= WasserhiebMPKost){
                currentMagicPoints -=  WasserhiebMPKost;
                GameObject skill = Instantiate(WasserHieb, transform.position, transform.rotation);
                /* Start Slash Animation maybe? */

            } else if (equippedAbility == Ability.Elektrowirbel && currentMagicPoints >= ElektrowirbelMPKost){
                currentMagicPoints -= ElektrowirbelMPKost;
                GameObject skill = Instantiate(ElektroWirbel, transform.position, transform.rotation);
                /*Start Wirbel Animation maybe?*/

            } else if (equippedAbility == Ability.Rage && currentMagicPoints >= RageMPKost){
                currentMagicPoints -= RageMPKost;
                GameObject skill = Instantiate(Rage, transform.position, transform.rotation);
                /*Insert Rage Animation here, falls es so etwas jemals geben sollte lmao - Was eine gute Idee nur einen Artist zu haben*/

            } else if (equippedAbility == Ability.Feuerball && currentMagicPoints >= FeuerballMPKost){
                currentMagicPoints -= FeuerballMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                /* skill.AddComponent<Feuerball>(); */

            } else if (equippedAbility == Ability.Wasserflaeche && currentMagicPoints >= WasserflaecheMPKost){
                currentMagicPoints -= WasserflaecheMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<Wasserfläche>();

            } else if (equippedAbility == Ability.Kettenblitz && currentMagicPoints >= KettenblitzMPKost){
                currentMagicPoints -= KettenblitzMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<Kettenblitz>();

            }
            updateUIStatusBar();
        }
    }
    #endregion

    #region GetterAndSetterAndAdder
    public int getAttack(){return totalAttack;}
    public int getDefense(){return totalDefense;}
    public int getStrength(){return totalStrength;}
    public int getDexterity(){return totalDexterity;}
    public int getIntelligence(){return totalIntelligence;}
    public void setStrength(int newValue){totalStrength = newValue;}
    public void addPermanentStats(int addAttack, int addDefense, int addStrength, int addDexterity, int addIntelligence){
        baseAttack       += addAttack;
        baseDefense      += addDefense;
        baseStrength     += addStrength;
        baseDexterity    += addDexterity;
        baseIntelligence += addIntelligence;
        recalculateStats();
    }
    #endregion
}
