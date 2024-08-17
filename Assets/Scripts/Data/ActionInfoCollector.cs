using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using Varjo.XR;
public class ActionInfoCollector : MonoBehaviour
{

    public string FileDescriptor = "ActionInfo";

    string participantID;

    public List<ActionInfoCollectorItem> actionInfoItems;

    string fileLocationPreset = "C:/Users/Season/Documents/SwEYEpe/Assets/Data/";

    float trialStartTime;

    string conditionType = "";

    private GameManager gameManager;

    private ExperimentManager experimentManager;

    private GlobalSettings globalSettings;

    private HandTapDetector handTapDetector;

    private GazeClickDetector gazeClickDetector;

    private SwypeDetector swypeDetector;

    private int configuration;

    private TMPro.TMP_InputField currentText;

    public bool HandTap = false;

    public bool GazePinch = false;

    public bool Sweyepe = false;

    // Start is called before the first frame update
    void Start()
    {
        actionInfoItems = new List<ActionInfoCollectorItem>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        experimentManager = GameObject.Find("ExperimentManager").GetComponent<ExperimentManager>();
        globalSettings = GameObject.Find("GlobalSettings").GetComponent<GlobalSettings>();
        handTapDetector = GameObject.Find("GlobalSettings").GetComponent<HandTapDetector>();
        gazeClickDetector = GameObject.Find("GlobalSettings").GetComponent<GazeClickDetector>();
        swypeDetector = GameObject.Find("GlobalSettings").GetComponent<SwypeDetector>();
        participantID = experimentManager.participantID;
        configuration = experimentManager.configuration;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // create a new action info item
        ActionInfoCollectorItem actionInfoItem = new ActionInfoCollectorItem();

        actionInfoItem.absoluteTime = LSL.LSL.local_clock();
        actionInfoItem.trialTime = Time.time - trialStartTime;
        actionInfoItem.deltaTime = Time.deltaTime;

        // get the configuration
        configuration = experimentManager.configuration;
        // get the current text
        currentText = globalSettings.keyboardOutputText;

        actionInfoItem.trialIndex = configuration;
        actionInfoItem.currentText = currentText.text;
        actionInfoItem.targetText = experimentManager.targetSentences[configuration];

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
        else if( gameManager.initStateController.gameObject.activeSelf)
        {
            conditionType = "InitState";
        }
        else if (gameManager.endStateController.gameObject.activeSelf)
        {
            conditionType = "EndState";
        }
        else if(gameManager.calibrationStateController.gameObject.activeSelf)
        {
            conditionType = "WellComeState";
        }
        else if (gameManager.introductionInstructionStateController.gameObject.activeSelf)
        {
            conditionType = "TrialIntroState";
        }
        else if (gameManager.startStateController.gameObject.activeSelf)
        {
            conditionType = "EyeCalibrationState";
        }
        else if(gameManager.initStateController.gameObject.activeSelf)
        {
            conditionType = "InitState";
        }
        else 
        {
            conditionType = "TechniqueIntroStates";
        }

        actionInfoItem.conditionType = conditionType;

        // detect the action and collect the information
        if(conditionType == "HandTap")
        {
            if(handTapDetector.keyPressed)
            {
                actionInfoItem.eventType = "HandTap";
                actionInfoItem.keyboardValue = handTapDetector.keyValue;
                actionInfoItem.xKeyHitLocal = float.NegativeInfinity;
                actionInfoItem.yKeyHitLocal = float.NegativeInfinity;
            }
            else
            {
                actionInfoItem.eventType = "None";
                actionInfoItem.keyboardValue = "";
                actionInfoItem.xKeyHitLocal = float.NegativeInfinity;
                actionInfoItem.yKeyHitLocal = float.NegativeInfinity;
            }

            actionInfoItem.candidate1 = "";
            actionInfoItem.candidate2 = "";
            actionInfoItem.candidate3 = "";
            actionInfoItem.candidate4 = "";
            // actionInfoItem.eyeTrackingStatus = "NotAvailable/Recalibrating";
        }
        else if(conditionType == "GazePinch")
        {
            if(gazeClickDetector.keyPressed)
            {
                actionInfoItem.eventType = "GazePinch";
                actionInfoItem.keyboardValue = gazeClickDetector.keyValue;
                actionInfoItem.xKeyHitLocal = gazeClickDetector.tapPosition.x;
                actionInfoItem.yKeyHitLocal = gazeClickDetector.tapPosition.y;
            }
            else
            {
                actionInfoItem.eventType = "None";
                actionInfoItem.keyboardValue = "";
                actionInfoItem.xKeyHitLocal = float.NegativeInfinity;
                actionInfoItem.yKeyHitLocal = float.NegativeInfinity;
            }

            actionInfoItem.candidate1 = "";
            actionInfoItem.candidate2 = "";
            actionInfoItem.candidate3 = "";
            actionInfoItem.candidate4 = "";
        }
        else if(conditionType == "Sweyepe")
        {
            if(swypeDetector.keyPressed)
            {
                actionInfoItem.eventType = "Sweyepe";
                actionInfoItem.keyboardValue = "";
                actionInfoItem.xKeyHitLocal = swypeDetector.tapPosition.x;
                actionInfoItem.yKeyHitLocal = swypeDetector.tapPosition.y;
            }
            else if (swypeDetector.keyPinched)
            {
                actionInfoItem.eventType = "GazePinch";
                actionInfoItem.keyboardValue = swypeDetector.keyValue;
                actionInfoItem.xKeyHitLocal = swypeDetector.tapPosition.x;
                actionInfoItem.yKeyHitLocal = swypeDetector.tapPosition.y;
            }
            else
            {
                actionInfoItem.eventType = "None";
                actionInfoItem.keyboardValue = "";
                actionInfoItem.xKeyHitLocal = float.NegativeInfinity;
                actionInfoItem.yKeyHitLocal = float.NegativeInfinity;
            }

            actionInfoItem.candidate1 = swypeDetector.candidate1;
            actionInfoItem.candidate2 = swypeDetector.candidate2;
            actionInfoItem.candidate3 = swypeDetector.candidate3;
            actionInfoItem.candidate4 = swypeDetector.candidate4;

        
        }
        else
        {
            // do nothing
            actionInfoItem.eventType = "None";
            actionInfoItem.keyboardValue = "";
            actionInfoItem.xKeyHitLocal = float.NegativeInfinity;
            actionInfoItem.yKeyHitLocal = float.NegativeInfinity;

            actionInfoItem.candidate1 = "";
            actionInfoItem.candidate2 = "";
            actionInfoItem.candidate3 = "";
            actionInfoItem.candidate4 = "";

        }

        if(VarjoEyeTracking.IsGazeAllowed() && VarjoEyeTracking.IsGazeCalibrated())
        {
            actionInfoItem.eyeTrackingStatus = "Available";
        }
        else
        {
            actionInfoItem.eyeTrackingStatus = "NotAvailable/Recalibrating";
        }

        actionInfoItems.Add(actionInfoItem);
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


        string fileLocation = fileLocationPreset + participantID + "/"+ experimentManager.trial.ToString()+"/"+ ModeName + "/";
        Debug.Log(actionInfoItems.Count);

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

        file.WriteLine("absoluteTime,trialTime,deltaTime,trialIndex,eventType,keyboardValue,xKeyHitLocal,yKeyHitLocal,candidate1,candidate2,candidate3,conditionType,currentText,targetText,eyeTrackingStatus");
        foreach(ActionInfoCollectorItem dataItem in actionInfoItems)
        {
            file.WriteLine(dataItem.absoluteTime + "," + dataItem.trialTime + "," + dataItem.deltaTime + "," + dataItem.trialIndex + "," + dataItem.eventType + "," + dataItem.keyboardValue + "," + dataItem.xKeyHitLocal + "," + dataItem.yKeyHitLocal + "," + dataItem.candidate1 + "," + dataItem.candidate2 + "," + dataItem.candidate3 + "," + dataItem.conditionType + "," + dataItem.currentText + "," + dataItem.targetText+ "," + dataItem.eyeTrackingStatus);
            file.Flush();
        }

        file.Close();
        actionInfoItems.Clear();
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

