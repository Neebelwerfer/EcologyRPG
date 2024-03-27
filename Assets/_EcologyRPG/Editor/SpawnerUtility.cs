using UnityEngine;

public class SpawnerUtility : MonoBehaviour
{
    public Mesh PlayerMesh;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireMesh(PlayerMesh, transform.position, transform.rotation, transform.localScale);
    }
}
