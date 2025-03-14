using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    private int _levelId;

    public int LevelId
    {
        get => _levelId;
        set
        {
            _levelId = value;
        }
    }

    private string _levelName;

    public string LevelName
    {
        get => _levelName;
        set
        {
            _levelName = value;
        }
    }
}
