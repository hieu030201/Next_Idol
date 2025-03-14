using UnityEngine;
using UnityEngine.UI;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class Model3DRenderer : MonoBehaviour
    {
        public GameObject model3D;
        public RawImage displayImage;
        public Camera renderCamera; // Camera có sẵn để render
        public int renderTextureSize = 256;

        private RenderTexture renderTexture;
        private RenderTexture originalRenderTexture;

        public void StartRender(RawImage image)
        {
            displayImage = image;
            if (renderCamera == null)
            {
                Debug.LogError("Render Camera is not assigned!");
                return;
            }

            // Lưu RenderTexture gốc của camera (nếu có)
            originalRenderTexture = renderCamera.targetTexture;

            // Tạo RenderTexture mới
            renderTexture = new RenderTexture(renderTextureSize, renderTextureSize, 16, RenderTextureFormat.ARGB32);
            renderTexture.Create();

            // Gán RenderTexture cho RawImage
            displayImage.texture = renderTexture;

            // Đặt model vào layer riêng
            // model3D.layer = LayerMask.NameToLayer("BedRoomUI");

            // Đặt vị trí model
            PositionModel();
        }

        void PositionModel()
        {
            // Đặt model ở vị trí phù hợp trong tầm nhìn của camera
            // model3D.transform.position = renderCamera.transform.position + renderCamera.transform.forward * 5f;
            // model3D.transform.rotation = Quaternion.identity;
        }

        void LateUpdate()
        {
            if (!displayImage)
                return;
            // Lưu cài đặt gốc của camera
            int originalCullingMask = renderCamera.cullingMask;
            CameraClearFlags originalClearFlags = renderCamera.clearFlags;
            Color originalBackgroundColor = renderCamera.backgroundColor;

            // Thiết lập camera để chỉ render model
            // renderCamera.cullingMask = 1 << LayerMask.NameToLayer("BedRoomUI");
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.backgroundColor = Color.clear;

            // Gán RenderTexture tạm thời
            renderCamera.targetTexture = renderTexture;

            // Xoay model (tùy chọn)
            // model3D.transform.Rotate(Vector3.up, 30f * Time.deltaTime);

            // Render model
            renderCamera.Render();

            // Khôi phục cài đặt gốc của camera
            renderCamera.cullingMask = originalCullingMask;
            renderCamera.clearFlags = originalClearFlags;
            renderCamera.backgroundColor = originalBackgroundColor;
            renderCamera.targetTexture = originalRenderTexture;
        }

        void OnDestroy()
        {
            // Dọn dẹp
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
            }
        }
    }
}