using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Virus : MonoBehaviour
{
    public float Health;

    public float Damage;

    public float Speed;

    public string Name;

    public Vector3 SpawnLocation;

    public abstract void Shooting();


    public abstract void Death();
    

}
