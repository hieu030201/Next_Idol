using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class BedRoom : BaseRoom
    {
        [SerializeField] private List<BedLevel1> bedLv1SList;
        [SerializeField] private List<BedLevel2> bedLv2SList;

        private List<Transform> _sleepPositionsLevel1;
        private List<Transform> _sleepPositionsLevel2;
        private GameObject _deserterDecor;

        public override void Init()
        {
            base.Init();
            if (transform.Find("Status_Container"))
            {
                Status = transform.Find("Status_Container").Find("Status").GetComponent<RoomStatus>();
                Status.gameObject.SetActive(false);
                // _status.gameObject.SetActive(true);
                StartStatusRotation = transform.Find("Status_Container").transform.rotation;
            }

            _deserterDecor = transform.Find("Deserter_Decor").gameObject;
            _deserterDecor.SetActive(false);

            transform.Find("Barrack_Lv1_a").gameObject.SetActive(false);
            transform.Find("Barrack_Lv1_b").gameObject.SetActive(false);
            transform.Find("Barrack_Lv1_VIP").gameObject.SetActive(false);
            transform.Find("Barrack_Lv2_a").gameObject.SetActive(false);
            transform.Find("Barrack_Lv2_b").gameObject.SetActive(false);
            transform.Find("Barrack_Lv2_VIP").gameObject.SetActive(false);

            slotsList = new List<Transform>();
            _sleepPositionsLevel1 = new List<Transform>();
            foreach (var bed in bedLv1SList)
            {
                slotsList.Add(bed.StandPosition.transform);
                _sleepPositionsLevel1.Add(bed.SleepPosition.transform);
            }

            _sleepPositionsLevel2 = new List<Transform>();
            foreach (var bed in bedLv2SList)
            {
                _sleepPositionsLevel2.Add(bed.SleepPosition1.transform);
            }

            foreach (var bed in bedLv2SList)
            {
                _sleepPositionsLevel2.Add(bed.SleepPosition2.transform);
            }
        }

        public bool IsDesertRoom { get; set; }

        [Button]
        public void ActiveDeserters()
        {
            Debug.Log("ActiveDeserters");
            _deserterDecor.transform.Find("Darkness").gameObject.SetActive(true);
            _deserterDecor.transform.Find("Escape_Hole").transform.localScale = new Vector3(.1f, .1f, .1f);
            _deserterDecor.transform.Find("Escape_Hole").transform
                .DOScale(1.5f, FacilityManager.Instance.PlayerConfig.DesertTime);
            _deserterDecor.SetActive(true);
            transform.Find("Status_Container").transform.rotation = StartStatusRotation;
            Status.gameObject.SetActive(true);
            HideConnectAnim();
            HideBuildPoint();
            FacilityManager.Instance.PlayerInfoUI.UpdateNavigation(_deserterDecor.gameObject, 1);
            FacilityManager.Instance.PlayerInfoUI.ShowNavigation();
            FacilityManager.Instance.PlayerInfoUI.CountDownNavigation(FacilityManager.Instance.PlayerConfig.DesertTime);
            IsDesertRoom = true;
            foreach (var client in ClientsList.Where(client => client))
            {
                if (client.IsBusy)
                    client.IsDeserting = true;
                else
                    client.StartDeserting();
                // client.gameObject.transform.LookAt(_deserterDecor.transform);
            }

            _damageRomCountdownNumber = FacilityManager.Instance.PlayerConfig.DesertTime;
            _desertersCoroutine = StartCoroutine(CountDownToDamageRoomRoutine());
        }

        private Coroutine _desertersCoroutine;
        private int _damageRomCountdownNumber;

        private IEnumerator CountDownToDamageRoomRoutine()
        {
            while (_damageRomCountdownNumber > 0)
            {
                if (!IsConnecting)
                {
                    _damageRomCountdownNumber--;
                    // Debug.Log("CountDownToDamageRoomRoutine");
                }
                yield return new WaitForSeconds(1f);
            }

            ActiveDamagedRoom();
        }

        [Button]
        public void DeactivateDeserters(bool isNoReward = false)
        {
            Status.gameObject.SetActive(false);
            HideConnectAnim();
            _deserterDecor.SetActive(false);
            FacilityManager.Instance.PlayerInfoUI.HideNavigation();
            if (_desertersCoroutine != null)
                StopCoroutine(_desertersCoroutine);
            IsDesertRoom = false;
            foreach (var client in ClientsList.Where(client => client))
            {
                client.StopDeserting();
            }

            FacilityManager.Instance.OnRoomDeactivatedDesert(this);

            if (isNoReward) return;
            
            if (_isShowedBuildPointLevel2)
                ShowBuildPoint();
            for (var i = 0; i < 2; i++)
            {
                DOVirtual.DelayedCall(0.5f * i, (() => FacilityManager.Instance.IncreaseLevelProgress(1)));
            }

            // DOVirtual.DelayedCall(1.5f, () => FacilityManager.Instance.AddMoney(200, Vector3.zero, true));
        }

        [Button]
        public override void ActiveDamagedRoom(bool isImmediately = false)
        {
            // Debug.Log("ActiveDamagedRoom");
            base.ActiveDamagedRoom(isImmediately);
            if (_desertersCoroutine != null)
                StopCoroutine(_desertersCoroutine);
            DeactivateDeserters(true);
            _deserterDecor.SetActive(true);
            _deserterDecor.transform.Find("Darkness").gameObject.SetActive(false);
            _deserterDecor.transform.Find("Escape_Hole").transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        [Button]
        public override void RepairDamagedRoom()
        {
            // Debug.Log("RepairDamagedRoom");
            base.RepairDamagedRoom();
            _deserterDecor.SetActive(false);
        }

        public override void Hide(bool isUpdateNavmesh = true)
        {
            base.Hide(isUpdateNavmesh);
            if (EntryPoint)
                EntryPoint.gameObject.SetActive(false);
        }

        public override void StartBuild(bool isBuildImmediately = false)
        {
            if (IsBuilt && !isBuildImmediately)
            {
                slotsList = new List<Transform>();
                foreach (var bed in bedLv2SList)
                {
                    slotsList.Add(bed.StandPosition1.transform);
                }

                foreach (var bed in bedLv2SList)
                {
                    slotsList.Add(bed.StandPosition2.transform);
                }
            }
            else if (isBuildImmediately)
            {
                if (Level == 2)
                {
                    slotsList = new List<Transform>();
                    foreach (var bed in bedLv2SList)
                    {
                        slotsList.Add(bed.StandPosition1.transform);
                    }

                    foreach (var bed in bedLv2SList)
                    {
                        slotsList.Add(bed.StandPosition2.transform);
                    }
                }
            }

            base.StartBuild(isBuildImmediately);
            FacilityManager.Instance.CheckNoBed();
            if (!isBuildImmediately)
                FacilityManager.Instance.OnRoomBuilt(this);
        }

        public override void AddClient(WarBaseClient client, bool immediately = false)
        {
            base.AddClient(client, immediately);

            if (_deserterDecor)
                client.DeserterDecor = _deserterDecor.transform;

            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] == client)
                {
                    switch (Level)
                    {
                        case 1:
                            client.SetSleepPosition(_sleepPositionsLevel1[i]);
                            break;
                        case 2:
                            client.SetSleepPosition(_sleepPositionsLevel2[i]);
                            break;
                    }
                }
            }

            if (immediately)
            {
                client.Rest(true);
            }
            else
            {
                client.EmotionState = WarBaseClient.ClientEmotionState.Rest;
            }
        }

        public override void RemoveClient(WarBaseClient client, bool isKeepPosition = false, float delay = 0)
        {
            base.RemoveClient(client, true, delay);
            client.HideConnectPoint();
        }

        public override void SetType(int type, bool isImmediately = false)
        {
            base.SetType(type, isImmediately);

            // Nếu phòng ngủ là phòng vip từ xem quảng cáo
            if (type is 2 or 5)
            {
                if (NoelDecor)
                {
                    NoelDecor.transform.Find("Furniture_Type_AB_Noel").gameObject.SetActive(false);
                    NoelDecor.transform.Find("Furniture_Type_C_Noel").gameObject.SetActive(true);
                }
            }
            else
            {
                if (NoelDecor)
                {
                    NoelDecor.transform.Find("Furniture_Type_AB_Noel").gameObject.SetActive(true);
                    NoelDecor.transform.Find("Furniture_Type_C_Noel").gameObject.SetActive(false);
                }
            }

            if (type == 2)
                outcomes[0] *= 2;
            if (type == 5)
                outcomes[1] *= 2;

            // BuildElementsList = new List<GameObject>();

            Transform buildTransform = null;

            transform.Find("Barrack_Lv1_a").gameObject.SetActive(false);
            transform.Find("Barrack_Lv1_b").gameObject.SetActive(false);
            transform.Find("Barrack_Lv1_VIP").gameObject.SetActive(false);
            transform.Find("Barrack_Lv2_a").gameObject.SetActive(false);
            transform.Find("Barrack_Lv2_b").gameObject.SetActive(false);
            transform.Find("Barrack_Lv2_VIP").gameObject.SetActive(false);

            switch (type)
            {
                case 0:
                    transform.Find("Barrack_Lv1_a").gameObject.SetActive(true);
                    buildTransform = transform.Find("Barrack_Lv1_a");
                    break;
                case 1:
                    transform.Find("Barrack_Lv1_b").gameObject.SetActive(true);
                    buildTransform = transform.Find("Barrack_Lv1_b");
                    break;
                case 2:
                    transform.Find("Barrack_Lv1_VIP").gameObject.SetActive(true);
                    buildTransform = transform.Find("Barrack_Lv1_VIP");
                    break;
                case 3:
                    transform.Find("Barrack_Lv2_a").gameObject.SetActive(true);
                    buildTransform = transform.Find("Barrack_Lv2_a");
                    break;
                case 4:
                    transform.Find("Barrack_Lv2_b").gameObject.SetActive(true);
                    buildTransform = transform.Find("Barrack_Lv2_b");
                    break;
                case 5:
                    transform.Find("Barrack_Lv2_VIP").gameObject.SetActive(true);
                    buildTransform = transform.Find("Barrack_Lv2_VIP");
                    break;
            }

            // if (EntryPoint)
            //     BuildElementsList.Add(EntryPoint.gameObject);
            if (buildTransform == null) return;
            foreach (Transform child in buildTransform)
            {
                BuildElementsList.Add(child.gameObject);
            }

            // Debug.Log(Level);
            // Debug.Log(IsBuilt);
            SavePositionsList = new List<Vector3>();
            foreach (var buildElement in BuildElementsList)
            {
                if (!buildElement) continue;
                var position = buildElement.transform.position;
                SavePositionsList.Add(position);
                if (isImmediately) continue;
                position += new Vector3(0, -3f, 0);
                buildElement.transform.position = position;
                buildElement.SetActive(false);
            }
        }
    }
}