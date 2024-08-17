using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionInfoCollectorItem
{
    public ActionInfoCollectorItem()
    {
        absoluteTime = 0;
        trialTime = 0; 
        deltaTime = 0;
        trialIndex = 0;

        eventType = "";
        keyboardValue = "";
        xKeyHitLocal = 0;
        yKeyHitLocal = 0;

        candidate1 = "";
        candidate2 = "";
        candidate3 = "";
        candidate4 = "";

        conditionType = "";
        currentText = "";
        targetText = "";
        eyeTrackingStatus = "";

    }


    public double absoluteTime;

    public float trialTime;

    public float deltaTime;

    public int trialIndex;

    public string eventType;

    public string keyboardValue;

    public float xKeyHitLocal;

    public float yKeyHitLocal;
    public string candidate1;

    public string candidate2;

    public string candidate3;

    public string candidate4;

    public string conditionType;

    public string currentText;

    public string targetText;

    public string eyeTrackingStatus;

    
}
