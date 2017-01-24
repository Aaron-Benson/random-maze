using UnityEngine;
using System.Collections;

/// <summary>
/// Create object mesh: blue diamond.
/// </summary>
public class CreateObjectMesh : MonoBehaviour
{

    public float width = 1f;
    public float height = 1f;
    public float length = 1f;


    // Use this for initialization
    void Start()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        mesh.Clear();

        float length = 1f;
        float width = 1f;
        float height = 1f;

        Vector3 p0 = new Vector3(-length * 1f, -width * 1f, height * 1f);
        Vector3 p1 = new Vector3(length * .5f, -width * .5f, height * .5f);
        Vector3 p2 = new Vector3(length * .5f, -width * .5f, -height * .5f);
        Vector3 p3 = new Vector3(-length * .5f, -width * .5f, -height * .5f);

        Vector3 p4 = new Vector3(-length * .5f, width * .5f, height * .5f);
        Vector3 p5 = new Vector3(length * .5f, width * .5f, height * .5f);
        Vector3 p6 = new Vector3(length * 1f, width * 1f, -height * 1f);
        Vector3 p7 = new Vector3(-length * .5f, width * .5f, -height * .5f);

        Vector3[] vertices = new Vector3[]
        {
        	p0, p1, p2, p3,     //Bottom
    	    p4, p5, p1, p0,     //Front
            p7, p4, p0, p3,     //Left	
        	p6, p7, p3, p2,     //Back
        	p5, p6, p2, p1,     //Right
            p7, p6, p5, p4      //Top
        };


        int[] triangles = new int[]
        {
		    //Bottom
		    3, 1, 0,
            3, 2, 1,			

            //Front
		    3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
            3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

		    //Left
		    3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
            3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
 
		    //Back
		    3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
            3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
 
		    //Right
		    3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
            3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
 
		    //Top
		    3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
            3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
        };


        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
        	down, down, down, down,     //Bottom     
        	front, front, front, front, //Front   	
        	left, left, left, left,     //Left	        
	        back, back, back, back,     //Back
            right, right, right, right, //Right
	        up, up, up, up              //Top
        };


        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, 1f);
        Vector2 _11 = new Vector2(1f, 1f);

        Vector2[] uvs = new Vector2[]
        {
	    	
	    	_11, _01, _00, _10,     //Bottom
		    _11, _01, _00, _10,     //Front    	
	    	_11, _01, _00, _10,     //Left		    
		    _11, _01, _00, _10,     //Back
		    _11, _01, _00, _10,     //Right
		    _11, _01, _00, _10,     //Top
        };
        
        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();

		transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
    }
}