using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectData
{
    public string firstName;
    public string lastName;
    public string birthDate;

    public SuspectData(string firstName, string lastName, string birthDate)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.birthDate = birthDate;
    }
}
