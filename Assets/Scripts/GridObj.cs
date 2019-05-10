using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObj : MonoBehaviour
{
    public Vector2 GridPosition;
    public bool Attatched = false;

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