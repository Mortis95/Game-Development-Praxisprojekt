using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Choice
{
    public string option;
    public string reactingName;
    public string reaction;
    public UnityEvent reactionEvent;
}
