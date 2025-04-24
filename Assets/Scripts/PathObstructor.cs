using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PathObstructor : MonoBehaviour
{
    [SerializeField] private float pathWidth = 1f;
    [SerializeField] private GameObject pathColliderContainerPrefab;

    private void Start()
    {
        CreatePathColliders();
    }

    [ContextMenu("ClearPathColliders")]
    public void ClearPathColliders()
    {
        foreach (Transform node in transform)
        {
            if (node.childCount > 0)
                foreach (Transform child in node)
                    if (Application.isEditor) DestroyImmediate(child.gameObject);
                    else Destroy(child.gameObject);
        }
    }

    [ContextMenu("CreatePathColliders")]
    public void CreatePathColliders()
    {
        if (pathColliderContainerPrefab == null) return;
        if (transform.childCount < 2) return;

        ClearPathColliders();

        Transform curr = transform.GetChild(0);

        for (int i = 1; i < transform.childCount; ++i)
        {
            Transform next = transform.GetChild(i);
            Vector3 distance = next.position - curr.position;

            GameObject colliderContainer = Instantiate(pathColliderContainerPrefab, curr);
            colliderContainer.transform.localPosition = new(distance.x / 2 / curr.localScale.x, distance.y / 2 / curr.localScale.y, distance.z / 2 / curr.localScale.z);
            colliderContainer.transform.eulerAngles = new(0, 0, Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg);
            colliderContainer.transform.localScale = new(1 / curr.localScale.x, 1 / curr.localScale.y, 1 / curr.localScale.z);

            CapsuleCollider newCollider = colliderContainer.GetComponent<CapsuleCollider>();
            newCollider.height = distance.magnitude + pathWidth;
            newCollider.center = Vector3.zero;
            newCollider.radius = pathWidth / 2;

            colliderContainer.transform.position += newCollider.radius * Vector3.back;

            curr = next;
        }
    }
}
