using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Yun.Scripts.Animations
{
    public class FadeInOutAnimation : MonoBehaviour
    {
        private Image image;
        private float fadeTime = 0.5f; // Thời gian fade là 1 giây
        private float timer = 0f;
        private bool fadingIn = true;

        private bool _isFading;
        public void StartFading()
        {
            // Lấy component Image
            image = GetComponent<Image>();
        
            // Đảm bảo alpha bắt đầu từ 0
            Color startColor = image.color;
            startColor.a = 0f;
            image.color = startColor;
            _isFading = true;
        }

        public void StopFading()
        {
            _isFading = false;
        }

        private void Update()
        {
            if(!_isFading)
                return;
            timer += Time.deltaTime;
        
            // Tính toán giá trị alpha dựa trên thời gian
            float alpha;
            if (fadingIn)
            {
                alpha = timer / fadeTime; // Tăng dần từ 0 đến 1
            }
            else
            {
                alpha = 1 - (timer / fadeTime); // Giảm dần từ 1 về 0
            }
        
            // Giới hạn alpha trong khoảng [0,1]
            alpha = Mathf.Clamp01(alpha);
        
            // Cập nhật màu của image với alpha mới
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
        
            // Kiểm tra xem đã hết thời gian fade chưa
            if (timer >= fadeTime)
            {
                timer = 0f; // Reset timer
                fadingIn = !fadingIn; // Đảo chiều fade
            }
        }
    }
}