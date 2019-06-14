using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialObj : MonoBehaviour
{
    public Color DefaultColor;
    public Color HighLightColor;
    public Image ObjImage;
    public KeyCode keyActivation;
    // Start is called before the first frame update
    void Start()
    {
        DefaultColor = ObjImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyActivation))
        {
            ObjImage.color = HighLightColor;
        }
        else if(Input.GetKeyUp(keyActivation))
        {
            ObjImage.color = DefaultColor;
        }
        
    }
    
}
