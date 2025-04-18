using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public void Awake(){
        slider = GetComponent<Slider>();
    }
    public void UpdateHealthBar(float curHealth, float maxHealth){
        slider.value = (curHealth/maxHealth);
    }
}
