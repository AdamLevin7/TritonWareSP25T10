using UnityEngine;
using System.Collections.Generic;

public class EnemyPathing : MonoBehaviour
{
    //Enemy Specific References Paired by Index
    public GameObject EnemyPrefab;
    public List<GameObject> EnemyList = new List<GameObject>();
    public List<Vector3> VelocityList = new List<Vector3>();
    public List<int> CurrentNodeList = new List<int>();
    public float EnemySpeed;
    //References for Creating Node Path
    public Transform NodeContainer;
    public List<Vector3> NodePositionList = new List<Vector3>();
    
    void UpdateNewVelocity(GameObject enemy,int idx)
    {
        Vector3 velocity = (NodePositionList[CurrentNodeList[idx] + 1] - NodePositionList[CurrentNodeList[idx]]).normalized;
        VelocityList[idx] = velocity;
    }
    void CreateNewEnemy()
    {
        GameObject newEnemy = Instantiate(EnemyPrefab, NodePositionList[0], Quaternion.identity);
        VelocityList.Add(new Vector3(0,0,0));
        CurrentNodeList.Add(0);
        EnemyList.Add(newEnemy);
    }

    void Start()
    {
        // This should be done on load of the scene with the path
        foreach (Transform child in NodeContainer)
        {
            NodePositionList.Add(child.position);
        }

        // This should be done on event trigger for a new Enemy
        CreateNewEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        for (int idx = EnemyList.Count - 1; idx >= 0; idx--)
        {
            if (EnemyList[idx] == null)
            {
                EnemyList.RemoveAt(idx);
                VelocityList.RemoveAt(idx);
                CurrentNodeList.RemoveAt(idx);
                continue;
            }

            int nodeIndex = CurrentNodeList[idx];

            if (nodeIndex + 1 < NodePositionList.Count)
            {
                UpdateNewVelocity(EnemyList[idx], idx);
                EnemyList[idx].transform.position += EnemySpeed * VelocityList[idx] * Time.deltaTime;

                float DistanceToNode = Vector3.Distance(EnemyList[idx].transform.position, NodePositionList[nodeIndex + 1]);
                float reachRadius = 1f;

                if (DistanceToNode < reachRadius)
                {
                    EnemyList[idx].transform.position = NodePositionList[nodeIndex+1];
                    CurrentNodeList[idx]++;
                }
            }
        }

        //Test of new enemy creation and or deletion
        if (Input.GetKeyUp(KeyCode.J))
        { 
            CreateNewEnemy();
        }
    }
}
