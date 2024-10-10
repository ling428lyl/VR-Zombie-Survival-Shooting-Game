// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour {
    
    public Transform goal;
    NavMeshAgent agent;
    
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position; 
    }
    void Update(){
        // keep update object postion
        agent.destination = goal.position;
    }
}