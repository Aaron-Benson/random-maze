using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the arrow key movement of the object. This will check for collisions with the maze created by
/// Create9x9Maze and will rotate the object according to input.
/// </summary>
public class MeshBehaviour : MonoBehaviour {

	int positionX = -4;
	int positionY = 4;
    int lastRotation = 3;

	// Update is called once per frame
	void Update () {

		// Handle input up. Translate and rotate if there is an opening.
		if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && coastIsClear (positionX, positionY - 1))
        {
			positionY--;

			if (lastRotation == 1) 
				transform.Rotate (Vector3.down * 180);
			else if (lastRotation == 4) 
				transform.Rotate(Vector3.up * 90);
			else if (lastRotation == 2) 
				transform.Rotate(Vector3.down * 90);
			
			transform.Translate(Vector3.back);
			transform.Rotate(Vector3.up * 90);
            lastRotation = 2;
        }

		// Handle input down. Translate and rotate if there is an opening.
		else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && coastIsClear(positionX, positionY + 1))
        {
			positionY++;

            if (lastRotation == 1)
                transform.Rotate(Vector3.down * 180);
            else if (lastRotation == 4)
                transform.Rotate(Vector3.up * 90);
            else if (lastRotation == 2)
				transform.Rotate(Vector3.down * 90);
            
            transform.Translate(Vector3.forward);
			transform.Rotate(Vector3.down * 90);
            lastRotation = 4;
        }

		// Handle input left. Translate and rotate if there is an opening.
		else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && coastIsClear(positionX - 1, positionY))
        {
			positionX--;

            if (lastRotation == 1)
                transform.Rotate(Vector3.down * 180);
            else if (lastRotation == 4)
                transform.Rotate(Vector3.up * 90);
            else if (lastRotation == 2)
                transform.Rotate(Vector3.down * 90);
			
            transform.Translate(Vector3.right);
			transform.Rotate(Vector3.up * 180);
            lastRotation = 1;
        }

		// Handle input right. Translate and rotate if there is an opening.
		else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && coastIsClear(positionX + 1, positionY))
        {
			positionX++;

            if (lastRotation == 1)
                transform.Rotate(Vector3.down * 180);
            else if (lastRotation == 4)
                transform.Rotate(Vector3.up * 90);
            else if (lastRotation == 2)
                transform.Rotate(Vector3.down * 90);
			
            transform.Translate(Vector3.left);
            lastRotation = 3;
        }
    }
	/// <summary>
	/// Checks if there is an opening in the maze grid at the coordinates (newX, newY).
	/// </summary>
	/// <returns><c>true</c>, if there is an opening in the maze grid at the coordinates (newX, newY), <c>false</c> otherwise.</returns>
	/// <param name="newX">Coordinate X in grid.</param>
	/// <param name="newY">Coordinate Y in grid.</param>
	bool coastIsClear(int newX, int newY)
    {
		// Get the Create9x9Maze component reference from the parent of this.
        int[,] mazeGrid = transform.parent.GetComponent<Create9x9Maze>().mazeGrid;

		// Get coordinates according to the maze grid.
        int mazeX = newX / 3;
        int mazeY = newY / 3;

		// Get coordinates according to individual maze tile cells.
        int cellX = newX % 3;
        int cellY = newY % 3;

		// Check that the coordinates are inside of the cell and maze.
        if (newX < -4 || (newY < 3 && newX < 0) || (newY > 5 && newX < 0))
            return false;
        if (newX > 30 || (newY < 21 && newX > 26) || (newY > 23 && newX > 26))
            return false;
        if (newX < 0 || newX > 3*9 || newY < 0 || newY > 3*9 || mazeX > 8 || mazeY > 8)
            return true;
		
        //Debug.Log(mazeGrid[mazeX, mazeY] + " " + newX + " " + newY);

		// Check that the maze tile at the maze grid coordinates to determine if there is an empty location.
        switch (mazeGrid[mazeX, mazeY])
        {
            case 0:
                if (cellX == 1 || cellY == 1)
                    return true;
                return false;
            case 1:
                if (cellX == 2 || cellY == 0 || cellY == 2)
                    return false;
                return true;
            case 2:
                if (cellX == 0 || cellX == 2 || cellY == 0)
                    return false;
                return true;
            case 3:
                if (cellX == 0 || cellX == 2 || cellY == 2)
                    return false;
                return true;
            case 4:
                if (cellX == 0 || cellY == 2 || cellY == 0)
                    return false;
                return true;
            case 5:
                if (cellX == 2 || cellY == 0 || (cellX == 0 && cellY == 2))
                    return false;
                return true;
            case 6:
                if (cellY == 2 || cellY == 0)
                    return false;
                return true;
            case 7:
                if (cellY == 0 || (cellX == 0 && cellY == 2) || (cellX == 2 && cellY == 2))
                    return false;
                return true;
            case 8:
                if (cellX == 2 || cellY == 2 || (cellX == 0 && cellY == 0))
                    return false;
                return true;
            case 9:
                if (cellX == 2 || (cellX == 0 && cellY == 0) || (cellX == 0 && cellY == 2))
                    return false;
                return true;
            case 10:
                if (cellY == 2 || (cellX == 0 && cellY == 0) || (cellX == 2 && cellY == 0))
                    return false;
                return true;
            case 11:
                return false;
            case 12:
                if (cellX == 0 || cellY == 0 || (cellX == 2 && cellY == 2))
                    return false;
                return true;
            case 13:
                if (cellX == 0 || cellX == 2)
                    return false;
                return true;
            case 14:
                if (cellX == 0 || cellY == 2 || (cellX == 2 && cellY == 0))
                    return false;
                return true;
            case 15:
                if (cellX == 0 || (cellX == 2 && cellY == 0) || (cellX == 2 && cellY == 2))
                    return false;
                return true;
        }
        return false;
	}
}
