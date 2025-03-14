using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Logics;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class ArrangeRoomsTool : MonoBehaviour
    {
        [SerializeField] private GameObject roomsContainer;

        [SerializeField] private GameObject slotPrefab,
            slotsContainer;
        
        private GameObject[,] slotsArr;
        private const int RowNumber = 100;
        private const int ColumnNumber = 100;
        private const float Unit = 3.5f;
        
        [Button]
        public void GenerateSlot()
        {
            slotsArr = new GameObject[RowNumber, ColumnNumber];
            for (var row = 0; row < RowNumber; row++)
            {
                for (var column = 0; column < ColumnNumber; column++)
                {
                    var slot = Instantiate(slotPrefab, slotsContainer.transform);
                    slot.name = "R_" + row + ", C_" + column;
                    slot.transform.position = new Vector3(Unit * column, 0, Unit * row);
                    slotsArr[row, column] = slot;
                }
            }
        }

        [Button]
        public void ArrangeRooms()
        {
            ArrangeRoomLogic.ArrangeAllDefaultRoom(roomsContainer, RowNumber, ColumnNumber, Unit);
        }
    }
}
