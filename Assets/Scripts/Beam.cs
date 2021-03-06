﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public GameObject Sparkparticle;
    public ParticleSystem GlassParticle;
    
    public Gradient RedGradient,BlueGradient, GreenGradient, CyanGradient, MagGradient,YellowGradient;
    public LineRenderer line;
    private grid gridcontrol;

    private List<GameObject> particles = new List<GameObject>();    
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
        foreach(GameObject part in particles){
            Destroy(part);
        }
        particles.Clear();
        Segments = new List<lightBeamSeg>();
        AvailableLines = new List<LineRenderer>(lineSegments);
        gridcontrol.ResetTargets();
   
    }
    IEnumerator CreateBeam(GridPanel initialPanel,Direction InitialDir,Vector3 InitialPosition)
    {
    ResetCounters();
    float time=0;
    bool HasLineOpen = true;
    List<lightBeamSeg> newLines = new List<lightBeamSeg>();
    List<lightBeamSeg> Openlines = new List<lightBeamSeg>();
   
    Direction CurrentDir = InitialDir;
    int CurRow = (int)initialPanel.position.x;
    int CurCol = (int)initialPanel.position.y;

    lightBeamSeg CurrentLine = new lightBeamSeg(AvailableLines[0],CurrentDir);
    CurrentLine.line.transform.position = InitialPosition;
    Vector3[] oldpos = new Vector3[CurrentLine.line.positionCount];
    
    CurrentLine.line.GetPositions(oldpos);
    CurrentLine.points.Add(new GridValue{Row = CurRow,Col = CurCol});
    List<Vector3> positions = new List<Vector3>(oldpos);
    Openlines.Add(CurrentLine);
    AvailableLines.RemoveAt(0);
    Vector3Int nextPos = Vector3Int.zero;
    while(HasLineOpen){
        GridPanel obj =lineCheck(CurRow,CurCol,CurrentDir);
        if(obj.HasItem){
            switch(obj.item.objtype)
            {
                case ObjType.REFLECTOR:
                nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                AddPositiontoLine(CurrentLine.line,nextPos);
                Direction newDir = Reflector.returnnewDirection(obj.item.DirRot,CurrentDir);
                Debug.Log(newDir + " " + CurrentDir);
                CurCol = (int)obj.position.y;
                CurRow = (int)obj.position.x;
                if(newDir == Direction.INVALID)
                {
                    Openlines.Remove(CurrentLine);
                    newLines.Add(CurrentLine);
                }
                
                CurrentDir = newDir;
                break;

                case ObjType.DOUBLE:
                
                Direction newRaydir = Reflector.returnnewDoubleDirection(obj.item.DirRot,CurrentDir);
                nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                AddPositiontoLine(CurrentLine.line,nextPos);
                CurCol = (int)obj.position.y;
                CurRow = (int)obj.position.x;
                if(newRaydir != Direction.INVALID){
                    CurrentDir = newRaydir;
                    CurrentLine.dir = CurrentDir;
                    lightBeamSeg newseg =new lightBeamSeg(AvailableLines[0], 
                    Reflector.ReturnReverseDirection(newRaydir));   
                    AvailableLines.RemoveAt(0);
                    Vector3 newraypos = obj.transform.position;
                    newraypos.y = CurrentLine.line.transform.position.y;
                    newseg.line.transform.position = newraypos;
                    newseg.lineColor = CurrentLine.lineColor;
                    newseg.line.colorGradient = SetLineColor(CurrentLine.lineColor);
                    newseg.points.Add(new GridValue{Row = CurRow, Col = CurCol});
                    Openlines.Add(newseg);
                }
                else
                {
                    Openlines.Remove(CurrentLine);
                    newLines.Add(CurrentLine);
                }
             
                break;

                case ObjType.GLASS:
                lightBeamSeg seg =new lightBeamSeg(AvailableLines[0],CurrentDir);    
                // Openlines.Add(seg);
                AvailableLines.RemoveAt(0);      
                Vector3 newpos = obj.transform.position;
                newpos.y = CurrentLine.line.transform.position.y;
                seg.line.transform.position = newpos;
                nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                AddPositiontoLine(CurrentLine.line,nextPos);   
                Objcolor newcolor = mixColor(CurrentLine.lineColor,obj.item.oColor);
                seg.line.colorGradient = SetLineColor(newcolor);
                seg.lineColor = newcolor;
                CurCol = (int)obj.position.y;
                CurRow = (int)obj.position.x;
                newLines.Add(CurrentLine);
                Openlines.Remove(CurrentLine);
                seg.points.Add(new GridValue{Row = CurRow, Col = CurCol});
                if(CurrentLine.points[0].Row != CurRow || CurrentLine.points[0].Col != CurCol)
                Openlines.Add(seg);
                particles.Add(CreateGlassParticle(obj.item.GetColor(),newpos,seg.dir).gameObject);                
                break;

                case ObjType.WALL:
                nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                AddPositiontoLine(CurrentLine.line,nextPos);
                Openlines.Remove(CurrentLine);
                newLines.Add(CurrentLine);
                Vector3 pos = obj.transform.position;
                pos.y = CurrentLine.line.transform.position.y;
                particles.Add(CreateWallParticle(pos,CurrentLine.dir));
                break;

                case ObjType.TARGET:
                nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
                AddPositiontoLine(CurrentLine.line,nextPos);
                Openlines.Remove(CurrentLine);
                newLines.Add(CurrentLine);
                if(CurrentLine.lineColor == obj.item.oColor)
                obj.item.LightUP();
                break;

                case ObjType.OTHER:
                break;
                case ObjType.PRESSURE:
                CurCol = (int)obj.position.y;
                CurRow = (int)obj.position.x;
                break;
                
            }
        }
        else
        {
            Debug.Log(obj.position + " " + CurRow + " " + CurCol);
            nextPos += new Vector3Int((int)obj.position.x- CurRow,0,(int)obj.position.y- CurCol);
            AddPositiontoLine(CurrentLine.line,nextPos);
            Debug.Log("No item Found");
            Openlines.Remove(CurrentLine);
            newLines.Add(CurrentLine);
        }
        Debug.Log("Running");
        if(Openlines.Count<= 0)
        HasLineOpen = false;
        else if(!Openlines.Contains(CurrentLine)){
        CurrentLine = Openlines[0];
        CurrentDir = CurrentLine.dir;
        nextPos = Vector3Int.zero;
        oldpos = new Vector3[CurrentLine.line.positionCount];
        CurrentLine.line.GetPositions(oldpos);
        positions = new List<Vector3>(oldpos);
        CurRow = CurrentLine.points[0].Row;
        CurCol = CurrentLine.points[0].Col;
        }
        time+= Time.deltaTime;
        yield return null;
        }
    Segments = new List<lightBeamSeg>(newLines);
    Debug.Log(time);
    }
    void AddPositiontoLine(LineRenderer line, Vector3 Pos)
    {
        Vector3[] oldpos = new Vector3[line.positionCount];
        line.GetPositions(oldpos);
        List<Vector3> positions = new List<Vector3>(oldpos);
        positions.Add(Pos);
        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());
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
    Objcolor mixColor(Objcolor curColor,Objcolor secondColor)
    {
        if(curColor == secondColor)
        return curColor;
        switch(curColor)
        {
            case Objcolor.RED:
            if(secondColor == Objcolor.BLUE)
                return Objcolor.MAGENTA;
            else if(secondColor == Objcolor.GREEN)
            return Objcolor.YELLOW;
            else
            return secondColor;
    
            case Objcolor.GREEN:
             if(secondColor == Objcolor.BLUE)
                return Objcolor.CYAN;
            else if(secondColor == Objcolor.RED)
            return Objcolor.YELLOW;
             else
            return secondColor;

            case Objcolor.BLUE:
             if(secondColor == Objcolor.RED)
                return Objcolor.MAGENTA;
            else if(secondColor == Objcolor.GREEN)
            return Objcolor.CYAN;
             else
            return secondColor;
        }
        return secondColor;
    }
    Gradient SetLineColor(Objcolor color){
        return color == Objcolor.RED? RedGradient:
               color == Objcolor.GREEN? GreenGradient: 
               color == Objcolor.BLUE? BlueGradient:
               color == Objcolor.CYAN? CyanGradient:
               color == Objcolor.MAGENTA? MagGradient:
               color == Objcolor.YELLOW? YellowGradient:
               NewGradient;
    }
    GameObject CreateWallParticle(Vector3 position,Direction dir)
    {
        Direction newdir =Reflector.ReturnReverseDirection(dir);
        Vector3 direction = dirtoVector(newdir);
        
        Quaternion rotation = Quaternion.LookRotation(direction,Vector3.up);
        GameObject spark = Instantiate(Sparkparticle,position+direction *.5f,rotation);
        return spark;
    }
    ParticleSystem CreateGlassParticle(Color color, Vector3 position, Direction dir)
    {
        Vector3 direction = dirtoVector(dir);
        
        Debug.Log(dir);
        Quaternion rotation = Quaternion.LookRotation(direction,Vector3.up);
        //rotation = Quaternion.Euler(rotation.eulerAngles.x,Quaternion.LookRotation(direction,Vector3.up).y + 90,rotation.eulerAngles.z);
        ParticleSystem part = Instantiate(GlassParticle, position, rotation);
        var main =part.main;
        var sub = part.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        main.startColor = color;
        sub.startColor = color;
        return part;
    }
    Vector3 dirtoVector(Direction dir)
    {
        switch(dir){
        case Direction.FORWARD:
        return Vector3.forward;
        
        case Direction.BACK:
        return Vector3.back;
        
        case Direction.LEFT:
        return Vector3.left;
        
        case Direction.RIGHT:
        return Vector3.right;
        }
        return Vector3.zero;
    }
}
[System.Serializable]
public class lightBeamSeg{
    public LineRenderer line;
    public Direction dir;
    public Objcolor lineColor = Objcolor.NONE;
    public List<GridValue> points = new List<GridValue>();
    public lightBeamSeg(LineRenderer line,Direction dir)
    {
        this.line = line;
        this.dir = dir;
        //this.points = new List<GridValue>(points);
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