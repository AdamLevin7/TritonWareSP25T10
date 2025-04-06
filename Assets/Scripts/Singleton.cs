using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    private void Awake()
    {
        CreateSingleton();
    }

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Duplicate singleton detected; deleting");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
