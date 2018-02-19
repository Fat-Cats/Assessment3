﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour {

    //public Slider Volume;
   
	// Update is called once per frame
	void Update () {
        AudioListener.volume = this.gameObject.GetComponent<Slider>().value; // set music volume to value of slider
	}
}
