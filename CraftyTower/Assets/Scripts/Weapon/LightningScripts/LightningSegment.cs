using UnityEngine;
using System.Collections;

public class LightningSegment {
    public Vector3 startPoint;
    public Vector3 endPoint;

    public LightningSegment(Vector3 startPointInit, Vector3 endPointInit)
    {
        startPoint = startPointInit;
        endPoint = endPointInit;
    }
}
