using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectData
{
    public string firstName;
    public string lastName;
    public string birthDate;
    public bool isHacker;

    public SuspectData(string firstName, string lastName, string birthDate, bool isHacker)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.birthDate = birthDate;
        this.isHacker = isHacker;
    }
}
