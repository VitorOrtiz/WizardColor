using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public Staff playerStaff;

    #region GridControl
    public Attatchment Att;
    public enum MoveState{
        FreeWalk,
        Attatched
    }
    private bool AttMoving;
    public MoveState state = MoveState.FreeWalk;
    public GridPanel CurPanel;
    #endregion
    #region FreeControl
    float PlayerInputHor;
    float PlayerInputVer;
    public float MoveSpeed;
    public Vector3 InitialLookDirection;
    private Vector3 LookDirection;
    private Vector3 EulerRotation;
    private Quaternion TargetRotation;
    #endregion

    private bool DroppingStaff;
    Rigidbody rig;
    public static Player player;
    void Awake()
    {
        player =this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Att = new Attatchment();
        EulerRotation = InitialLookDirection;
        rig = GetComponent<Rigidbody>();
        LookDirection = InitialLookDirection;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Input.GetKeyDown(KeyCode.Space)&& !DroppingStaff)
        { 
            if(!Att.Attatched){
            Attatch();
            }
            else {
            Att.RemoveAttatch();
            Debug.Log(state);
            state = MoveState.FreeWalk;
            }
        }
        if(Att.Attatched)
        {
            if(Input.GetKeyDown(KeyCode.E))
                Att.AttatchedObj.RotateObject(1);
            
            else if(Input.GetKeyDown(KeyCode.Q))
                Att.AttatchedObj.RotateObject(-1);
        }
    
        if(Input.GetKeyDown(KeyCode.F) && !Att.Attatched)
        {
            if(!playerStaff.InGrid){
                Direction curdir = MoveDirection(EulerRotation) ;
                if(curdir!= Direction.INVALID){
                    
                    int Row = (int)(CurPanel.position.x + EulerRotation.x);
                    int Col = (int)(CurPanel.position.y + EulerRotation.z);
                    GridPanel targetPanel = grid.thisgrid.getPanel(Row,Col);
                    if( targetPanel == null ||targetPanel.HasItem && targetPanel.item.objtype != ObjType.PRESSURE|| targetPanel.Wall)
                    return;
                    DroppingStaff = true;
                    
                    Vector3 targetpos = CurPanel.transform.position;
                    targetpos.y = transform.position.y;
                    transform.position = targetpos;
                    playerStaff.DropStaff(targetPanel);
                    Att = new Attatchment(true,curdir,playerStaff);
                    state = MoveState.Attatched;
                }
            }
            else
            {           
                playerStaff.ReturnStaff(this.transform);
            }       
        }
        else if(Input.GetKeyUp(KeyCode.F))
        {
            if(playerStaff.InGrid){
                DroppingStaff = false;
            Att.RemoveAttatch();    
            state = MoveState.FreeWalk;
            }
        }
    
    }
    public Direction playerDirection{
        get{return MoveDirection(EulerRotation);}
        
    }
    void FixedUpdate()
    {
        Vector3 PlayerInput = new Vector3(PlayerInputHor,0, PlayerInputVer).normalized;
        if(LookDirection != Vector3.zero){
            TargetRotation = Quaternion.LookRotation(LookDirection,transform.up);
            EulerRotation = LookDirection;
        }   
        if(state == MoveState.FreeWalk){
            Quaternion NewRotation = Quaternion.Slerp(transform.rotation,TargetRotation,.5f);
            transform.rotation = NewRotation;
            rig.velocity = PlayerInput * MoveSpeed;
        }
        else {
        rig.velocity = Vector3.zero;

        }
        CheckPanelPos();
    }
    void CheckPanelPos()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, 10, 1 << 11))
        {
            GridPanel panel = hit.transform.GetComponent<GridPanel>();//CurPanel = hit.transform.GetComponent<GridPanel>();
            if (panel != CurPanel)
            {
                if (CurPanel != null)
                    CurPanel.ChangeColor();
                CurPanel = panel;
                CurPanel.ChangeColor();
            }
        }
    }

    void Move()
    {
         PlayerInputHor = Input.GetAxisRaw("Horizontal");
        PlayerInputVer = Input.GetAxisRaw("Vertical");

        if(state == MoveState.Attatched)
        {
            if(!AttMoving)
            InvokeRepeating("MoveOnGrid",0,.15f);
        }
        
        LookDirection.x = PlayerInputHor;
        LookDirection.z = PlayerInputVer;
    }
    public void ResetPlayer()
    {
        state = MoveState.FreeWalk;
        Att = new Attatchment();
        playerStaff.ReturnStaff(this.transform);
        DroppingStaff = false;
    }
    void MoveOnGrid()
    {
        AttMoving = true;
        Vector3 PlayerInput = new Vector3(PlayerInputHor,0,PlayerInputVer);
        if(MoveDirection(PlayerInput) == Direction.INVALID ||state == MoveState.FreeWalk){
            CancelInvoke("MoveOnGrid");
            AttMoving = false;
        }
            else
            Darg(PlayerInput);
    }
    void Darg(Vector3 Dir)
    {
        
        int Row,Col,ObjRow,ObjCol;
        Row = (int)(CurPanel.position.x + Dir.x);
        Col = (int)(CurPanel.position.y + Dir.z);
        ObjRow = (int)(Att.AttatchedObj.GridPosition.x + Dir.x);
        ObjCol = (int)(Att.AttatchedObj.GridPosition.y + Dir.z);
        if(Col >= grid.thisgrid.Columns || ObjCol >= grid.thisgrid.Columns ||
            Row >= grid.thisgrid.Rows || ObjRow >= grid.thisgrid.Rows ||
            Row < 0 || Col < 0 || ObjCol < 0 || ObjRow < 0)
        return;
        GridPanel targetPanel, ObjTargetPanel;
        
        targetPanel = grid.thisgrid.getPanel(Row,Col);
        ObjTargetPanel = grid.thisgrid.getPanel(ObjRow,ObjCol);
        if(!targetPanel.CanDragOver|| !ObjTargetPanel.CanDragOver||targetPanel.HasItem || targetPanel.Wall || ObjTargetPanel.HasItem|| ObjTargetPanel.Wall)
        return;

        StartCoroutine(WalktoObjPos(targetPanel,ObjTargetPanel));   
    }
    IEnumerator WalktoObjPos(GridPanel targetPanel,GridPanel ObjTargetPanel){
        
        Vector3 objtargetpos = ObjTargetPanel.transform.position;
        objtargetpos.y = Att.AttatchedObj.transform.position.y;

        Vector3 targetpos = targetPanel.transform.position;
        targetpos.y = transform.position.y;
        
       // yield return new WaitForSeconds(.01f);
        grid.thisgrid.panellist[(int)Att.AttatchedObj.GridPosition.x,
                (int)Att.AttatchedObj.GridPosition.y].RemoveItem();
        
        transform.position = targetpos;
        //  grid.thisgrid.panellist[(int)ObjTargetPanel.position.x,
        //      (int)ObjTargetPanel.position.y].SetItem(Att.AttatchedObj);
        Att.AttatchedObj.SetPosition(objtargetpos,ObjTargetPanel.position);
        CheckPanelPos();
        CurPanel = targetPanel;
        yield return null;

    }
    void Attatch()
    {
        if(!Att.Attatched){// se não tiver um objeto ligado
            Vector2 CurPanelindex = CurPanel.position;// pega a posição do jogador no grid

            if(MoveDirection(EulerRotation) != Direction.INVALID){
            int row = (int)(CurPanelindex.x +EulerRotation.x);// pega o proximo baseado na direçao
            int col = (int)(CurPanelindex.y + EulerRotation.z);//que o jogador esta olhando
            GridPanel targetPanel = grid.thisgrid.getPanel(row,col);
            if(targetPanel != null)
                if(targetPanel.HasItem){// se o proximo tiver um objeto
                    if(!targetPanel.item.CanDrag)
                    return;
                    Direction dir = MoveDirection(EulerRotation);//pega a orientação 
                    Att = new Attatchment(true, dir, targetPanel.item);//cria novo Attatch com o objeto e orientação
                    Att.AttatchedObj.AttatchtoPlayer();
                    state = MoveState.Attatched;// muda o sistema do movimento
                }
            }
            
        }
    }

    Direction MoveDirection(Vector3 Dir)
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

[System.Serializable]
public class Attatchment
{
    public bool Attatched;
     public Direction AttDirection;
    public GridObj AttatchedObj;
    public bool inPosition;
    public Attatchment()
    {
        this.Attatched = false;
        this.AttDirection = Direction.INVALID;
        this.AttatchedObj = null;
    }
    public Attatchment (bool Attatched, Direction AttDirection,GridObj AttatchedObj)
    {
        this.Attatched = Attatched;
        this.AttDirection = AttDirection;
        this.AttatchedObj = AttatchedObj;
    }
    public void RemoveAttatch()
    {
        Attatched = false;
        AttDirection = Direction.INVALID;
        AttatchedObj.AttachtoGrid();
        AttatchedObj = null;
    }

}