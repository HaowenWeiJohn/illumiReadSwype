using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour
{
    public GameObject InputFieldCanvas;
    public TMP_InputField inputField;

    GameSettings gameSettings;

    // Start is called before the first frame update
    void Start()
    {

        gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();

        // put the input field canvas right distance away from the player
        Vector3 pos = InputFieldCanvas.transform.position;
        InputFieldCanvas.transform.position = new Vector3(pos.x, pos.y, gameSettings.distToPlayer);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddText(string text)
    {
        inputField.text += text;
    }
}
