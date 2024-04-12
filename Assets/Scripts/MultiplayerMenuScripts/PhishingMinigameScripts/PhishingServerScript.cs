using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PhishingServerScript : NetworkBehaviour
{
    public static PhishingServerScript Instance { get; private set; }

    [Header("Email Related Container")]
    [SerializeField] private Transform emailContainer;
    [SerializeField] private Transform emailSingleTemplate;

    [Header("Suspect Related Container")]
    [SerializeField] private Transform suspectContainer;
    [SerializeField] private Transform suspectSingleTemplate;

    private List<EmailData> emailList = new List<EmailData>();
    private List <SuspectData> suspectList = new List<SuspectData>();

    private void Awake()
    {
        Instance = this;

        emailList = new List<EmailData>();

        
    }

    private void Start()
    {
        CreateEmailList();
        CreateSuspectsList();
    }

    private void Update()
    {
        
    }

    private void CreateEmailList()
    {
        // Hide the template for the emails
        emailSingleTemplate.gameObject.SetActive(false);

        // Clean every email aside from the template one
        foreach (Transform child in emailContainer)
        {
            if (child == emailSingleTemplate) continue;

            Destroy(child.gameObject);
        }


        // Create the email info to be used in the game and keep it in emailList
        //CreateEmailInfoAlgorithm();
        CreateEmailInfoStatic();


        // Create 10 emails for the minigame
        for (int i = 0; i < 10; i++)
        {
            // Create the email in the container
            Transform currentCreatedEmail = Instantiate(emailSingleTemplate, emailContainer);

            // Change the email info
            EmailData currentEmailInfo = emailList[i];
            currentCreatedEmail.gameObject.GetComponent<SingleEmailTemplateScript>().AssociateEmail(currentEmailInfo);

            // Make the email visable
            currentCreatedEmail.gameObject.SetActive(true);
        }
    }

    private void CreateEmailInfoAlgorithm(Transform currentCreatedEmail)
    {

    }

    private void CreateEmailInfoStatic()
    {
        Profile profile1 = new Profile("A", "a@gmail.com", 1, "12/12/2012");
        Profile profile2 = new Profile("B", "b@gmail.com", 1, "12/12/2012");
        Profile profile3 = new Profile("C", "c@gmail.com", 1, "12/12/2012");
        Profile profile4 = new Profile("D", "d@gmail.com", 1, "12/12/2012");

        EmailData email1 = new EmailData(profile1, "subject1", "12/12/2024", "content1", true);
        EmailData email2 = new EmailData(profile1, "subject2", "12/12/2024", "content2", true);
        EmailData email3 = new EmailData(profile1, "subject3", "12/12/2024", "content3", true);
        EmailData email4 = new EmailData(profile2, "subject4", "12/12/2024", "content4", false);
        EmailData email5 = new EmailData(profile2, "subject5", "12/12/2024", "content5", false);
        EmailData email6 = new EmailData(profile2, "subject6", "12/12/2024", "content6", false);
        EmailData email7 = new EmailData(profile3, "subject7", "12/12/2024", "content7", true);
        EmailData email8 = new EmailData(profile3, "subject8", "12/12/2024", "content8", true);
        EmailData email9 = new EmailData(profile3, "subject9", "12/12/2024", "content9", true);
        EmailData email10 = new EmailData(profile4, "subject10", "12/12/2024", "content10", true);

        emailList.Add(email1);
        emailList.Add(email2);
        emailList.Add(email3);
        emailList.Add(email4);
        emailList.Add(email5);
        emailList.Add(email6);
        emailList.Add(email7);
        emailList.Add(email8);
        emailList.Add(email9);
        emailList.Add(email10);
    }

    private void CreateSuspectsList()
    {
        // Hide the template for the suspects
        suspectSingleTemplate.gameObject.SetActive(false);

        // Clean every suspect aside from the template one
        foreach (Transform child in suspectContainer)
        {
            if (child == suspectSingleTemplate) continue;

            Destroy(child.gameObject);
        }


        // Create the suspect info to be used in the game and keep it in suspectList
        CreateSuspectInfoStatic();


        // Create 4 suspects for the minigame
        for (int i = 0; i < 4; i++)
        {
            // Create the suspect in the container
            Transform currentCreatedSuspect = Instantiate(suspectSingleTemplate, suspectContainer);

            // Change the suspect info
            SuspectData currentSuspectInfo = suspectList[i];
            currentCreatedSuspect.gameObject.GetComponent<SingleSuspectTemplateScript>().AssociateSuspect(currentSuspectInfo);

            // Make the suspect visable
            currentCreatedSuspect.gameObject.SetActive(true);
        }
    }

    private void CreateSuspectInfoStatic()
    {
        SuspectData sus1 = new SuspectData("John", "Tiger", "12/02/2000");
        SuspectData sus2 = new SuspectData("Carla", "Marla", "12/02/2000");
        SuspectData sus3 = new SuspectData("Josh", "Denver", "12/02/2000");
        SuspectData sus4 = new SuspectData("Michael", "Philips", "12/02/2000");

        suspectList.Add(sus1);
        suspectList.Add(sus2);
        suspectList.Add(sus3);
        suspectList.Add(sus4);

    }

}
