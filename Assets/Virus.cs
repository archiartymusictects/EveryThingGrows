using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Virus : Cell
{

    protected Virus virusClass;

    public Virus(Virus virusClass)
    {
        this.virusClass = virusClass;
    }


}
