using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public string nextLevel;

    public void GoToLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
