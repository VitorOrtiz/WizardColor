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
public static grid thisgrid;
#endregion
    void Start()
    { 
        thisgrid = this;
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
       GridObj newobj = Instantiate(obj, panellist[(int)Rows/2,0].transform.position, Quaternion.identity).GetComponent<GridObj>();
       newobj.GridPosition = new Vector2((int)Rows/2,0); 
       panellist[(int)Rows/2,0].SetItem(newobj);
    }
    void CreateWalls()
    {
        
    }

}
