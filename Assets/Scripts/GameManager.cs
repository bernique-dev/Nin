using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ILoadable, ILoadingWaiter {

    public void Load() {
        //todo Load save if there is one
    }

    public void Begin() {
        //todo Starts :
        //todo - Game as new if no save
        //todo - Game as saved if save
    }
}