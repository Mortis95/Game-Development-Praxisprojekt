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

    #region movementAttributes
    //Basic Attributes needed for Movement and Movement Animation
    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;
    public Vector2 movement;
    public Direction lastFacedDirection;
    #endregion

    #region Equipment
    public Inventory inventory;
    public Equipment equipment;
    public Weapon equippedWeapon;
    public Shield equippedShield;
    public Consumable equippedConsumable;
    public Armor equippedArmor;
    public Ability equippedAbility;
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
    [HideInInspector]
    public int expToNextLevel;
    public int skillPointsPerLevel;
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
    public bool FeuerpfeilLearned               = false;
    private int FeuerpfeilMPKost                = 10;
    public bool WasserpfeilhagelLearned         = false;
    private int WasserpfeilhagelMPKost          = 100;
    public bool ScharfschussLearned             = false;
    private int ScharfschussMPKost              = 5;
    public bool WasserhiebLearned               = false;
    private int WasserhiebMPKost                = 20;
    public bool ElektrowirbelLearned            = false;
    private int ElektrowirbelMPKost             = 20;
    public bool RageLearned                     = false;
    private int RageMPKost                      = 10;
    public bool FeuerballLearned                = false;
    private int FeuerballMPKost                 = 20;
    public bool WasserflaecheLearned            = false;
    private int WasserflaecheMPKost             = 100;
    public bool KettenblitzLearned              = false;
    private int KettenblitzMPKost               = 5;
    #endregion
    
    
    public AudioSource audioSource;
    public AudioClip swordSound;
    public AudioClip walkingSound;

    //Levelsystem
    //Vorläufige Leveleinteilung, enthalten sind die nötige Menge an totalen EXP die man benötigt
    int[] experiencePointThreshholds = new int[] { 0, 10, 30, 70, 150, 310, 630, 1270, 2550, 5110, 10230, 20470, 40950}; 

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

        baseAttack = 1;
        baseDefense = 1;
        baseStrength = 1;
        baseDexterity = 1;
        baseIntelligence = 1;
        recalculateStats();

    }
   
    public void UpdateEquipment(){
        this.equippedWeapon = equipment.equippedWeapon;
        this.equippedShield = equipment.shieldInHand;
        this.equippedConsumable = equipment.consumableInHand;
        this.equippedArmor = equipment.equippedArmor;
        this.equippedAbility = equipment.equippedAbility;
        recalculateStats();
    }

    void Update(){
        processMovement();
        processUseConsumableInput();
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
        //TextPopup.createPlayerNotificationPopup(transform, "+ " + xp + " EXP", Color.white); //A little bit TOO obnoxious to have a floating Popup for every defeated enemy.
        checkLevelup();
    }

    public void addHealthPoints(int amount){
        if (currentHealthPoints + amount > maxHealthPoints){
            currentHealthPoints = maxHealthPoints;
        } else {
            currentHealthPoints += amount;
        }

        TextPopup.createPlayerHealPopup(transform, amount);
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

        TextPopup.createPlayerNotificationPopup(transform,amount.ToString(),Color.blue);
        updateUIStatusBar();
    }
    public void addMagicPointsPercentage(float percentage){
        addHealthPoints((int)((float) maxHealthPoints * percentage));
    }
    void checkLevelup(){
        int minimumLevelReached = 1;
        for(int i = 1; i < experiencePointThreshholds.Length; i++){
            if(experiencePoints < experiencePointThreshholds[i]){
                if(minimumLevelReached > currentLevel){
                    int numOfLevelUps = minimumLevelReached - currentLevel;     //How many LevelUps did happen?
                    for(int n = 0; n < numOfLevelUps; n++){levelUp();}          //Level up this many times. (usually this should be 1.)
                    currentLevel = minimumLevelReached;
                } else {
                    expToNextLevel = experiencePointThreshholds[i] - experiencePoints;
                }
                return;
            } else {
                minimumLevelReached += 1;  
            }
        }
    }

    void levelUp(){
        currentSkillpoints += skillPointsPerLevel;
        TextPopup.createPlayerNotificationPopup(transform, "Level Up!", Color.white);
        //Play Level Up Sound
        SkillTree.getInstance().skillTreeChangedCallback(); //Let SkillTree know something changed
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
        totalDexterity = baseDexterity + bonusDexterity;
        totalIntelligence = baseIntelligence + bonusIntelligence;
    }
    public void takeDamage(int dmg){
        currentHealthPoints -= dmg;
        updateUIStatusBar();
        TextPopup.createPlayerDamagePopup(transform, dmg);
    }
    public void getKnockedBack(Rigidbody2D source){
        
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
    public void processUseConsumableInput(){
        if(Input.GetKeyDown(KeyCode.F)){
            if(equippedConsumable != null){
                equippedConsumable.Use();
                equipment.invokeCallback();
            }
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
    public void standStill(){
        movement = Vector2.zero;
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
                skill.AddComponent<FeuerPfeil>();

            } else if (equippedAbility == Ability.Wasserpfeilhagel && currentMagicPoints >= WasserpfeilhagelMPKost){
                currentMagicPoints -= WasserpfeilhagelMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<WasserPfeile>();

            } else if (equippedAbility == Ability.Scharfschuss && currentMagicPoints >= ScharfschussMPKost){
                currentMagicPoints -= ScharfschussMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<ScharfSchuss>();

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
                skill.AddComponent<FeuerBall>();

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
    
    //Skill-Damage Multiplier. This is calculated by: Lerp(1,2, currentLevel / maxLevel)
    // -> In other words, the higher the player level, the closer the Skill-Damage Multiplier gets to 2.
    // -> So at first Level, Skills do 1 * Attribute = Damage
    // -> So at last  Level, Skills do 2 * Attribute = Damage
    // --> Goal: Make Level-Ups feel like an Upgrade.
    public float getSkillDamageMultiplier(){
        float levelProgression = (float) currentLevel / (float) experiencePointThreshholds.Length;
        return Mathf.Lerp(1f,2f, levelProgression);
    }
    #endregion
}
