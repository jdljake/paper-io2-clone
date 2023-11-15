using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollider : MonoBehaviour
{
    TrailRenderer myTrail;
    EdgeCollider2D myCollider;

    void Awake()
    {
        myTrail = this.GetComponent<TrailRenderer>();

        GameObject colliderGameObject = new GameObject("TrailCollider", typeof(EdgeCollider2D));
        myCollider = colliderGameObject.GetComponent<EdgeCollider2D>();
        myCollider.isTrigger = true;
        myCollider.edgeRadius = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        SetColliderPointsFromTrail(myTrail, myCollider);

    }

    void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        for(int position = 0; position<(trail.positionCount-5); position++)
        {
            //ignores z axis when translating vector3 to vector2
            points.Add(trail.GetPosition(position));
        }
        collider.SetPoints(points);
    }
    
}
