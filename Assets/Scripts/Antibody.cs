using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Antibody : Cell
{

    //protected Antibody antibodyClass;

    //public Antibody(Antibody antibodyClass)
    //{
    //    this.antibodyClass = antibodyClass;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayerTakeDamage(10);
            Destroy(gameObject);
        }
    }

}