using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffType { batPoop}
public class Debuff : MonoBehaviour
{
    [SerializeField]
    DebuffType type;
    public DebuffType Type { get { return type; } }

    /**����� ��ġ
     *batPoop : 0. �̵��ӵ� ���� / 1. ������ ����*/
    public float[] floatValue;
}
