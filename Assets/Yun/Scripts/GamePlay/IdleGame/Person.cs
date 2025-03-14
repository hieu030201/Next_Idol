using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame
{
    public class Person : YunBehaviour
    {
        [SerializeField] protected GameObject stateUIPosition;
        [SerializeField] protected GuideArrowOnPoint guideArrowOnPoint;

        public int Level { get; set; }

        private int _id;

        public int Id
        {
            set
            {
                _id = value;
                if (!transform.Find("Id_Txt")) return;
                transform.Find("Id_Txt").GetComponent<TextMeshPro>().text = value.ToString();
                transform.Find("Id_Txt").gameObject.SetActive(false);
            }
            get => _id;
        }

        public List<WarBaseClient> ClientsList { get; private set; }

        public virtual void Reset()
        {
            Level = 0;
            Id = 0;
            ClientsList = new List<WarBaseClient>();
            if (guideArrowOnPoint)
                guideArrowOnPoint.gameObject.SetActive(false);
            PlayerFollowingGuidePoint = null;
            GuidePoint = null;
        }

        public virtual void AddClient(WarBaseClient client)
        {
            ClientsList.Add(client);
        }

        public virtual void RemoveClient(WarBaseClient client)
        {
            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] != client) continue;
                ClientsList.RemoveAt(i);
                return;
            }

            ShowErrorLog("CLIENT NOT EXIST !!!");
        }

        protected override void Awake()
        {
            base.Awake();
            ClientsList = new List<WarBaseClient>();
            if (guideArrowOnPoint)
                guideArrowOnPoint.gameObject.SetActive(false);
        }

        protected IdlePlayer PlayerFollowingGuidePoint;
        public GuidePoint GuidePoint;
        // show mũi tên hướng dẫn chỉ vào chính mình
        public void ShowGuideArrowToSelf(GuidePoint guidePoint)
        {
            GuidePoint = guidePoint;
            if (guideArrowOnPoint)
                guideArrowOnPoint.gameObject.SetActive(true);
        }

        public void HideGuideArrowOnPoint()
        {
            if (guideArrowOnPoint)
                guideArrowOnPoint.gameObject.SetActive(false);
        }

        protected virtual void FixedUpdate()
        {
        }
    }
}