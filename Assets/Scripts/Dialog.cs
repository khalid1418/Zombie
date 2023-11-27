using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog 
{
    public struct DialogData
    {
        public string name;
        [TextArea(3, 10)]
        public string[] sentences;
    }
    public DialogData[] dialogArray;

    public string name;
    [TextArea(3,10)]
    public string[] sentences;

    
}
