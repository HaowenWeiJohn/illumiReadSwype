using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour
{
    [Header("Participant Information")]
    public string participantID;

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
    

    [Header("Data Collectors")]
    public List<DataCollector> alwaysOnDCs = new List<DataCollector>();

    [Header("Action Info collectors")]
    public List<ActionInfoCollector> actionInfoDCs = new List<ActionInfoCollector>();

    public List<List<DataCollector>> trialSpecificDCs = new List<List<DataCollector>>();

    [Header("Target word list")]
    public List<string> targetwords = new List<string>();

    [Header("Easy word list")]
    public List<string> easyPracticeWords = new List<string>();

    public List<string> easyWords = new List<string>();

    [Header("Hard word list")]
    public List<string> hardPracticeWords = new List<string>();
    public List<string> hardWords = new List<string>();
    // public variable for the current text configuration
    public int configuration;

    [Header("Trial count for experiment")]
    public int ExperimentTrialCount = 15;

    [Header("Trial count for practice")]
    public int PracticeTrialCount = 3;


    [Header("Study Techniques Trial Count (do not change)")]
    public int HandTapTrialCount = 0;

    public int GazePinchTrialCount = 0;

    public int SweyepeTrialCount = 0;

    

    void Awake()
    {
        acquireDataCollectors();

        // shuffle the word lists

        // easyWords = easyWords.OrderBy(word => Guid.NewGuid()).ToList();
        // hardWords = hardWords.OrderBy(word => Guid.NewGuid()).ToList();

        if(isPractice == false)
        {
            HandTapTrialCount = ExperimentTrialCount;
            GazePinchTrialCount = ExperimentTrialCount;
            SweyepeTrialCount = ExperimentTrialCount;

            if(gameMode == GameMode.Easy)
            {
                targetwords = easyWords;
            }
            else if(gameMode == GameMode.Hard)
            {
                targetwords = hardWords;
            }

            targetwords = targetwords.OrderBy(word => Guid.NewGuid()).ToList();

            if(targetwords.Count <(HandTapTrialCount+GazePinchTrialCount+SweyepeTrialCount))
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
                targetwords = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    easyPracticeWords = easyPracticeWords.OrderBy(word => Guid.NewGuid()).ToList();
                    targetwords.AddRange(easyPracticeWords);
                }
            }
            else if(gameMode == GameMode.Hard)
            {
                targetwords = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    hardPracticeWords = hardPracticeWords.OrderBy(word => Guid.NewGuid()).ToList();
                    targetwords.AddRange(hardPracticeWords);
                }
            }

            if(targetwords.Count < PracticeTrialCount * 3)
            {
                Debug.LogError("Not enough words in the practice target word list");
            }
        }
        
    }

    // void OnApplicationQuit()
    // {
    //     writeClearDataCollectors();
    // }



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