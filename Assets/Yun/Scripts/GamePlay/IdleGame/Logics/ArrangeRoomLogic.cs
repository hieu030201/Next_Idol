using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Logics
{
    public abstract class ArrangeRoomLogic
    {
        public static void ArrangeAllDefaultRoom(GameObject roomsContainer, int rowNumber, int columnNumber, float unit)
        {
            var usedSlotsArr =
                CreateUsedSlotsArrFromRoomsContainer(roomsContainer, rowNumber, columnNumber, unit);

            // Duyệt hết các default room để sắp lại tường của các default room
            for (var i = 0; i < roomsContainer.transform.childCount; i++)
            {
                var child = roomsContainer.transform.GetChild(i);
                if (!child.gameObject.activeSelf) continue;
                var row = (int) Mathf.Round(child.transform.position.z / unit);
                var column = (int) Mathf.Round(child.transform.position.x / unit);
                if (child.GetComponent<DefaultRoom>())
                {
                    ArrangeDefaultRoom(usedSlotsArr, child, row, column);
                }
            }
        }

        private static void ArrangeDefaultRoom(bool[,] usedSlotsArr, Transform roomChild, int row, int column)
        {
            var room = roomChild.GetComponent<DefaultRoom>();
            var width = roomChild.GetComponent<BaseRoom>().width;
            var height = roomChild.GetComponent<BaseRoom>().height;
            room.wallUp.SetActive(true);
            room.wallDown.SetActive(true);
            room.wallLeft.SetActive(true);
            room.wallRight.SetActive(true);
            if (usedSlotsArr[row + height, column])
                room.wallUp.SetActive(false);
            if (row - 1 >= 0 && usedSlotsArr[row - 1, column])
                room.wallDown.SetActive(false);
            if (column - 1 >= 0 && usedSlotsArr[row, column - 1])
                room.wallLeft.SetActive(false);
            if (usedSlotsArr[row, column + width])
                room.wallRight.SetActive(false);
        }

        private static bool[,] CreateUsedSlotsArrFromRoomsContainer(GameObject roomsContainer, int rowNumber,
            int columnNumber, float unit)
        {
            var usedSlotsArr = new bool[rowNumber, columnNumber];
            // Duyệt hết các room để cập nhật những ô đã được sử dụng để xây nhà
            for (var i = 0; i < roomsContainer.transform.childCount; i++)
            {
                var child = roomsContainer.transform.GetChild(i);
                if (!child.gameObject.activeSelf) continue;
                var row = (int) Mathf.Round(child.transform.position.z / unit);
                var column = (int) Mathf.Round(child.transform.position.x / unit);
                // Debug.Log("Room: " + i + ", row: " + row + ", column: " + column);
                // Debug.Log("room name: " + child.name + ", row: " + row + ", column: " + column + ", width: " + child.GetComponent<BaseRoom>().width + ", height: " + child.GetComponent<BaseRoom>().height);
                for (var j = 0; j < child.GetComponent<BaseRoom>().height; j++)
                {
                    for (var k = 0; k < child.GetComponent<BaseRoom>().width; k++)
                    {
                        usedSlotsArr[row + j, column + k] = true;
                        Debug.Log("Row: " + (row + j) + ", Column: " + (column + k));
                    }
                }
            }

            return usedSlotsArr;
        }
    }
}