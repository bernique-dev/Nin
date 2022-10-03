using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Devinision {

    public string nindaVersion;
    public string humanVersion;

    public string commentary;

    public bool isFoldedOut;

    public override string ToString() {
        return (nindaVersion.Length > 0 ? nindaVersion : "???")  + " - " + (humanVersion.Length > 0 ? humanVersion : "???"); 
    }
}
