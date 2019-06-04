using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObj : MonoBehaviour
{
    
    public bool CanDrag;
    public Material LitUpRed,LitUpGreen,LitUpBlue;
    public Objcolor oColor = Objcolor.NONE;
    public ObjType objtype;
    public Vector2 GridPosition;
    public bool Cancolor;
    public Direction DirRot;
    public int idRotation;
    public Vector3 RotDirection;
    private bool Rotating;
    public bool Attatched = false;

    private bool lightUp;
    #region Glass

    public MeshRenderer mat;
    private Material defmat;
    public Material redMat,GreenMat,BlueMat;
    // Start is called before the first frame update
      public void ChangeColor(Objcolor TarColor)
    {
        
        switch(TarColor){
            case Objcolor.RED:
            oColor = Objcolor.RED;
            mat.material = redMat;
            break;
            case Objcolor.GREEN:
            oColor = Objcolor.GREEN;
            mat.material = GreenMat;
            break;
            case Objcolor.BLUE:
            oColor = Objcolor.BLUE;
            mat.material = BlueMat;
            break;
            default:
            oColor = Objcolor.NONE;
            mat.material = defmat;
            break;
        }
        oColor = TarColor;
    }
    #endregion
    
    public void LightUP(){
        if(objtype != ObjType.TARGET)
            return;
        switch(oColor)
        {
            case Objcolor.RED:
            mat.material = LitUpRed;
            break;
            case Objcolor.GREEN:
            mat.material = LitUpGreen;
            break;
            case Objcolor.BLUE:
            mat.material = LitUpBlue;
            break;
        }
        
        
    }
    
    public void LightDown(){
        if(objtype != ObjType.TARGET)
            return;
        switch(oColor)
        {
            case Objcolor.RED:
            mat.material = redMat;
            break;
            case Objcolor.GREEN:
            mat.material = GreenMat;
            break;
            case Objcolor.BLUE:
            mat.material =BlueMat;
            break;
            case Objcolor.NONE:
            mat.material = defmat;
            break;
        }
        
    }
    void Start(){
       // mat = transform.GetChild(0).GetComponent<MeshRenderer>();
       LightDown();
        defmat = mat.material;
        RotDirection = SetRot();
    }
    
    void Update()
    {
        Rotate();
    }
    public void Rotate()
    {
        Quaternion newangle = Quaternion.LookRotation(RotDirection,transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation,newangle,.5f);   
    }
    public void RotateObject(int dir)
    {
        
        idRotation +=dir;
        if(idRotation> 3)
        idRotation =0;
        else if(idRotation<0)
        idRotation = 3;
        RotDirection = SetRot();
        DirRot = MoveDirection(RotDirection);
        //StartCoroutine(Rotate(idRotation));
    }
    public Vector3 SetRot()
    {
        switch(idRotation)
        {
            case 0:
            return Vector3.forward;

            case 1:
            return Vector3.right;

            case 2:
            return Vector3.back;

            case 3:
            return Vector3.left;

            default:
            return Vector3.zero;
        }
    }
    
    public void SetPosition(Vector3 newPosition,Vector2 newGridPosition)
    {
        transform.position = newPosition;
        GridPosition = newGridPosition;

    }
    public void AttatchtoPlayer()
    {
        Attatched = true;
        grid.thisgrid.panellist[(int)GridPosition.x,(int)GridPosition.y].RemoveItem();
    }
    public void AttachtoGrid()
    {
        Attatched = false;
        grid.thisgrid.panellist[(int)GridPosition.x,(int)GridPosition.y].SetItem(this);
    }
    public Direction MoveDirection(Vector3 Dir)
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
public enum Objcolor{
        GREEN,
        BLUE,
        RED,
        YELLOW,
        MAGENTA,
        CYAN,
        NONE
    }

    public enum ObjType
    {
        REFLECTOR,
        DOUBLE, 
        GLASS,
        WALL,
        TARGET,
        OTHER
    }