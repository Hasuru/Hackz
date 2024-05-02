using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    [SerializeField] private Image puzzleUIImage;

    [Header("Sprite for Attachments UI")]
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    [SerializeField] private Sprite sprite3;
    [SerializeField] private Sprite sprite4;
    [SerializeField] private Sprite sprite5;
    [SerializeField] private Sprite sprite6;

    private EmailData associatedEmail;

    private float moveSpeed = 0.1f;
    private float maxDeltaX = 10f;
    private float maxDeltaY = 10f;

    private bool isMessingWithCursor = false;


    private void Awake()
    {
        attachmentButton.onClick.AddListener(() =>
        {
            DownloadAttachment();
        });
    }


    /** 
     * This method defines the pieces and debuffs given by the attachments, depending on the email clicked and on the suspect
     * */
    private void DownloadAttachment()
    {
        if (associatedEmail != null)
        {
            // If the associated Email is authentic (has attachment), give authentic puzzle piece
            if (associatedEmail.type == EmailType.Authentic)
            {
                switch (PhishingServerScript.Instance.AuthenticEmailsDownloaded())
                {
                    case 0:
                        SetPuzzlePiece(0);
                        PhishingServerScript.Instance.IncrementAuthentic();
                        break;
                    case 1:
                        SetPuzzlePiece(1);
                        PhishingServerScript.Instance.IncrementAuthentic();
                        break;
                    case 2:
                        SetPuzzlePiece(2);
                        PhishingServerScript.Instance.IncrementAuthentic();
                        break;
                    case 3:
                        SetPuzzlePiece(3);
                        PhishingServerScript.Instance.IncrementAuthentic();
                        break;
                    default:
                        SetPuzzlePiece(1);
                        PhishingServerScript.Instance.IncrementAuthentic();
                        break;
                }
                puzzleUI.SetActive(true);
            }
            // If associated email is fraudulent, check amount of fraudulents downloaded until now and decide on what to do
            else if (associatedEmail.type == EmailType.Fraudulent)
            {
                switch (PhishingServerScript.Instance.FraudulentEmailsDownloaded())
                {
                    case 0:
                        SetFraudPiece();
                        break;
                    case 1:
                        //SetDebuff();
                        break;
                    case 2:
                        PhishingServerScript.Instance.EndGame(false);
                        break;
                    default:
                        
                        break;
                }
            }

            // Set the Puzzle Window visible
            puzzleUI.SetActive(true);
        }
    }

    private void SetPuzzlePiece(int index)
    {
        // We need to give a piece that was not yet utilized, so we check how many downloads for authentic pieces
        // The pieces are given by the order that the suspect pieces are placed in the array.
        SuspectData suspect = PhishingServerScript.Instance.GetHacker();

        Debug.Log("Reached here 1");

        if (suspect != null)
        {
            switch (suspect.puzzlePieces[index])
            {
                case 1:
                    puzzleUIImage.sprite = sprite1;
                    break;
                case 2:
                    puzzleUIImage.sprite = sprite2;
                    break;
                case 3:
                    puzzleUIImage.sprite = sprite3;
                    break;
                case 4:
                    puzzleUIImage.sprite = sprite4;
                    break;
                case 5:
                    puzzleUIImage.sprite = sprite5;
                    break;
                default:
                    puzzleUIImage.sprite = sprite1;
                    break;
            }
        }
    }

    private void SetFraudPiece()
    {
        int[] authenticPieces = PhishingServerScript.Instance.GetHacker().puzzlePieces;

        int missingPiece = FindMissingPiece(authenticPieces);

        if (missingPiece != -1)
        {
            switch (missingPiece)
            {
                case 1:
                    puzzleUIImage.sprite = sprite1;
                    break;
                case 2:
                    puzzleUIImage.sprite = sprite2;
                    break;
                case 3:
                    puzzleUIImage.sprite = sprite3;
                    break;
                case 4:
                    puzzleUIImage.sprite = sprite4;
                    break;
                case 5:
                    puzzleUIImage.sprite = sprite5;
                    break;
                default:
                    puzzleUIImage.sprite = sprite1;
                    break;
            }
        }
    }

    private int FindMissingPiece(int[] array)
    {
        for (int i = 1; i <= 5; i++)
        {
            bool found = false;
            foreach (int num in array)
            {
                if (num == i)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                return i;
            }
        }

        // If no missing piece is found, return -1
        return -1;
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
