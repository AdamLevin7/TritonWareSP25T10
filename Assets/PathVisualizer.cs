using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    Transform[] nodes;
    [SerializeField] bool visualizePath = false;

    private void OnDrawGizmos()
    {
        if (!Application.isEditor) return;
        if (!visualizePath) return;
        if (transform.childCount == 0) return;
        nodes = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
        {
            nodes[i] = transform.GetChild(i);
        }
        for (int i = 0; i < nodes.Length - 1; ++i)
        {
            Gizmos.DrawSphere(nodes[i].position, 0.1f);
            Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
        }
        Gizmos.DrawSphere(nodes[nodes.Length - 1].position, 0.1f);
    }
}
