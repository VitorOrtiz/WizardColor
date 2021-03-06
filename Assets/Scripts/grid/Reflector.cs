﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : GridObj
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static Direction returnnewDirection(Direction thisdir,Direction receivingDir)
    {
        switch(receivingDir)
        {
            case Direction.FORWARD:
            if(thisdir == Direction.LEFT)
                return Direction.LEFT;
            else if(thisdir == Direction.BACK)
                return Direction.RIGHT;
            else
                return Direction.INVALID;

            case Direction.BACK:
            if(thisdir == Direction.FORWARD)
                return Direction.LEFT;
            else if(thisdir == Direction.RIGHT)
                return Direction.RIGHT;
            else
                return Direction.INVALID;

            case Direction.LEFT:
              if(thisdir == Direction.RIGHT)
                return Direction.FORWARD;
            else if(thisdir == Direction.BACK)
                return Direction.BACK;
            else
                return Direction.INVALID;

            case Direction.RIGHT:
              if(thisdir == Direction.FORWARD)
                return Direction.FORWARD;
            else if(thisdir == Direction.LEFT)
                return Direction.BACK;
            else
                return Direction.INVALID;
        }
        return Direction.INVALID;
    }
     public static Direction returnnewDoubleDirection(Direction thisdir,Direction receivingDir)
    {
        switch(receivingDir)
        {
            case Direction.FORWARD:
            if(thisdir == Direction.BACK)
                return Direction.RIGHT;
            else
                return Direction.INVALID;

            case Direction.BACK:
            if(thisdir == Direction.FORWARD)
                return Direction.LEFT;
            else
                return Direction.INVALID;

            case Direction.LEFT:
              if(thisdir == Direction.RIGHT)
                return Direction.FORWARD;
            else
                return Direction.INVALID;

            case Direction.RIGHT:
            if(thisdir == Direction.LEFT)
                return Direction.BACK;
            else
                return Direction.INVALID;
        }
        return Direction.INVALID;
    }
    public static Direction ReturnReverseDirection(Direction curDirection)
    {
        switch(curDirection)
        {
            case Direction.FORWARD:
            return Direction.BACK;
            case Direction.BACK:
            return Direction.FORWARD;
            case Direction.LEFT:
            return Direction.RIGHT;
            case Direction.RIGHT:
            return Direction.LEFT;
        }
        return curDirection;
    }
}
