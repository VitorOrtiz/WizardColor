using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObj : MonoBehaviour
{
    public Vector2 GridPosition;

    public int idRotation;
    Vector3 RotDirection;
    private bool Rotating;
    public bool Attatched = false;
    void Start(){
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
        //StartCoroutine(Rotate(idRotation));
    }
    Vector3 SetRot()
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
}