using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPanel : MonoBehaviour
{
    public bool Wall;
    public bool HasItem;
    public bool HasPlayer;
    public bool HasPressure =false;
    public GridObj item;
    public Vector2 position;
    public float size;
    public Material highlightMat;
    public Material defaultMat;
    private int ColorChoice = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetItem(GridObj Item)
    {
        if(Item.objtype !=ObjType.PRESSURE)
        HasItem = true;
        if(Item.objtype == ObjType.WALL)
        Wall = true;
        item = Item;
    }
    public void RemoveItem(){
        HasItem = false;
        item = null;
    }
    public void ChangeColor()
    {
        MeshRenderer CurColor = GetComponent<MeshRenderer>();
        if (ColorChoice == 0)
        {
            CurColor.material = highlightMat;
            ColorChoice = 1;
            HasPlayer = true;
        }
        else
        {
            CurColor.material = defaultMat;
            ColorChoice = 0;
            HasPlayer = false;
        }

    }
}
