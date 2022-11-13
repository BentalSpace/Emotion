using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;
    public Sprite[] portraitArr;

    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
       
    }

    public string GetTalk(int id, int talkIndex) //Object�� id , string�迭�� index
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex]; //�ش� ���̵��� �ش�
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        //id�� NPC�ѹ� , portraitIndex : ǥ����ȣ(?)
        return portraitData[id + portraitIndex];
    }
}