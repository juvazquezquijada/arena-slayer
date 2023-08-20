using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollow : MonoBehaviour
{
    public Transform TargetTransform;
    public Vector3 Offset;
    
    // Update is called once per frame
    void Update()
    {
        TargetTransform.position = TargetTransform.position + Offset;
    }
}
