using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeButton
{
    NoBounce = 0,
    BounceButton = 1,
    EslaticButton = 2,
    NoneEffect = 3,
}
public class EffectButtonActive : MonoBehaviour
{
    [SerializeField] public TypeButton typebtn;
}
