using EcologyRPG.Core.Character;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCBinding : CharacterBinding
{
    public NavMeshAgent Agent { get
        {
            agent ??= GetComponent<NavMeshAgent>();
            return agent;
        }
    }

    NavMeshAgent agent;
}