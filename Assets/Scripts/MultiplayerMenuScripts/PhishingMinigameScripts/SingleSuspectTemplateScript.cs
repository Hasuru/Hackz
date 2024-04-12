using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleSuspectTemplateScript : MonoBehaviour
{
    [Header("Field Attributes")]
    [SerializeField] private Image photoIcon;
    [SerializeField] private TextMeshProUGUI firstName;
    [SerializeField] private TextMeshProUGUI lastName;
    [SerializeField] private TextMeshProUGUI birthDate;
    [SerializeField] private Button templateButton;

    [Header("Related UI")]

    [Header("Assets to use")]

    private SuspectData associatedSuspect = null;

    private bool isFlagged = false;

    public void AssociateSuspect(SuspectData sussy)
    {
        if (sussy != null)
        {
            associatedSuspect = sussy;

            firstName.text = sussy.firstName;
            lastName.text = sussy.lastName;
            birthDate.text = sussy.birthDate;
        }
    }
}
