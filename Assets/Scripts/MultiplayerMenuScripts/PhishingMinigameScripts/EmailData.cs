using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailData
{
    public Profile profileData;
    public string subject;
    public string content;
    public string receivedDate;
    public bool hasAttachment;
    public int attachmentIndex;
    public bool isFraudulent;


    public EmailData(Profile profileData, string subject, string receivedDate, string content, bool hasAttachment, bool isFraudulent) 
    {
        this.profileData = profileData;
        this.subject = subject;
        this.content = content;
        this.receivedDate = receivedDate;
        this.hasAttachment = hasAttachment;
        this.isFraudulent = isFraudulent;
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
