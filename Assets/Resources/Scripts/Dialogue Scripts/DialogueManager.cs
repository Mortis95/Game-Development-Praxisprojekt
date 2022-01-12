using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{

    public Queue<TextBox> textBoxes;
    public KeyCode interactionKey;
    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    //Wieviel Frames pro Textbox gewartet werden müssen bevor man skippen kann
    public int dialogueDelay;
    public bool activeDialogue;
    public TextBoxType currentBoxType;
    private TextBox currentBox;
    private int maxAcceptableAnswer;

    //Singleton Pattern
    private static DialogueManager instance;

    private void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Eine Queue nutzen ist praktisch für Konversationen, da FIFO.
        textBoxes = new Queue<TextBox>();
    }

    //Etwas crappy, dass ich das hier rufe. Besser wäre wenn das interactable Script sofort den Interact-Button mitgibt, sodass wir keine 2 seperaten InteractKeys haben maybe? Not sure.
    void Update(){

        if (currentBoxType == TextBoxType.SentenceBox || currentBoxType == TextBoxType.HiddenBox){
            if (dialogueDelay <= 0 && Input.GetKeyDown(interactionKey)){
                DisplayNextBox();
            }
        }

        //Dialogue Options
        //Check if any number from 1-5 is pressed
        if (currentBoxType == TextBoxType.ChoiceBox){
            //Set input manually to 0, if it stays at 0 no Key was pressed
            int input = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                input = 1;
            } else if (Input.GetKeyDown(KeyCode.Alpha2)){
                input = 2;
            } else if (Input.GetKeyDown(KeyCode.Alpha3)){
                input = 3;
            } else if (Input.GetKeyDown(KeyCode.Alpha4)){
                input = 4;
            } else if (Input.GetKeyDown(KeyCode.Alpha5)){
                input = 5;
            }

            //Check if a choice was made and also if the current TextBox even allows this choice.
            //If yes, display the corresponding selection
            if (input != 0 && input <= maxAcceptableAnswer){
                Choice answer = currentBox.choices[input-1];
                nameText.text = answer.reactingName;

                //Type out the corresponding reaction
                StopAllCoroutines();
                StartCoroutine(TypeSentence(answer.reaction));

                //If the Choice has a specified ReactionEvent, play that Event
                if(answer.reactionEvent != null){
                    answer.reactionEvent.Invoke();
                }

                //Set currentBoxType manually to sentenceBox, so the user can once again press interactionKey to progress Dialogue
                currentBoxType = TextBoxType.SentenceBox;
            }
    }
        
        
        if (dialogueDelay > 0){
            dialogueDelay -= 1;
        }
    }

    public static DialogueManager getInstance(){
        return instance;
    }

    //Start Dialogue Methode, wird aus anderem Skript gerufen
    public void StartDialogue(Dialogue dialogue){
        animator.SetBool("IsVisible", true);
        activeDialogue = true;
        textBoxes.Clear();

        foreach (TextBox t in dialogue.textBoxen){
            Debug.Log("Enqueing TextBox of type: " + t.typeOfBox);
            textBoxes.Enqueue(t);
        }
        DisplayNextBox();
    }
    
    public void DisplayNextBox(){
        if (textBoxes.Count == 0){
            EndDialogue();
            return;
        }
        //60 frames of delay
        dialogueDelay = 60;

        //Get next Box and assign acknowledge it as current Box
        TextBox box = textBoxes.Dequeue();
        currentBoxType = box.typeOfBox;
        currentBox = box;

        //Je nach verschiedener Box muss was anderes gemacht werden
        switch (currentBoxType){
            //Bei einer SentenceBox kann die nächste Box einfach getyped werden
            case TextBoxType.SentenceBox:
                nameText.text = box.name;
                StopAllCoroutines();
                StartCoroutine(TypeSentence(box.text));
                break;
            //Bei einer ChoiceBox muss man zuerst die Option angeben, und festlegen was die höchste erlaubte Antwort ist.
            //In der Update Methode wird im Fall dass currentBoxType == ChoiceBox abgefragt, ob die Tasten 1,2,3,4...len gedrückt werden
            //Wenn eine zulässige Auswahl getroffen wurde, dann wird die zugehörige Antwort angezeigt und der currentBoxType auf SentenceBox gesetzt, damit man die nächste Box anzeigen kann
            case TextBoxType.ChoiceBox:
                string options = "";
                int len = box.choices.Length;
                for (int i = 0; i < len; i++){
                    options += (i+1) + " : " + box.choices[i].option + "\n";
                }
                maxAcceptableAnswer = len;
                StopAllCoroutines();
                StartCoroutine(TypeSentence(options));
                break;
            
        }

        //If this Box has an Event, call the Event
        if (box.toExecute != null){
            box.toExecute.Invoke();
        }
        
    }

    //Render each letter by itself and not all at once
    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()){
            //Debug.Log("Code is executing here");
            AudioManager.getInstance().Play("NPCDialog");
            dialogueText.text += letter;
            //Wait for waitTime many seconds
            float waitTime = 0.02f;
            yield return new WaitForSeconds(waitTime);
        }
        //After Text has finished rendering
        AudioManager.getInstance().Stop("NPCDialog");
        
    }

    //End Dialogue and make sure the animator gets the notice
    void EndDialogue(){
        activeDialogue = false;
        animator.SetBool("IsVisible", false);
        Debug.Log("End of Dialogue");
    }

}
