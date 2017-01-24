using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Quits the application when the user hits the escape key. Restarts the application when the user hits the "R" key.
/// </summary>
public class ApplicationManager : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();

		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene("random-maze");
	}
}
