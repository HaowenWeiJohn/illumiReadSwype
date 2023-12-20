using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyboardInputFieldCanvasController : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_InputField inputField;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateKeyInput(KeyParams.Keys key)
    {
        inputField.text += KeyParams.KeysString[key];
    }

    public void ResetInputField()
    {
        inputField.text = "";
    }

    public void SetUserInputWithWordList(List<string> wordList)
    {

        string userInput = "";
        foreach (string word in wordList)
        {
            userInput += word + " ";
        }

        inputField.text = userInput;


    }

}
