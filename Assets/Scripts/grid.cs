using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{
    #region GridInfo
    public int Rows;
    public int Columns;
    public float panelSize;

    public GameObject Limit;
    public GridPanel panel;

    public GameObject newLevel;
    public GridPanel[,] panellist; 
#endregion

#region GridObjects
public GameObject Wall;
public GameObject obj;
public GameObject Reflector;
public GameObject Glass;
public GameObject Target;
public GameObject Double;
public static grid thisgrid;

public List<GridObj> targets = new List<GridObj>();
#endregion
    void Awake()
    {
        thisgrid = this;
    }
    void Start()
    { 
       
      
        Fase1();
    }
    void CreatePlaneArea(int Rows,int Columns)
    {
        this.Rows = Rows;
        this.Columns = Columns;
        newLevel = new GameObject("GridLevel");
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
        createLimits(new Vector3(1,0,0));
        createLimits(new Vector3(-1,0,0));
        createLimits(new Vector3(0,0,1));
        createLimits(new Vector3(0,0,-1));
    }
    void createLimits(Vector3 Direction)
    {
        Vector3 pos = newLevel.transform.position;
        pos+= new Vector3(Direction.x *(Rows/2) +Direction.x,pos.y,Direction.z *(Columns/2) + Direction.z);
        if(Direction.x != 0)
        {
            GameObject limit = Instantiate(Limit,pos,Quaternion.identity);
            limit.transform.localScale = new Vector3(1,limit.transform.localScale.y,Columns);
        } 
        else if(Direction.z !=0)
        {
            GameObject limit = Instantiate(Limit,pos,Quaternion.identity);
            limit.transform.localScale = new Vector3(Rows,limit.transform.localScale.y,1);
        }
        

    }
    public void ResetTargets()
    {
        foreach(GridObj obj in targets)
        {
            obj.LightDown();
        }
    }
    void SpawnObj(GameObject obj,int row, int col,bool CanDrag, Objcolor color = Objcolor.NONE)
    {
        if(panellist[row,col].Wall)
        {
            Debug.Log("Panel has Wall in: " + row +" " + col);
            return;
        }
        Vector3 newobjpos= panellist[row,col].transform.position;
        newobjpos.y +=.5f;
        GridObj newobj = Instantiate(obj, newobjpos, Quaternion.identity).GetComponent<GridObj>();
        if(newobj.objtype == ObjType.TARGET)
        {
        targets.Add(newobj);
        if(color != Objcolor.NONE)
        newobj.ChangeColor(color);
        }
        newobj.CanDrag = CanDrag;
        newobj.GridPosition = new Vector2(row,col); 
        panellist[row,col].SetItem(newobj);
    }
    void CreateWalls(int[,] positions)
    {
        for(int i =0;i <positions.GetLength(0);i++)
        {
            GridPanel CurPanel = panellist[positions[i,0],positions[i,1]];
            
            Vector3 newpos = CurPanel.transform.position;
            newpos.y +=1;
            Instantiate(Wall,newpos,Quaternion.identity);
            CurPanel.Wall = true;
        }
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
        return getPanel(Row,Col);
    }
    public GridPanel getPanel(int Row, int Col)
    {
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
    void Fase1(){
        CreatePlaneArea(7,7);
        int[,] poss = new int[,] {{0,0}};
        CreateWalls(poss);
        SpawnObj(Double, Rows/2,Columns/2,true);
        SpawnObj(Reflector, Rows-1, Columns/2,true);
        SpawnObj(Reflector, Rows-1, Columns-1,true);
        SpawnObj(Glass, 0, Columns/2,true);
        SpawnObj(Glass, 0, Columns/2 + 1,true);
        SpawnObj(Target,0,Columns -1,false,Objcolor.RED);
        SpawnObj(Target,1,Columns -1,false,Objcolor.YELLOW);
        SpawnObj(Target,2,Columns -1,false,Objcolor.GREEN);
        SpawnObj(Target,4,Columns -1,false,Objcolor.CYAN);
        SpawnObj(Target,5,Columns -1,false,Objcolor.BLUE);
        SpawnObj(Target,6,Columns -1,false,Objcolor.MAGENTA);
        
    }
}
public enum Direction{
     
        FORWARD,
        BACK,
        LEFT,
        RIGHT,
        INVALID
    }

