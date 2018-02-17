using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

	public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next scene in build order
        
    }

    public void QuitGame() {

        Debug.Log("Quit"); // For Testing
        Application.Quit();

    }

	
}

