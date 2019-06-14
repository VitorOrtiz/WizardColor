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
public GridObj Wall;
public GameObject obj;
public GridObj Reflector;
public GridObj Glass;
public GridObj Target;
public GridObj Double;
public Pressure Pressure;
[HideInInspector]
public Pressure LevelPressure;
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
    public void CreateLevel(string levelId)
    {
        GameObject.Destroy(newLevel);
        targets.Clear();
        Invoke(levelId,0);
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
            GameObject limit = Instantiate(Limit,pos,Quaternion.identity,newLevel.transform);
            limit.transform.localScale = new Vector3(1,limit.transform.localScale.y,Columns);
        } 
        else if(Direction.z !=0)
        {
            GameObject limit = Instantiate(Limit,pos,Quaternion.identity,newLevel.transform);
            limit.transform.localScale = new Vector3(Rows,limit.transform.localScale.y,1);
        }
        

    }
    public void ResetTargets()
    {
        foreach(GridObj obj in targets)
        {
            obj.LightDOWN();
        }
    }
    GridObj SpawnObj(GridObj obj,int row, int col,bool CanDrag, Objcolor color = Objcolor.NONE)
    {
        if(panellist[row,col].Wall)
        {
            Debug.Log("Panel has Wall in: " + row +" " + col);
            return null;
        }
        Vector3 newobjpos= panellist[row,col].transform.position;
        newobjpos.y +=.5f;
        GridObj newobj = Instantiate(obj, newobjpos, Quaternion.identity,newLevel.transform);
        if(newobj.objtype == ObjType.TARGET)
        {
            targets.Add(newobj);
            if(color != Objcolor.NONE)
            newobj.ChangeColor(color);
        }
        newobj.CanDrag = CanDrag;
        newobj.GridPosition = new Vector2(row,col); 
        panellist[row,col].SetItem(newobj);
        return newobj;
        }
    void CreatePressure(int Row, int Col,PressureObj[] objects)
    {
        if(panellist[Row,Col].Wall)
        {
            Debug.Log("Panel has Wall in: " + Row +" " + Col);
            return;
        }
        Vector3 newobjpos= panellist[Row,Col].transform.position;
        newobjpos.y +=.05f;
        Pressure newobj = Instantiate(Pressure, newobjpos, Quaternion.identity,newLevel.transform);
        for(int i = 0; i < objects.Length;i++)
        {
            GridObj newPobj =SpawnObj(objects[i].obj,objects[i].Row,objects[i].Col,false);
            newPobj.gameObject.SetActive(objects[i].DefActive);
            objects[i].obj = newPobj;
            newobj.Objects.Add(objects[i]);
        }
        newobj.GridPosition = new Vector2(Row,Col); 
        newobj.curpanel = panellist[Row,Col];
        panellist[Row,Col].HasPressure = true;
        LevelPressure = newobj;
    }
    void CreateWalls(int[,] positions)
    {
        for(int i =0;i <positions.GetLength(0);i++)
        {
            SpawnObj(Wall,positions[i,0],positions[i,1],false);
           // SpawnWall(positions[i,0],positions[i,1]);
        }
    }
    GridObj SpawnWall(int Row,int Col)
    {
        GridPanel CurPanel = panellist[Row,Col];
        
        Vector3 newpos = CurPanel.transform.position;
        newpos.y +=1;
        CurPanel.Wall = true;
        return Instantiate(Wall,newpos,Quaternion.identity,newLevel.transform);
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
    	void Fase1()
	{
		CreatePlaneArea(7, 5);
		SpawnObj(Target, 4, 2, false, Objcolor.RED);
		SpawnObj(Glass, 1, 2, true);
	}
    void Fase101(){
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
        CreatePressure(2,1, new PressureObj[]{new PressureObj(Glass,6,1,true)});
    }
    void Fase2()
	{
		CreatePlaneArea(8, 6);
		int[,] poss = new int[,] { { 0, 1 }, { 1, 1 }, { 2, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 6, 0 }, { 6, 2 }, { 6, 3 }, { 6, 5 } };
		CreateWalls(poss);
		SpawnObj(Target, 2, 0, false, Objcolor.GREEN);
	//	SpawnObj(Glass, 4, 1, false);
		SpawnObj(Reflector, 5, 0, true);
		SpawnObj(Reflector, 2, 5, true);
		CreatePressure(2, 3,new PressureObj[]{new PressureObj(Wall,6,4,true),new PressureObj(Wall,3,1,false),new PressureObj(Glass,4,1,false)});

	}
	void Fase3()
	{
		CreatePlaneArea(8, 6);
		int[,] poss = new int[,] { { 4, 0 }, { 4, 1 }, { 6, 0 }, { 6, 1 } };
		CreateWalls(poss);
		SpawnObj(Target, 5, 5, false, Objcolor.GREEN);
		SpawnObj(Target, 5, 0, false, Objcolor.RED);
		SpawnObj(Double, 7, 2, true);
		SpawnObj(Glass, 3, 0, true);
		SpawnObj(Glass, 1, 5, true);
		CreatePressure(1, 3, new PressureObj[]{new PressureObj(Wall,5,1,true)});

	}
	void Fase4()
	{
		CreatePlaneArea(8, 6);
		SpawnObj(Double, 7, 0, true);
		SpawnObj(Double, 0, 5, true);
		SpawnObj(Double, 7, 5, true);
		SpawnObj(Target, 2, 5, false, Objcolor.GREEN);
		SpawnObj(Target, 0, 1, false, Objcolor.MAGENTA);
		SpawnObj(Target, 7, 3, false, Objcolor.BLUE);
		SpawnObj(Target, 7, 1, false, Objcolor.RED);
		SpawnObj(Glass, 2, 4, false);
		SpawnObj(Glass, 6, 3, false);
		SpawnObj(Glass, 2, 2, false);
		SpawnObj(Glass, 1, 1, false);
	}
	void Fase5()
	{
		CreatePlaneArea(13, 8);
		SpawnObj(Double, 7, 0, true);
		SpawnObj(Double, 0, 5, true);
		SpawnObj(Double, 7, 5, true);
		SpawnObj(Glass, 2, 4, false);
		SpawnObj(Glass, 6, 3, false);
		SpawnObj(Glass, 2, 2, false);
		SpawnObj(Glass, 1, 1, false);
		SpawnObj(Reflector, 2, 5, true);
		SpawnObj(Reflector, 2, 5, true);
		SpawnObj(Reflector, 2, 5, true);
		SpawnObj(Reflector, 2, 5, true);
		SpawnObj(Reflector, 2, 5, true);
		SpawnObj(Reflector, 2, 5, true);
		SpawnObj(Reflector, 2, 5, true);

	}

}
public enum Direction{
     
        FORWARD,
        BACK,
        LEFT,
        RIGHT,
        INVALID
    }

