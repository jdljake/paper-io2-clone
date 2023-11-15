// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class BaseController : MonoBehaviour
// {
//     private PolygonCollider2D poly2D;
//     private EdgeCollider2D trailCollider;
//     public Material baseMat;
    
//     private MeshRenderer renderer;
//     private MeshFilter filter;
//     private Vector2[] vertices2D;

//     // Start is called before the first frame update
//     void Start()
//     {
//         trailCollider = GameObject.Find("TrailCollider").GetComponent<EdgeCollider2D>();
//         poly2D = gameObject.AddComponent<PolygonCollider2D>(); //init polyCol
//         poly2D.isTrigger = true;

//         // Create Vector2 vertices
//         // vertices2D = new Vector2[] {
//         //     // new Vector2(0,4),
//         //     // new Vector2(0,8),
//         //     // new Vector2(-8,0),
//         //     // new Vector2(0,-8),
//         //     // new Vector2(0,-4),
//         //     // new Vector2(-1,0),
//         // };

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
//         Mesh msh = new Mesh();
//         msh.vertices = vertices;
//         msh.triangles = indices;
//         msh.RecalculateNormals();
//         msh.RecalculateBounds();
 
//         // Set up game object with mesh;
//         renderer = gameObject.AddComponent<MeshRenderer>();
//         filter = gameObject.AddComponent<MeshFilter>();
//         filter.mesh = msh;
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

//     // Update is called once per frame
//     void Update()
//     {
//         // UpdateBase();
//         if (Input.GetKeyDown("space"))
//         {
//             UpdateBase();
//             TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
//             playerTrail.Clear();
//         }
//         if (Input.GetKeyDown(KeyCode.J))
//         {
//             foreach(Vector2 point in vertices2D){
//                 Debug.Log(point);
//             }
            
//         }
//         if (Input.GetKeyDown(KeyCode.R))
//         {
//             SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//         }
//     }

//     // void OnTriggerEnter2D(Collider2D collider){
//     //     // Debug.Log("BASE TRIGGER ENTER");
//     //     if(collider.name == "player"){
//     //         //update base
//     //         Debug.Log("update Base");
//     //         // UpdateBase();
//     //     }
//     // }
     

//     void UpdateBase(){
//         // ClearForUpdate();
//         ClearForUpdate2();
//         Vector2[] trailPoints = trailCollider.points;
//         Vector2[] updatedBase2D = new Vector2[vertices2D.Length + trailPoints.Length];
//         int iterator = 0;
//         foreach (Vector2 point in vertices2D){
//             updatedBase2D[iterator] = point;
//             iterator += 1;
//         }
//         foreach (Vector2 point in trailPoints){
//             updatedBase2D[iterator] = point;
//             iterator += 1;
//         }

//         /// TODO: implement generalization of vertices2D
//             vertices2D = updatedBase2D;
//         ///

//         // Use the triangulator to get indices for creating triangles
//         Triangulator tr = new Triangulator(updatedBase2D);
//         int[] indices = tr.Triangulate();
 
//         // Create the Vector3 vertices
//         Vector3[] vertices = new Vector3[updatedBase2D.Length];
//         for (int i=0; i<vertices.Length; i++) {
//             vertices[i] = new Vector3(updatedBase2D[i].x, updatedBase2D[i].y, 0);
//         }

//         // Create the mesh
//         Mesh msh = new Mesh();
//         msh.vertices = vertices;
//         msh.triangles = indices;
//         msh.RecalculateNormals();
//         msh.RecalculateBounds();
 
//         filter.mesh = msh;

//         poly2D.points = updatedBase2D; //update polyCol
//     }


//     void OnTriggerStay2D(Collider2D collider){
//         TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
//         playerTrail.Clear();
//         Debug.Log("Stay");
//     }
//     void OnTriggerEnter2D(Collider2D collider){
//         Debug.Log("Enter");
//         TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
//         playerTrail.Clear();
//         UpdateBase();
//     }

//     void ClearForUpdate2(){
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

//     void ClearForUpdate(){
//         //old points vector2[] //should be vertices2D for now I think
//         //enter point == end of trail
//         //exit point == beggining of trail
        
//         Vector2 enterPoint = trailCollider.points[trailCollider.points.Length-1];
//         Vector2 exitPoint = trailCollider.points[0];

//         float largerY = 0;
//         float smallerY = 0;

//         float largerX = 0;
//         float smallerX = 0;

//         if(exitPoint.y - enterPoint.y > 0){
//             //exit is larger
//             largerY = exitPoint.y;
//             smallerY = enterPoint.y;
//         } else {
//             largerY = enterPoint.y;
//             smallerY = exitPoint.y;
//         }
//         if(exitPoint.x - enterPoint.x > 0){
//             //exit is larger
//             largerX = exitPoint.x;
//             smallerX = enterPoint.x;
//         } else {
//             largerX = enterPoint.x;
//             smallerX = exitPoint.x;
//         }

//         List<Vector2> verticesList = new List<Vector2>(vertices2D);
//         List<Vector2> toRemove = new List<Vector2>();

//         foreach(Vector2 point in vertices2D){
//             if((smallerY < point.y) && (point.y < largerY)){
//                 if((smallerX < point.x) && (point.x < largerX)){
//                     //delete point
//                     toRemove.Add(point);
//                 }   
//             }
//         }

//         verticesList.RemoveAll(x => toRemove.Contains(x));
//         vertices2D = verticesList.ToArray();
        
//     }

// //on stay in mesh
// //TrailRenderer playerTrail = GameObject.Find("shadow").GetComponent<TrailRenderer>();
// //playerTrail.Clear();
// }
