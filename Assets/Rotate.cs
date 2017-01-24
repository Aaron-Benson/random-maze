using UnityEngine;
using System.Collections;

/// <summary>
/// Allows the object to be rotated slowly on a toggle activated by the "U" key.
/// </summary>
public class Rotate : MonoBehaviour {

    bool isRotating = false;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.U))
            isRotating = !isRotating;

        if (isRotating)
            transform.Rotate(Vector3.up * Time.deltaTime * 20);
	}
}
