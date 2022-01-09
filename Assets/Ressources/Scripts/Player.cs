using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SkillTree;

public class Player : MonoBehaviour
{

    //Singleton-Pattern
    private static Player instance;


    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    public Animator animator;

    public Vector2 movement;
    public Direction lastFacedDirection;

    //Kampf shit
    public Transform attackpoint;
    public Transform attackpointLeft;
    public Transform attackpointRight;
    public Transform attackpointUp;
    public Transform attackpointDown;
    public float attackrange = 0.5f;
    public LayerMask enemyLayers;
    public float attackRate = 2;
    float nextAttackTime = 0f;
    public Weapon EquipedWeapon;
    public Shield EquipedShield;
    public Armor EquipedArmor;
    public bool meleeAttack=false;
    
    //Instantiates 
    public GameObject ArrowPrefabLeft;   
    public GameObject ArrowPrefabRight;    
    public GameObject ArrowPrefabUp;    
    public GameObject ArrowPrefabDown;

    public GameObject JaegerArrowPrefabLeft;   
    public GameObject JaegerArrowPrefabRight;    
    public GameObject JaegerArrowPrefabUp;    
    public GameObject JaegerArrowPrefabDown;
    public GameObject emptySkill;

    public GameObject Wasserflaeche;
    public GameObject WasserPfeil;
    public GameObject ElektroWirbel;
    public GameObject WasserHieb;
    public GameObject Rage;


    //Attribute aus Spielmechaniken.pdf
    public int currentHealth=100;
    public int maxHealth=100;
    public int MP = 100;
    public int MPMax=100;
    public int atk=40;  //physischer Schaden
    public int matk=0;  //magischer Schaden
    public int def=0; //Verteidigung
    public int mdef=0; //magische Verteidigung 
    public int str=15; //staerke
    public int dex=15; //Geschicklichkeit
    public int inte =15; // intelligenz

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
    public bool SturmketteLearned               = false;
    private int SturmketteMPKost                = 5;

    
    //canvasSkilltree für skilltree
    public GameObject canvasSkilltree;
    public AudioSource audioSource;
    public AudioClip swordSound;
    public AudioClip walkingSound;
    bool canvasSkilltreeisActive;
    //Levelsystem
    public int exp=0;
    int level=0;
    //Vorläufige leveleinteilung, enthalten sind die mengen an nötigen xp
    int[] levelStufen = new int[] { 0,300,700,1200,1800,2400,   
                                    3100,3900,4700,5600,6600,
                                    7700,8900,10200,11600,13100,
                                    14700,16400,1900,2200,23000,
                                    25000,30000,35000,400000 }; 

    //InventoryShit
    private Inventory inventory;
    [SerializeField] private UI_Inventory ui_Inventory;
    //Canvas für Inventar
    public GameObject canvasInventory;
    bool canvasInventoryisActive;
    public Vector2 position;
    //ItemBools
    public bool HealPotionNormalEquiped=false;
    public bool HealPotionGroßEquiped=false;
    public bool  ManaTrankEquiped=false;
    public bool ManaTrankGroßEquiped=false;
    public bool MettBrotEquiped=false;
    public bool BlätterwasserEquiped=false;
    public bool LederRüstungEquiped=false;
    public bool KettenRüstungEquiped=false;
    public bool PlattenstahlRüstungEquiped=false;
    public bool HolzSchildEquiped=false;
    public bool EisenSchildEquiped=false;
    public bool StahlSchildEquiped=false;
    public bool BeginnerBogenEquiped=false;
    public bool JägerBogenEquiped=false;
    public bool AkolythenstabEquiped=false;
    public bool ElementarstabEquiped=false;
    public bool MeisterStabEquiped=false;
    public bool SchwertEquiped=false;
    public bool KampfAxtEquiped=false;
    public bool StreitKolbenEquiped=false;
    public float knockbackSpeed = 400f;
    

    AudioSource attackSound;

    //UI Status Bar
    public StatusBar healthBar;
    public StatusBar manaBar;
    public Text UIcurrentLevel;

