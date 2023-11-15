using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapController : MonoBehaviour
{
    public GameObject player;
    public GameObject playerIcon;
    public GameObject miniMap;
    public GameObject miniMeshObj;
    float miniMapScale;
    float levelScale;
    float rescaleFactor;
    Vector2 unitVector;
    Vector2 playerIconOffset;
    Vector3 playerIconPos;

    private Mesh baseMapMesh;
    private MeshRenderer renderer;
    private MeshFilter filter;
    private Vector2[] vertices2D;
    public Material baseMat;

    // Start is called before the first frame update
    void Start()
    {
        Vector2[] vertices2D = new Vector2[]{
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(1, 0),
        };

        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
 
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        baseMapMesh = new Mesh();
        baseMapMesh.vertices = vertices;
        baseMapMesh.triangles = indices;
        baseMapMesh.RecalculateNormals();
        baseMapMesh.RecalculateBounds();

        renderer = miniMeshObj.AddComponent<MeshRenderer>();
        filter = miniMeshObj.AddComponent<MeshFilter>();
        filter.mesh = baseMapMesh;
        renderer.material = baseMat;
        



        miniMapScale = miniMap.transform.localScale.x;
        levelScale = GameObject.Find("wallcollider").transform.localScale.x;
        rescaleFactor = miniMapScale / levelScale;
        updateMap();
    }

    // Update is called once per frame
    void Update()
    {
        playerIconOffset = (player.transform.position / levelScale) * miniMapScale;
        playerIconPos = new Vector3(playerIconOffset.x + miniMap.transform.position.x, playerIconOffset.y + miniMap.transform.position.y, 0);
        playerIcon.transform.position = playerIconPos;

        // updateMap();
    }

    void updateMap(){
        Debug.Log("updateMap");
        Vector2[] baseOutline = GameObject.Find("Base").GetComponent<PolygonCollider2D>().points;
        // List<Vector2> basePointsOffsetList = new List<Vector2>();
        foreach(Vector2 point in baseOutline){
            // basePointsOffsetList.Add((point / levelScale) * miniMapScale);
            // baseOutline[Array.IndexOf(baseOutline, point)] = ((point / levelScale) * miniMapScale);
            baseOutline[Array.IndexOf(baseOutline, point)] = ((point / levelScale));
        }
        List<Vector2> basePointsPos = new List<Vector2>();
        int iterator = 0;
        foreach(Vector2 point in baseOutline){
            // Vector2 newPoint = new Vector3(baseOutline[iterator].x + miniMap.transform.position.x, baseOutline[iterator].y + miniMap.transform.position.y);
            Vector2 newPoint = new Vector3(baseOutline[iterator].x, baseOutline[iterator].y, 0);
            basePointsPos.Add(newPoint);
            iterator += 1;
        }

        Triangulator tr = new Triangulator(basePointsPos.ToArray());
        int[] indices = tr.Triangulate();
 
        Vector3[] vertices = new Vector3[basePointsPos.ToArray().Length];
        for (int i=0; i<vertices.Length; i++) {
            vertices[i] = new Vector3(basePointsPos.ToArray()[i].x, basePointsPos.ToArray()[i].y, 0);
        }
        baseMapMesh.vertices = vertices;
        baseMapMesh.triangles = indices;
        baseMapMesh.RecalculateNormals();
        baseMapMesh.RecalculateBounds();
    }
}
