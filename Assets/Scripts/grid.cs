using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{
    #region GridInfo
    public int Rows;
    public int Columns;
    public float panelSize;
    public GameObject Wall;
    public GridPanel panel;

    public GameObject newLevel;
    public GridPanel[,] panellist; 
#endregion

#region GridObjects
public GameObject obj;
public GameObject Reflector;
public GameObject Glass;
public GameObject Target;
public static grid thisgrid;

public List<GridObj> targets = new List<GridObj>();
#endregion
    void Awake()
    {
        thisgrid = this;
    }
    void Start()
    { 
       
        newLevel = new GameObject("GridLevel");

        CreatePlaneArea();
    }
    void CreatePlaneArea()
    {
        panellist = new GridPanel[Rows, Columns];
        for (int i = 0; i < Columns; i++) {
            for (int x = 0; x < Rows; x++)
            {
                float Xpos = x - Rows / 2;
                float Zpos = i - Columns / 2;
                Vector3 Position = new Vector3(Xpos,0,Zpos);
                
                GridPanel newpanel =Instantiate(panel, Position, Quaternion.Euler(90, 0, 0),newLevel.transform).GetComponent<GridPanel>();
                newpanel.size = panelSize;
                newpanel.position = new Vector2(x,i);
                panellist[x,i] = newpanel;
            }
        }
        SpawnObj(Reflector, Rows-1, (int)Columns/2);
        SpawnObj(Glass, 0, (int)Columns/2);
        SpawnObj(Target,(int)Rows/2 - 1,Columns -1,Objcolor.RED);
        SpawnObj(Target,(int)Rows/2,Columns -1,Objcolor.GREEN);
        SpawnObj(Target,(int)Rows/2 +1,Columns -1,Objcolor.BLUE);
    }
    public void ResetTargets()
    {
        foreach(GridObj obj in targets)
        {
            obj.LightDown();
        }
    }
    void SpawnObj(GameObject obj,int row, int col, Objcolor color = Objcolor.NONE)
    {
        Vector3 newobjpos= panellist[row,col].transform.position;
        newobjpos.y +=.5f;
        GridObj newobj = Instantiate(obj, newobjpos, Quaternion.identity).GetComponent<GridObj>();
        if(newobj.objtype == ObjType.TARGET)
        {
        targets.Add(newobj);
        if(color != Objcolor.NONE)
        newobj.ChangeColor(color);
        }
        newobj.GridPosition = new Vector2(row,col); 
        panellist[row,col].SetItem(newobj);
    }
    void CreateWalls()
    {
        
    }
    public GridPanel getNextPanel(int Row, int Col, Direction dir)
    {
        switch(dir)
        {
            case Direction.FORWARD:
            Col++;
            break;
            
            case Direction.BACK:
            Col--;
            break;
            
            case Direction.LEFT:
            Row--;
            break;
            
            case Direction.RIGHT:
            Row++;
            break;
        }
        if(Row < 0|| Col <0|| Row >= Rows|| Col >=Columns)
        return null;
        else
        return panellist[Row,Col];
        
    }
    
    Direction MoveDirection(Vector3 Dir)
    {
        if(Dir == Vector3.forward)
        return Direction.FORWARD;

        else if(Dir == Vector3.back)
        return Direction.BACK;

        else if(Dir == Vector3.left)
        return Direction.LEFT;

        else if(Dir == Vector3.right)
        return Direction.RIGHT;

        else 
        return Direction.INVALID; 

    }
}
public enum Direction{
     
        FORWARD,
        BACK,
        LEFT,
        RIGHT,
        INVALID
    }

