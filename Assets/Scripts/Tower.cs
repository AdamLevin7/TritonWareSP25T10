using System;
using UnityEngine;

[Serializable]
public class Tower {
    public string name;
    public int cost;
    public GameObject prefab;
    public Tower (string name, int cost, GameObject prefab){
        this.name = name;
        this.cost = cost;
        this.prefab = prefab;

    }
}