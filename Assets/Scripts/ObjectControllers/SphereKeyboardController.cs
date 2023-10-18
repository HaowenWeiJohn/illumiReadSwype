using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class SphereKeyboardController : MonoBehaviour
{
    public KeyController[] keyControllers = new KeyController[26];

    public float keySpacings = 1.1f;
    public float rowOffset = -0.5f;
    public float horizontalOffset = -2.5f;


    public GameObject letterPrefab;
    GameSettings gameSettings;

    List<GameObject> keys = new List<GameObject>();

    void Start()
    {

        gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();

        // Instantiate the letters and put them in the right positions.
        for (int i = 0; i < Params.LetterOrders.Count; i ++)
        {
            float vertical_pos = keySpacings * i;

            for(int j = 0; j < Params.LetterOrders[i].Count; j ++)
            {
                // Instantiate and position the letter using the prefab.
                Vector3 position = new Vector3(keySpacings * j + rowOffset * i + horizontalOffset, vertical_pos, gameSettings.distToPlayer);
                GameObject letterObject = Instantiate(letterPrefab, position, Quaternion.identity);
                letterObject.GetComponent<KeyController>().SetChar(Params.LetterOrders[i][j]);
                letterObject.name = Params.LetterOrders[i][j];

                // rotate to face the center, but flip so the letters are facing the camera.
                letterObject.transform.rotation = Quaternion.LookRotation(letterObject.transform.position);

                keys.Add(letterObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
