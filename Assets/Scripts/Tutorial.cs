using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    public int Id;
    public GameObject[] objects;
    public Sprite[] Images;
    public Image TutImage;
    public Text TutText;
    public CanvasGroup TextAlpha;
    private LanguageOptions language;
    public NewTutorial tut;
    // Start is called before the first frame update
    void Start()
    {
        language = GameController.controller.language;
        //SetText(0);
        StartTutorial(Id);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.controller.WinStatus)
        Destroy(gameObject);
    }
    public void StartTutorial(int id)
    {
        switch(id)
        {
            case 1:
            StartCoroutine(Tutorial1());
            break;
            case 2:
            StartCoroutine(Tutorial2());
            break;
        }
    }
    string GetText(int i)
    {
  
        switch(language)
        {
            case LanguageOptions.Português:
            return tut.pt_BRText[i];
            
            case LanguageOptions.English:
            return tut.en_USText[i];
        }
        return "";
    }
    IEnumerator Tutorial2()
    {
        
        //Parte 1 Piso de pressao
        TutText.text =GetText(0);//isto é um piso de pressão
        TutImage.sprite = Images[0];
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        yield return new WaitForSeconds(4);
         StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutImage.gameObject.SetActive(false);
        TutText.text = GetText(1); //ele ativa e desativa objetos karaio
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        yield return new WaitForSeconds(4);
         StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        
        TutText.text = GetText(2);//pise nele.
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        bool Steppedon = false;
        while(!Steppedon)
        {
            if(grid.thisgrid.LevelPressure.active)
            Steppedon = true;
            yield return null;
        }
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutText.text = GetText(3);//enquanto você ou um objeto estiver em cima, ele ficará ativo.
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        
        yield return new WaitForSeconds(4);

        //2 Refletor
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutImage.sprite = Images[1];
        TutImage.gameObject.SetActive(true);
        TutText.text = GetText(4);//este é um refletor.
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        yield return new WaitForSeconds(4);

        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutImage.sprite = Images[2];
        TutImage.rectTransform.sizeDelta= new Vector2(200,200);
        TutText.text = GetText(5); //ele muda a direção do raio.
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        
        yield return new WaitForSeconds(4);
        
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutImage.sprite = Images[3];
        TutImage.rectTransform.sizeDelta= new Vector2(100,100);
        TutText.text = GetText(6); //ele muda a direção do raio.
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        yield return new WaitForSeconds(3);
          StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
         
        TutImage.sprite = Images[4];
        TutImage.rectTransform.sizeDelta= new Vector2(100,100);
        TutText.text = GetText(7); //ele muda a direção do raio.
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
         yield return new WaitForSeconds(4);
        
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
    }
    IEnumerator Tutorial1()
    {
        TutText.text =GetText(0);
        TutImage.gameObject.SetActive(false);
        objects[0].SetActive(true);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        float timepassed = 0;
        bool ArrowsInput = false;
        while(!ArrowsInput)
        {
            timepassed+= Time.deltaTime;
            if(Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.DownArrow)){
                ArrowsInput = true;
                
            }
            yield return null;
        }
        
        yield return new WaitForSeconds((timepassed > 3?0:3-timepassed));

        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutImage.gameObject.SetActive(true);
        objects[0].SetActive(false);
        TutText.text = GetText(1);
        TutImage.sprite = Images[0];
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        bool Colored = false;
        while(!Colored)
        {
            
            if(Coloring.coloring.CurColoring){
                Colored = true;
            }
            yield return null;
        }
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutText.text = GetText(2);
        TutImage.sprite = Images[1];
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        bool DroppedStaff =false;
        while(!DroppedStaff)
        {
            if(Player.player.playerStaff.InGrid)
            DroppedStaff = true;
            yield return null;
        }
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutText.text = GetText(3);
        TutImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(.1f);
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        yield return new WaitForSeconds(2);
         StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutText.text = GetText(4);

        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        bool Holding = false;
        while(!Holding)
        {
            if(Player.player.state == Player.MoveState.Attatched)
            Holding = true;
            yield return null;
        }
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutText.text = GetText(5);

        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        bool Rotated = false;
        while(!Rotated)
        {
            if(Player.player.Att.Attatched && (Input.GetKeyDown(KeyCode.Q)|| Input.GetKeyDown(KeyCode.E)))
            Rotated = true;
            yield return null;
        }
        
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutText.text = GetText(6);//Este é o alvo.
        TutImage.gameObject.SetActive(true);
        TutImage.sprite = Images[2];
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        yield return new WaitForSeconds(2f);
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutText.text = GetText(7);//Seu objetivo é ativa-lo, acertando-o com o raio de sua cor.
        TutImage.sprite = Images[3];
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        yield return new WaitForSeconds(3f);
        
        StartCoroutine(Fade(TextAlpha,1,0,.1f));
        yield return new WaitForSeconds(.4f);
        TutText.text = GetText(8);//Posicione o cajado alinhado com o vidro e o alvo.
        RectTransform imgrect= TutImage.rectTransform;
        float def = imgrect.rect.width;
        imgrect.sizeDelta =new Vector2(360,100);
        TutImage.sprite = Images[4];
        StartCoroutine(Fade(TextAlpha,0,1,.1f));
        bool ActivatedTarget = false;
        while(!ActivatedTarget)
        {
            if(GameController.controller.WinStatus)
            ActivatedTarget = true;
            yield return null;
        }

        StartCoroutine(Fade(TextAlpha,1,0,.1f));
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
}

[System.Serializable]
public class NewTutorial{
    public Text[] TextObjects;
    public string[] pt_BRText;
    public string[] en_USText;
    
}