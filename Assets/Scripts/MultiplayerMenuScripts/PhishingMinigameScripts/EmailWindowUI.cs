using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailWindowUI : MonoBehaviour
{
    [Header("Field Attributes")]
    [SerializeField] private Image senderPhoto;
    [SerializeField] private TextMeshProUGUI dateProfileText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI emailText;
    [SerializeField] private TextMeshProUGUI subjectText;
    [SerializeField] private TextMeshProUGUI dateReceivedText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Button attachmentButton;


    public void SetEmailInfo(EmailData email)
    {
        dateProfileText.text = email.profileData.creationDate;
        nameText.text = email.profileData.senderName;
        emailText.text = email.profileData.email;
        subjectText.text = email.subject;
        dateReceivedText.text = email.receivedDate;
        contentText.text = email.content;
        if (email.hasAttachment)
        {
            attachmentButton.gameObject.SetActive(true);
        } else
        {
            attachmentButton.gameObject.SetActive(false);
        }
    }
}
