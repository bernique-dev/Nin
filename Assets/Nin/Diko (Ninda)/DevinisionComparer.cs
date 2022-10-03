using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevinisionComparer : IComparer<Devinision> {
    public int Compare(Devinision x, Devinision y) {
        return string.Compare(x.nindaVersion, y.nindaVersion);
    }
}

