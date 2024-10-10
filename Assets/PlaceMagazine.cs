using UnityEngine;

public class PlaceMagazine : MonoBehaviour
{
    public GameObject magazinePrefab;
    public GameObject floor; 
    public int numberOfMagazines = 50; 
    
    void Start()
    {
        SpawnMagazines();
    }

    void SpawnMagazines()
    {
        if (floor != null)
        {
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            if (floorRenderer != null)
            {
                Bounds bounds = floorRenderer.bounds; // Get the bounds of the floor model
                for (int i = 0; i < numberOfMagazines; i++)
                {
                    // Generate a random position within the bounds
                    float xPosition = Random.Range(bounds.min.x, bounds.max.x);
                    float zPosition = Random.Range(bounds.min.z, bounds.max.z);
                    Vector3 spawnPosition = new Vector3(xPosition, floor.transform.position.y, zPosition);

                    // Generate a random rotation around the X-axis
                    Quaternion randomRotation = Quaternion.Euler(Random.Range(30, 360), 0, 0);

                    // Instantiate the magazine prefab at the random position with a random rotation
                    Instantiate(magazinePrefab, spawnPosition, randomRotation);
                }
            }
            else
            {
                Debug.LogError("No Renderer found on the floor model");
            }
        }
        else
        {
            Debug.LogError("Floor model not assigned");
        }
    }

}
