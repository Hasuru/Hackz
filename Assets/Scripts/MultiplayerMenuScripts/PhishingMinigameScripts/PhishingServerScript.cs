using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;


/** PhishingServerScript
 * 
 * Objective:   This Script serves to control and populate the overall game structure for the Server Player.
 *              It will populate the emails and suspect lists with the correct information.
 *              It will also keep track of the amount of fraudulent Emails clicked and their interactions.
 *              Also keeps track of the game's timer.
 * 
 * Where to find:   ServerUI gameobject
 * 
 * */
public class PhishingServerScript : NetworkBehaviour
{
    public static PhishingServerScript Instance { get; private set; }

    [Header("Email Related Container")]
    [SerializeField] private Transform emailContainer;
    [SerializeField] private Transform emailSingleTemplate;

    [Header("Suspect Related Container")]
    [SerializeField] private Transform suspectContainer;
    [SerializeField] private Transform suspectSingleTemplate;

    [Header("Suspect Related Container")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Game End UI")]
    [SerializeField] private GameObject finishUI;
    [SerializeField] private GameObject badBackground;
    [SerializeField] private GameObject goodBackground;
    [SerializeField] private TextMeshProUGUI resultText;

    private List<EmailData> emailList = new List<EmailData>();
    private List <SuspectData> suspectList = new List<SuspectData>();

    private int fraudulentEmailsDownloaded;
    private int authenticEmailsDownloaded;
    private float totalTime = 300.0f; //5 minutes for now
    private float currentTime;

    public int FraudulentEmailsDownloaded()
    {
        return fraudulentEmailsDownloaded;
    }

    public void IncrementFraudulent()
    {
        fraudulentEmailsDownloaded++;
    }

    public int AuthenticEmailsDownloaded()
    {
        return authenticEmailsDownloaded;
    }

    public void IncrementAuthentic()
    {
        authenticEmailsDownloaded++;
    }

    private void Awake()
    {
        Instance = this;

        emailList = new List<EmailData>();
        fraudulentEmailsDownloaded = 0;
        authenticEmailsDownloaded = 0;
    }

    private void Start()
    {
        // Populate the suspect list
        CreateSuspectsList();

        // Decide the suspect to find
        int randomIndex = UnityEngine.Random.Range(0, suspectList.Count);
        suspectList[randomIndex].isHacker = true;
        Debug.Log(GetHacker().firstName + " " + GetHacker().lastName);

        // Populate the email list
        CreateEmailList();

        // Timer countdown stuff
        currentTime = totalTime;
        StartCoroutine(CountdownTimer());
    }

    public SuspectData GetHacker()
    {
        foreach (SuspectData suspect in suspectList)
        {
            if (suspect.isHacker)
            {
                return suspect;
            }
        }
        return null;
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

    private void CreateEmailInfoStatic()
    {
        // The email accounts for the emails
        Profile profile1 = new Profile("A", "a@gmail.com", 1, "12/12/2012");
        Profile profile2 = new Profile("B", "b@gmail.com", 1, "12/12/2012");
        Profile profile3 = new Profile("C", "c@gmail.com", 1, "12/12/2012");
        Profile profile4 = new Profile("D", "d@gmail.com", 1, "12/12/2012");

        // Content fot the emails
        string content1 = "authentic";
        string content2 = "fraud";
        string content3 = "bonus";

        // The emails for the email list
        EmailData email1 = new EmailData(profile1, "subject1", "12/12/2024", content1, EmailType.Authentic);
        EmailData email2 = new EmailData(profile1, "subject2", "12/12/2024", content1, EmailType.Authentic);
        EmailData email3 = new EmailData(profile1, "subject3", "12/12/2024", content1, EmailType.Authentic);
        EmailData email4 = new EmailData(profile2, "subject4", "12/12/2024", content1, EmailType.Authentic);
        EmailData email5 = new EmailData(profile2, "subject5", "12/12/2024", content2, EmailType.Fraudulent);
        EmailData email6 = new EmailData(profile2, "subject6", "12/12/2024", content2, EmailType.Fraudulent);
        EmailData email7 = new EmailData(profile3, "subject7", "12/12/2024", content2, EmailType.Fraudulent);
        EmailData email8 = new EmailData(profile3, "subject8", "12/12/2024", content3, EmailType.Bonus);
        EmailData email9 = new EmailData(profile3, "subject9", "12/12/2024", content3, EmailType.Bonus);
        EmailData email10 = new EmailData(profile4, "subject10", "12/12/2024", content3, EmailType.Bonus);

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

        // Randomize list order
        RandomizeList(emailList);
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

        // Create 5 suspects for the minigame
        for (int i = 0; i < 5; i++)
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
        // Because we have 5 puzzle pieces for now, we shall have 5 suspects (possible answers)
        // Then, for each one, we have different puzzle pieces
        int[] puz1 = { 1, 2, 3, 4 };
        int[] puz2 = { 1, 2, 3, 5 };
        int[] puz3 = { 1, 2, 5, 4 };
        int[] puz4 = { 1, 5, 3, 4 };
        int[] puz5 = { 5, 2, 3, 4 };

        SuspectData sus1 = new SuspectData("John", "Tiger", "12/02/2000", puz1);
        SuspectData sus2 = new SuspectData("Carla", "Marla", "12/02/2000", puz2);
        SuspectData sus3 = new SuspectData("Josh", "Denver", "12/02/2000", puz3);
        SuspectData sus4 = new SuspectData("Michael", "Philips", "12/02/2000", puz4);
        SuspectData sus5 = new SuspectData("Sara", "Clover", "12/02/2000", puz5);

        suspectList.Add(sus1);
        suspectList.Add(sus2);
        suspectList.Add(sus3);
        suspectList.Add(sus4);
        suspectList.Add(sus5);

        // Randomize list order
        RandomizeList(suspectList);
    }

    public static void RandomizeList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int numberEntries = list.Count;

        while (numberEntries > 1)
        {
            numberEntries--;

            int k = rng.Next(numberEntries + 1);
            T value = list[k];
            list[k] = list[numberEntries];
            list[numberEntries] = value;
        }
    }


    IEnumerator CountdownTimer()
    {
        // Run while timer isnt 0
        while (currentTime > 0)
        {
            UpdateTimerDisplay();
            yield return new WaitForSeconds(1.0f); // Stop, wait a sec
            currentTime -= 1.0f; // Decrease the current time by 1 sec
        }

        // When Timer reaches zero actions:
        currentTime = 0;
        UpdateTimerDisplay();
        Debug.Log("Timer End");
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
