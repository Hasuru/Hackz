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

    [Header("Related UI")]
    [SerializeField] private GameObject puzzleUI;

    [Header("Sprite for Attachments UI")]
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    [SerializeField] private Sprite sprite3;
    [SerializeField] private Sprite sprite4;
    [SerializeField] private Sprite sprite5;
    [SerializeField] private Sprite sprite6;


    private EmailData associatedEmail;


    private void Awake()
    {
        attachmentButton.onClick.AddListener(() =>
        {
            DownloadAttachment();
        });
    }


    private void DownloadAttachment()
    {
        if (associatedEmail == null)
        {
            // If the associated Email is authentic and has attachment, give authentic puzzle piece
            if (!associatedEmail.isFraudulent && associatedEmail.hasAttachment)
            {
                switch (associatedEmail.attachmentIndex)
                {
                    case 1:
                        puzzleUI.GetComponent<Image>().sprite = sprite1;
                        break;
                    case 2:
                        puzzleUI.GetComponent<Image>().sprite = sprite2;
                        break;
                    case 3:
                        puzzleUI.GetComponent<Image>().sprite = sprite3;
                        break;
                    case 4:
                        puzzleUI.GetComponent<Image>().sprite = sprite4;
                        break;
                    case 5:
                        puzzleUI.GetComponent<Image>().sprite = sprite5;
                        break;
                    case 6:
                        puzzleUI.GetComponent<Image>().sprite = sprite6;
                        break;
                    default:
                        puzzleUI.GetComponent<Image>().sprite = sprite1;
                        break;
                }

                puzzleUI.SetActive(true);
            }
            // If associated email is fraudulent, check amount of fraudulents downloaded until now
            else if (associatedEmail.isFraudulent)
            {

            }
        }
    }

    public void SetEmailInfo(EmailData email)
    {
        associatedEmail = email;

        dateProfileText.text = email.profileData.creationDate;
        nameText.text = email.profileData.senderName;
        emailText.text = email.profileData.email;
        subjectText.text = email.subject;
        dateReceivedText.text = email.receivedDate;
        contentText.text = email.content;

        // Alter Attachment button visability
        if (email.hasAttachment)
        {
            Color newColor = new Color(1f, 1f, 1f, 1f);
            attachmentButton.gameObject.GetComponent<Image>().color = newColor;
            attachmentButton.interactable = true;
            attachmentButton.gameObject.SetActive(true);
        } else
        {
            Color newColor = new Color(1f, 1f, 1f, 0.2f);
            attachmentButton.gameObject.GetComponent<Image>().color = newColor;
            attachmentButton.interactable = false;
            attachmentButton.gameObject.SetActive(true);
        }
    }
}
