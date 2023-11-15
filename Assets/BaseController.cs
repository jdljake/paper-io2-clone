using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class BaseController : MonoBehaviour
{
    //DEBUG
    public GameObject marker;
    //

    private PolygonCollider2D poly2D;
    private EdgeCollider2D trailCollider;
    public Material baseMat;
    
    bool trigEntered;

    private MeshRenderer renderer;
    private MeshFilter filter;
    private Vector2[] vertices2D;
    private Mesh baseMesh;

    // Start is called before the first frame update
    void Start()
    {
        trigEntered = false;
        trailCollider = GameObject.Find("TrailCollider").GetComponent<EdgeCollider2D>();
        poly2D = gameObject.AddComponent<PolygonCollider2D>(); //init polyCol
        poly2D.isTrigger = true;

        vertices2D = circleGen(8f, 30);
 
        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
 
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }
 
        // Create the mesh
        baseMesh = new Mesh();
        baseMesh.vertices = vertices;
        baseMesh.triangles = indices;
        baseMesh.RecalculateNormals();
        baseMesh.RecalculateBounds();
 
        // Set up game object with mesh;
        renderer = gameObject.AddComponent<MeshRenderer>();
        filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = baseMesh;
        renderer.material = baseMat;
        
        poly2D.points = vertices2D; //update polyCol

        // UpdateBase();
    }

    private Vector2[] circleGen(float radius, int numPoints){
        Vector2[] edgePoints = new Vector2[numPoints + 1];
       
        for(int loop = 0; loop <= numPoints; loop++)
        {
            float angle = (Mathf.PI * 2.0f / numPoints) * loop;
            edgePoints[loop] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        }
        return edgePoints;
    }
     

    void UpdateBase(){
        Vector2[] trailPoints = trailCollider.points;
        // Vector2 trailEnd = trailPoints[trailPoints.Length-1];
        Vector2 trailEnd = new Vector2(GameObject.Find("player").transform.position.x, GameObject.Find("player").transform.position.y);
        Vector2 trailStart = trailPoints[0];
        
        Vector2 startNode = new Vector2();
        double shortestDistanceStart = 100;
        foreach(Vector2 point in vertices2D){
            double currentDistance = Math.Sqrt(Math.Pow((point.x - trailStart.x), 2) + Math.Pow((point.y - trailStart.y), 2));
            if(currentDistance < shortestDistanceStart){
                startNode = point;
                shortestDistanceStart = currentDistance;
            }
        }
        Vector2 endNode = new Vector2();
        double shortestDistanceEnd = 100;
        foreach(Vector2 point in vertices2D){
            double currentDistance = Math.Sqrt(Math.Pow((point.x - trailEnd.x), 2) + Math.Pow((point.y - trailEnd.y), 2));
            if(currentDistance < shortestDistanceEnd){
                endNode = point;
                shortestDistanceEnd = currentDistance;
            }
        }

        List<Vector2> verticesList = new List<Vector2>(vertices2D);
        int index_startNode = verticesList.IndexOf(startNode);
        int index_endNode = verticesList.IndexOf(endNode);
        
        int smaller = 0;
        int larger = 0;
        if (index_startNode >= index_endNode){
            smaller = index_endNode;
            larger = index_startNode;
            // Debug.Log("TOP");
        } else {
            smaller = index_startNode;
            larger = index_endNode;
            // Debug.Log("BOT");
        }

//at this point have smaller and larger, the indicies on vector2D of trail intersect
//


        List<Vector2> newList = NewOutline_NormalRemove(index_startNode, smaller, larger, vertices2D, trailPoints);
        Mesh testMsh1 = CreateMesh(newList);

        List<Vector2> altList = NewOutline_AltRemove(index_startNode, smaller, larger, vertices2D, trailPoints);
        Mesh testMsh2 = CreateMesh(altList);

        if(CalculateSurfaceArea(testMsh1) < CalculateSurfaceArea(testMsh2)){
            filter.mesh = testMsh2;
            baseMesh = testMsh2;
            vertices2D = altList.ToArray();
        } else {
            filter.mesh = testMsh1;
            baseMesh = testMsh1;
            vertices2D = newList.ToArray();
        }

        poly2D.points = vertices2D; //update polyCol
        GameObject.Find("minimap").SendMessage("updateMap");
        // MapController.updateMap(vertices2D);
    }

    float CalculateSurfaceArea(Mesh mesh) {
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;
        double sum = 0.0;
        for(int i = 0; i < triangles.Length; i += 3) {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;
            sum += Vector3.Cross(a, b).magnitude;
        }
        return (float)(sum/2.0);
    }

    private Mesh CreateMesh(List<Vector2> outlineList){
        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(outlineList.ToArray());
        int[] indices = tr.Triangulate();
 
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[outlineList.ToArray().Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(outlineList.ToArray()[i].x, outlineList.ToArray()[i].y, 0);
        }

        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();
        
        return msh;
    }

    private List<Vector2> NewOutline_AltRemove(int index_startNode, int smaller, int larger, Vector2[] vertices2D, Vector2[] trailPoints){
        List<Vector2> newOutline = new List<Vector2>();

        foreach(Vector2 point in trailPoints){
            newOutline.Add(point);
            // Vector3 pos3 = new Vector3(point.x, point.y, -5);
            // Instantiate(marker, pos3, transform.rotation);
        }
        List<Vector2> oldVerts = new List<Vector2>(vertices2D);
        if(smaller == index_startNode){
            oldVerts.Reverse();
        } else {
            // oldVerts.Reverse();
        }
        foreach(Vector2 point in oldVerts.GetRange(smaller, (larger - smaller + 1))){
            newOutline.Add(point);
            // Vector3 pos3 = new Vector3(point.x, point.y, -5);
            // Instantiate(marker, pos3, transform.rotation);
        }
        return newOutline;
    }

    private List<Vector2> NewOutline_NormalRemove(int index_startNode, int smaller, int larger, Vector2[] vertices2D, Vector2[] trailPoints){
        
        List<Vector2> newVerticesList = new List<Vector2>(vertices2D);
        foreach(Vector2 point in newVerticesList.GetRange(smaller, (larger - smaller + 1))){
            // Vector3 pos3 = new Vector3(point.x, point.y, -5);
            // Instantiate(marker, pos3, transform.rotation);
            newVerticesList.Remove(point);
            // Debug.Log(point);
        }
        
        
        List<Vector2> trailList = new List<Vector2>(trailPoints);

        if(smaller == index_startNode){
            newVerticesList.InsertRange(smaller, trailList);
        } else {
            trailList.Reverse();
            newVerticesList.InsertRange(smaller, trailList);
        }
        return newVerticesList;
    }

    void OnTriggerEnter2D(Collider2D collider){
        // Debug.Log("Enter");
        if(trigEntered){
            UpdateBase();
            TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
            playerTrail.Clear();
        } else {
            TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
            playerTrail.Clear();
            trigEntered = true;
        }  
    }
    void OnTriggerStay2D(Collider2D collider){
        TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
        playerTrail.Clear();
        // Debug.Log("Stay");
    }

    void Update()
    {
        // UpdateBase();
        if (Input.GetKeyDown("space"))
        {
            UpdateBase();
            TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
            playerTrail.Clear();
        }
        if (Input.GetKeyDown(KeyCode.J)){foreach(Vector2 point in vertices2D){Debug.Log(point);}}
        if (Input.GetKeyDown(KeyCode.R)){SceneManager.LoadScene(SceneManager.GetActiveScene().name);}
    }
    
}
