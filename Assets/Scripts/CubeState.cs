using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    // ������� ����
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(List<GameObject> cubeSide){
        foreach (GameObject face in cubeSide){
            // ������������� �������� ������ ����� (��������� ���)
            // � �������� 4-�� ������� (����������� �������� �� ������� ����),
            // ���� ������ �� ��� �� �������� 4-� ��������.
            if (face != cubeSide[4]){
                
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }
        }
        // ��������� ������ �������� ��������
        cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
    }
}
