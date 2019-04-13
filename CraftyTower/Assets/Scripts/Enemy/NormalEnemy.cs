using UnityEngine;
using System.Collections;
using System;

public class NormalEnemy : Enemy, IEnemy
{
    private Tower tower;

    private void Start()
    {
        tower = FindObjectOfType<Tower>();
    }

    public void Move(Vector3 target)
    {
        Debug.Log("FUCK");
        //throw new NotImplementedException();
    }

    private void FixedUpdate()
    {
        Move(tower.transform.position);
    }
}
