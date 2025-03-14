using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Animations;

public class DesertCountdown : MonoBehaviour
{
    [SerializeField] private GameObject timeBg;

    private Image _fillImage;
    
    private float _totalTime;
    private float _currentTime;
    private Tween _fillTween;
    
    public void StartCountdown(float time)
    {
        timeBg.GetComponent<FlutterAnimation>().StartFlutter(1.1f, 0.5f);
        _fillImage = timeBg.GetComponent<Image>();
        _totalTime = time;
        _currentTime = 0;
        
        // Reset fill amount về 0
        _fillImage.fillAmount = 0;
        
        // Sử dụng DOTween để tạo hiệu ứng trôi mượt từ 0 đến 1
        _fillTween = _fillImage.DOFillAmount(1, _totalTime)
            .SetEase(Ease.Linear) // Sử dụng Linear để đều đặn
            .OnUpdate(() => 
            {
                // Cập nhật thời gian hiện tại
                _currentTime = _totalTime * _fillImage.fillAmount;
            })
            .OnComplete(() => 
            {
                // Xử lý khi kết thúc đếm ngược
                Debug.Log("Countdown completed!");
            });
            
        // Bắt đầu đếm ngược
        // StartCoroutine(UpdateCountdown());
    }
    
    private IEnumerator UpdateCountdown()
    {
        while (_currentTime < _totalTime)
        {
            yield return null; // Đợi đến frame tiếp theo
        }
    }
    
    public void StopCountdown()
    {
        if (_fillTween != null && _fillTween.IsActive())
        {
            _fillTween.Kill();
        }
        
        StopAllCoroutines();
    }
    
    // Có thể thêm phương thức pause/resume nếu cần
    public void PauseCountdown()
    {
        if (_fillTween != null && _fillTween.IsActive())
        {
            _fillTween.Pause();
        }
    }
    
    public void ResumeCountdown()
    {
        if (_fillTween != null && _fillTween.IsPlaying() == false)
        {
            _fillTween.Play();
        }
    }
}
