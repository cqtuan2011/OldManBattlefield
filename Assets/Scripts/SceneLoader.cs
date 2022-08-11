using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private const string MENU_SCENE = "Menu";
    private const string LOADING_SCENE = "Loading";
    private const string GAME_SCENE = "Game";
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }
    
    public void LoadMenuScene(string scene = MENU_SCENE)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadGameScene(string scene = GAME_SCENE)
    {
        SceneManager.LoadScene(scene);
    }
}
