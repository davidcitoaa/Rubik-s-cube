using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide; // активная на данный момент сторона
    private Vector3 localForward;
    private Vector3 mouseRef; // положение курсора мыши
    private bool dragging = false; // было ли вращение грани
    //private float sensitivity = 100.4f;
    private Vector3 rotation;

    private ReadCube readCube;
    private CubeState cubeState;
    

    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
        /*rotation = Vector3.zero;
        if(Input.GetKeyDown(KeyCode.H))
                rotation.z = -90f;
        transform.Rotate(rotation, Space.Self);*/

        if(dragging){
            SpinSide(activeSide);
            if(Input.GetMouseButtonUp(0))
                dragging = false;
        }

    }

    private void SpinSide(List<GameObject> side){
        // сбросить значения вращения
        rotation = Vector3.zero;

        //
        Vector3 mouseOffSet = (Input.mousePosition - mouseRef);

        if (side == cubeState.front){
            //if(Input.GetKeyDown(KeyCode.A))
                rotation.z = -90f;
            //(mouseOffSet.x + mouseOffSet.y) * sensitivity * -1;
        }
        //
        print("Yes");
        if(Input.GetKeyDown(KeyCode.H))
                rotation.z = -90f;
        //print(rotation.z);
        print(mouseOffSet);
        transform.Rotate(rotation, Space.Self);

        //
        mouseRef = Input.mousePosition;
    }

    public void Rotate(List<GameObject> side) {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;
        // создаем вектор для вращения
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;

        

    }
    
}
