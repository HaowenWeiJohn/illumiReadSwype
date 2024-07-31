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


    // the list should contain the set of target words
    public List<string> targetWordList;

    private int currentWordIndex = 0;

    private Vector3 keyboardLocalPosition;

    private Vector3 keyboardLocalRotation;

    // Start is called before the first frame update
    void Start()
    {
        keyboardLocalPosition = KeyBoard.transform.localPosition;
        keyboardLocalRotation = KeyBoard.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 forwardDirection = Camera.main.transform.forward;
        
        // Calculate the desired position with the height offset
        Vector3 desiredPosition = cameraPosition + forwardDirection * 0.5f;
        desiredPosition.y -= 0.05f;

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

        stateShift();
    }


    public override void enterState()
    {
        keyboardDewellTimeStateGUIController.EnableSelf();
        simpleFacingCameraCallbacks.enabled = false;
        palmUIPivot.SetActive(true);
        targetWordText.text = targetWordList[0];
        KeyBoard.SetActive(true);
        leftHandModel.SetActive(true);
        rightHandModel.SetActive(true);
        base.enterState();

    }

    public override void exitState()
    {
        keyboardDewellTimeStateGUIController.DisableSelf();
        simpleFacingCameraCallbacks.enabled = true;
        palmUIPivot.SetActive(false);
        targetWordText.text = "";
        KeyBoard.SetActive(false);
        leftHandModel.SetActive(false);
        rightHandModel.SetActive(false);
        KeyBoard.transform.localPosition = keyboardLocalPosition;
        KeyBoard.transform.localEulerAngles = keyboardLocalRotation;
        base.exitState();

    }

    public override void stateShift()
    {
        string outputText = keyboardDewellTimeStateGUIController.keyboardManager.outputField.text;
        if(currentWordIndex>=targetWordList.Count)
        {
            exitState();
        }
        else if (outputText==targetWordList[currentWordIndex] || Input.GetKeyDown(Presets.NextStateKey))
        {
            currentWordIndex += 1;
            if(currentWordIndex<targetWordList.Count)
            {
                targetWordText.text = targetWordList[currentWordIndex];
            }
            keyboardDewellTimeStateGUIController.keyboardManager.ClearOutputFieldText();
        }
        
        if(Input.GetKeyDown(Presets.InterruptKey))
        {
            base.currentState = Presets.State.InterruptState;
        }
    }


}
