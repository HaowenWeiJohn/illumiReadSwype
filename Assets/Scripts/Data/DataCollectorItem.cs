using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollectorItem
{
    public DataCollectorItem()
    {
        absoluteTime = 0;
        trialTime = 0;
        deltaTime = 0;
        trialIndex = 0;
        xPos = 0;
        yPos = 0;
        zPos = 0;
        xRot = 0;
        yRot = 0;
        zRot = 0;
        
        conditionType = "";
        currentText = "";
        targetText = "";
    }

    public double absoluteTime;

    public float trialTime;
    public float deltaTime;

    public int trialIndex;

    public float xPos;

    public float yPos;

    public float zPos;

    public float xRot;

    public float yRot;

    public float zRot;

    public string conditionType;

    public string currentText;

    public string targetText;

}
