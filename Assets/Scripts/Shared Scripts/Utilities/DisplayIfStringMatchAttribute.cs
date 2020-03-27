using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayIfStringMatchAttribute : PropertyAttribute
{
    public string stringVariableName;
    public string stringValue;

    public DisplayIfStringMatchAttribute(string stringName, string stringValue)
    {
        this.stringVariableName = stringName;
        this.stringValue = stringValue;
    }
}
