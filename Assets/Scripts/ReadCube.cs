using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    public Transform tUp;
    public Transform tDown;
    public Transform tRight;
    public Transform tLeft;
    public Transform tFront;
    public Transform tBack;
    public GameObject emptyGO;


    private int layerMask = 1 << 3;
    CubeState cubeState;
    private List<GameObject> frontRays = new List<GameObject> ();
    private List<GameObject> backRays = new List<GameObject> ();
    private List<GameObject> rightRays = new List<GameObject> ();
    private List<GameObject> leftRays = new List<GameObject> ();
    private List<GameObject> upRays = new List<GameObject> ();
    private List<GameObject> downRays = new List<GameObject> ();

    // Start is called before the first frame update
    void Start()
    {

        cubeState = FindObjectOfType<CubeState>();
        SetRayTransforms();

        
    }

    // Update is called once per frame
    void Update()
    {

        ReadState();
        

    }

    public void ReadState(){
        cubeState = FindObjectOfType<CubeState>();

        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);
    }

    void SetRayTransforms(){
        leftRays = BuildRaysRightAndLeft(tLeft);
        rightRays = BuildRaysRightAndLeft(tRight);
        upRays = BuildRaysUpAndDown(tUp);
        downRays = BuildRaysUpAndDown(tDown);
        frontRays = BuildRaysFrontAndBack(tFront);
        backRays = BuildRaysFrontAndBack(tBack);
    }

    List<GameObject> BuildRaysRightAndLeft(Transform rayTransform){
        // количество лучей использумые для именования лучей,
        // поэтому мы можем быть уверены, что они расположены в правильном порядке.
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject> ();
        // это создает 9 лучей в форме стороны куба с лучом 0 слева сверху и лучом 8 внизу справа
        // |0|1|2|                 
        // |3|4|5|    
        // |6|7|8|  

        // иллюстрация к циклам ниже
        //          y        
        //     |  | 1 | |
        //   x |-1|0,0|1| 
        //     |  |-1 | |
        
       for (int y = 1; y > -2; y--){
           for (int x = -1; x < 2; x++){
                Vector3 startPos = new Vector3( rayTransform.transform.position.x + x,
                                                rayTransform.transform.position.y + y,
                                                rayTransform.transform.position.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
           }
        }
        
        return rays;
    }

    List<GameObject> BuildRaysUpAndDown(Transform rayTransform){
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject> ();
        
       for (int y = 1; y > -2; y--){
           for (int x = -1; x < 2; x++){
                Vector3 startPos = new Vector3( rayTransform.transform.position.x + x,
                                                rayTransform.transform.position.y,
                                                rayTransform.transform.position.z + y);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
           }
        }
        
        return rays;
    }

    List<GameObject> BuildRaysFrontAndBack(Transform rayTransform){
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject> ();
        
       for (int y = 1; y > -2; y--){
           for (int x = -1; x < 2; x++){
                Vector3 startPos = new Vector3( rayTransform.transform.position.x,
                                                rayTransform.transform.position.y + y,
                                                rayTransform.transform.position.z + x);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
           }
        }
        
        return rays;
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform){
        List<GameObject> facesHit = new List<GameObject>(); // ������� �� �������
        foreach (GameObject rayStart in rayStarts) {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            //пересекается ли луч в маске слоя?
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask)){
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                //print(hit.collider.gameObject.name);

            }
            else {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }

        

        return facesHit;
    }

}
