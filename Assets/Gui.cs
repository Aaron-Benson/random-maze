using UnityEngine;
using System.Collections;

/// <summary>
/// Displays GUI describing controls for the game.
/// </summary>
public class Gui : MonoBehaviour {
	
	public void OnGUI() {
		int x = 20;
		int y = 20;
		GUI.Label (new Rect(x, y, 200, 20), "Controls:");
		GUI.Label (new Rect(x, y + 20, 200, 25), "W - Move up");
		GUI.Label (new Rect(x, y + 40, 200, 25), "A - Move left");
		GUI.Label (new Rect(x, y + 60, 200, 25), "S - Move down");
		GUI.Label (new Rect(x, y + 80, 200, 25), "D - Move right");
		GUI.Label (new Rect(x, y + 100, 250, 25), "U - Toggle rotation of maze");
		GUI.Label (new Rect(x, y + 120, 500, 25), "R - Reset game and generate a new maze");
		GUI.Label (new Rect(x, y + 140, 200, 25), "Escape - Quit game");
	}
}
