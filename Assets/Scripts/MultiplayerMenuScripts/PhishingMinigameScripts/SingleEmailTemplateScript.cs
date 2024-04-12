using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class SingleEmailTemplateScript : MonoBehaviour
{
    [Header("Field Attributes")]
    [SerializeField] private Image background;
    [SerializeField] private Image readIcon;
    [SerializeField] private TextMeshProUGUI senderName;
    [SerializeField] private TextMeshProUGUI subject;
    [SerializeField] private TextMeshProUGUI dateReceived;
    [SerializeField] private Button flagButton;
    [SerializeField] private Button templateButton;

    [Header("Related UI")]
    [SerializeField] private GameObject emailWindowUI;
    [SerializeField] private GameObject openedEmailTaskButton;

    [Header("Assets to use")]
    [SerializeField] private Sprite readTrueIcon;
    [SerializeField] private Sprite readFalseIcon;
    [SerializeField] private Sprite flaggedTrueIcon;
    [SerializeField] private Sprite flaggedFalseIcon;

    private EmailData associatedEmail = null;

    private bool isRead = false;
    private bool isFlagged = false;


    private void Awake()
    {

        flagButton.onClick.AddListener(() => {
            // Mark or de-mark as fraudulent
            MarkAsFlagged();
        });

        templateButton.onClick.AddListener(() =>
        {
            ClickEmail();
        });

        // Setting initial aspect
        Color whiteBackground = Color.white;
        background.color = whiteBackground;

        readIcon.sprite = readFalseIcon;
    }


    private void ClickEmail()
    {
        // Open the Email Window and send the email info to it
        emailWindowUI.GetComponent<EmailWindowUI>().SetEmailInfo(associatedEmail);
        emailWindowUI.SetActive(true);
        openedEmailTaskButton.SetActive(true);
    }

    public void AssociateEmail(EmailData email)
    {
        if (email != null)
        {
            associatedEmail = email;

            senderName.text = associatedEmail.profileData.senderName;
            subject.text = associatedEmail.profileData.email;
            dateReceived.text = "12/02/2000";
        }
    }

    
    public void MarkAsFlagged()
    {
        if (isFlagged) 
        {
            //Change to not flagged = white background
            Color whiteBackground = Color.white;
            background.color = whiteBackground;

        } else
        {
            //Change to flagged = red background
            Color redBackground = Color.red;
            background.color = redBackground;
        }

        isFlagged = !isFlagged;
    }

    public void MarkAsRead()
    {
        if (!isRead)
        {
            readIcon.sprite = readTrueIcon;
        }
    }
}
