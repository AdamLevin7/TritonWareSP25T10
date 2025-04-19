using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class StartGame : MonoBehaviour
{
    public string SceneToLoad;
    public void LoadScene(){
        SceneManager.LoadScene(SceneToLoad);
    }
}
