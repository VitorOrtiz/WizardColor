using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public Gradient RedGradient,BlueGradient, GreenGradient;
    public LineRenderer line;
    private grid gridcontrol;
    
    public List<lightBeamSeg> Segments = new List<lightBeamSeg>();
    public List<LineRenderer> lineSegments = new List<LineRenderer>();

    public List<LineRenderer> AvailableLines = new List<LineRenderer>();


    private Gradient NewGradient;
    public Vector2 gridPosition;
    void Start()
    {
     gridcontrol = grid.thisgrid;   
     for(int i = 0;i < 10;i++){
        AvailableLines.Add(Instantiate(line));
        //AvailableLines[i].gameObject.SetActive(false);

        }
        lineSegments = new List<LineRenderer>(AvailableLines);
        NewGradient = lineSegments[0].colorGradient;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CBeam(GridPanel initialPanel, Direction InitialDir,Vector3 InitialPosition)
    {
        StartCoroutine(CreateBeam(initialPanel, InitialDir, InitialPosition));
    }
    public void ResetCounters()
    {
        foreach(lightBeamSeg seg in Segments)
        {
            seg.Reset(NewGradient);
        }
        Segments = new List<lightBeamSeg>();
        AvailableLines = new List<LineRenderer>(lineSegments);
   
    }
    IEnumerator CreateBeam(GridPanel initialPanel,Direction InitialDir,Vector3 InitialPosition)
    {
    ResetCounters();
    bool HasLineOpen = true;
    List<lightBeamSeg> newLines = new List<lightBeamSeg>();
   // List<lightBeamSeg> Openlines = new List<lightBeamSeg>();
    //Openlines.Add(InitialLine);
    Direction CurrentDir = InitialDir;
    int CurRow = (int)initialPanel.position.x;
    int CurCol = (int)initialPanel.position.y;

    lightBeamSeg CurrentLine = new lightBeamSeg(AvailableLines[0],CurrentDir,new List<GridValue>());
    CurrentLine.line.transform.position = InitialPosition;
    Vector3[] oldpos = new Vector3[CurrentLine.line.positionCount];
    
    CurrentLine.line.GetPositions(oldpos);
    List<Vector3> positions = new List<Vector3>(oldpos);
    
    AvailableLines.RemoveAt(0);
    Vector3Int nextPos = Vector3Int.zero;
    while(HasLineOpen){
       
        GridPanel obj =lineCheck(CurRow,CurCol,CurrentDir);
        if(obj.HasItem){
            switch(obj.item.objtype)
            {
                case ObjType.REFLECTOR:
                nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                positions.Add(nextPos);
                CurrentLine.line.positionCount = positions.Count;
                CurrentLine.line.SetPositions(positions.ToArray());
                Direction newDir = Reflector.returnnewDirection(obj.item.DirRot,CurrentDir);
                Debug.Log(newDir + " " + CurrentDir);
                CurCol = (int)obj.position.y;
                CurRow = (int)obj.position.x;
                if(newDir == Direction.INVALID)
                HasLineOpen =false;
                CurrentDir = newDir;
                
                break;

                case ObjType.GLASS:
                lightBeamSeg seg =new lightBeamSeg(AvailableLines[0],CurrentDir,new List<GridValue>());    
                // Openlines.Add(seg);
                AvailableLines.RemoveAt(0);
                Debug.Log(CurrentLine.line.transform.position);
                Vector3 newpos = obj.transform.position;
                newpos.y = CurrentLine.line.transform.position.y;
                Debug.Log(newpos);
                seg.line.transform.position = newpos;
                nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                positions.Add(nextPos);
                seg.line.colorGradient = (obj.item.oColor == Objcolor.RED)?RedGradient:
                obj.item.oColor == Objcolor.GREEN?GreenGradient: 
                obj.item.oColor == Objcolor.BLUE?BlueGradient:NewGradient;

                CurrentLine.line.positionCount = positions.Count;
                CurrentLine.line.SetPositions(positions.ToArray());
                CurCol = (int)obj.position.y;
                CurRow = (int)obj.position.x;
                newLines.Add(CurrentLine);
                CurrentLine = seg;
                nextPos = Vector3Int.zero;
                oldpos = new Vector3[CurrentLine.line.positionCount];
                CurrentLine.line.GetPositions(oldpos);
                positions = new List<Vector3>(oldpos);
                break;

                case ObjType.WALL:
                   nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                positions.Add(nextPos);
                CurrentLine.line.positionCount = positions.Count;
                CurrentLine.line.SetPositions(positions.ToArray());
                    HasLineOpen = false;
                    newLines.Add(CurrentLine);
                break;
                case ObjType.TARGET:
                   nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                positions.Add(nextPos);
                CurrentLine.line.positionCount = positions.Count;
                CurrentLine.line.SetPositions(positions.ToArray());
                    HasLineOpen = false;
                    newLines.Add(CurrentLine);
                break;
                case ObjType.OTHER:
                break;
                
            }
        }
        else
        {
            Debug.Log(obj.position + " " + CurRow + " " + CurCol);
              nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                positions.Add(nextPos);
                CurrentLine.line.positionCount = positions.Count;
                CurrentLine.line.SetPositions(positions.ToArray());
                Debug.Log("No item Found");
                    HasLineOpen = false;
                newLines.Add(CurrentLine);
        }
        Debug.Log("Running");
        yield return null;
        }
    Segments = new List<lightBeamSeg>(newLines);
    }
    void CheckItemType(){
    }
    GridPanel lineCheck(int Row,int Col,Direction dir)
    {
        int CurRow = Row;
        int CurCol = Col;
        int limit = (gridcontrol.Rows> gridcontrol.Columns)? gridcontrol.Rows:gridcontrol.Columns;
        GridPanel curpanel = null;
        for(int i = 0; i<limit;i++)
        {
            curpanel = gridcontrol.getNextPanel(CurRow,CurCol,dir);
            Debug.Log(CurRow+ " "+CurCol);
            if(curpanel == null)
            return gridcontrol.panellist[CurRow,CurCol];
            else if(curpanel.HasItem)
            return curpanel;
            else 
            {
                CurRow = (int)curpanel.position.x;
                CurCol = (int)curpanel.position.y;
            }
            
        }
        return null;
    }
}
[System.Serializable]
public class lightBeamSeg{
    public LineRenderer line;
    public Direction dir;
    public List<GridValue> points = new List<GridValue>();
    public lightBeamSeg(LineRenderer line,Direction dir, List<GridValue> points)
    {
        this.line = line;
        this.dir = dir;
        this.points = new List<GridValue>(points);
    }
    public void Reset(Gradient grad)
    {
        line.positionCount = 1;
        line.colorGradient = grad;
        //line.gameObject.SetActive(false);
        //line.colorGradient.alphaKeys.
    }
   
}
public class GridValue{
    public int Row, Col;
}
public class ObjPosReturn
{
    public Objcolor color;
    public ObjType objtype = ObjType.OTHER;
    public int Row;
    public int Col;    
    public ObjPosReturn(int Row, int Col,ObjType otype, Objcolor color = Objcolor.NONE)
    {
        objtype = otype;
        this.Row = Row;
        this.Col = Col;
        this.color = color;
    }
}