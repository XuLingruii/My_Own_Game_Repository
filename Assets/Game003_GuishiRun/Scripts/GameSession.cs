using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public int DeathCount { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 登记当前组件，并让它所在的物体在场景重载时继续存在。
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void RecordDeath()
    {
        DeathCount++;
    }
}
