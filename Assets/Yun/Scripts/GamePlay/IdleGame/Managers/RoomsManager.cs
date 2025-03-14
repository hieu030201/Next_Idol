using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Managers
{
    public class RoomsManager
    {
        public List<BaseRoom> RoomsList { get; private set; } = new();

        public void InitRoomsInArea(GameObject functionRoomsContainer, int areaNumber, RoomConfig roomConfig)
        {
            var roomsListConfigInArea = areaNumber switch
            {
                0 => roomConfig.roomsList_Area_1,
                1 => roomConfig.roomsList_Area_2,
                2 => roomConfig.roomsList_Area_3,
                3 => roomConfig.roomsList_Area_4,
                4 => roomConfig.roomsList_Area_5,
                5 => roomConfig.roomsList_Area_6,
                10 => roomConfig.roomsList_Area_1,
                _ => null
            };
            for (var i = 0; i < functionRoomsContainer.transform.childCount; i++)
            {
                var functionRoom = functionRoomsContainer.transform.GetChild(i).GetComponent<BaseRoom>();
                if (!functionRoom) continue;
                var defaultRoomDataConfig = new RoomDataConfig
                {
                    levelsPlayerToActive = new List<int> {100},
                    prices = new List<int> {0, 0, 0}
                };
                if (roomsListConfigInArea != null)
                {
                    foreach (var roomDataConfig in roomsListConfigInArea.Where(roomDataConfig =>
                        functionRoom.name == roomDataConfig.name))
                    {
                        defaultRoomDataConfig = roomDataConfig;
                    }
                }

                functionRoom.SetConfig(defaultRoomDataConfig);
                functionRoom.AreaNumber = areaNumber;
                functionRoom.gameObject.SetActive(true);
                RoomsList.Add(functionRoom);
                functionRoom.Init();
                functionRoom.Id = RoomsList.Count;
            }
        }

        public void UpdateRoomBySaveData(SaveIdleGameData saveGameData)
        {
            for (var i = 0; i < RoomsList.Count; i++)
            {
                if (saveGameData.roomsList[i].IsBuilt)
                {
                    RoomsList[i].UpdateRoomBySaveData(saveGameData.roomsList[i]);
                }
                else
                {
                    RoomsList[i].Hide(false);
                }
            }
        }

        private int _currentIndex;
        private Tween _tweenToFocusPlayer;
    }
}