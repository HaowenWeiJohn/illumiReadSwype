using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExcelDataReader;

public class ExperimentManager : MonoBehaviour
{
    public enum Trials
    {
        One=1,
        Two=2,
        Three=3,
        Four=4,
        Five=5
    }

    public enum Chiral
    {
        Left,
        Right
    }

    [Header("Participant Information")]
    public string participantID;

    public Trials trial;

    public Chiral chiral;

    // the test modes are: Easy, Hard, Practice
    // public string testMode;

    

    public enum GameMode
    {
        Easy,
        Hard
    }

    [Header("Game Mode Information")]
    public GameMode gameMode;

    public bool isPractice = false;

    public bool recordData = true;
    

    [Header("Target Sentences list")]
    public List<string> targetSentences = new List<string>();

    [Header("Easy Sentences list")]
    public List<string> easyPracticeSentences = new List<string>();

    private List<string> easySentences = new List<string>();

    [Header("Hard Sentences list")]
    public List<string> hardPracticeSentences = new List<string>();
    private List<string> hardSentences = new List<string>();
    // public variable for the current text configuration

    [Header("Sentence Index Configuration")]
    public int configuration;

    [Header("Trial count for experiment")]
    public int ExperimentTrialCount = 15;

    [Header("Trial count for practice")]
    public int PracticeTrialCount = 3;


    [Header("Study Techniques Trial Count (do not change)")]
    public int HandTapTrialCount = 0;

    public int GazePinchTrialCount = 0;

    public int SweyepeTrialCount = 0;


    [Header("Data Collectors")]
    public List<DataCollector> alwaysOnDCs = new List<DataCollector>();

    [Header("Action Info collectors")]
    public List<ActionInfoCollector> actionInfoDCs = new List<ActionInfoCollector>();

    public List<List<DataCollector>> trialSpecificDCs = new List<List<DataCollector>>();


    public string GetFileName()
    {
        switch(trial)
        {
            case Trials.One:
                return "session1-short-longe-sentences.xlsx";
            case Trials.Two:
                return "session2-short-longe-sentences.xlsx";
            case Trials.Three:
                return "session3-short-longe-sentences.xlsx";
            case Trials.Four:
                return "session4-short-longe-sentences.xlsx";
            case Trials.Five:
                return "session5-short-longe-sentences.xlsx";
            default:
                return "";
        }
    }

    

    void Awake()
    {
        acquireDataCollectors();

        string fileName = GetFileName();
        string relativePath = "ExcelPath/"+fileName;
        string fullPath = Path.Combine(Application.dataPath, relativePath);

        FileStream stream = File.Open(fullPath, FileMode.Open, FileAccess.Read);

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        easySentences = new List<string>();
        hardSentences = new List<string>();

        // create reader for the Excel file
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
            DataSet result = reader.AsDataSet();

            DataTable table = result.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                easySentences.Add(row[0].ToString());

                hardSentences.Add(row[1].ToString());
            }
        }

        // shuffle the word lists

        // easySentences = easySentences.OrderBy(word => Guid.NewGuid()).ToList();
        // hardSentences = hardSentences.OrderBy(word => Guid.NewGuid()).ToList();

        if(isPractice == false)
        {
            HandTapTrialCount = ExperimentTrialCount;
            GazePinchTrialCount = ExperimentTrialCount;
            SweyepeTrialCount = ExperimentTrialCount;

            if(gameMode == GameMode.Easy)
            {
                targetSentences = easySentences;
            }
            else if(gameMode == GameMode.Hard)
            {
                targetSentences = hardSentences;
            }

            targetSentences = targetSentences.OrderBy(word => Guid.NewGuid()).ToList();

            if(targetSentences.Count <(HandTapTrialCount+GazePinchTrialCount+SweyepeTrialCount))
            {
                Debug.LogError("Not enough words in the test target word list");
            }
        }
        else
        // when in practice mode 
        {
            HandTapTrialCount = PracticeTrialCount;
            GazePinchTrialCount = PracticeTrialCount;
            SweyepeTrialCount = PracticeTrialCount;

            if(gameMode == GameMode.Easy)
            {
                targetSentences = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    easyPracticeSentences = easyPracticeSentences.OrderBy(word => Guid.NewGuid()).ToList();
                    targetSentences.AddRange(easyPracticeSentences);
                }
            }
            else if(gameMode == GameMode.Hard)
            {
                targetSentences = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    hardPracticeSentences = hardPracticeSentences.OrderBy(word => Guid.NewGuid()).ToList();
                    targetSentences.AddRange(hardPracticeSentences);
                }
            }

            if(targetSentences.Count < PracticeTrialCount * 3)
            {
                Debug.LogError("Not enough words in the practice target word list");
            }
        }
        
    }

    void OnApplicationQuit()
    {
        if(recordData)
        {
            writeClearDataCollectors();
        }
        // writeClearDataCollectors();
    }



    void activateDataCollectors() 
    {
        // this is the start of the next trial
        // activate data collectors
        // set TrialID
        // set Trial Start Time
        float startTime = Time.time;

        Debug.Log("inside activate");

        foreach (DataCollector curr in alwaysOnDCs) 
        {
            curr.gameObject.SetActive(true);
            // curr.SetTrialID(configuration.ToString());
            curr.SetTrialStartTime(startTime);
        }

        foreach (ActionInfoCollector curr in actionInfoDCs) 
        {
            curr.gameObject.SetActive(true);
            // curr.SetTrialID(configuration.ToString());
            curr.SetTrialStartTime(startTime);
        }

    }

    public void writeClearDataCollectors()
    {
        Debug.Log("writing and clearing");
        foreach (DataCollector curr in alwaysOnDCs) 
        {
            curr.WriteAndClear();
            curr.gameObject.SetActive(false);
        }

        foreach (ActionInfoCollector curr in actionInfoDCs) 
        {
            curr.WriteAndClear();
            curr.gameObject.SetActive(false);
        }



        // foreach (DataCollector curr in trialSpecificDCs[configuration]) 
        // {
        //     curr.WriteAndClear();
        // }
        // trialSpecificDCs[configuration][0].gameObject.SetActive(false);
    }

    void acquireDataCollectors()
    {
        Debug.Log("getting data collectors");
        GameObject alwaysOnDCGB = GameObject.Find("DataCollectors");
        foreach (Transform child in alwaysOnDCGB.transform) 
        {
            DataCollector curr = child.gameObject.GetComponent<DataCollector>();
            alwaysOnDCs.Add(curr);
        }

        GameObject ActionInfoDCGB = GameObject.Find("ActionInfoCollector");
        ActionInfoCollector ActionInfo = ActionInfoDCGB.GetComponent<ActionInfoCollector>();
        actionInfoDCs.Add(ActionInfo);
    }
}