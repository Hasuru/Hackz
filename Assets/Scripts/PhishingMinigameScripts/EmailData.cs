using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmailType
{
    Fraudulent,
    Authentic,
    Bonus
}

public class EmailData
{
    public int id;
    public Profile profileData;
    public string subject;
    public string content;
    public string receivedDate;
    public bool hasAttachment;
    public EmailType type;
    public bool hasBeenDownloaded;


    public EmailData(int id, Profile profileData, string subject, string receivedDate, string content, EmailType type) 
    {
        this.id = id;
        this.profileData = profileData;
        this.subject = subject;
        this.content = content;
        this.receivedDate = receivedDate;
        this.type = type;

        if (type == EmailType.Bonus)
        {
            this.hasAttachment = false;
        }
        else
        {
            this.hasAttachment = true;
        }

        this.hasBeenDownloaded = false;
    }
}

public class Profile
{
    public string senderName;
    public string email;
    public int photoIndex;
    public string creationDate;


    public Profile(string senderName, string email, int photoIndex, string creationDate)
    {
        this.senderName = senderName;
        this.email = email;
        this.photoIndex = photoIndex;
        this.creationDate = creationDate;
    }
}
