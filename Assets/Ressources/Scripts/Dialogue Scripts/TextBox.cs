using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TextBox{


    public TextBoxType typeOfBox;

    //In case of SentenceBox
    public string name;
    public string text;

    //In case of ChoiceBox
    public Choice[] choices;

    //Plays this Event once the TextBox is reached, IF an Event is specified
    public UnityEvent toExecute;

}

//Controls what kind of TextBox will be displayed:
//SentenceBox - A normal TextBox that shows a Name and some Text
//ChoiceBox   - A TextBox that displays several options as specified in the Choice-Class. With Numbers 1,2,3,...,N the user can choose an option.
//              The TextBox will then display the corresponding ReactionName, Reaction and play an Event if one is specified.
//HiddenBox   - An invisible TextBox that will keep the appearance of the previous TextBox, but play the specified Event.
public enum TextBoxType {
    SentenceBox,
    ChoiceBox,
    HiddenBox
}