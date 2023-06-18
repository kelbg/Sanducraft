using System;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public event Action<GameObject> DragStart;
    public event Action<GameObject> DragEnd;

    public const float OffsetZ = 0.5f; // Offset when dragging an item

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DragStart?.Invoke(gameObject);
        }
        else if (Input.GetMouseButton(0))
        {
            // Moves object to mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -Camera.main.nearClipPlane - OffsetZ;
            transform.position = mousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DragEnd?.Invoke(gameObject);
            Destroy(this); // Removes drag and drop script component
        }
    }
}
