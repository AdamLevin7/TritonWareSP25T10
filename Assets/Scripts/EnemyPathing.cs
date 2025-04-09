using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class EnemyPathing : MonoBehaviour
{
    //Enemy Specific References Paired by Index
    public GameObject EnemyPrefab;
    public List<GameObject> EnemyList = new List<GameObject>();
    public List<Vector3> VelocityList = new List<Vector3>();
    public List<int> CurrentNodeList = new List<int>();
    public List<int> EnemyHPList = new List<int>();
    public float EnemySpeed;
    //References for Creating Node Path
    public Transform NodeContainer;
    public List<Vector3> NodePositionList = new List<Vector3>();

    void UpdateNewVelocity(GameObject enemy,int idx)
    {
        Vector3 velocity = (NodePositionList[CurrentNodeList[idx] + 1] - NodePositionList[CurrentNodeList[idx]]).normalized;
        VelocityList[idx] = velocity;
    }
    void DummyAddMoney()
    {
        Debug.Log("Money added");
    }
    void EnemyTakeDamage(GameObject EnemyReference, int damageHPAmount)
    {
        int idx = EnemyList.IndexOf(EnemyReference);
        EnemyHPList[idx] -= damageHPAmount;
    }
    void CreateNewEnemy(int TotalHP)
    {
        GameObject newEnemy = Instantiate(EnemyPrefab, NodePositionList[0], Quaternion.identity);
        VelocityList.Add(new Vector3(0,0,0));
        CurrentNodeList.Add(0);
        EnemyHPList.Add(TotalHP);
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
        CreateNewEnemy(1);
    }

    // Update is called once per frame
    void Update()
    {
        for (int idx = EnemyList.Count - 1; idx >= 0; idx--)
        {
            //This check for deletion is done twice just as an edge case
            if (EnemyList[idx] == null)
            {
                EnemyList.RemoveAt(idx);
                VelocityList.RemoveAt(idx);
                CurrentNodeList.RemoveAt(idx);
                EnemyHPList.RemoveAt(idx);
                continue;
            }

            if(EnemyHPList[idx] <= 0)
            {
                DummyAddMoney();
                DestroyEnemy(idx);
            }

            if (EnemyList[idx] == null)
            {
                EnemyList.RemoveAt(idx);
                VelocityList.RemoveAt(idx);
                CurrentNodeList.RemoveAt(idx);
                EnemyHPList.RemoveAt(idx);
                continue;
            }

            int nodeIndex = CurrentNodeList[idx];

            if (nodeIndex + 1 < NodePositionList.Count)
            {
                UpdateNewVelocity(EnemyList[idx], idx);
                EnemyList[idx].transform.position += EnemySpeed * VelocityList[idx] * Time.deltaTime;

                float DistanceToNode = Vector3.Distance(EnemyList[idx].transform.position, NodePositionList[nodeIndex + 1]);
                float reachRadius = 1f;

                // changed this to look a little smoother
                if (DistanceToNode < 0.1f)
                {

                    EnemyList[idx].transform.position = NodePositionList[nodeIndex+1];
                    CurrentNodeList[idx]++;

                    Debug.Log(CurrentNodeList[idx]);
                    // new code! -aiden
                    // if hits last node, delete enemy and subtract life
                    if (CurrentNodeList[idx] >= NodePositionList.Count - 1)
                    {
                        // arbitrary number for now
                        GameManager.Instance.lives -= Mathf.Max(0,EnemyHPList[idx]);
                        DestroyEnemy(idx);
                    }
                }
            }
        }

        //Test of new enemy creation and or deletion
        if (Input.GetKeyUp(KeyCode.J))
        { 
            CreateNewEnemy(10);
        }
        //Test Damage and Health
        if (Input.GetKeyUp(KeyCode.K))
        { 
            EnemyTakeDamage(EnemyList[0], 5);
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            CreateNewEnemy(1);
        }

    }

    private void DestroyEnemy(int i)
    {
        Destroy(EnemyList[i]);
        EnemyList.RemoveAt(i);
        VelocityList.RemoveAt(i);
        CurrentNodeList.RemoveAt(i);
        EnemyHPList.RemoveAt(i);
    }
}
