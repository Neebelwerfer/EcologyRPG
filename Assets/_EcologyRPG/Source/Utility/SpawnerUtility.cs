using UnityEngine;

public class SpawnerUtility : MonoBehaviour
{
    public Mesh PlayerMesh;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireMesh(PlayerMesh, transform.position, transform.rotation, transform.localScale);
    }
#endif
}
