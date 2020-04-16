using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoSaveFileFoundException : Exception
{
    public NoSaveFileFoundException() { }
    public NoSaveFileFoundException(string message)
        : base(message)
    {
    }
    public NoSaveFileFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}