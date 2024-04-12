using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerGeneralUI : MonoBehaviour
{
    [Header("EmailListWindow UI")]
    [SerializeField] private GameObject emailListWindowUI;
    [SerializeField] private Button minimizeEmailListBtn;
    [SerializeField] private Button emailListBtn;

    [Header("SuspectWindow UI")]
    [SerializeField] private GameObject suspectWindowUI;
    [SerializeField] private Button minimizeSuspectBtn;
    [SerializeField] private Button suspectBtn;

    [Header("EmailOpenWindow UI")]
    [SerializeField] private GameObject emailOpenWindowUI;
    [SerializeField] private Button minimizeEmailBtn;
    [SerializeField] private Button emailOpenBtn;

    private void Awake()
    {
        emailListWindowUI.SetActive(true);
        emailOpenWindowUI.SetActive(false);

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
}
