using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    public Camera mainCamera;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    UnityEngine.AI.NavMeshAgent agent;
    private float lostPlayerTimer = 0f;
    private float timeToLosePlayer = 2f;

    private void Start()
    {
        playerRef = GameObject.FindWithTag("MainCamera");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }

            }
            else
            {
                canSeePlayer = false;
            }

        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }

        // if canSeePlayer being false for 3 seconds, then the player is lost
        if (!canSeePlayer)
        {
            lostPlayerTimer += Time.deltaTime;
            // Debug.Log("Lost player for " + lostPlayerTimer + " seconds");
            if (lostPlayerTimer >= timeToLosePlayer)
            {
                this.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = GetRandomLocation();
                lostPlayerTimer = 0f; // reset the timer
            }
        }
        else
        {
            lostPlayerTimer = 0f; // reset the timer
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = playerRef.transform.position;
        }

    }

    Vector3 GetRandomLocation()
    {
        UnityEngine.AI.NavMeshTriangulation navMeshData = UnityEngine.AI.NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        // Debug.Log("Random point: " + point);
        return point;
    }
}

