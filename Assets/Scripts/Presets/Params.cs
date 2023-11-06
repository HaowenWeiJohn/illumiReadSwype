using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Params
{

    public static List<List<string>> LetterOrders = new List<List<string>> {
        new List<string> { "Z", "X", "C", "V", "B", "N", "M" },
        new List<string> { "A", "S", "D", "F", "G", "H", "J", "K", "L" },
        new List<string> { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" }
    };


    // define an enum for different interaction modes
    public enum InteractionMode
    {
        DwellTime = 1,
        ButtonClick = 2,
        IllumiReadSwype = 3,
        FreeSwitch = 4
    }



}
