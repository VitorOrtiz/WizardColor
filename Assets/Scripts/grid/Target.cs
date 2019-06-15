using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : GridObj
{
    
    public ParticleSystem ActiveParticles;
    // Start is called before the first frame update
    void Start()
    {
        var main = ActiveParticles.main;
        main.startColor = oColor == Objcolor.BLUE? 
        Color.blue:oColor == Objcolor.RED?Color.red: oColor == Objcolor.GREEN?Color.green:
        oColor == Objcolor.CYAN?Color.cyan:oColor == Objcolor.MAGENTA?Color.magenta:
        oColor == Objcolor.YELLOW?Color.yellow:ActiveParticles.main.startColor;
    }
   public override void LightUP(){
       LightUp();
       ActiveParticles.gameObject.SetActive(true);
       ActiveParticles.Play();
       BeamHit = true;
       grid.thisgrid.CheckWin();
   }
   public override void LightDOWN(){
       LightDown();
       BeamHit =false;
       ActiveParticles.gameObject.SetActive(false);
       ActiveParticles.Stop();
   }

}
