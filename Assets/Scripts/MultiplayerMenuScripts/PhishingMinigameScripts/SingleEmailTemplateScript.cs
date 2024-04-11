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

    [Header("Assets to use")]
    [SerializeField] private Sprite readTrueIcon;
    [SerializeField] private Sprite readFalseIcon;
    [SerializeField] private Sprite flaggedTrueIcon;
    [SerializeField] private Sprite flaggedFalseIcon;

    private bool isRead;
    private bool isFlagged;


    private void Awake()
    {
        flagButton.onClick.AddListener(() => {
            // Mark or de-mark as fraudulent
        });

        isRead = false;
        isFlagged = false;

        // Setting initial aspect
        Color whiteBackground = Color.white;
        background.color = whiteBackground;

        readIcon.sprite = readFalseIcon;

    }

    
    public void markAsFlagged()
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

    public void markAsRead()
    {
        if (!isRead)
        {
            readIcon.sprite = readTrueIcon;
        }
    }
}
