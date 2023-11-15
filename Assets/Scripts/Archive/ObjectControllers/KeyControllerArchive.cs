using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using UnityEngine.VFX;

public class KeyControllerArchive : MonoBehaviour
{
    // TPM text field
    public TextMeshProUGUI textMesh;
    public RawImage bgImage;

    public bool hasGazeThisFrame = false;
    public float dwellCounter;

    // add an audio source
    GameSettings gameSettings;
    InputFieldController inputFieldController;
    bool selected = false;

    ParamsArchive.InteractionMode prevInteractionMode;

    // Start is called before the first frame update
    void Start()
    {
        gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
        inputFieldController = GameObject.Find("UIController").GetComponent<InputFieldController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (prevInteractionMode != gameSettings.interactionMode)
        {
            // reset the dwell counter
            dwellCounter = 0;
            // set color to white
            bgImage.color = Color.white;
            // reset the selected flag
            selected = false;
        }

        // check the interaction mode
        if (gameSettings.interactionMode == ParamsArchive.InteractionMode.button)
        {
            ButtonInteraction();
        }

        else if (gameSettings.interactionMode == ParamsArchive.InteractionMode.dwellTime)
        {
            DwellTimeInteraction();
        }
        else if (gameSettings.interactionMode == ParamsArchive.InteractionMode.VEP)
        {
            VEPInteraction();
        }
        else
        {
            Debug.Log("Invalid interaction mode!");
        }

        prevInteractionMode = gameSettings.interactionMode;
        hasGazeThisFrame = false;  // reset the flag
    }

    public void SetChar(string character)
    {
        textMesh.text = character;
    }

    public void HasGaze()
    {
        hasGazeThisFrame = true;
    }

    public void clearGaze()
    {
        hasGazeThisFrame = false;
    }

    void ButtonInteraction()
    {
        if (hasGazeThisFrame)
        {
            if (!selected)
            {
                // play the audio clip
                AudioSource.PlayClipAtPoint(gameSettings.keyHoverAudioClip, transform.position);
            }
            // set bg image to red
            bgImage.color = Color.red;
            selected = true;

            // if the space key is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // play the audio clip
                AudioSource.PlayClipAtPoint(gameSettings.keyPressAudioClip, transform.position);
                inputFieldController.GetComponent<InputFieldController>().AddText(textMesh.text);
            }
        }
        else
        {
            bgImage.color = Color.white;
            selected = false;
        }
    }

    void DwellTimeInteraction()
    {
        if (hasGazeThisFrame)
        {
            if (!selected)
            {
                // play the audio clip
                AudioSource.PlayClipAtPoint(gameSettings.keyHoverAudioClip, transform.position);
            }

            // increment the dwell counter
            dwellCounter += Time.deltaTime;

            // lerp the color from white to red
            bgImage.color = Color.Lerp(Color.white, Color.red, dwellCounter / gameSettings.dwellActivateTime);

            // if the dwell counter is greater than the dwell time
            if (dwellCounter > gameSettings.dwellActivateTime)
            {
                // play the audio clip
                AudioSource.PlayClipAtPoint(gameSettings.keyPressAudioClip, transform.position);
                inputFieldController.GetComponent<InputFieldController>().AddText(textMesh.text);
                dwellCounter = 0;
            }
            // selected = true;
        }
        else
        {
            dwellCounter = 0;
            bgImage.color = Color.white;
            selected = false;
        }
    }

    void VEPInteraction()
    {
        // flash the bg image between white and light grey at random intervals
        if (Random.Range(0, 100) < 50)
        {
            bgImage.color = Color.Lerp(Color.white, Color.grey, Random.Range(0.0f, 0.2f));
        }
        else
        {
            bgImage.color = Color.white;
        }
    }
}
