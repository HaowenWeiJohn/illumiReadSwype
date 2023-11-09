using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyBoardController : MonoBehaviour
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


    public GameObject keyPrefab;
    public Dictionary<Params.Keys, KeyController> keyControllers = new Dictionary<Params.Keys, KeyController>();
    

    void Start()
    {
        initKeyBoard();


    }

    void Update()
    {
        
    }

    public void initKeyBoard()
    {
        // init chars
        for (int i = 0; i< Params.CharsOrders.Count; i++)
        {
            for(int j = 0; j < Params.CharsOrders[i].Count; j ++) {

                float x = Params.CharOffset[i].x + j * Params.KeyHorizontalSpace;
                float y = Params.CharOffset[i].y;

                Vector3 position = new Vector3(x, y, 0);
                

                GameObject keyObject = Instantiate(keyPrefab, position, Quaternion.identity);
                keyObject.transform.SetParent(transform, false);
                KeyController thisKeyController = keyObject.GetComponent<KeyController>();
                thisKeyController.setKey(Params.CharsOrders[i][j]);
                keyControllers.Add(Params.CharsOrders[i][j], thisKeyController);
            }
        }
    }




}
