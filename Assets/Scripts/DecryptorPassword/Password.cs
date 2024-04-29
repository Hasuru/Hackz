using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Password", menuName = "Passwords/New Password")]
public class Password : ScriptableObject
{
    [SerializeField] string initialPassword;
    public string InitialPassword { get { return initialPassword; } }
    [SerializeField] string scannedPassword;
    public string ScannedPassword { get { return scannedPassword; } }
    [SerializeField] string finalPassword;
    public string FinalPassword { get { return finalPassword; } }
    [SerializeField] Color[] protocols = new Color[3];
    public Color[] Protocols { get { return protocols;} }
}
