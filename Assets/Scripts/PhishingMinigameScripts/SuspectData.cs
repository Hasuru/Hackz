using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectData
{
    public string firstName;
    public string lastName;
    public string birthDate;
    public bool isHacker;
    public int[] puzzlePieces;

    public SuspectData(string firstName, string lastName, string birthDate, int[] puzzlePieces)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.birthDate = birthDate;
        this.puzzlePieces = puzzlePieces;
    }

    public void ChangeIsHacker(bool isHacker)
    {
        this.isHacker = isHacker;
    }
}
