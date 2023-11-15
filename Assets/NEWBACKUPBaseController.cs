// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System;

// public class BaseController : MonoBehaviour
// {
//     private PolygonCollider2D poly2D;
//     private EdgeCollider2D trailCollider;
//     public Material baseMat;
    
//     bool trigEntered;

//     private MeshRenderer renderer;
//     private MeshFilter filter;
//     private Vector2[] vertices2D;
//     private Mesh baseMesh;

//     // Start is called before the first frame update
//     void Start()
//     {
//         trigEntered = false;
//         trailCollider = GameObject.Find("TrailCollider").GetComponent<EdgeCollider2D>();
//         poly2D = gameObject.AddComponent<PolygonCollider2D>(); //init polyCol
//         poly2D.isTrigger = true;

//         vertices2D = circleGen(8f, 30);
 
//         // Use the triangulator to get indices for creating triangles
//         Triangulator tr = new Triangulator(vertices2D);
//         int[] indices = tr.Triangulate();
 
//         // Create the Vector3 vertices
//         Vector3[] vertices = new Vector3[vertices2D.Length];
//         for (int i=0; i<vertices.Length; i++) {
//             vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
//         }
 
//         // Create the mesh
//         baseMesh = new Mesh();
//         baseMesh.vertices = vertices;
//         baseMesh.triangles = indices;
//         baseMesh.RecalculateNormals();
//         baseMesh.RecalculateBounds();
 
//         // Set up game object with mesh;
//         renderer = gameObject.AddComponent<MeshRenderer>();
//         filter = gameObject.AddComponent<MeshFilter>();
//         filter.mesh = baseMesh;
//         renderer.material = baseMat;
        
//         poly2D.points = vertices2D; //update polyCol

//         // UpdateBase();
//     }

//     private Vector2[] circleGen(float radius, int numPoints){
//         Vector2[] edgePoints = new Vector2[numPoints + 1];
       
//         for(int loop = 0; loop <= numPoints; loop++)
//         {
//             float angle = (Mathf.PI * 2.0f / numPoints) * loop;
//             edgePoints[loop] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
//         }
//         return edgePoints;
//     }
     

//     void UpdateBase(){
//         ClearForUpdateNew();
//         Vector2[] trailPoints = trailCollider.points;
//         // Vector2[] updatedBase2D = new Vector2[vertices2D.Length + trailPoints.Length];
        

//         List<Vector2> verticesList = new List<Vector2>(vertices2D);
//         List<Vector2> trailList = new List<Vector2>(trailPoints);
//         List<Vector2> newPoints = new List<Vector2>();
        
//         Vector2 exitPoint = trailPoints[0];
//         Vector2 enterPoint = trailPoints[trailPoints.Length-1];

//         int baseExitIndex = verticesList.IndexOf(exitPoint);
//         int baseEnterIndex = verticesList.IndexOf(enterPoint);

//         if(baseExitIndex == -1){
//             double shortestDistance = 100;
//             foreach(Vector2 point in vertices2D){
//                 double currentDistance = Math.Sqrt(Math.Pow((point.x - exitPoint.x), 2) + Math.Pow((point.y - exitPoint.y), 2));
//                 if(currentDistance < shortestDistance){
//                     baseExitIndex = verticesList.IndexOf(point);
//                 }
//             }
//         }
//         if(baseEnterIndex == -1){
//             double shortestDistance = 100;
//             foreach(Vector2 point in vertices2D){
//                 double currentDistance = Math.Sqrt(Math.Pow((point.x - exitPoint.x), 2) + Math.Pow((point.y - exitPoint.y), 2));
//                 if(currentDistance < shortestDistance){
//                     baseEnterIndex = verticesList.IndexOf(point);
//                 }
//             }
//         }

//         int smaller = 0;
//         int larger = 0;
//         if (baseEnterIndex >= baseExitIndex){
//             smaller = baseExitIndex;
//             larger = baseEnterIndex;
//         } else {
//             smaller = baseEnterIndex;
//             larger = baseExitIndex;
//         }

//         for(int i = 0; i<smaller; i++){
//             newPoints.Add(vertices2D[i]);
//         }
//         foreach (Vector2 point in trailList){
//             newPoints.Add(point);
//         }
//         for(int i = larger; i<vertices2D.Length-1; i++){
//             newPoints.Add(vertices2D[i]);
//         }

//         vertices2D = newPoints.ToArray();

//         // Use the triangulator to get indices for creating triangles
//         Triangulator tr = new Triangulator(vertices2D);
//         int[] indices = tr.Triangulate();
 
//         // Create the Vector3 vertices
//         Vector3[] vertices = new Vector3[vertices2D.Length];
//         for (int i=0; i<vertices.Length; i++) {
//             vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
//         }

//         baseMesh.vertices = vertices;
//         baseMesh.triangles = indices;
//         baseMesh.RecalculateNormals();
//         baseMesh.RecalculateBounds();
 
//         filter.mesh = baseMesh;

//         poly2D.points = vertices2D; //update polyCol
//     }

//     void ClearForUpdateNew(){
//         Vector2 enterPoint = trailCollider.points[trailCollider.points.Length-1];
//         Vector2 exitPoint = trailCollider.points[0];

//         List<Vector2> verticesList = new List<Vector2>(vertices2D);
        
//         int exitIndex = verticesList.IndexOf(exitPoint);
//         int enterIndex = verticesList.IndexOf(enterPoint);

//         if(enterIndex < exitIndex){
//             for(int i = enterIndex; i < exitIndex; i++){
//                 verticesList.RemoveAt(i);
//             }
//         }
//         if(enterIndex > exitIndex){
//             for(int i = exitIndex; i < enterIndex; i++){
//                 verticesList.RemoveAt(i);
//             }
//         }
//         vertices2D = verticesList.ToArray();
//     }

//     void OnTriggerEnter2D(Collider2D collider){
//         // Debug.Log("Enter");
//         if(trigEntered){
//             UpdateBase();
//             TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
//             playerTrail.Clear();
//         } else {
//             TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
//             playerTrail.Clear();
//             trigEntered = true;
//         }  
//     }
//     void OnTriggerStay2D(Collider2D collider){
//         TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
//         playerTrail.Clear();
//         // Debug.Log("Stay");
//     }

//     void Update()
//     {
//         // UpdateBase();
//         if (Input.GetKeyDown("space"))
//         {
//             UpdateBase();
//             TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
//             playerTrail.Clear();
//         }
//         if (Input.GetKeyDown(KeyCode.J)){foreach(Vector2 point in vertices2D){Debug.Log(point);}}
//         if (Input.GetKeyDown(KeyCode.R)){SceneManager.LoadScene(SceneManager.GetActiveScene().name);}
//     }
    
// }
