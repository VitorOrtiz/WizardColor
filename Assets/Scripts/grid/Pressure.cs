using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressure : GridObj
{
    // Start is called before the first frame update
    public string LevelID;
    
    public List<PressureObj> Objects = new List<PressureObj>();
    

    public bool active;
    public Vector2Int ObjPos;
    public GridPanel curpanel;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(curpanel.HasItem || curpanel.HasPlayer)
        {
            Fase(true);
        }
        else{
            Fase(false);    
        }
    }
    public void Fase(bool act)
    {
       foreach(PressureObj obj in Objects)
       obj.ActivateObj(act);
       if((active && !act)||(!active && act))
       {
       active = act;
       Player.player.playerStaff.RedoBeam();
       }
       
       
    }
    public void AddItem(PressureObj obj)
    {
        Objects.Add(obj);
       
    }
}
[System.Serializable]
public class PressureObj{
    public GridObj obj;
    public int Row;
    public int Col;
    public ObjType objtype;
    public bool DefActive;
    private bool CurActive;
    public PressureObj(GridObj obj, int Row, int Col, bool DefActive)
    {
        
        this.obj = obj;
        this.Row = Row;
        this.Col = Col;
        objtype = obj.objtype;
        this.DefActive = DefActive;

    }
    public void ActivateObj(bool activeset)
    {
        if(CurActive ==activeset)
        return;
        bool active = activeset? !DefActive:DefActive;
        obj.gameObject.SetActive(active);
        
        grid.thisgrid.getPanel(Row,Col).HasItem = active;
        grid.thisgrid.getPanel(Row,Col).Wall = active;
        CurActive = activeset;

    }
}
