using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

    private void Awake() {
        Player.Instance.OnStarCollected += Player_OnStarCollected;
    }

    private void Player_OnStarCollected(object sender, Player.OnStarCollectedEventArgs e) {
        
    }

    private void DestroySelf() {
        
    }

}
