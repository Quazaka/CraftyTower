using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeting : MonoBehaviour {

    public GameObject ChooseTargetScanType(List<GameObject> enemies, int targetSwitch)
    {
        GameObject currentTarget;
        switch (targetSwitch)
        {
            case 1:
                currentTarget = GetClosestEnemy(enemies);
                break;
            case 2:
                currentTarget = GetRandomEnemy(enemies);
                break;
            case 3:
                currentTarget = GetFurthestEnemy(enemies);
                break;
            case 4:
                currentTarget = GetHighestHealthEnemy(enemies);
                break;
            case 5:
                currentTarget = GetLowestHealthEnemy(enemies);
                break;
            default:
                currentTarget = GetClosestEnemy(enemies);
                break;
        }
        return currentTarget;
    }

    //Search for every enemy in range and pick the closest
    //TargetSwitch 1 and default
    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        GameObject closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies) //Find distance to each enemy in range
        {
            if (isTargetNull(potentialTarget)) { break; }
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrTotarget = directionToTarget.sqrMagnitude;
            if (dSqrTotarget < closestDistanceSqr) //if distance is lowest store it with the enemy
            {
                closestDistanceSqr = dSqrTotarget;
                closestTarget = potentialTarget;
            }
        }
        return closestTarget; //return loweste distance enemy.
    }

    //TODO improve
    //Search for every enemy in range and pick the furthest
    //TargetSwitch 2
    GameObject GetFurthestEnemy(List<GameObject> enemies)
    {
        GameObject closestTarget = null;
        float furthestDistanceSqr = -1;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies) //Find distance to each enemy in range
        {
            if (isTargetNull(potentialTarget)) { break; }
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrTotarget = directionToTarget.sqrMagnitude;
            if (dSqrTotarget > furthestDistanceSqr) //if distance is furthest store it with the enemy
            {
                furthestDistanceSqr = dSqrTotarget;
                closestTarget = potentialTarget;
            }
        }
        return closestTarget; //return loweste distance enemy.
    }


    //Pick a random unit in range
    //TargetSwitch 3
    GameObject GetRandomEnemy(List<GameObject> enemies)
    {
        int i = Random.Range(0, enemies.Count -1);
        return enemies[i];
    }

    //TODO Test this
    //Pick unit with highest health
    //TargetSwitch 4
    GameObject GetHighestHealthEnemy(List<GameObject> enemies)
    {
        float highestHealth = -1;
        GameObject highestHealthTarget = null;

        foreach  (GameObject potentialTarget in enemies)
        {
            IHealth enemy = potentialTarget.GetComponent<BaseEnemy>();
            if (enemy.futureHealth > highestHealth)
            {
                highestHealthTarget = potentialTarget;
                highestHealth = enemy.futureHealth;
            }
        }
        return highestHealthTarget;
    }

    //TODO Test this
    //Pick unit with lowest health
    //TargetSwitch 5
    GameObject GetLowestHealthEnemy(List<GameObject> enemies)
    {
        float lowestHealth = -1;
        GameObject lowestHealthTarget = null;

        foreach (GameObject potentialTarget in enemies)
        {
            IHealth enemy = potentialTarget.GetComponent<BaseEnemy>();
            if (enemy.futureHealth > lowestHealth)
            {
                lowestHealthTarget = potentialTarget;
                lowestHealth = enemy.futureHealth;
            }
        }
        return lowestHealthTarget;
    }

    //Check for null reference
    private bool isTargetNull(GameObject currentTarget)
    {
        if (currentTarget == null)
        {
            return true;
        }
        return false;
    }
}
