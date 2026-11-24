using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRestart : MonoBehaviour
{
    private bool isRestarting = false;

    public void RestartLevel()
    {
        if (isRestarting)
        {
            return ;
        }
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex < 0)
        {
            Debug.LogError("无法重开: 请将当前场景加入到Scenes In Build. ");
            return ;
        }
        isRestarting = true;
        SceneManager.LoadScene(sceneIndex);

    }
}
