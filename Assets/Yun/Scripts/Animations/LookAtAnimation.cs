using UnityEngine;

public class LookAtAnimation : MonoBehaviour
{
    public float moveDistance = -0.4f;   // Khoảng cách di chuyển tối đa
    public float moveSpeed = 4f;      // Tốc độ di chuyển
    
    private Vector3 startPosition;      // Vị trí ban đầu của đối tượng
    
    void Start()
    {
        // Lưu vị trí ban đầu khi script bắt đầu
        startPosition = transform.localPosition;
    }
    
    void Update()
    {
        // Sử dụng hàm sin để tạo chuyển động lên xuống mượt mà
        // Sin tạo giá trị từ -1 đến 1, nên chúng ta nhân với khoảng cách di chuyển
        float yOffset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        
        // Áp dụng vị trí mới, chỉ thay đổi thành phần y
        transform.localPosition = new Vector3(
            startPosition.x,
            startPosition.y + yOffset,
            startPosition.z
        );
    }
}