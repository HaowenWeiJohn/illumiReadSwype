using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyboardController : MonoBehaviour
{
    //public KeyControllerArchive[] keyControllers = new KeyControllerArchive[26];

    //public float keySpacings = 1.1f;
    //public float rowOffset = -0.5f;
    //public float horizontalOffset = -2.5f;


    //public GameObject letterPrefab;
    //GameSettings gameSettings;

    //List<GameObject> keys = new List<GameObject>();

    //void Start()
    //{

    //    gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();

    //    // Instantiate the letters and put them in the right positions.
    //    for (int i = 0; i < ParamsArchive.LetterOrders.Count; i++)
    //    {
    //        float vertical_pos = keySpacings * i;

    //        for (int j = 0; j < ParamsArchive.LetterOrders[i].Count; j++)
    //        {
    //            // Instantiate and position the letter using the prefab.
    //            Vector3 position = new Vector3(keySpacings * j + rowOffset * i + horizontalOffset, vertical_pos, gameSettings.distToPlayer);
    //            GameObject letterObject = Instantiate(letterPrefab, position, Quaternion.identity);
    //            letterObject.GetComponent<KeyControllerArchive>().SetChar(ParamsArchive.LetterOrders[i][j]);
    //            letterObject.name = ParamsArchive.LetterOrders[i][j];

    //            // rotate to face the center, but flip so the letters are facing the camera.
    //            letterObject.transform.rotation = Quaternion.LookRotation(letterObject.transform.position);

    //            keys.Add(letterObject);
    //        }
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public UserInterfaceController userInterfaceController;
    public GameObject keyPrefab;
    public Dictionary<KeyParams.Keys, KeyController> keyControllers = new Dictionary<KeyParams.Keys, KeyController>();

    public Presets.InteractionMode interactionMode;


    public KeyController gazeKeyController;






    void Start()
    {
        InitKeyBoard();


    }

    void Update()
    {
        switch (interactionMode)
        {
            case Presets.InteractionMode.DwellTime:
                KeyboardDwellTimeCallback();
                break;
            case Presets.InteractionMode.ButtonClick:
                KeyboardButtonClickCallback();
                break;
            case Presets.InteractionMode.IllumiReadSwype:
                KeyboardIllumiReadSwypeCallback();
                break;
            case Presets.InteractionMode.FreeSwitch:
                KeyboardFreeSwitchCallback();
                break;

        }


        
        




    }

    public void InitKeyBoard()
    {
        // init chars
        for (int i = 0; i< KeyParams.CharsOrders.Count; i++)
        {
            for(int j = 0; j < KeyParams.CharsOrders[i].Count; j ++) {

                float x = KeyParams.CharOffset[i].x + j * KeyParams.KeyHorizontalSpace;
                float y = KeyParams.CharOffset[i].y;

                Vector3 position = new Vector3(x, y, 0);
                

                GameObject keyObject = Instantiate(keyPrefab, position, Quaternion.identity);
                keyObject.transform.SetParent(transform, false);
                KeyController thisKeyController = keyObject.GetComponent<KeyController>();
                thisKeyController.keyboardController = gameObject.GetComponent<KeyboardController>();
                thisKeyController.SetKey(KeyParams.CharsOrders[i][j]);
                thisKeyController.ClearGaze();
                keyControllers.Add(KeyParams.CharsOrders[i][j], thisKeyController);
            }
        }
    }

    public void MoveKeyBoard()
    {
        // controll with WASD


    }


    public void SetInteractionMode(Presets.InteractionMode interactionMode)
    {
        this.interactionMode = interactionMode;
    }



    public void SetGazeKey(KeyParams.Keys gazeKey)
    {
        gazeKeyController = keyControllers[gazeKey];
    }

    public void ClearGazeKey()
    {
        gazeKeyController = null;
    }


    public void UpdateKeyInput(KeyParams.Keys key)
    {
        userInterfaceController.UpdateKeyInput(key);
    }


    // callback functions
    public void KeyboardDwellTimeCallback()
    {

    }

    public void KeyboardButtonClickCallback()
    {

    }

    public void KeyboardIllumiReadSwypeCallback()
    {

    }

    public void KeyboardFreeSwitchCallback()
    {

    }

    public void ResetKeyboard()
    {
        foreach(KeyController keyController in keyControllers.Values)
        {
            keyController.ClearGaze();
            keyController.ResetKeyColor();
        }
    }


}
