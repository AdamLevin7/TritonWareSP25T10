using UnityEngine;
using System.Collections.Generic;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public List<GameObject> EnemyList = new List<GameObject>();
    void Start()
    {
        GameObject newEnemy = Instantiate(EnemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        EnemyList.Add(newEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        { 
            for (int idx = 0; idx < EnemyList.Count; idx++)
            {
                EnemyList[idx].transform.position = new Vector3(5, 0, 0);
            }
        }
    }
}
