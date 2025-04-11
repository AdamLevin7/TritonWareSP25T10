using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Synergy : MonoBehaviour
{
    private int redTowers;
    private int blueTowers;
    private int yellowTowers;
    private int greenTowers;
    private bool rbSynergy;
    private bool rgSynergy;
    private bool bgSynergy;
    private bool totalSynergy;
    [SerializeField] public Image rbSyn;
    [SerializeField] public Image rgSyn;
    [SerializeField] public Image bgSyn;
    [SerializeField] public Image totalSyn;

    // Update is called once per frame
    private void Awake(){
        
        rbSyn.enabled = false;
        rgSyn.enabled = false;
        bgSyn.enabled = false;
        totalSyn.enabled = false;
        
    }
    public void UpdateTowerSynergy(string type)
    {
        if(type == "Red"){
            redTowers++;
        }
        if(type == "Blue"){
            blueTowers++;
        }
        if(type == "Yellow"){
            yellowTowers++;
        }
        if(type == "Green"){
            greenTowers++;
        }
        CalculateSynergies();
    }
    void CalculateSynergies(){
        if(0.9*redTowers < blueTowers && 1.1*redTowers>blueTowers){
            rbSynergy = true;
            rbSyn.enabled = true;
        }
        else{
            rbSynergy = false;
            rbSyn.enabled = false;
        }
        
        if(0.9*redTowers < greenTowers && 1.1*redTowers>greenTowers){
            rgSynergy = true;
            rgSyn.enabled = true;
        }
        else{
            rgSynergy = false;
            rgSyn.enabled = false;
        }

        if(0.9*blueTowers < greenTowers && 1.1*greenTowers>greenTowers){
            bgSynergy = true;
            bgSyn.enabled = true;
        }
        else{
            bgSynergy = false;
            bgSyn.enabled = false;
        }

        if(rbSynergy && rgSynergy){
            totalSynergy = true;
            totalSyn.enabled = true;
        }
        else{
            totalSynergy = false; 
            totalSyn.enabled = false;
        }
    }
    void Update(){
        /*
        Debug.Log("rb " + rbSynergy + " r " + redTowers);
        Debug.Log("rg " + rgSynergy + " b " + blueTowers);
        Debug.Log("bg " + bgSynergy + " g " + greenTowers);
        Debug.Log("total " + totalSynergy);
        */
    } 
}
