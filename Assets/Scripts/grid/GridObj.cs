using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObj : MonoBehaviour
{
    
    public bool CanDrag;
    public bool CanWalkThrough = false;
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
    public Color redMat = Color.red,
    GreenMat =Color.green,
    BlueMat = Color.blue,
    CyanMat = Color.cyan,
    MagMat = Color.magenta,
    YellowMat = Color.yellow;
    public Material defmaterial,Litup;
    // Start is called before the first frame update
      public void ChangeColor(Objcolor TarColor)
    {
        switch(TarColor){
            case Objcolor.RED:
            mat.material.color = redMat;
            mat.material.SetColor("_EmissionColor",redMat);
            break;

            case Objcolor.GREEN:
            mat.material.color = GreenMat;
            mat.material.SetColor("_EmissionColor",GreenMat);
            break;

            case Objcolor.BLUE:
            mat.material.color = BlueMat;
            mat.material.SetColor("_EmissionColor",BlueMat);
            break;

                case Objcolor.CYAN:
            mat.material.color = CyanMat;
            mat.material.SetColor("_EmissionColor",CyanMat);
            break;

                case Objcolor.MAGENTA:
            mat.material.color = MagMat;
            mat.material.SetColor("_EmissionColor",MagMat);
            break;

                case Objcolor.YELLOW:
                mat.material.color = YellowMat;
                mat.material.SetColor("_EmissionColor",YellowMat);
            break;

            default:
            mat.material = defmat;
            mat.material.color = Color.white;
            mat.material.SetColor("_EmissionColor",Color.white);
            break;
        }
        oColor = TarColor;
    }
    #endregion

    #region Target
    public virtual void LightUP(){
        LightUp();
    }
    public virtual void LightDOWN()
    {
        LightDown();
    }
    #endregion
    public void LightUp(){
        if(objtype != ObjType.TARGET)
            return;
        mat.material =Litup;
        ChangeColor(oColor);
        Debug.Log("gay");
        
    }
    
    public void LightDown(){
        if(objtype != ObjType.TARGET)
            return;
            mat.material = defmaterial;
        ChangeColor(oColor);
        
    }
    void Start(){
       // mat = transform.GetChild(0).GetComponent<MeshRenderer>();
       LightDown();
        defmat = mat.material;
        RotDirection = SetRot();
    }
    
    void Update()
    {
        if(CanDrag)
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
    public virtual void Fase(){

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
        PRESSURE,
        OTHER
    }