using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyParams
{







    public enum Keys
    {
        None = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        F = 6,
        G = 7,
        H = 8,
        I = 9,
        J = 10,
        K = 11,
        L = 12,
        M = 13,
        N = 14,
        O = 15,
        P = 16,
        Q = 17,
        R = 18,
        S = 19,
        T = 20,
        U = 21,
        V = 22,
        W = 23,
        X = 24,
        Y = 25,
        Z = 26,


        Apostrophe = 27,
        Dot = 28,

    }

    public static List<List<Keys>> CharsOrders = new List<List<Keys>> {
    new List<Keys> { Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P },
    new List<Keys> { Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.H, Keys.J, Keys.K, Keys.L },
    new List<Keys> { Keys.Z, Keys.X, Keys.C, Keys.V, Keys.B, Keys.N, Keys.M }
    };


    public static float KeyHorizontalSpace = 0.1f;
    public static float KeyVirticalSpace = 0.1f;

    public static float KeyWidth = 0.1f;
    public static float KeyHeight = 0.1f;






    public static Vector2[] CharOffset = new Vector2[]
    {
        new Vector2(KeyWidth/2.0f, KeyHeight*0.5f + 2*KeyVirticalSpace),
        new Vector2(KeyWidth/2.0f+0.02f, KeyHeight*0.5f + KeyVirticalSpace),
        new Vector2(KeyWidth/2.0f+0.08f, KeyHeight*0.5f)
            //new Vector2(0.5f, 2.5f),
            //new Vector2(0.75f, 1.5f),
            //new Vector2(0.8f, 0.5f)
    };


    public static Dictionary<Keys, int> KeysID = new Dictionary<Keys, int>
    {
        [Keys.None] = 0,
        [Keys.A] = 1,
        [Keys.B] = 2,
        [Keys.C] = 3,
        [Keys.D] = 4,
        [Keys.E] = 5,
        [Keys.F] = 6,
        [Keys.G] = 7,
        [Keys.H] = 8,
        [Keys.I] = 9,
        [Keys.J] = 10,
        [Keys.K] = 11,
        [Keys.L] = 12,
        [Keys.M] = 13,
        [Keys.N] = 14,
        [Keys.O] = 15,
        [Keys.P] = 16,
        [Keys.Q] = 17,
        [Keys.R] = 18,
        [Keys.S] = 19,
        [Keys.T] = 20,
        [Keys.U] = 21,
        [Keys.V] = 22,
        [Keys.W] = 23,
        [Keys.X] = 24,
        [Keys.Y] = 25,
        [Keys.Z] = 26,

        [Keys.Apostrophe] = 27,
        [Keys.Dot] = 28
    };

    public static Dictionary<int, Keys> IDKeys = new Dictionary<int, Keys>
    {
        [0] = Keys.None,
        [1] = Keys.A,
        [2] = Keys.B,
        [3] = Keys.C,
        [4] = Keys.D,
        [5] = Keys.E,
        [6] = Keys.F,
        [7] = Keys.G,
        [8] = Keys.H,
        [9] = Keys.I,
        [10] = Keys.J,
        [11] = Keys.K,
        [12] = Keys.L,
        [13] = Keys.M,
        [14] = Keys.N,
        [15] = Keys.O,
        [16] = Keys.P,
        [17] = Keys.Q,
        [18] = Keys.R,
        [19] = Keys.S,
        [20] = Keys.T,
        [21] = Keys.U,
        [22] = Keys.V,
        [23] = Keys.W,
        [24] = Keys.X,
        [25] = Keys.Y,
        [26] = Keys.Z,

        [27] = Keys.Apostrophe,
        [28] = Keys.Dot
    };


    public static Dictionary<Keys, string> KeysString = new Dictionary<Keys, string>
    {
        [Keys.None] = "",
        [Keys.A] = "A",
        [Keys.B] = "B",
        [Keys.C] = "C",
        [Keys.D] = "D",
        [Keys.E] = "E",
        [Keys.F] = "F",
        [Keys.G] = "G",
        [Keys.H] = "H",
        [Keys.I] = "I",
        [Keys.J] = "J",
        [Keys.K] = "K",
        [Keys.L] = "L",
        [Keys.M] = "M",
        [Keys.N] = "N",
        [Keys.O] = "O",
        [Keys.P] = "P",
        [Keys.Q] = "Q",
        [Keys.R] = "R",
        [Keys.S] = "S",
        [Keys.T] = "T",
        [Keys.U] = "U",
        [Keys.V] = "V",
        [Keys.W] = "W",
        [Keys.X] = "X",
        [Keys.Y] = "Y",
        [Keys.Z] = "Z",

        [Keys.Apostrophe] = "'",
        [Keys.Dot] = "."
    };



    // define an enum for different interaction modes
    public static Color KeyInactiveColor = Color.white;
    public static Color KeyActiveColor = Color.red;

    public static float KeyboardDwellActivateTime = 1.0f;


    public static string KeyTag = "Key";
    public static string KeyboardBackgroundTag = "KeyboardBackground";
    public static string KeyboardSuggestionStrip = "KeyboardSuggestionStrip";

}