    //potentielles Equipment
    public Weapon BeginnerBogen = new Weapon(10,Weapon.WeaponType.Bow);
    public Weapon JägerBogen = new Weapon(20,Weapon.WeaponType.Bow);
    public Weapon Akolythenstab = new Weapon(40,Weapon.WeaponType.Staff);
    public Weapon Elementarstab = new Weapon(60,Weapon.WeaponType.Staff);
    public Weapon MeisterStab = new Weapon(80,Weapon.WeaponType.Staff);
    public Weapon Schwert = new Weapon(40,Weapon.WeaponType.Melee);
    public Weapon KampfAxt = new Weapon(50,Weapon.WeaponType.Melee);
    public Weapon StreitKolben = new Weapon(60,Weapon.WeaponType.Melee); 
    
    public Shield HolzSchild = new Shield(10,10);    
    public Shield EisenSchild = new Shield(20,15);   
    public Shield StahlSchild = new Shield(30,20); 

    public Armor LederRüstung = new Armor(10);     
    public Armor KettenRüstung = new Armor(20); 
    public Armor PlattenstahlRüstung = new Armor(30);   

    public enum Direction
   {
       Left,
       Right,
       Up,
       Down

   }      

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

    void Start()
    {        	
        
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSources[0].clip = walkingSound;
        audioSources[1].clip = swordSound;
        audioSource = audioSources[0];
        attackSound = audioSources[1];
        //Bug rudimentär gefixed, keine ahnung was das problem eigentlich ist ¯\_(ツ)_/¯
        canvasSkilltree.SetActive(false);
        canvasSkilltreeisActive=false;
        /* canvasInventory.SetActive(false);
        canvasInventoryisActive=false; */
        //Array mit allen durch den Baum erhaltenden Skills(Ka ob jemals gebraucht, war für etwas anderes geplant)
        /*skillarray = new bool[] {elementarPfeil,elementarRegen,
                                            scharfschuss,elementarhieb,
                                            elementarwirbel,rage,elementarball,
                                            elementarflaeche,sturmkette};*/

        //InventoryShit
        inventory=new Inventory(UseItem);
        ui_Inventory.setInventory(inventory);
        ui_Inventory.setInventory(inventory);  
        //Standardwaffen erstellen
        
      
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemworld = collider.GetComponent<ItemWorld>();
        if(itemworld != null)
        {
            if(inventory.inhalt<inventory.capacity)
            {
                //Debug.Log("Item berührt");
                inventory.addItem(itemworld.getItem());
                //Debug.Log(inventory.getItemList().Count);
                 itemworld.DestroySelf();
            }
           
        }
    }
   

    void Update()
    {   
        //Movement sound
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            audioSource.volume = 0.04f;
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        } else
        {
            audioSource.Stop();
        }

        //Movement
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveY = Input.GetAxisRaw("Vertical"); 
        movement.x = moveX;
        movement.y = moveY;
        movement.Normalize();   //Normalizes the vector to have a magnitude of 1. Heißt im Klartext, unser Spieler läuft Diagonal genauso schnell wie horizontal / vertikal
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

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

        //Kampf shit
        if(EquipedWeapon!=null){
            if(EquipedWeapon.weaponType == Weapon.WeaponType.Melee)
        {
                
            nextAttackTime= Time.time + 1f / attackRate;
            meleeAttack=true;
        }
        else{
            meleeAttack=false;
        }
        }
        
