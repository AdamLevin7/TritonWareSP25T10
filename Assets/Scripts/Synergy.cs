using UnityEngine;
using System.Collections.Generic;

public class Synergy : MonoBehaviour
{
    private int redTowers;
    private int blueTowers;
    private int yellowTowers;
    private int greenTowers;

    // Update is called once per frame
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
    }
    void Update(){
        Debug.Log("red " + redTowers);
        Debug.Log("blue " + blueTowers);
        Debug.Log("yellow " + yellowTowers);
        Debug.Log("green " + greenTowers);
    }
}
