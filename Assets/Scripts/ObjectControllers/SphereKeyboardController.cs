using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereKeyboardController : MonoBehaviour
{
    // Start is called before the first frame update


    //x = r* sin(Theta) * cos(phi);
    //y = r* sin(Theta) * sin(phi);
    //z = r* cos(Theta);

    public KeyController A;
    public KeyController B;
    public KeyController C;
    public KeyController D;
    public KeyController E;
    public KeyController F;
    public KeyController G;
    public KeyController H;
    public KeyController I;
    public KeyController J;
    public KeyController K;
    public KeyController L;
    public KeyController M;
    public KeyController N;
    public KeyController O;
    public KeyController P;
    public KeyController Q;
    public KeyController R;
    public KeyController S;
    public KeyController T;
    public KeyController U;
    public KeyController V;
    public KeyController W;
    public KeyController X;
    public KeyController Y;
    public KeyController Z;

    public KeyController[] keyControllers = new KeyController[26];

    public float keyHorizontalSpaceAngle = 0.1f;
    public float keyVerticalSpaceAngle = 0.1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
