using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public int levelID;
    public bool WinStatus;
    public LanguageOptions language;
    #region singleton
    public static GameController controller;
    void Awake()
    {
        controller = this;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Win(){
        WinStatus = true;
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Escape))
        Application.Quit();   
    }
  
}
    public enum LanguageOptions
    {
        Português,
        English
    }