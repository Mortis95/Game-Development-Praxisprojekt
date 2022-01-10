using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Slider slider;

    public void setMaxValue(int value){
        slider.maxValue = value;
        if (slider.value > value){
            slider.value = value;
        }
    }

    public void setValue(int value){
        slider.value = value;
    }
}
