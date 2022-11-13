using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DevinisionDisplayer : MonoBehaviour {

    public Devinision devinision {
        get {
            return m_devinision;
        }
        set {
            m_devinision = value;
            UpdateDisplay();
        }
    }
    [SerializeField] private Devinision m_devinision;

    public TMP_Text nindaText;
    public TMP_Text humanText;
    public TMP_Text commentaryText;

    public void UpdateDisplay() {
        nindaText.text = devinision.nindaVersion;
        humanText.text = devinision.humanVersion;
        commentaryText.text = devinision.commentary;
    }

}
