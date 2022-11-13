using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DikoDisplayer : MonoBehaviour {

    public Diko diko;
    
    public bool updateInEditorMode;
    private int lastCount;
    private DateTime lastUpdateTime;

    public GameObject devinisionDisplayerPrefab;

    public void Start() {
        UpdateDevinisions();
    }

    private void Update() {
        if (updateInEditorMode && !Application.isPlaying) {
            if (lastCount != diko.devinisions.Count || diko.lastModificationDate > lastUpdateTime) {
                UpdateDevinisions();
                lastUpdateTime = DateTime.Now;
                lastCount = diko.devinisions.Count;
            }
        }
    }

    public void UpdateDevinisions() {
        int children = transform.childCount;
        for (int i = children - 1; i >= 0; i--) {
            //Debug.Log(transform.GetChild(i));
            if (Application.isPlaying) {
                Destroy(transform.GetChild(i).gameObject);
            } else {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        foreach (Devinision devinision in diko.devinisions) {
            //Debug.Log(devinision);
            GameObject instance = Instantiate(devinisionDisplayerPrefab, transform);
            instance.name = "devinision (" + devinision.ToString() + ")";
            DevinisionDisplayer devinisionDisplayer = instance.GetComponent<DevinisionDisplayer>();
            devinisionDisplayer.devinision = devinision;
        }
    }

}
