using UnityEngine;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class GuidePoint : YunBehaviour
    {
        [SerializeField] private int levelActive;
        [SerializeField] protected GameObject arrowAnim;
        private GameObject _guideArrow;

        public int LevelActive => levelActive;

        private GameObject _testArrow;
        protected override void Awake()
        {
            base.Awake();

            if (arrowAnim)
            {
                arrowAnim.SetActive(false);
                // Debug.Log("Awake GuidePoint: " + transform.parent.name);
            }
        }

        public void ShowGuideArrow()
        {
            // Debug.Log("ShowGuideArrow: " + gameObject.name);
            arrowAnim.SetActive(true);
        }

        public bool IsShowingGuideArrow => arrowAnim.activeSelf;

        public void HideGuideArrow()
        {
            arrowAnim.SetActive(false);
        }
    }
}