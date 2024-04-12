using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerGeneralUI : MonoBehaviour
{
    [Header("EmailListWindow UI")]
    [SerializeField] private GameObject emailListWindowUI;
    [SerializeField] private Button minimizeEmailListBtn;
    [SerializeField] private Button emailListBtn; // Task Bar Button

    [Header("SuspectWindow UI")]
    [SerializeField] private GameObject suspectWindowUI;
    [SerializeField] private Button minimizeSuspectBtn;
    [SerializeField] private Button suspectBtn; // Task Bar Button

    [Header("EmailOpenWindow UI")]
    [SerializeField] private GameObject emailOpenWindowUI;
    [SerializeField] private Button minimizeEmailBtn;
    [SerializeField] private Button emailOpenBtn; // Task Bar Button

    private void Awake()
    {
        // Button functions
        minimizeEmailListBtn.onClick.AddListener(() =>
        {
            emailListWindowUI.SetActive(false);
        });

        emailListBtn.onClick.AddListener(() =>
        {
            emailListWindowUI.SetActive(!emailListWindowUI.activeInHierarchy);
        });

        minimizeSuspectBtn.onClick.AddListener(() =>
        {
            suspectWindowUI.SetActive(false);
        });

        suspectBtn.onClick.AddListener(() =>
        {
            suspectWindowUI.SetActive(!suspectWindowUI.activeInHierarchy);
        });

        minimizeEmailBtn.onClick.AddListener(() =>
        {
            emailOpenWindowUI.SetActive(false);
        });

        emailOpenBtn.onClick.AddListener(() =>
        {
            emailOpenWindowUI.SetActive(!emailOpenWindowUI.activeInHierarchy);
        });
    }

    private void Start()
    {
        // Set UI visability
        emailListWindowUI.SetActive(true);
        emailOpenWindowUI.SetActive(false);

        // Set Task Bar Button visibility
        emailListBtn.gameObject.SetActive(true);
        suspectBtn.gameObject.SetActive(true);
        emailOpenBtn.gameObject.SetActive(false);
    }
}
