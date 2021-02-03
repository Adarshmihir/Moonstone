using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoStatModTypeException : Exception
{
    public NoStatModTypeException()
        : base("StatModType not existing.")
    {
    }
}
