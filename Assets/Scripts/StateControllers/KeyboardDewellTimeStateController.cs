using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardDewellTimeStateController : StateController
{




    public KeyboardDewellTimeStateGUIController keyboardDewellTimeStateGUIController;

    public Leap.Unity.Interaction.SimpleFacingCameraCallbacks simpleFacingCameraCallbacks;

    public GameObject palmUIPivot;

    public GameObject leftHandModel;

    public GameObject rightHandModel;

    public GameObject KeyBoard;

    public TextMeshProUGUI targetWordText;


    private Vector3 keyboardLocalPosition;

    private Vector3 keyboardLocalRotation;

    public ExperimentManager experimentManager;

    private int StateEndIndex =0;

    private int currentWordIndex = 0;

    public AudioSource audioSource;

    public AudioClip correctInputClip;

    // Start is called before the first frame update
    void Start()
    {
        keyboardLocalPosition = KeyBoard.transform.localPosition;
        keyboardLocalRotation = KeyBoard.transform.localEulerAngles;

        // experimentManager = GameObject.Find("ExperimentManager").GetComponent<ExperimentManager>();
        StateEndIndex = experimentManager.HandTapTrialCount;
    }

    // Update is called once per frame
    void Update()
    {
        stateShift();
    }


    public override void enterState()
    {
        keyboardDewellTimeStateGUIController.EnableSelf();
        simpleFacingCameraCallbacks.enabled = false;
        palmUIPivot.SetActive(true);
        targetWordText.text = experimentManager.targetwords[experimentManager.configuration];
        KeyBoard.SetActive(true);
        // leftHandModel.SetActive(true);
        // rightHandModel.SetActive(true);

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 forwardDirection = Camera.main.transform.forward;
        
        // Calculate the desired position with the height offset
        Vector3 desiredPosition = cameraPosition + forwardDirection * 0.5f;
        desiredPosition.y -= 0.1f;

        // Set the KeyBoard's position
        KeyBoard.transform.position = desiredPosition;

        // Make the KeyBoard face the camera with a 180-degree rotation on the Y-axis
        Vector3 lookDirection = cameraPosition - KeyBoard.transform.position;
        lookDirection.y = 0; // Keep the rotation only around the Y-axis

        // Calculate the rotation to face the camera with an upward tilt
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(15f, 180, 0);
        KeyBoard.transform.rotation = targetRotation;

        // Optionally, ensure the KeyBoard is only rotating around the Y-axis (vertical)
        KeyBoard.transform.rotation = Quaternion.Euler(KeyBoard.transform.rotation.eulerAngles.x, KeyBoard.transform.rotation.eulerAngles.y, 0);

        base.enterState();

    }

    public override void exitState()
    {
        keyboardDewellTimeStateGUIController.DisableSelf();
        simpleFacingCameraCallbacks.enabled = true;
        palmUIPivot.SetActive(false);
        targetWordText.text = "";
        KeyBoard.SetActive(false);
        // leftHandModel.SetActive(true);
        // rightHandModel.SetActive(true);
        KeyBoard.transform.localPosition = keyboardLocalPosition;
        KeyBoard.transform.localEulerAngles = keyboardLocalRotation;
        base.exitState();

    }

    public override void stateShift()
    {
        string outputText = keyboardDewellTimeStateGUIController.keyboardManager.outputField.text;
        if(currentWordIndex>=StateEndIndex)
        {
            exitState();
        }
        else if (outputText.ToLower()==experimentManager.targetwords[experimentManager.configuration].ToLower() )
        {
            StartCoroutine(ConfirmCorrectInput());
        }
        else if (Input.GetKeyDown(Presets.NextStateKey))
        {
            if(experimentManager.configuration <= experimentManager.targetwords.Count-1)
            {
                experimentManager.configuration += 1;
            }
            currentWordIndex += 1;
            if(currentWordIndex<StateEndIndex)
            {
                targetWordText.text = experimentManager.targetwords[experimentManager.configuration];
            }
            keyboardDewellTimeStateGUIController.keyboardManager.ClearOutputFieldText();
        }
        
        if(Input.GetKeyDown(Presets.InterruptKey))
        {
            base.currentState = Presets.State.InterruptState;
        }
    }

    private IEnumerator ConfirmCorrectInput()
    {
        // Change text color to green
        keyboardDewellTimeStateGUIController.keyboardManager.outputField.text = "<color=green>" + keyboardDewellTimeStateGUIController.keyboardManager.outputField.text + "</color>";
        
        // Play the correct input audio clip
        if (audioSource != null && correctInputClip != null)
        {
            audioSource.PlayOneShot(correctInputClip);
        }

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        if(experimentManager.configuration <= experimentManager.targetwords.Count-1)
        {
            experimentManager.configuration += 1;
        }
        currentWordIndex += 1;
        if(currentWordIndex<StateEndIndex)
        {
            targetWordText.text = experimentManager.targetwords[experimentManager.configuration];
        }

        // Clear the output field text (or reset it if needed)
        keyboardDewellTimeStateGUIController.keyboardManager.ClearOutputFieldText();
    }


}
