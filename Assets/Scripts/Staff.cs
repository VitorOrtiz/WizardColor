using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : GridObj
{
    public Beam beam;
    public bool InGrid;
    private bool Firing;
    public Transform FiringPoint;
    Vector3 HoldingPosition;
    Quaternion HoldingRotation;
    Quaternion GridRotation;

    GridPanel panel;


    // Start is called before the first frame update
    void Start()
    {
        HoldingPosition = transform.position;
        HoldingRotation = transform.rotation;
        GridRotation = Quaternion.identity;    
        RotDirection = SetRot();
    }

    // Update is called once per frame
    void Update()
    {
    if(InGrid){
        Rotate();   
      
        if(panel.position != GridPosition)
            panel = grid.thisgrid.panellist[(int)GridPosition.x,(int)GridPosition.y];   

        
        if((Attatched && Firing) || (Player.player.Att.Attatched && Firing)){
            Reset();
            }
        else if(!Attatched && !Player.player.Att.Attatched && !Firing)
            {
            Activate();
            }
        }
        
        
    }
    public void RedoBeam()
    {
        if(InGrid && !Attatched && Firing)
        Activate();
    }
    void Reset(){
        beam.ResetCounters();
         Firing = false;
    }
    public void Activate()
    {
        Firing = true;
        beam.CBeam(panel,MoveDirection(RotDirection),FiringPoint.position);
    }
    public void DropStaff(GridPanel TargetPanel)
    {
        if(InGrid)
        return;
        InGrid = true;
        Attatched = true;
        transform.rotation = GridRotation;
        transform.position = TargetPanel.transform.position;
        GridPosition = TargetPanel.position;   
        TargetPanel.SetItem(this);
        panel = TargetPanel;
        transform.SetParent(null);
    }
    public void ReturnStaff(Transform parent)
    {
        transform.SetParent(parent);
        transform.localRotation = HoldingRotation;
        transform.localPosition = HoldingPosition;
        InGrid = false;
        Attatched = false;
        beam.ResetCounters();
        panel.RemoveItem();
        panel = null;
        AttatchtoPlayer();
    }
}
