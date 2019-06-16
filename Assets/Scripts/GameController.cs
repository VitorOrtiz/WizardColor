using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public CanvasGroup FadeScreen;
    public int levelID;
    public int Limit = 4;
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
        grid.thisgrid.CreateLevel(levelID);  
        
    }
    public void Win(){
        WinStatus = true;
        levelID ++;
        if(levelID <= Limit)
        StartCoroutine(SetLevel(levelID));
    }
    public IEnumerator SetLevel(int level)
    {
         yield return new WaitForSeconds(.6f);
        newFade(0,1,.06f);
        yield return new WaitForSeconds(1.2f);
        
        Player.player.ResetPlayer();
        WinStatus = false;
        grid.thisgrid.CreateLevel(level);
        grid.thisgrid.SetDefaultPos();
        yield return new WaitForSeconds(.4f);
        newFade(1,0,.06f);
    }
    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Escape))
        Application.Quit();   
    }
    
     public static IEnumerator Fade(CanvasGroup Alpha, float StartAlpha,float EndAlpha, float time)
    {
        float i = StartAlpha;
        
        float end = EndAlpha;
        while(Alpha.alpha != EndAlpha)
        {
            i = Mathf.Lerp(i, EndAlpha,time);
            if(Mathf.Abs(i - EndAlpha) < .05f)
                i = EndAlpha;
            Alpha.alpha = i;
            yield return null;
        }
        Alpha.alpha = EndAlpha;

    }
    public void newFade(int Start, int End, float time)
    {
        StartCoroutine(Fade(FadeScreen,Start,End,time));
    }
}
    public enum LanguageOptions
    {
        Português,
        English
    }