using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Animations;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.Vehicles.BattleService.Component
{
    public class BattleTankSelectLevel : MonoBehaviour
    {
        // [SerializeField] private GameObject[] vehiclesListLevel;
        // [SerializeField] private Tank _currentTank;
        // [SerializeField] private BattleRoom _battleRoom;
        // [SerializeField]
        // public Tank CurrentTank
        // {
        //     get { return _currentTank; }
        //     set {     _currentTank = value;   
        //         // Cập nhật BattleRoom khi CurrentTank thay đổi  
        //         if (_battleRoom != null)  
        //         {  
        //             int index = GetIndexOfThisLevel(); // Lấy chỉ số của BattleTankSelectLevel trong BattleRoom  
        //            
        //         }
        //     }
        // }
        //
        // private int indexLevel = 0;
        //
        // public int IndexLevel
        // {
        //     get { return indexLevel; }
        //     set { indexLevel = value; }
        // }
        //
        // public void Start()
        // {
        //     //vehiclesListLevel[indexLevel].SetActive(true);
        //     _currentTank = vehiclesListLevel[indexLevel].GetComponent<Tank>();  
        //     _battleRoom = FacilityManager.Instance.GetCurrentBattleRoom();
        // }
        //
        // [Button]
        // public void UpdateLevel()
        // {
        //     vehiclesListLevel[indexLevel].SetActive(false);
        //     indexLevel++;
        //     if (indexLevel >= vehiclesListLevel.Length)  
        //     {  
        //         indexLevel = 0; 
        //     }  
        //     
        //     vehiclesListLevel[indexLevel].SetActive(true);
        //     _currentTank = vehiclesListLevel[indexLevel].GetComponent<Tank>();
        //     
        // }
        //
        // private int GetIndexOfThisLevel()  
        // {  
        //     // Tìm chỉ số của BattleTankSelectLevel trong BattleRoom  
        //     for (int i = 0; i < _battleRoom._tankClientLevels.Count; i++)  
        //     {  
        //         if (_battleRoom._tankClientLevels[i] == this)  
        //         {  
        //             return i;  
        //         }  
        //     }  
        //     return -1; // Không tìm thấy  
        // }  
    }
}

