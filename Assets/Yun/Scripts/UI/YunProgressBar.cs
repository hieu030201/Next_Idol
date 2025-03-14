using System;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Cores;

namespace Yun.Scripts.UI
{
    public class YunProgressBar : YunBehaviour
    {
        [SerializeField] private int distance;
        [SerializeField] private GameObject childBar;
        [SerializeField] private GameObject childBarFollow;
        [SerializeField] private GameObject childBarFollow2;

        private float _startY;
        private float _followDistance;

        protected override void Awake()
        {
            base.Awake();
            _startY = childBar.transform.localPosition.y;
            // Debug.Log("Yun progress bar :" + _startY);
            if (childBarFollow)
            {
                childBarFollow.transform.position = childBarFollow2.transform.position;
            }
        }

        public float Progress;
        public void UpdateProgress(float value, bool isImmediately = true)
        {
            if (value > 1)
                value = 1;
            if (value < 0)
                value = 0;
            Progress = value;
            if (isImmediately)
            {
                childBar.GetComponent<RectTransform>().localPosition =
                    new Vector3(-distance + distance * value, _startY, 0);
            }
            else
            {
                childBar.GetComponent<RectTransform>()
                    .DOLocalMove(new Vector3(-distance + distance * value, _startY, 0), 0.5f);
            }

            if (!childBarFollow) return;
            // var yPos = childBarFollow.transform.localPosition.y;
            // var zPos = childBarFollow.transform.localPosition.z;
            // var xPos = _followDistance + childBarFollow.transform.localPosition.x;
            // childBarFollow.transform.localPosition = new Vector3(xPos, yPos, zPos);
            // childBarFollow.transform.position = childBarFollow2.transform.position;
        }

        public void Update()
        {
            if (childBarFollow)
                childBarFollow.transform.position = childBarFollow2.transform.position;
        }

        public void HideChildBar()
        {
            childBar.SetActive(false);
        }
    }
}