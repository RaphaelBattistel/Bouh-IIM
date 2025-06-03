using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats
{
    public int life;
    public int stamina;
    public float speedMove;
    public float forceJump;
    public float fallingSpeed;

    public Stats(int life, int def, float fallingSpeed, float speedMove, float forceJump)
    {
        this.life = life;
        this.stamina = def;
        this.speedMove = speedMove;
        this.forceJump = forceJump;
        this.fallingSpeed =  fallingSpeed;
    }

    public Stats(int level)
    {
        life = level;
        stamina = level;
        speedMove = level;
        forceJump = level;
        fallingSpeed = level;
    }
}