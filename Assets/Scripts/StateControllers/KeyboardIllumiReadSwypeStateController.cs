using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardIllumiReadSwypeStateController : StateController
{




    public KeyboardIllumiReadSwypeStateGUIController keyboardIllumiReadSwypeStateGUIController;

    public GameObject KeyBoard;
    public TextMeshProUGUI targetWordText;

    public TextMeshProUGUI trialNumberText;

    private int StateEndIndex =0;

    private int currentWordIndex = 0;

    public ExperimentManager experimentManager;

    public AudioSource audioSource;

    public AudioClip correctInputClip;

    public GameObject leftPaintCursor;

    public GameObject rightPaintCursor;


    // Start is called before the first frame update
    void Start()
    {
        // experimentManager = GameObject.Find("ExperimentManager").GetComponent<ExperimentManager>();
        StateEndIndex = experimentManager.SweyepeTrialCount;
    }

    // Update is called once per frame
    void Update()
    {
        // base.Update();
        stateShift();
    }


    public override void enterState()
    {
        keyboardIllumiReadSwypeStateGUIController.EnableSelf();
        targetWordText.text = experimentManager.targetSentences[experimentManager.configuration];
        trialNumberText.text = "Trial: " + (experimentManager.configuration+1).ToString();
        KeyBoard.SetActive(true);
        // PaintCursor.SetActive(true);

        if(experimentManager.chiral.ToString() == "Left")
        {
            leftPaintCursor.SetActive(true);
            rightPaintCursor.SetActive(false);
        }
        else if (experimentManager.chiral.ToString() == "Right")
        {
            leftPaintCursor.SetActive(false);
            rightPaintCursor.SetActive(true);
        }


        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 forwardDirection = Camera.main.transform.forward;
        
        // Calculate the desired position with the height offset
        Vector3 desiredPosition = cameraPosition + forwardDirection * 0.7f;
        desiredPosition.y -= 0.03f;

        // Set the KeyBoard's position
        KeyBoard.transform.position = desiredPosition;

        // Make the KeyBoard face the camera with a 180-degree rotation on the Y-axis
        Vector3 lookDirection = cameraPosition - KeyBoard.transform.position;
        lookDirection.y = 0; // Keep the rotation only around the Y-axis

        // Calculate the rotation to face the camera with an upward tilt
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(10f, 180, 0);
        KeyBoard.transform.rotation = targetRotation;

        // Optionally, ensure the KeyBoard is only rotating around the Y-axis (vertical)
        KeyBoard.transform.rotation = Quaternion.Euler(KeyBoard.transform.rotation.eulerAngles.x, KeyBoard.transform.rotation.eulerAngles.y, 0);

        base.enterState();

    }

    public override void exitState()
    {
        keyboardIllumiReadSwypeStateGUIController.DisableSelf();
        targetWordText.text = "";
        trialNumberText.text = "";
        KeyBoard.SetActive(false);
        // PaintCursor.SetActive(false);
        leftPaintCursor.SetActive(false);
        rightPaintCursor.SetActive(false);
        base.exitState();

    }

    public override void stateShift()
    {
        string outputText = keyboardIllumiReadSwypeStateGUIController.keyboardManager.outputField.text;
        if(currentWordIndex>=StateEndIndex)
        {
            exitState();
        }
        else if (outputText.ToLower()==experimentManager.targetSentences[experimentManager.configuration].ToLower())
        {
            StartCoroutine(ConfirmCorrectInput());
        }
        else if (Input.GetKeyDown(Presets.NextStateKey))
        {
            if(experimentManager.configuration <= experimentManager.targetSentences.Count-1)
            {
                experimentManager.configuration += 1;
            }
            currentWordIndex += 1;
            if(currentWordIndex<StateEndIndex)
            {
                targetWordText.text = experimentManager.targetSentences[experimentManager.configuration];
                trialNumberText.text = "Trial: " + (experimentManager.configuration+1).ToString();
            }
            keyboardIllumiReadSwypeStateGUIController.keyboardManager.ClearOutputFieldText();
        }
        
        if(Input.GetKeyDown(Presets.InterruptKey))
        {
            base.currentState = Presets.State.InterruptState;
        }
    }

    private IEnumerator ConfirmCorrectInput()
    {
        // Change text color to green
        keyboardIllumiReadSwypeStateGUIController.keyboardManager.outputField.text = "<color=green>" + keyboardIllumiReadSwypeStateGUIController.keyboardManager.outputField.text + "</color>";
        
        // Play the correct input audio clip
        if (audioSource != null && correctInputClip != null)
        {
            audioSource.PlayOneShot(correctInputClip);
        }

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        if(experimentManager.configuration <= experimentManager.targetSentences.Count-1)
        {
            experimentManager.configuration += 1;
        }
        currentWordIndex += 1;
        if(currentWordIndex<StateEndIndex)
        {
            targetWordText.text = experimentManager.targetSentences[experimentManager.configuration];
            trialNumberText.text = "Trial: " + (experimentManager.configuration+1).ToString();
        }

        // Clear the output field text (or reset it if needed)
        keyboardIllumiReadSwypeStateGUIController.keyboardManager.ClearOutputFieldText();
        keyboardIllumiReadSwypeStateGUIController.keyboardManager.DeactivateSuggesitonKeys();
        keyboardIllumiReadSwypeStateGUIController.keyboardManager.ActivateSuggesitonKeys();
    }


}
