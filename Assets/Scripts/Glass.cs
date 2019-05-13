using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : GridObj
{
    private MeshRenderer mat;
    private Material defmat;
    public Material redMat,GreenMat,BlueMat;
    // Start is called before the first frame update
    void Start()
    {
     mat = GetComponent<MeshRenderer>();
     defmat = mat.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeColor(Objcolor TarColor)
    {
        
        switch(TarColor){
            case Objcolor.RED:
            mat.material = redMat;
            break;
            case Objcolor.GREEN:
            mat.material = GreenMat;
            break;
            case Objcolor.BLUE:
            mat.material = BlueMat;
            break;
            default:
            mat.material = defmat;
            break;
        }
        oColor = TarColor;
    }
}
