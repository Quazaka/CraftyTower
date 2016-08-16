using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LightningArc : MonoBehaviour
{
    #region initialization
    private int jumpCount; //How many jumps
    private GameObject currentTarget; //Who to jump to
    private float damage; //Damage from jump
    private float jumpDistance; //How far to jump
    private Vector3 lightningOrigin; //Where the lightning have origin
    private List<GameObject> enemyList; //Store enemies in range
    [SerializeField]
    private GameObject lightningWeapon; //Reference to weapon
    [SerializeField]
    private GameObject lightningRendererPrefab; //Reference to lightningRenderePrefab
    private List<GameObject> IHaveBeenHit; //List to store enemies already hit by this chain
    private GameObject previousTarget;
    private float lifeTime; //How long should we show a lighting
    private bool firstJump; // First time istanciated?
    private bool instantiated; //Is class instanciated?
    private bool done; //Have we peformed our attack for this chain link

    public void Instantiate(int jumpCountInit, GameObject currentTargetInit, float damageInit, float jumpDistanceInit, Vector3 lightningOriginInit, bool fistJumpInit, List<GameObject> IHaveBeenHitInit)
    {
        jumpCount = jumpCountInit;
        currentTarget = currentTargetInit;
        previousTarget = currentTargetInit; // Jump from this target
        damage = damageInit;
        jumpDistance = jumpDistanceInit;
        lightningOrigin = lightningOriginInit;
        firstJump = fistJumpInit;
        lifeTime = 0.5f;
        done = false;

        if (IHaveBeenHitInit != null) //If the class is initiated from weapon, this will be null, as no enemies have yet been hit
        {
            IHaveBeenHit = IHaveBeenHitInit;
        }
        else
        {
            IHaveBeenHit = new List<GameObject>();
        }
        instantiated = true;
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        if (instantiated && !done)
        {
            if (currentTarget != null)
            {
                DrawLightArc();
                Object.Destroy(gameObject, lifeTime); // Value - time in seconds before lighting vanish. Lower better for performance
                done = true;
            }
        }
    }

    //Get the next enemy, arch to it, deal damage, and peform the next jump
    private void DrawLightArc()
    {
        DrawArc(); // Draw arch
        DealDamage(); // Deal damage
        IHaveBeenHit.Add(currentTarget); // Mark target at hit
        GetEnemisInJumpDistance(lightningOrigin, GetJumpDistance()); // Get units in range

        if (enemyList.Count != 0)
        {
            Debug.Log(currentTarget);
            currentTarget = SetNextTarget(enemyList); // Set random target in range if there any whitin range
            Debug.Log(currentTarget);
            if (currentTarget != null) // if SetNextTarget found a target
            {
                JumpAgain(); // If more jumps are to be peformed, jump
            }
        }
    }

    private void DrawArc()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        int vertexCount = 16;

        lineRenderer.GetComponent<LineRenderer>().enabled = true; //Enable line renderer
        lineRenderer.GetComponent<LineRenderer>().SetWidth(0.05f, 0.05f); // set width of lightning
        lineRenderer.GetComponent<LineRenderer>().SetVertexCount(vertexCount); //How many parts to devide the line into
        lineRenderer.GetComponent<LineRenderer>().SetPosition(0, lightningOrigin); //Starting point

        for (int i = 1; i < vertexCount; i++) //Generate random points along the path
        {
            var pos = Vector3.Lerp(lightningOrigin, currentTarget.transform.position, i / (float)vertexCount);

            //randomises lines position
            pos.x += Random.Range(-0.1f, 0.1f);
            pos.y += Random.Range(-0.1f, 0.1f);

            lineRenderer.GetComponent<LineRenderer>().SetPosition(i, pos);
        }
    }

    //Jump if there are more jumps to be done
    private void JumpAgain()
    {
        Debug.Log(jumpCount);
        if (jumpCount > 0)
        {
            jumpCount--; //Remove one jump

            GameObject lightningArc = (GameObject)Instantiate(lightningRendererPrefab, transform.position, Quaternion.identity);
            lightningArc.GetComponent<LightningArc>().Instantiate(jumpCount, currentTarget, damage, GetJumpDistance(), previousTarget.transform.position, false, IHaveBeenHit);

            /*
            LightningArc lightningArc = GetComponent<LightningArc>();
            lightningArc.Instantiate(jumpCount, currentTarget, damage, GetJumpDistance(), previousTarget.transform.position, false, IHaveBeenHit);
            */
        }
    }

    //Set the currentTarget. Who to jump to next
    //Pick random unit, and if unit have been hit by lightning before it will not hit it again.
    private GameObject SetNextTarget(List<GameObject> enemiesInRange) //If Cluser fuck
    {
        int i = Random.Range(0, enemiesInRange.Count - 1);

        if (enemiesInRange.Count != 0)
        {
            if (IHaveBeenHit != null)//Ensure selcted enemy is not already hit
            {
                foreach (GameObject enemy in IHaveBeenHit)
                {
                    if (enemy != enemiesInRange[i])
                    {
                        return enemiesInRange[i];
                    }
                }
            }
        }
        else if (IHaveBeenHit == null && enemiesInRange.Count != 0)// if first jump. no gameobj on IHaveBeenHit, check against
        {
            return enemiesInRange[i];
        }
        return null; // Return null if no enemies are whitin range to jump to
    }

    //Deal damage and set future health(used in other weapons)
    private void DealDamage()
    {
        //Set future health to prevent overkill
        IHealth enemyHealth = (IHealth)currentTarget.GetComponent<BaseEnemy>();
        enemyHealth.futureHealth -= damage;

        //Damage target instantly
        IDamage enemyDealDamage = (IDamage)currentTarget.GetComponent<BaseEnemy>();
        enemyDealDamage.damage = damage;
    }

    //Get enemies in jumpDistance and add them to the target list
    private void GetEnemisInJumpDistance(Vector3 center, float radius)
    {
        //OverlapSphere returns an array - converted to list here
        Collider[] hitCollidersArray = Physics.OverlapSphere(center, radius);
        List<GameObject> hitCollidersList = new List<GameObject>();

        //"Convert" the Array<Collider> to List<GameObject>
        for (int i = 0; i < hitCollidersArray.Length; i++)
        {
            hitCollidersList.Add(hitCollidersArray[i].gameObject);
        }

        //Remove non enemies from list
        for (int i = hitCollidersList.Count - 1; i >= 0; i--)
        {
            if (!hitCollidersList[i].GetComponent<BaseEnemy>())
            {
                hitCollidersList.RemoveAt(i);
            }
        }

        //Set list
        enemyList = hitCollidersList;
    }

    private float GetJumpDistance()//If its the first jump, use towers range
    {
        float TowerRange = lightningWeapon.GetComponent<LightningWeapon>().range; 

        if (firstJump)
        {
            return TowerRange;
        }
        else
        {
            return jumpDistance;
        }
    }

}