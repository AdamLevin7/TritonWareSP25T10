using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum SynergyType{
    Red,
    Blue,
    Green
}

public class Synergy : MonoBehaviour
{
    public static Synergy Instance;
    private int redTowers;
    private int blueTowers;
    private int greenTowers;
    private int totalTowers;
    public bool rbSynergy;
    public bool rgSynergy;
    public bool bgSynergy;
    public bool totalSynergy;
    //public Image rbSyn;
    //public Image rgSyn;
    //public Image bgSyn;
    public Image redSyn;
    public Image greenSyn;
    public Image blueSyn;
    //public Image totalSyn;

    public float globalDamageScaleFactor;
    public float globalRangeScaleFactor;
    public float globalFirerateScaleFactor;
    public float globalMoneyScaleFactor;

    // Update is called once per frame
    private void Awake(){
        
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        /*
        rbSyn.enabled = false;
        rgSyn.enabled = false;
        bgSyn.enabled = false;
        totalSyn.enabled = false;
        */
    }

    public void UpdateTowerSynergy(SynergyType type, int towersAdded)
    {
        switch (type)
        {
            case SynergyType.Red:
                redTowers += towersAdded;
                totalTowers++;
                break;
            case SynergyType.Green:
                greenTowers += towersAdded;
                totalTowers++;
                break;
            case SynergyType.Blue:
                blueTowers += towersAdded;
                totalTowers++;
                break;
            default:
                Debug.LogError("Tower has an invalid synergy type: " + type);
                break;
        }
        CalculateSynergies();
    }

    public void CalculateSynergies(){
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

        OrderResonanceBar();
    }

    public void OrderResonanceBar(){
        if(redTowers >= greenTowers && greenTowers >= blueTowers && redTowers >= blueTowers){
            redSyn.transform.SetSiblingIndex(0);
            greenSyn.transform.SetSiblingIndex(1);
            blueSyn.transform.SetSiblingIndex(2);
        }
        else if(redTowers >= blueTowers && blueTowers >= greenTowers && redTowers >= greenTowers){
            redSyn.transform.SetSiblingIndex(0);
            greenSyn.transform.SetSiblingIndex(2);
            blueSyn.transform.SetSiblingIndex(1);
        }
        else if(greenTowers >= redTowers && redTowers >= blueTowers && greenTowers >= blueTowers){
            redSyn.transform.SetSiblingIndex(1);
            greenSyn.transform.SetSiblingIndex(0);
            blueSyn.transform.SetSiblingIndex(2);
        }
        else if(greenTowers >= blueTowers && blueTowers >= redTowers && greenTowers >= redTowers){
            redSyn.transform.SetSiblingIndex(2);
            greenSyn.transform.SetSiblingIndex(0);
            blueSyn.transform.SetSiblingIndex(1);
        }
        else if(blueTowers >= redTowers && redTowers >= greenTowers && blueTowers >= greenTowers){
            redSyn.transform.SetSiblingIndex(1);
            greenSyn.transform.SetSiblingIndex(2);
            blueSyn.transform.SetSiblingIndex(0);
        }
        else if(blueTowers >= greenTowers && greenTowers >= redTowers && blueTowers >= redTowers){
            redSyn.transform.SetSiblingIndex(2);
            greenSyn.transform.SetSiblingIndex(1);
            blueSyn.transform.SetSiblingIndex(0);
        }
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
