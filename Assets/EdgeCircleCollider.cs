using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCircleCollider : MonoBehaviour
{
    void Awake() {
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        if(poly == null){
            poly = gameObject.AddComponent<PolygonCollider2D>();
        }
        int poly_length = poly.points.Length;
        Vector2[] edge_points = new Vector2[poly_length+1];
        // Debug.Log(poly_length);
        for(int i=0; i<poly_length; i++){
            edge_points[i] = poly.points[i];
            // Debug.Log(i);
            // Debug.Log(poly.points[i]);
        }
        edge_points[poly_length] = poly.points[0];

        // Vector2[] edge_points = new Vector2[21];
        // Debug.Log(edge_points.Length);
        // for(int i=0; i<20; i++){
        //     edge_points[i] = poly.points[i];
        //     Debug.Log(i);
        //     Debug.Log(poly.points[i]);
        // }
        // edge_points[20] = poly.points[0];

        EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D>();
        edge.points = edge_points;
        Destroy(poly);
    }
}
 