using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : GridObj
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InGrid)
     Rotate();   
    }
    public void Activate()
    {
        
    }
    public void DropStaff(GridPanel TargetPanel)
    {
        if(InGrid)
        return;
        InGrid = true;
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
        panel.RemoveItem();
        panel = null;
        AttatchtoPlayer();
    }
}
