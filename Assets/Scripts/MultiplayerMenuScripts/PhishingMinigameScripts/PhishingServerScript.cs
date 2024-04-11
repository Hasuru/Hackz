using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PhishingServerScript : NetworkBehaviour
{
    public static PhishingServerScript Instance { get; private set; }

    [SerializeField] private Transform emailContainer;
    [SerializeField] private Transform emailSingleTemplate;

    private List<EmailData> emailList;

    private void Awake()
    {
        Instance = this;

        emailList = new List<EmailData>();

        
            CreateEmailList();
        
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

        EmailData email1 = new EmailData(profile1, "subject1", "content1", false);
        EmailData email2 = new EmailData(profile1, "subject2", "content2", false);
        EmailData email3 = new EmailData(profile1, "subject3", "content3", false);
        EmailData email4 = new EmailData(profile2, "subject4", "content4", false);
        EmailData email5 = new EmailData(profile2, "subject5", "content5", false);
        EmailData email6 = new EmailData(profile2, "subject6", "content6", false);
        EmailData email7 = new EmailData(profile3, "subject7", "content7", false);
        EmailData email8 = new EmailData(profile3, "subject8", "content8", false);
        EmailData email9 = new EmailData(profile3, "subject9", "content9", false);
        EmailData email10 = new EmailData(profile4, "subject10", "content10", false);

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

}
