using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour {

    public Slider Volume;
    public AudioSource backgorundMusic;
	
	// Update is called once per frame
	void Update () {
        backgorundMusic.volume = Volume.value; // set music volume to value of slider
	}
}
