using UnityEngine;
using UnityEditor;

namespace Yun.Scripts.Editor
{
    public class AddCollidersEditor : EditorWindow
    {
        [MenuItem("Tools/Add Colliders to Children")]
        static void AddCollidersToChildren()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogWarning("Không có object nào được chọn.");
                return;
            }

            Renderer[] renderers = selectedObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer.gameObject.GetComponent<Collider>() == null)
                {
                    MeshCollider collider = renderer.gameObject.AddComponent<MeshCollider>();
                    collider.convex = true;
                }
            }

            Debug.Log("Đã thêm colliders cho tất cả children có Renderer.");
        }
    }
}
