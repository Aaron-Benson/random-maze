using UnityEngine;
using System.Collections;

/// <summary>
/// Creates a 9x9 maze where there is a valid path from the entrance to the exit.
/// </summary>
public class Create9x9Maze : MonoBehaviour {

    Object mazeLeftUpRightDown, mazeLeft, mazeDown, mazeUp, mazeRight, mazeLeftDown, mazeLeftRight, mazeLeftRightDown, mazeLeftUp, mazeLeftUpDown, mazeLeftUpRight, mazeNone, mazeRightDown, mazeUpDown, mazeUpRight, mazeUpRightDown;
    Object[] mazeTiles;
    public int[,] mazeGrid;
    int[] leftArr, upArr, rightArr, downArr;

    struct mazeIndex
    {
        public int x;
        public int y;
    }

    ArrayList listOfCheckedIndices = new ArrayList();

    // Use this for initialization
    void Start() {

		// Initializes maze tile objects.
        mazeTiles = new Object[16]; 
		mazeLeftUpRightDown = Resources.Load("leftuprightdown", typeof(GameObject));
        mazeTiles[0] = mazeLeftUpRightDown;
		mazeLeft = Resources.Load("left", typeof(GameObject));
        mazeTiles[1] = mazeLeft;
		mazeDown = Resources.Load("down", typeof(GameObject));
        mazeTiles[2] = mazeDown;
		mazeUp = Resources.Load("up", typeof(GameObject));
        mazeTiles[3] = mazeUp;
		mazeRight = Resources.Load("right", typeof(GameObject));
        mazeTiles[4] = mazeRight;
		mazeLeftDown = Resources.Load("leftdown", typeof(GameObject));
        mazeTiles[5] = mazeLeftDown;
		mazeLeftRight = Resources.Load("leftright", typeof(GameObject));
        mazeTiles[6] = mazeLeftRight;
		mazeLeftRightDown = Resources.Load("leftrightdown", typeof(GameObject));
        mazeTiles[7] = mazeLeftRightDown;
		mazeLeftUp = Resources.Load("leftup", typeof(GameObject));
        mazeTiles[8] = mazeLeftUp;
		mazeLeftUpDown = Resources.Load("leftupdown", typeof(GameObject));
        mazeTiles[9] = mazeLeftUpDown;
		mazeLeftUpRight = Resources.Load("leftupright", typeof(GameObject));
        mazeTiles[10] = mazeLeftUpRight;
		mazeNone = Resources.Load("none", typeof(GameObject));
        mazeTiles[11] = mazeNone;
		mazeRightDown = Resources.Load("rightdown", typeof(GameObject));
        mazeTiles[12] = mazeRightDown;
		mazeUpDown = Resources.Load("updown", typeof(GameObject));
        mazeTiles[13] = mazeUpDown;
		mazeUpRight = Resources.Load("upright", typeof(GameObject));
        mazeTiles[14] = mazeUpRight;
		mazeUpRightDown = Resources.Load("uprightdown", typeof(GameObject));
        mazeTiles[15] = mazeUpRightDown;

		// Indexes maze tiles on openings to the left, right, down, and up.
        leftArr = new int[9] { -1, 0, 4, 6, 7, 10, 12, 14, 15 };
        rightArr = new int[9] { -1, 0, 1, 5, 6, 7, 8, 9, 10 };
        downArr = new int[9] { -1, 0, 3, 8, 9, 10, 13, 14, 15 };
        upArr = new int[9] { -1, 0, 2, 5, 7, 9, 12, 13, 15 };

		// Creates a valid maze.
        bool validMaze = false;
        while (!validMaze)
        {
			// Initializes grid.
            mazeGrid = new int[9, 9];
            for (int i = 0; i < 81; i++)
                mazeGrid[i % 9, i / 9] = -1;

			// Places a left right maze tile in the middle of the grid.
            mazeGrid[4, 4] = 6;
            GameObject mazeTile = Instantiate(mazeTiles[6], new Vector3(4 * 3 - 12, 0, 4 * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;

			// Places a left right up down maze tile in the upper left hand corner and in the lower right hand corner.
            mazeGrid[0, 1] = 0;
            GameObject mazeTile1 = Instantiate(mazeTiles[0], new Vector3(1 * 3 - 12, 0, 0 * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile1.transform.parent = gameObject.transform;
            mazeGrid[8, 7] = 0;
            GameObject mazeTile2 = Instantiate(mazeTiles[0], new Vector3(7 * 3 - 12, 0, 8 * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile2.transform.parent = gameObject.transform;

			// Places tiles recursively from the center left right maze tile.
            RecursivelyPlaceTiles(3, 4, "right", mazeGrid);
            RecursivelyPlaceTiles(5, 4, "left", mazeGrid);

			// If there are any openings left in the maze grid, fill them with random tiles.
            for (int i = 0; i < 81; i++)
            {
                if (mazeGrid[i % 9, i / 9] == -1)
                {
                    mazeGrid[i % 9, i / 9] = Mathf.FloorToInt(Random.Range(0.0f, 15.999999f));
                    GameObject tile = Instantiate(mazeTiles[mazeGrid[i % 9, i / 9]], new Vector3((i / 9) * 3 - 12, 0, (i % 9) * 3 - 12), Quaternion.identity) as GameObject;
                    tile.transform.parent = gameObject.transform;
                }
            }

			// Initializes checked maze indexes.
            listOfCheckedIndices.Clear();
            mazeIndex mt = new mazeIndex();
            mt.x = -2;
            mt.y = -2;
            listOfCheckedIndices.Add(mt);

			// Checks that the maze is valid. If it is valid, the maze is complete. If it is not valid, destroy the maze and try again.
            if (validMazeTest(mazeGrid, 0, 1))
                validMaze = true;
            else 
                for (int g = 4; g < gameObject.transform.childCount; g++)
                    Destroy(gameObject.transform.GetChild(g).gameObject);
        }


    }

	/// <summary>
	/// Recursively places maze tiles on the grid.
	/// </summary>
	/// <param name="x">The x coordinate of the grid location to be filled.</param>
	/// <param name="y">The y coordinate of the grid location to be filled.</param>
	/// <param name="origin">The direction that the parent tile is in.</param>
	/// <param name="mazeGrid">The current maze grid.</param>
    void RecursivelyPlaceTiles(int x, int y, string origin, int[,] mazeGrid)
    {
		// Checks that the coordinates are valid.
        if (x < 0 || x > 8 || y < 0 || y > 8 || mazeGrid[x, y] != -1) 
			return;

		// Retrieves the status of the 4 neighbours surrounding the grid location to be filled.
        int trackOpenLeft, trackOpenRight, trackOpenUp, trackOpenDown, emptyGridLeft, emptyGridRight, emptyGridUp, emptyGridDown;
        trackOpenLeft = x > 0 && (mazeGrid[x - 1, y] == leftArr[1] || mazeGrid[x - 1, y] == leftArr[2] || mazeGrid[x - 1, y] == leftArr[3] || mazeGrid[x - 1, y] == leftArr[4] || mazeGrid[x - 1, y] == leftArr[5] || mazeGrid[x - 1, y] == leftArr[6] || mazeGrid[x - 1, y] == leftArr[7] || mazeGrid[x - 1, y] == leftArr[8]) ? 1 : 0;
        trackOpenRight = x < 8 && (mazeGrid[x + 1, y] == rightArr[1] || mazeGrid[x + 1, y] == rightArr[2] || mazeGrid[x + 1, y] == rightArr[3] || mazeGrid[x + 1, y] == rightArr[4] || mazeGrid[x + 1, y] == rightArr[5] || mazeGrid[x + 1, y] == rightArr[6] || mazeGrid[x + 1, y] == rightArr[7] || mazeGrid[x + 1, y] == rightArr[8]) ? 1 : 0;
        trackOpenUp = y > 0 && (mazeGrid[x, y - 1] == upArr[1] || mazeGrid[x, y - 1] == upArr[2] || mazeGrid[x, y - 1] == upArr[3] || mazeGrid[x, y - 1] == upArr[4] || mazeGrid[x, y - 1] == upArr[5] || mazeGrid[x, y - 1] == upArr[6] || mazeGrid[x, y - 1] == upArr[7] || mazeGrid[x, y - 1] == upArr[8]) ? 1 : 0;
        trackOpenDown = y < 8 && (mazeGrid[x, y + 1] == downArr[1] || mazeGrid[x, y + 1] == downArr[2] || mazeGrid[x, y + 1] == downArr[3] || mazeGrid[x, y + 1] == downArr[4] || mazeGrid[x, y + 1] == downArr[5] || mazeGrid[x, y + 1] == downArr[6] || mazeGrid[x, y + 1] == downArr[7] || mazeGrid[x, y + 1] == downArr[8]) ? 1 : 0;
        emptyGridLeft = x > 0 && mazeGrid[x - 1, y] == -1 ? 1 : 0;
        emptyGridRight = x < 8 && mazeGrid[x + 1, y] == -1 ? 1 : 0;
        emptyGridDown = y < 8 && mazeGrid[x, y + 1] == -1 ? 1 : 0;
        emptyGridUp = y > 0 && mazeGrid[x, y - 1] == -1 ? 1 : 0;

		// Determines the tile required according to the status of the 4 neighbours. Randomizes if the neighbour is empty. 
        bool requiresLeft = trackOpenLeft + trackOpenRight + trackOpenUp + trackOpenDown == 1 && "left" == origin;
        requiresLeft = requiresLeft || (trackOpenLeft == 1 && Random.Range(0, 1001) < 701) || (emptyGridLeft == 1 && Random.Range(0, 1001) < 400);
        bool requiresRight = trackOpenLeft + trackOpenRight + trackOpenUp + trackOpenDown == 1 && "right" == origin;
        requiresRight = requiresRight || (trackOpenRight == 1 && Random.Range(0, 1001) < 701) || (emptyGridRight == 1 && Random.Range(0, 1001) < 400);
        bool requiresUp = trackOpenLeft + trackOpenRight + trackOpenUp + trackOpenDown == 1 && "up" == origin;
        requiresUp = requiresUp || (trackOpenUp == 1 && Random.Range(0, 1001) < 701) || (emptyGridUp == 1 && Random.Range(0, 1001) < 400);
        bool requiresDown = trackOpenLeft + trackOpenRight + trackOpenUp + trackOpenDown == 1 && "down" == origin;
        requiresDown = requiresDown || (trackOpenDown == 1 && Random.Range(0, 1001) < 701) || (emptyGridDown == 1 && Random.Range(0, 1001) < 400);

		// Places the appropriate tile at the coordinates and makes a recursive call to continue building the maze.
        if (requiresLeft && requiresUp && requiresRight && requiresDown) {
            GameObject mazeTile = Instantiate(mazeTiles[0], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 0;
            RecursivelyPlaceTiles(x - 1, y, "right", mazeGrid);
            RecursivelyPlaceTiles(x + 1, y, "left", mazeGrid);
            RecursivelyPlaceTiles(x, y + 1, "up", mazeGrid);
            RecursivelyPlaceTiles(x, y - 1, "down", mazeGrid);
        } else if (requiresLeft && requiresUp && requiresRight) {
            GameObject mazeTile = Instantiate(mazeTiles[10], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 10;
            RecursivelyPlaceTiles(x - 1, y, "right", mazeGrid);
            RecursivelyPlaceTiles(x + 1, y, "left", mazeGrid);
            RecursivelyPlaceTiles(x, y - 1, "down", mazeGrid);
        } else if (requiresLeft && requiresUp && requiresDown) {
            GameObject mazeTile = Instantiate(mazeTiles[9], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 9;
            RecursivelyPlaceTiles(x - 1, y, "right", mazeGrid);
            RecursivelyPlaceTiles(x, y + 1, "up", mazeGrid);
            RecursivelyPlaceTiles(x, y - 1, "down", mazeGrid);
        } else if (requiresLeft && requiresDown && requiresRight) {
            GameObject mazeTile = Instantiate(mazeTiles[7], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 7;
            RecursivelyPlaceTiles(x - 1, y, "right", mazeGrid);
            RecursivelyPlaceTiles(x + 1, y, "left", mazeGrid);
            RecursivelyPlaceTiles(x, y + 1, "up", mazeGrid);
        } else if (requiresLeft && requiresUp) {
            GameObject mazeTile = Instantiate(mazeTiles[8], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 8;
            RecursivelyPlaceTiles(x - 1, y, "right", mazeGrid);
            RecursivelyPlaceTiles(x, y - 1, "down", mazeGrid);
        } else if (requiresLeft && requiresRight) {
            GameObject mazeTile = Instantiate(mazeTiles[6], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 6;
            RecursivelyPlaceTiles(x - 1, y, "right", mazeGrid);
            RecursivelyPlaceTiles(x + 1, y, "left", mazeGrid);
        } else if (requiresLeft && requiresDown) {
            GameObject mazeTile = Instantiate(mazeTiles[5], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 5;
            RecursivelyPlaceTiles(x - 1, y, "right", mazeGrid);
            RecursivelyPlaceTiles(x, y + 1, "up", mazeGrid);
        } else if (requiresLeft) {
            GameObject mazeTile = Instantiate(mazeTiles[1], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 1;
            RecursivelyPlaceTiles(x - 1, y, "right", mazeGrid);
        } else if (requiresUp && requiresRight && requiresDown) {
            GameObject mazeTile = Instantiate(mazeTiles[15], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 15;
            RecursivelyPlaceTiles(x + 1, y, "left", mazeGrid);
            RecursivelyPlaceTiles(x, y + 1, "up", mazeGrid);
            RecursivelyPlaceTiles(x, y - 1, "down", mazeGrid);
        } else if (requiresUp && requiresRight) {
            GameObject mazeTile = Instantiate(mazeTiles[14], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 14;
            RecursivelyPlaceTiles(x + 1, y, "left", mazeGrid);
            RecursivelyPlaceTiles(x, y - 1, "down", mazeGrid);
        } else if (requiresUp && requiresDown) {
            GameObject mazeTile = Instantiate(mazeTiles[13], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 13;
            RecursivelyPlaceTiles(x, y + 1, "up", mazeGrid);
            RecursivelyPlaceTiles(x, y - 1, "down", mazeGrid);
        } else if (requiresUp) {
            GameObject mazeTile = Instantiate(mazeTiles[3], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 3;
            RecursivelyPlaceTiles(x, y - 1, "down", mazeGrid);
        } else if (requiresRight && requiresDown) {
            GameObject mazeTile = Instantiate(mazeTiles[12], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 12;
            RecursivelyPlaceTiles(x + 1, y, "left", mazeGrid);
            RecursivelyPlaceTiles(x, y + 1, "up", mazeGrid);
        } else if (requiresRight) {
            GameObject mazeTile = Instantiate(mazeTiles[4], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 4;
            RecursivelyPlaceTiles(x + 1, y, "left", mazeGrid);
        } else if (requiresDown) {
            GameObject mazeTile = Instantiate(mazeTiles[2], new Vector3(y * 3 - 12, 0, x * 3 - 12), Quaternion.identity) as GameObject;
            mazeTile.transform.parent = gameObject.transform;
            mazeGrid[x, y] = 2;
            RecursivelyPlaceTiles(x, y + 1, "up", mazeGrid);
        }
        return;
    }

	/// <summary>
	/// Checks that the maze can be navigated.
	/// </summary>
	/// <returns><c>true</c>, if the maze can be navigated starting from coordinates (currentX, currentY), <c>false</c> otherwise.</returns>
	/// <param name="mazeGrid">The current maze grid.</param>
	/// <param name="currentX">The current x coordinate starting location.</param>
	/// <param name="currentY">the current y coordinate starting location.</param>
    bool validMazeTest(int[,] mazeGrid, int currentX, int currentY)
    {
		// Returns true if the coordinates being considered are the exit location
        if (currentX == 8 && currentY == 7)
            return true;

		// Returns false if the coordinates being considered are outside of the maze.
        if (currentX < 0 || currentY < 0 || currentX > 8 || currentY > 8)
            return false;

		// Checks that the coordinate being examined has not already been examined.
        foreach (mazeIndex index in listOfCheckedIndices)
            if (index.x == currentX && index.y == currentY)
                return false;
        
		// Adds the current index to the checked index list so it is not checked again.
        mazeIndex currIndex = new mazeIndex();
        currIndex.x = currentX;
        currIndex.y = currentY;
        listOfCheckedIndices.Add(currIndex);

		// Retrieves the status of the 4 neighbours surrounding the grid location.
        int x = currentX;
        int y = currentY;
        int trackOpenLeft = x > 0 && (mazeGrid[x - 1, y] == leftArr[1] || mazeGrid[x - 1, y] == leftArr[2] || mazeGrid[x - 1, y] == leftArr[3] || mazeGrid[x - 1, y] == leftArr[4] || mazeGrid[x - 1, y] == leftArr[5] || mazeGrid[x - 1, y] == leftArr[6] || mazeGrid[x - 1, y] == leftArr[7] || mazeGrid[x - 1, y] == leftArr[8]) ? 1 : 0;
        int trackOpenRight = x < 8 && (mazeGrid[x + 1, y] == rightArr[1] || mazeGrid[x + 1, y] == rightArr[2] || mazeGrid[x + 1, y] == rightArr[3] || mazeGrid[x + 1, y] == rightArr[4] || mazeGrid[x + 1, y] == rightArr[5] || mazeGrid[x + 1, y] == rightArr[6] || mazeGrid[x + 1, y] == rightArr[7] || mazeGrid[x + 1, y] == rightArr[8]) ? 1 : 0;
        int trackOpenUp = y > 0 && (mazeGrid[x, y - 1] == upArr[1] || mazeGrid[x, y - 1] == upArr[2] || mazeGrid[x, y - 1] == upArr[3] || mazeGrid[x, y - 1] == upArr[4] || mazeGrid[x, y - 1] == upArr[5] || mazeGrid[x, y - 1] == upArr[6] || mazeGrid[x, y - 1] == upArr[7] || mazeGrid[x, y - 1] == upArr[8]) ? 1 : 0;
        int trackOpenDown = y < 8 && (mazeGrid[x, y + 1] == downArr[1] || mazeGrid[x, y + 1] == downArr[2] || mazeGrid[x, y + 1] == downArr[3] || mazeGrid[x, y + 1] == downArr[4] || mazeGrid[x, y + 1] == downArr[5] || mazeGrid[x, y + 1] == downArr[6] || mazeGrid[x, y + 1] == downArr[7] || mazeGrid[x, y + 1] == downArr[8]) ? 1 : 0;

		bool status = false;

		// Checks the current tile and status of the neighbours to recursively move to every possible location in the maze to determine if there is an exit.
        if (mazeGrid[x, y] == 0) {
            if (trackOpenLeft == 1)
				status = status || validMazeTest(mazeGrid, currentX - 1, currentY);
            if (trackOpenRight == 1)
				status = status || validMazeTest(mazeGrid, currentX + 1, currentY);
            if (trackOpenUp == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY - 1);
            if (trackOpenDown == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY + 1); 
        } else if (mazeGrid[x, y] == 10) {
            if (trackOpenLeft == 1)
				status = status || validMazeTest(mazeGrid, currentX - 1, currentY);
            if (trackOpenRight == 1)
				status = status || validMazeTest(mazeGrid, currentX + 1, currentY);
            if (trackOpenUp == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY - 1);
        } else if (mazeGrid[x, y] == 9) {
            if (trackOpenLeft == 1)
				status = status || validMazeTest(mazeGrid, currentX - 1, currentY);
            if (trackOpenUp == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY - 1);
            if (trackOpenDown == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY + 1);
        } else if (mazeGrid[x, y] == 7) {
            if (trackOpenLeft == 1)
				status = status || validMazeTest(mazeGrid, currentX - 1, currentY);
            if (trackOpenRight == 1)
				status = status || validMazeTest(mazeGrid, currentX + 1, currentY);
            if (trackOpenDown == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY + 1);
        } else if (mazeGrid[x, y] == 8) {
            if (trackOpenLeft == 1)
				status = status || validMazeTest(mazeGrid, currentX - 1, currentY);
            if (trackOpenUp == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY - 1);
        } else if (mazeGrid[x, y] == 6) {
            if (trackOpenLeft == 1)
				status = status || validMazeTest(mazeGrid, currentX - 1, currentY);
            if (trackOpenRight == 1)
				status = status || validMazeTest(mazeGrid, currentX + 1, currentY);
        } else if (mazeGrid[x, y] == 5) {
            if (trackOpenLeft == 1)
				status = status || validMazeTest(mazeGrid, currentX - 1, currentY);
            if (trackOpenDown == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY + 1);
		} else if (mazeGrid[x, y] == 1) {
			status = false;
        } else if (mazeGrid[x, y] == 15) {
            if (trackOpenRight == 1)
				status = status || validMazeTest(mazeGrid, currentX + 1, currentY);
            if (trackOpenUp == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY - 1);
            if (trackOpenDown == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY + 1);
        } else if (mazeGrid[x, y] == 14) {
            if (trackOpenRight == 1)
				status = status || validMazeTest(mazeGrid, currentX + 1, currentY);
            if (trackOpenUp == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY - 1);
        } else if (mazeGrid[x, y] == 13) {
            if (trackOpenUp == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY - 1);
            if (trackOpenDown == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY + 1);
        } else if (mazeGrid[x, y] == 3) {
			status = false;
        } else if (mazeGrid[x, y] == 12) {
            if (trackOpenRight == 1)
				status = status || validMazeTest(mazeGrid, currentX + 1, currentY);
            if (trackOpenDown == 1)
				status = status || validMazeTest(mazeGrid, currentX, currentY + 1);
        } else if (mazeGrid[x, y] == 4) {
			status = false;
        } else if (mazeGrid[x, y] == 2) {
			status = false;
        }

        return status;
    }
}