        if(Time.time >= nextAttackTime)
        {
            if(EquipedWeapon!=null)
            {

                if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if(meleeAttack){
                        Attack(Direction.Up);
                    }else{
                        RangeAttack(Direction.Up);
                        Debug.Log("UP range attack");
                    }
                    nextAttackTime= Time.time + 1f / attackRate;
                    
                }
                if(Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if(meleeAttack){
                        Attack(Direction.Down);
                    }else{
                        RangeAttack(Direction.Down);
                    }
                    nextAttackTime= Time.time + 1f / attackRate;
                }
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if(meleeAttack){
                        Attack(Direction.Left);
                    }else{
                        RangeAttack(Direction.Left);
                    }
                    nextAttackTime= Time.time + 1f / attackRate;
                }
                if(Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if(meleeAttack){
                        Attack(Direction.Right);
                    }else{
                        RangeAttack(Direction.Right);
                    }
                    nextAttackTime= Time.time + 1f / attackRate;
                }
                    }      

        }
        //Debug Taste, alles möglich zum testen kann hier rein
        if(Input.GetKeyDown(KeyCode.X))
        {   
            
            Instantiate(WasserPfeil,  GameObject.FindGameObjectWithTag("Player").transform.position , Quaternion.identity);
        }
        //Skilltree aufrufen
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(canvasSkilltreeisActive){
                canvasSkilltree.SetActive(false);
                canvasSkilltreeisActive=false;

            }else{
                canvasSkilltree.SetActive(true);
                canvasSkilltreeisActive=true;
            }
        }   
        //Inventar aufrufen
        if(Input.GetKeyDown(KeyCode.I))
        {
            ui_Inventory.switchVisiblity();
            
        }

        //Skills nutzen
        //Ein Skill wird benutzt indem zuerst geschaut wird welcher Skill ausgerüstet ist und ob die MP dafür reichen
        //Wenn dies erfüllt ist, werden die MP dementsprechend verringert, und ein neues komplett leeres GameObject wird instanziiert.
        //Das leere GameObject wird an der gleichen Stelle erstellt, wo der Spieler sich aktuell befindet.
        //Der Plan ist, diesem leeren GameObject dann das jeweilige Skill-Skript hinzuzufügen. 
        //Das jeweilige Skill-Skript soll das komplette Behaviour vom Skill bestimmen (Lebenszeit, Hitbox, Schaden).
        //Dazu kann in der "private void Awake()" - Methode eine Referenz zum Spieler mithilfe von "Player.getInstance()" erstellt werden.
        //Mithilfe dieser Referenz können dann Spielerattribute (STR, DEX, INT) geholt werden, welche eventuell zur Schadensberechnung wichtig sind.

        //Notiz: Dieser Code ist nicht hübsch, sollte aber Performancetechnisch kein Problem darstellen.
        //Vermutlich ließe sich das hier besser lösen über ScriptableObjects, wenn man mehr Ahnung von Unity hätte, allerdings haben wir Zeitdruck und müssen fertig werden.
        //Ich bitte euch daher einfach diese suboptimale Lösung zu akzeptieren, weil sie funktioniert.
        if(Input.GetKeyDown(KeyCode.Q)){
            if (equippedAbility == Ability.Feuerpfeil && MP >= FeuerpfeilMPKost){
                MP -= FeuerpfeilMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                /* skill.AddComponent<Feuerpfeil>(); */

            } else if (equippedAbility == Ability.Wasserpfeilhagel && MP >= WasserpfeilhagelMPKost){
                MP -= WasserpfeilhagelMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<WasserPfeile>();

            } else if (equippedAbility == Ability.Scharfschuss && MP >= ScharfschussMPKost){
                MP -= ScharfschussMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                /* skill.AddComponent<Scharfschuss>(); */

            } else if (equippedAbility == Ability.Wasserhieb && MP >= WasserhiebMPKost){
                MP -=  WasserhiebMPKost;
                GameObject skill = Instantiate(WasserHieb, transform.position, transform.rotation);
                /* Start Slash Animation maybe? */

            } else if (equippedAbility == Ability.Elektrowirbel && MP >= ElektrowirbelMPKost){
                MP -= ElektrowirbelMPKost;
                GameObject skill = Instantiate(ElektroWirbel, transform.position, transform.rotation);
                /*Start Wirbel Animation maybe?*/

            } else if (equippedAbility == Ability.Rage && MP >= RageMPKost){
                MP -= RageMPKost;
                GameObject skill = Instantiate(Rage, transform.position, transform.rotation);
                /*Insert Rage Animation here, falls es so etwas jemals geben sollte lmao - Was eine gute Idee nur einen Artist zu haben*/

            } else if (equippedAbility == Ability.Feuerball && MP >= FeuerballMPKost){
                MP -= FeuerballMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                /* skill.AddComponent<Feuerball>(); */

            } else if (equippedAbility == Ability.Wasserflaeche && MP >= WasserflaecheMPKost){
                MP -= WasserflaecheMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<Wasserfläche>();

            } else if (equippedAbility == Ability.Sturmkette && MP >= SturmketteMPKost){
                MP -= SturmketteMPKost;
                GameObject skill = Instantiate(emptySkill, transform.position, transform.rotation);
                skill.AddComponent<Kettenblitz>();

            }
        }

        //Update UI Statusbar
        healthBar.setMaxValue(maxHealth);
        healthBar.setValue(currentHealth);

        manaBar.setMaxValue(MPMax);
        manaBar.setValue(MP);     
    }

    void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        position=rb.position;
    }

    void Attack(Direction direction)
    {
        attackSound.volume = 0.4f;
        attackSound.Play();
        if (direction == Direction.Left)
        {
            Debug.Log("Left");
            //animator.SetTrigger("Attack_Left");
            attackpoint=attackpointLeft;
        }
        if(direction == Direction.Right)
        {
            Debug.Log("right");
            //animator.SetTrigger("Attack_Right");
            attackpoint=attackpointRight;
        }
        if(direction == Direction.Up)
        {
            Debug.Log("up");
            //animator.SetTrigger("Attack_Up");
            attackpoint=attackpointUp;
        }
        if(direction == Direction.Down)
        {
            Debug.Log("down");
            //animator.SetTrigger("Attack_Down");
            attackpoint=attackpointDown;
        }
        //Animation
        animator.SetTrigger("Attack");
        //Attack enemies
        Collider2D [] hitEnemies= Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemyLayers);
        //dmg to enemies
        foreach(Collider2D enemy in hitEnemies)
        {
             
                Debug.Log("Hit"+enemy.name);
                enemy.GetComponent<fightable>().TakeDMG(atk);             
            
        }
    } 


        
    void RangeAttack(Direction direction)
    {
        if(EquipedWeapon != null & EquipedWeapon == BeginnerBogen)
        {
            if(direction == Direction.Left)
            {               
                Instantiate(ArrowPrefabLeft, attackpointLeft.position, attackpoint.rotation);
            }
            if(direction == Direction.Right)
            {
                Instantiate(ArrowPrefabRight, attackpointRight.position, attackpoint.rotation);
            }
            if(direction == Direction.Up)
            {
                Instantiate(ArrowPrefabUp, attackpointUp.position, attackpoint.rotation);
            }
            if(direction == Direction.Down)
            {
                Instantiate(ArrowPrefabDown, attackpointDown.position, attackpoint.rotation);
            }
        }

        if(EquipedWeapon != null & EquipedWeapon == JägerBogen)
        {
            if(direction == Direction.Left)
            {               
                Instantiate(JaegerArrowPrefabLeft, attackpointLeft.position, attackpoint.rotation);
            }
            if(direction == Direction.Right)
            {
                Instantiate(JaegerArrowPrefabRight, attackpointRight.position, attackpoint.rotation);
            }
            if(direction == Direction.Up)
            {
                Instantiate(JaegerArrowPrefabUp, attackpointUp.position, attackpoint.rotation);
            }
            if(direction == Direction.Down)
            {
                Instantiate(JaegerArrowPrefabDown, attackpointDown.position, attackpoint.rotation);
            }
        }       
                     
    }

    public void addExp(int xp)
    {
        exp+=xp;
        checkLevelup();
    }

    public void addHealth(int amount){
        if (currentHealth + amount > maxHealth){
            currentHealth = maxHealth;
        } else {
            currentHealth += amount;
        }
        
    }

    public void addMP(int amount){
        if (MP + amount > MPMax){
            MP = MPMax;
        } else {
            MP += amount;
        }
    }
    void checkLevelup()
    {
        int temp =0;
        foreach(int lv in levelStufen)  //Index(Level) wird gesucht, ab dem die exp kleiner sind als im Verzeichnis steht -> level wird zu den exp berechnet
        {
            if(lv < exp)
            {
                temp++;
            }
            else
            {
                break;
            }
        }
        if(temp>level) //ist das errechnete Level größer als das aktuelle liegt ein levelup vor
        {             
            //Debug.Log(temp-level);
            skillTree.SkillPoints+=(temp-level);
            skillTree.LevelupSkillpoints+=(temp-level);
            level=temp;
            UIcurrentLevel.text = level.ToString();
            //Werden mehrere Level auf einmal erreicht(was zu vermeiden ist) funzt das system trotzdem,
        }
    }

    private void UseItem(Item item)
    {
        switch(item.itemtype)
        {          
                            
           default:
           case Item.Itemtype.HealPotionNormal:   
                currentHealth+=10;
                    if(currentHealth>maxHealth) 
                    {
                        currentHealth=maxHealth;
                    }
                inventory.RemoveItem(new Item{itemtype = Item.Itemtype.HealPotionNormal, amount=1});     
           break;
           case Item.Itemtype.HealPotionGroß:
                currentHealth+=50;
                if(currentHealth>maxHealth) 
                {
                    currentHealth=maxHealth;
                }
                inventory.RemoveItem(new Item{itemtype = Item.Itemtype.HealPotionGroß, amount=1});   
           break;
           case Item.Itemtype.ManaTrank:   
                MP+=10;
                if(MP>MPMax) 
                {
                    MP=MPMax;
                }
                inventory.RemoveItem(new Item{itemtype = Item.Itemtype.ManaTrank, amount=1});   
           break;
           case Item.Itemtype.ManaTrankGroß:
                MP+=50;
                if(MP>MPMax) 
                {
                    MP=MPMax;
                }
                inventory.RemoveItem(new Item{itemtype = Item.Itemtype.ManaTrankGroß, amount=1});           
           break;
           case Item.Itemtype.MettBrot:   
                currentHealth+=20;
                if(currentHealth>maxHealth) 
                {
                    currentHealth=maxHealth;
                }
                inventory.RemoveItem(new Item{itemtype = Item.Itemtype.MettBrot, amount=1});     
           break;
           case Item.Itemtype.Blätterwasser:   
           //TODO
           break;
           case Item.Itemtype.LederRüstung:  
           EquipedArmor = LederRüstung;
           item.isequipped=true;    
            //TODO
           break;
           case Item.Itemtype.KettenRüstung:  
           EquipedArmor = KettenRüstung;
           item.isequipped=true;       
            //TODO
           break;
           case Item.Itemtype.PlattenstahlRüstung:
           EquipedArmor = PlattenstahlRüstung;
           item.isequipped=true;
            //TODO
           break;
           case Item.Itemtype.HolzSchild:   
           item.isequipped=true;
           EquipedShield = HolzSchild;
            //TODO
           break;
           case Item.Itemtype.EisenSchild: 
           item.isequipped=true;
           EquipedShield = EisenSchild;
            //TODO   
           break;
           case Item.Itemtype.StahlSchild:  
           item.isequipped=true; 
           EquipedShield = StahlSchild;
            //TODO          
           break;
           case Item.Itemtype.BeginnerBogen:   
            //TODO
            EquipedWeapon = BeginnerBogen;
            item.isequipped=true;
           break;
           case Item.Itemtype.JägerBogen:  
            //TODO  
            EquipedWeapon = JägerBogen;
            item.isequipped=true;
           break;
           case Item.Itemtype.Akolythenstab:   
           //TODO
           EquipedWeapon = Akolythenstab;
           item.isequipped=true;
           break;
           case Item.Itemtype.Elementarstab:  
           //TODO 
           EquipedWeapon =Elementarstab;
           item.isequipped=true;
           break;
           case Item.Itemtype.MeisterStab:    
           //TODO   
           EquipedWeapon = MeisterStab;
           item.isequipped=true;
           break;
           case Item.Itemtype.Schwert:   
           //TODO 
           EquipedWeapon = Schwert;
           item.isequipped=true;
           break;
           case Item.Itemtype.KampfAxt:   
           //TODO
           EquipedWeapon = KampfAxt;
           item.isequipped=true;
           break;
           case Item.Itemtype.StreitKolben:  
           //TODO 
           EquipedWeapon = StreitKolben;
           item.isequipped=true;
           break;

             

        }
    }
    
    void OnDrawGizmosSelected() //Schöner kreis im editor um den player herum, hat auf spiel keinen einfluss
    {
        if(attackpoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackpoint.position, attackrange);
    }

    public void pushBack(Vector2 enemyPosition)
    {
        var direction = (position - enemyPosition).normalized;
        rb.AddForce(direction * knockbackSpeed);
    }

}


public enum Ability {
    NoAbilityEquipped,  //<- Wichtig damit Unity nicht einfach das Enum auf Feuerpfeil setzt, ohne dass man es je gelernt hat... Stupid, I know, right??
    Feuerpfeil,
    Wasserpfeilhagel,
    Scharfschuss,
    Wasserhieb,
    Elektrowirbel,
    Rage,
    Feuerball,
    Wasserflaeche,
    Sturmkette
}