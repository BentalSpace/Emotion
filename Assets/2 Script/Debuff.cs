using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffType { batPoop}
public class Debuff : MonoBehaviour
{
    [SerializeField]
    DebuffType type;
    public DebuffType Type { get { return type; } }

    /**디버프 수치
     *batPoop : 0. 이동속도 감소 / 1. 점프력 감소*/
    public float[] floatValue;
}
