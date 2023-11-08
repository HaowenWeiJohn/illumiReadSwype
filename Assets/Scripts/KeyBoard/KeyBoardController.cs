using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    void Start()
    {
        
    }

    void Update()
    {
        
    }

}
