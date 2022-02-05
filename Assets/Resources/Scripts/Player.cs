using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Vector2 movement;
    public Direction lastFacedDirection;
    private Animator animator;
    private AnimationState currentAnimationState;
    private float lastActionTime;
    private float currentActionDelaySeconds;
    private bool receivingKnockback;
    private float walkingSoundTime;
    private float walkingSoundDelay;

    public bool transitioned = false;
    public float designatedx;
    public float designatedy;
    private Vector2 respawnPosition;
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
    private int FeuerpfeilMPKost                = 8;
    public bool WasserpfeilhagelLearned         = false;
    private int WasserpfeilhagelMPKost          = 30;
    public bool ScharfschussLearned             = false;
    private int ScharfschussMPKost              = 12;
    public bool WasserhiebLearned               = false;
    private int WasserhiebMPKost                = 20;
    public bool ElektrowirbelLearned            = false;
    private int ElektrowirbelMPKost             = 20;
    public bool RageLearned                     = false;
    private int RageMPKost                      = 10;
    public bool FeuerballLearned                = false;
    private int FeuerballMPKost                 = 10;
    public bool WasserflaecheLearned            = false;
    private int WasserflaecheMPKost             = 30;
    public bool KettenblitzLearned              = false;
    private int KettenblitzMPKost               = 15;
    #endregion
    
    #region AnimatorStateChange
    //All AnimationStates for the player that we previously agreed on
    private enum AnimationState{
        PlayerWalkUp,
        PlayerWalkDown,
        PlayerWalkLeft,
        PlayerWalkRight,
        PlayerStandUp,
        PlayerStandDown,
        PlayerStandLeft,
        PlayerStandRight,
        PlayerMeleeAttackUp,
        PlayerMeleeAttackDown,
        PlayerMeleeAttackLeft,
        PlayerMeleeAttackRight,
        PlayerRangedAttackUp,
        PlayerRangedAttackDown,
        PlayerRangedAttackLeft,
        PlayerRangedAttackRight,
        PlayerMagicAttackUp,
        PlayerMagicAttackDown,
        PlayerMagicAttackLeft,
        PlayerMagicAttackRight,
        PlayerGameOver
    }


    private void changeAnimationState(AnimationState state){
        //If current animation is already playing, do not start it again.
        //Otherwise an animation would start every frame and never properly play out.
        if(state.Equals(currentAnimationState)){
            return;
        }
        animator.Play(state.ToString());
        currentAnimationState = state;
    }

    public void setMeleeAttackAnimation(){
        switch(lastFacedDirection){
            case Direction.Up:
                changeAnimationState(AnimationState.PlayerMeleeAttackUp);
                break;
            case Direction.Down:
                changeAnimationState(AnimationState.PlayerMeleeAttackDown);
                break;
            case Direction.Left:
                changeAnimationState(AnimationState.PlayerMeleeAttackLeft);
                break;
            case Direction.Right:
                changeAnimationState(AnimationState.PlayerMeleeAttackRight);
                break;
                
            }
    }

    public void setRangedAttackAnimation(){
        switch(lastFacedDirection){
            case Direction.Up:
                changeAnimationState(AnimationState.PlayerRangedAttackUp);
                break;
            case Direction.Down:
                changeAnimationState(AnimationState.PlayerRangedAttackDown);
                break;
            case Direction.Left:
                changeAnimationState(AnimationState.PlayerRangedAttackLeft);
                break;
            case Direction.Right:
                changeAnimationState(AnimationState.PlayerRangedAttackRight);
                break;
                
            }
    }

    public void setMagicAttackAnimation(){
        switch(lastFacedDirection){
            case Direction.Up:
                changeAnimationState(AnimationState.PlayerMagicAttackUp);
                break;
            case Direction.Down:
                changeAnimationState(AnimationState.PlayerMagicAttackDown);
                break;
            case Direction.Left:
                changeAnimationState(AnimationState.PlayerMagicAttackLeft);
                break;
            case Direction.Right:
                changeAnimationState(AnimationState.PlayerMagicAttackRight);
                break;
                
            }
    }


    #endregion


    //Levelsystem
    //Vorläufige Leveleinteilung, enthalten sind die nötige Menge an totalen EXP die man benötigt
    int[] experiencePointThreshholds = new int[] { 0, 10, 30, 70, 150, 310, 630, 1270, 2550, 5110, 10230, 20470, 40950}; 


    //UI Status Bar
    public StatusBar healthBar;
    public StatusBar manaBar;
    public Text UIcurrentLevel;
    public bool isDead;

    //Game Manager
    private GameManager gm;
                                                                

    void Start()
    {          
        

        animator = gameObject.GetComponent<Animator>();

        equipment = Equipment.getInstance();
        equipment.onEquipmentChangedCallback += UpdateEquipment;

        baseAttack = 1;
        baseDefense = 1;
        baseStrength = 1;
        baseDexterity = 1;
        baseIntelligence = 1;
        recalculateStats();

        lastActionTime = 0;
        currentActionDelaySeconds = 0;
        receivingKnockback = false;
        currentAnimationState = AnimationState.PlayerStandDown;
        isDead = false;

        walkingSoundTime = 0;
        walkingSoundDelay = 0.65f;

        gm = GameManager.getInstance();

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if(transitioned)
        {
            transform.position = new Vector3(designatedx,designatedy,0);
            respawnPosition = transform.position;
            transitioned=false;
            designatedx=0;
            designatedy=0;  //bugs durch default wert vorbeugen
        }
        if(isDead){processRespawnInput(); return;}
        if(gm.getGameIsPaused()){return;}
        if(isAllowedToTakeAction()){
            processMovement();
            processUseConsumableInput();
            processAttackInput();
            processSkillInput();
        } else {
            standStill();
        }
        checkGameOver();
    }

    void FixedUpdate()
    {
        //Movement
        if(!receivingKnockback){rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);}
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
        AudioManager.getInstance().PlaySound("SpielerLevelUp");
        SkillTree.getInstance().skillTreeChangedCallback(); //Let SkillTree know something changed
    }
    private void checkGameOver(){
        if(currentHealthPoints <= 0){
            isDead = true;
            AudioManager.getInstance().PlaySound("UIGameOver");
            changeAnimationState(AnimationState.PlayerGameOver);
        }
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
        
        SkillTree st = SkillTree.getInstance();
        
        bonusAttack       += st.getBonusAttack();
        bonusDefense      += st.getBonusDefense();
        bonusStrength     += st.getBonusStrength();
        bonusDexterity    += st.getBonusDexterity();
        bonusIntelligence += st.getBonusIntelligence();

        totalAttack       = baseAttack       + bonusAttack;
        totalDefense      = baseDefense      + bonusDefense;
        totalStrength     = baseStrength     + bonusStrength;
        totalDexterity    = baseDexterity    + bonusDexterity;
        totalIntelligence = baseIntelligence + bonusIntelligence;
    }
    public void takeDamage(int dmg){
        int actualDmg = Mathf.Max((dmg - getDefense()), 0);
        currentHealthPoints -= actualDmg;
        updateUIStatusBar();
        TextPopup.createPlayerDamagePopup(transform, actualDmg);
    }    
    public void getKnockedBack(Vector2 origin, float knockBackForce){
        StartCoroutine(knockBackLoop(origin, knockBackForce));
    }

    IEnumerator knockBackLoop(Vector2 origin, float knockBackForce){
        receivingKnockback = true;
        float startTime = Time.time;
        float knockBackTimeSeconds = 0.25f;
        Debug.Log("" + knockBackForce + " " +  getDefense() + " " + (knockBackForce - getDefense()));
        float actualKnockBackForce = Mathf.Max(knockBackForce - getDefense(), 0f); //Reduce Knockback force by Defense. But don't allow negative Knockback
        if((int)actualKnockBackForce == 0){
            receivingKnockback = false;
        } else {
            Vector2 direction = rb.position - origin;
            direction.Normalize();
            while (Time.time - startTime < knockBackTimeSeconds){
                rb.MovePosition(rb.position + direction * knockBackForce * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
            receivingKnockback = false;
        }

    }
    public void processMovement(){
        //Movement
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical"); 
        movement.x = moveX;
        movement.y = moveY;
        movement.Normalize();   //Normalizes the vector to have a magnitude of 1. Heißt im Klartext, unser Spieler läuft Diagonal genauso schnell wie horizontal / vertikal

        //Movement sound
        if(movement.magnitude > 0 && (Time.time - walkingSoundTime) >= walkingSoundDelay){
            AudioManager.getInstance().PlaySoundNotOverlapping("SpielerGehenLangsam");
            walkingSoundTime = Time.time;
            }

        //Wenn moveX und moveY == 0 dann liegt kein Movement vor und wir müssen die dazugehörige Steh-Animation spielen
        if(moveX == 0 && moveY == 0){
            switch(lastFacedDirection){
                case Direction.Up:
                    changeAnimationState(AnimationState.PlayerStandUp);
                    break;
                case Direction.Down:
                    changeAnimationState(AnimationState.PlayerStandDown);
                    break;
                case Direction.Left:
                    changeAnimationState(AnimationState.PlayerStandLeft);
                    break;
                case Direction.Right:
                    changeAnimationState(AnimationState.PlayerStandRight);
                    break;
            }
        //Ansonsten wenn der Winkel zwischen Movement und jeweiligen Richtungsvektor 
        //(Vector2.up (0,1), Vector2.down (0,-1), Vector2.left(-1,0), Vector2.right(1,0) <= 45° ist, 
        //spielen wir die jeweilige Lauf-Animation.
        //Somit haben wir alle Winkel in 360° abgedeckt. (Notiz: In Edge-Cases wird oben/unten priorisiert)  
        } else if(Vector2.Angle(Vector2.up, movement) <= 45){
            lastFacedDirection = Direction.Up;
            changeAnimationState(AnimationState.PlayerWalkUp);
        } else if(Vector2.Angle(Vector2.down, movement) <= 45 ){
            lastFacedDirection = Direction.Down;
            changeAnimationState(AnimationState.PlayerWalkDown);
        } else if(Vector2.Angle(Vector2.left, movement) < 45 ){
            lastFacedDirection = Direction.Left;
            changeAnimationState(AnimationState.PlayerWalkLeft);
        } else if(Vector2.Angle(Vector2.right, movement) < 45 ){
            lastFacedDirection = Direction.Right;
            changeAnimationState(AnimationState.PlayerWalkRight);
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
                    setActionDelaySeconds(0.5f);
                    setMeleeAttackAnimation();
                    break;
                case WeaponType.Range:
                    Instantiate(rangedAttackPrefab, transform.position, transform.rotation);
                    setActionDelaySeconds(0.55f);
                    setRangedAttackAnimation();
                    break;
                case WeaponType.Magic:
                    Instantiate(magicAttackPrefab, transform.position, transform.rotation);
                    setActionDelaySeconds(0.55f);
                    setMagicAttackAnimation();
                    break;
            }
        }
    }
    
    private void processRespawnInput(){
        if(Input.GetKeyDown(KeyCode.R)){
            respawn();
        }
    }

    private void respawn(){
        currentHealthPoints = maxHealthPoints;
        currentMagicPoints = maxMagicPoints;
        transform.position = respawnPosition;
        isDead = false;
        changeAnimationState(AnimationState.PlayerStandDown);
        updateUIStatusBar();
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
                setActionDelaySeconds(0.5f);
                setRangedAttackAnimation();

            } else if (equippedAbility == Ability.Wasserpfeilhagel && currentMagicPoints >= WasserpfeilhagelMPKost){
                currentMagicPoints -= WasserpfeilhagelMPKost;
                GameObject skill = Instantiate(WasserPfeil, transform.position, transform.rotation);
                skill.AddComponent<WasserPfeile>();
                setActionDelaySeconds(0.5f);
                setRangedAttackAnimation();

            } else if (equippedAbility == Ability.Scharfschuss && currentMagicPoints >= ScharfschussMPKost){
                currentMagicPoints -= ScharfschussMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<ScharfSchuss>();
                setActionDelaySeconds(0.5f);
                setRangedAttackAnimation();

            } else if (equippedAbility == Ability.Wasserhieb && currentMagicPoints >= WasserhiebMPKost){
                currentMagicPoints -=  WasserhiebMPKost;
                GameObject skill = Instantiate(WasserHieb, transform.position, transform.rotation);
                setActionDelaySeconds(0.5f);
                setMeleeAttackAnimation();

            } else if (equippedAbility == Ability.Elektrowirbel && currentMagicPoints >= ElektrowirbelMPKost){
                currentMagicPoints -= ElektrowirbelMPKost;
                GameObject skill = Instantiate(ElektroWirbel, transform.position, transform.rotation);
                setActionDelaySeconds(0.5f);
                setMeleeAttackAnimation();

            } else if (equippedAbility == Ability.Rage && currentMagicPoints >= RageMPKost){
                currentMagicPoints -= RageMPKost;
                GameObject skill = Instantiate(Rage, transform.position, transform.rotation);
                setActionDelaySeconds(0.5f);
                setMeleeAttackAnimation();

            } else if (equippedAbility == Ability.Feuerball && currentMagicPoints >= FeuerballMPKost){
                currentMagicPoints -= FeuerballMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<FeuerBall>();
                setActionDelaySeconds(0.5f);
                setMagicAttackAnimation();

            } else if (equippedAbility == Ability.Wasserflaeche && currentMagicPoints >= WasserflaecheMPKost){
                currentMagicPoints -= WasserflaecheMPKost;
                GameObject skill = Instantiate(Wasserflaeche, transform.position, transform.rotation);
                skill.AddComponent<Wasserfläche>();
                setActionDelaySeconds(0.5f);
                setMagicAttackAnimation();

            } else if (equippedAbility == Ability.Kettenblitz && currentMagicPoints >= KettenblitzMPKost){
                currentMagicPoints -= KettenblitzMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<Kettenblitz>();
                setActionDelaySeconds(0.5f);
                setMagicAttackAnimation();

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
    
    //Skill-Damage Multiplier. This is calculated by: Lerp(1f,1.8f, currentLevel / maxLevel)
    // -> In other words, the higher the player level, the closer the Skill-Damage Multiplier gets to 1.8f.
    // -> So at first Level, Skills do 1 * Attribute = Damage
    // -> So at last  Level, Skills do 1.8 * Attribute = Damage
    // --> Goal: Make Level-Ups feel like an Upgrade.
    public float getSkillDamageMultiplier(){
        float levelProgression = (float) currentLevel / (float) experiencePointThreshholds.Length;
        return Mathf.Lerp(1f,1.8f, levelProgression);
    }

    public void setActionDelaySeconds(float seconds){
        lastActionTime = Time.fixedTime;
        currentActionDelaySeconds = seconds;
    }

    public bool isAllowedToTakeAction(){
        return (Time.fixedTime - lastActionTime > currentActionDelaySeconds);
    }

    #endregion
}
