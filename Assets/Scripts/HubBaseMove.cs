using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform target2;
    UnityEngine.AI.NavMeshAgent agent;

    public void SetTarget(Transform nextTarget)
    {
        agent.SetDestination(nextTarget.position);
    }
    public void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(target.position);
    }

}
