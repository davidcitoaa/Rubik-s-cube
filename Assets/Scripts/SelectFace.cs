using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    CubeState cubeState; // состояние куба
    ReadCube readCube;
    int layerMask = 1 << 3;


    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(0)){
            // узнаем изначальное состояние куба
            readCube.ReadState();
            // ЭТО НУЖНО ДОПИСАТЬ И ПЕРЕВЕСТИ!!!
            //raycast от мыши к кубу, чтобы увидеть, попала ли грань
            RaycastHit hit; // нажатие на грань куба
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask)){
                GameObject face = hit.collider.gameObject;
                // составляем список всех сторон куба
                List<List<GameObject>> cubeSides = new List<List<GameObject>> (){
                    cubeState.up,
                    cubeState.down,
                    cubeState.right,
                    cubeState.left,
                    cubeState.front,
                    cubeState.back};
                    // если касание было внутри стороны
                    foreach (List<GameObject> cubeSide in cubeSides) {
                        if (cubeSide.Contains(face)){
                            // взять его ??
                            cubeState.PickUp(cubeSide);
                        }
                    }
            }
        }
    }

    
}
