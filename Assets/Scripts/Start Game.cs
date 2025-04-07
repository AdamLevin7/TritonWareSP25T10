using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public int SceneToLoad;
    public void LoadScene(){
        SceneManager.LoadScene(SceneToLoad);
    }
}
