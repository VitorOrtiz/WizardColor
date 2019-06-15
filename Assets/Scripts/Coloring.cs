using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coloring : MonoBehaviour
{
    public Image RedImage,BlueImage,Greenimage;
    private Image SelectedImage;
    public Objcolor CurColor;
    float defSize;
    public bool CurColoring;
    Player player;
    #region  singleton
    public static Coloring coloring;
    void Awake(){
        coloring = this;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        player = Player.player;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Vector2Int gridpos =new Vector2Int((int) player.CurPanel.position.x, (int)player.CurPanel.position.y);
            GridPanel panel = grid.thisgrid.getNextPanel(gridpos.x,gridpos.y,player.playerDirection);
            if(panel != null && panel.HasItem)
            {
                if(panel.item.Cancolor)
                {
                    panel.item.ChangeColor(CurColor);
                    if(panel.item.objtype == ObjType.GLASS)
                        Player.player.playerStaff.RedoBeam();
                    CurColoring =true;
                    Invoke("Paint",.2f);
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectColor(Objcolor.RED,RedImage);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            
            SelectColor(Objcolor.GREEN,Greenimage);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)){
            
            SelectColor(Objcolor.BLUE,BlueImage);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4)){
            SelectColor(Objcolor.NONE, null);
        }
    }
    void Paint()
    {
        CurColoring = false;
    }
    void SelectColor(Objcolor color, Image SelectedImage)
    {
        CurColor = color;
        if(this.SelectedImage != null)
        this.SelectedImage.transform.localScale =new Vector3(1,1,1);
        if(SelectedImage != null){
        this.SelectedImage = SelectedImage;
        this.SelectedImage.transform.localScale = new Vector3(2,2,1);
        }
    }
}
