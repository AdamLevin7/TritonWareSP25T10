using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Synergy : MonoBehaviour
{
    private int redTowers;
    private int blueTowers;
    private int greenTowers;
    private int totalTowers;
    private bool rbSynergy;
    private bool rgSynergy;
    private bool bgSynergy;
    private bool totalSynergy;
    //public Image rbSyn;
    //public Image rgSyn;
    //public Image bgSyn;
    public Image redSyn;
    public Image greenSyn;
    public Image blueSyn;
    //public Image totalSyn;

    // Update is called once per frame
    /*private void Awake(){
        
        rbSyn.enabled = false;
        rgSyn.enabled = false;
        bgSyn.enabled = false;
        totalSyn.enabled = false;
    } */
    public void UpdateTowerSynergy(string type, int towersAdded)
    {
        if(type == "Red"){
            redTowers += towersAdded;
            totalTowers++;
        }
        if(type == "Blue"){
            blueTowers += towersAdded;
            totalTowers++;
        }
        if(type == "Green"){
            greenTowers += towersAdded;
            totalTowers++;
        }
        CalculateSynergies();
    }
    public void CalculateSynergies(){
        /*if(0.9*redTowers < blueTowers && 1.1*redTowers>blueTowers){
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
        */
        if(0.9*redTowers < blueTowers && 1.1*redTowers>blueTowers){
            rbSynergy = true;
        }
        else{
            rbSynergy = false;
        }
        
        if(0.9*redTowers < greenTowers && 1.1*redTowers>greenTowers){
            rgSynergy = true;
        }
        else{
            rgSynergy = false;
        }

        if(0.9*blueTowers < greenTowers && 1.1*blueTowers>greenTowers){
            bgSynergy = true;
        }
        else{
            bgSynergy = false;
        }

        if(rbSynergy && rgSynergy){
            totalSynergy = true;
        }
        else{
            totalSynergy = false; 
        }
        redSyn.GetComponent<LayoutElement>().flexibleWidth = (float)redTowers/totalTowers;
        greenSyn.GetComponent<LayoutElement>().flexibleWidth = (float)greenTowers/totalTowers;
        blueSyn.GetComponent<LayoutElement>().flexibleWidth = (float)blueTowers/totalTowers;
    }
   /*
    void Update(){
        
        Debug.Log("rb " + rbSynergy + " r " + redTowers);
        Debug.Log("rg " + rgSynergy + " b " + blueTowers);
        Debug.Log("bg " + bgSynergy + " g " + greenTowers);
        Debug.Log("total " + totalSynergy + " t " + totalTowers);
        
    } 
    */
}
