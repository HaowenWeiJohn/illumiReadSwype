using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class DataCollector : MonoBehaviour
{
    public string FileDescriptor;
    string participantID;

    public List<DataCollectorItem> dataItems;
    public Transform transformToTrack;

    string fileLocationPreset = "C:/Users/Season/Documents/SwEYEpe/Assets/Data/";
    // C:\Users\Season\Documents\SwEYEpe\Assets\Scripts\Data\DataCollector.cs

    float trialStartTime;

    string conditionType = "";

    private GameManager gameManager;

    private ExperimentManager experimentManager;

    private GlobalSettings globalSettings;

    // the configuration for the current trial
    private int configuration;

    private TMPro.TMP_InputField currentText;

    void Awake()
    {
        dataItems = new List<DataCollectorItem>();
        // find the game manager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        experimentManager = GameObject.Find("ExperimentManager").GetComponent<ExperimentManager>();
        globalSettings = GameObject.Find("GlobalSettings").GetComponent<GlobalSettings>();
        participantID = experimentManager.participantID;
        configuration = experimentManager.configuration;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // check the condition type:
        if(gameManager.keyboardDewellTimeStateController.gameObject.activeSelf)
        {
            conditionType = "HandTap";
        }
        else if(gameManager.KeyboardClickStateController.gameObject.activeSelf)
        {
            conditionType = "GazePinch";
        }
        else if (gameManager.keyboardIllumiReadSwypeStateController.gameObject.activeSelf)
        {
            conditionType = "Sweyepe";
        }
        else
        {
            conditionType = "Unknown";
        }

        // get the current text
        currentText = globalSettings.keyboardOutputText;

        configuration = experimentManager.configuration;

        // add to the data collector item
        if(transformToTrack.gameObject.activeSelf)
        {
            DataCollectorItem dataItem = new DataCollectorItem();

            // LSL absolute time
            dataItem.absoluteTime = LSL.LSL.local_clock();

            dataItem.trialTime = Time.time - trialStartTime;
            dataItem.deltaTime = Time.deltaTime;

            dataItem.trialIndex = configuration;

            dataItem.xPos = transformToTrack.position.x;
            dataItem.yPos = transformToTrack.position.y;
            dataItem.zPos = transformToTrack.position.z;

            dataItem.xRot = transformToTrack.rotation.x;
            dataItem.yRot = transformToTrack.rotation.y;
            dataItem.zRot = transformToTrack.rotation.z;

            dataItem.conditionType = conditionType;

            dataItem.currentText = currentText.text;
            dataItem.targetText = experimentManager.targetwords[configuration];
            
            dataItems.Add(dataItem);
        }
        else
        {
            DataCollectorItem dataItem = new DataCollectorItem();

            // LSL absolute time
            dataItem.absoluteTime = LSL.LSL.local_clock();

            dataItem.trialTime = Time.time - trialStartTime;
            dataItem.deltaTime = Time.deltaTime;

            dataItem.trialIndex = configuration;

            dataItem.xPos = float.NegativeInfinity;
            dataItem.yPos = float.NegativeInfinity;
            dataItem.zPos = float.NegativeInfinity;

            dataItem.xRot = float.NegativeInfinity;
            dataItem.yRot = float.NegativeInfinity;
            dataItem.zRot = float.NegativeInfinity;

            dataItem.conditionType = conditionType;

            dataItem.currentText = currentText.text;
            dataItem.targetText = experimentManager.targetwords[configuration];

            dataItems.Add(dataItem);
        }

        
    }

    public void WriteAndClear()
    {
        Debug.Log("writing for "+ this.gameObject.name);
        string fileName = FileDescriptor;
        string ModeName = experimentManager.gameMode.ToString();
        bool isPractice = experimentManager.isPractice;
        if(isPractice)
        {
            ModeName = ModeName + "Practice";
        }
        else
        {
            ModeName = ModeName + "Experiment";
        }

        string fileLocation = fileLocationPreset + participantID + "/Trials/" + ModeName + "/";
        Debug.Log(dataItems.Count);

        bool exists = System.IO.Directory.Exists(fileLocation);
        if(!exists)
        {
            System.IO.Directory.CreateDirectory(fileLocation);
        }

        if(System.IO.File.Exists(fileLocation + fileName+".csv"))
        {
            Debug.Log("File already exists");
            return; 
        }

        System.IO.StreamWriter file = new System.IO.StreamWriter(fileLocation + fileName + ".csv");

        file.WriteLine("absoluteTime,trialTime,deltaTime,trialIndex,xPos,yPos,zPos,xRot,yRot,zRot,conditionType,currentText,targetText");
        foreach(DataCollectorItem dataItem in dataItems)
        {
            file.WriteLine(dataItem.absoluteTime+","+dataItem.trialTime + "," + dataItem.deltaTime + "," +dataItem.trialIndex+","+ dataItem.xPos + "," + dataItem.yPos + "," + dataItem.zPos + "," + dataItem.xRot + "," + dataItem.yRot + "," + dataItem.zRot + "," + dataItem.conditionType + "," + dataItem.currentText + "," + dataItem.targetText);
            file.Flush();
        }

        file.Close();
        dataItems.Clear();
    }

    public void SetTrialStartTime(float time)
    {
        trialStartTime = time;
    }

    public void SetParticipantID(string id)
    {
        participantID = id;
    }
}
