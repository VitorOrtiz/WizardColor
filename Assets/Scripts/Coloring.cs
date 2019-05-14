using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coloring : MonoBehaviour
{
    public Image RedImage,BlueImage,Greenimage;
    private Image SelectedImage;
    Objcolor CurColor;
    float defSize;
    Player player;
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
            if(panel.HasItem)
            {
                if(panel.item.Cancolor)
                {
                    panel.item.ChangeColor(CurColor);
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.F2))
        {
            SelectColor(Objcolor.RED,RedImage);
        }
        else if(Input.GetKeyDown(KeyCode.F3)){
            
            SelectColor(Objcolor.GREEN,Greenimage);
        }
        else if(Input.GetKeyDown(KeyCode.F4)){
            
            SelectColor(Objcolor.BLUE,BlueImage);
        }
        else if(Input.GetKeyDown(KeyCode.F5)){
            SelectColor(Objcolor.NONE, null);
        }
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
