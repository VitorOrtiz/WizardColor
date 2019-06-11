using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressure : GridObj
{
    // Start is called before the first frame update
    public string LevelID;
    public GameObject targetobj;
    public GridPanel targetobjpanel;
    public bool active;
    public Vector2Int ObjPos;
    public GridPanel curpanel;
    public enum objType{
        WALL,

    }
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
    public void Fase(bool activeset)
    {
        targetobj.SetActive(activeset);
        active = activeset;
        
        targetobjpanel.Wall = activeset;
        targetobjpanel.HasItem = activeset;
    }
}
