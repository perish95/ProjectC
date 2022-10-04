using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    //0 : Title, 1 : Loading. 2 : Home
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void GoToHomeScene() { Loading.LoadScene(2); }

    public void GoToStage() {
        //var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //gameManager.InitHealth(3);

        Loading.LoadScene(3); 
    }

    
}
