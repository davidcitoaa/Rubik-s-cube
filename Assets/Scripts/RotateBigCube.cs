using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBigCube : MonoBehaviour
{
    Vector2 firstPressPos; // положение первого нажатия
    Vector2 secondPressPos; // второго

    Vector2 currentSwipe; //текущий свайп
    Vector3 previosMousePosition; // предыдущее положение мыши
    Vector3 mouseDelta;

    public GameObject target; // нужная позиция

    float speed = 400f; // скорость вращения всего кубика


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Swipe();
        Drag();
    }

    void Drag(){
//         if (Input.GetMouseButton(1)) {
//             // пока мышь удерживается, куб может перемещаться вокруг своей центральной оси
//             mouseDelta = Input.mousePosition - previosMousePosition;
//             mouseDelta *= 0.1f; // снижение скорости вращения куба
//             transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
//        }
            if (transform.rotation != target.transform.rotation){
            // автоматическое перемещение в нужную позицию
            var step = speed * Time.deltaTime;
            // вращаем в нужном направлении
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
        }
        previosMousePosition = Input.mousePosition;


    }

    void Swipe(){
        // если было нажатие правой кнопкой мыши    
        if (Input.GetMouseButtonDown(1)){
            // получить 2D-позицию первого щелчка мыши
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // print(firstPressPos);
        } if (Input.GetMouseButtonUp(1)){
            // получить 2D-позицию второго щелчка мыши
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // создаем вестор для позиций первого и второго щелчка
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            
            // корректируем 2d вектор
            currentSwipe.Normalize();

            if (LeftSwipe(currentSwipe)){
                target.transform.Rotate(0, 90, 0, Space.World);}
            else if (RightSwipe(currentSwipe))
                target.transform.Rotate(0, -90, 0, Space.World);
            else if (UpLeftSwipe(currentSwipe))
                target.transform.Rotate(90, 0, 0, Space.World);
            else if (UpRightSwipe(currentSwipe))
                target.transform.Rotate(0, 0, -90, Space.World);
            else if (DownLeftSwipe(currentSwipe))
                target.transform.Rotate(0, 0, 90, Space.World);
            else if (DownRightSwipe(currentSwipe))
                target.transform.Rotate(-90, 0, 0, Space.World);
        }
        // если было нажатие на клавишу клавиатуры
        
            //y
            /*if (Input.GetKeyDown(KeyCode.Semicolon)){ // клавиша ;
                //target.transform.Rotate(0, -90, 0, Space.World);
                StartCoroutine(Rotate(AllCubePieces, new Vector3(0, 0, -1)));    
            }*/
            //y'
           /* if (Input.GetKeyDown(KeyCode.A))
                target.transform.Rotate(0, 90, 0, Space.World);
            //x
            else if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Y))
                target.transform.Rotate(0, 0, -90, Space.World);
            //x'
            else if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.N))
                target.transform.Rotate(0, 0, 90, Space.World);
            // z
            else if (Input.GetKeyDown(KeyCode.Q))
                target.transform.Rotate(90, 0, 0, Space.World);
            //z'
            else if (Input.GetKeyDown(KeyCode.P))
                target.transform.Rotate(-90, 0, 0, Space.World);*/
        

    }

    // возвращает 1, если свайп был налево
    bool LeftSwipe(Vector2 swipe){
        return currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    // возвращает 1, если свайп был направо
        bool RightSwipe(Vector2 swipe){
            return currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
        }

    // возвращает 1, если свайп был наверх с левой стороы куба
        bool UpLeftSwipe(Vector2 swipe){
            return currentSwipe.y > 0 && currentSwipe.x < 0f;
        }

    // возвращает 1, если свайп был наверх с правой стороны куба
        bool UpRightSwipe(Vector2 swipe){
            return currentSwipe.y > 0 && currentSwipe.x > 0f;
        }

     // возвращает 1, если свайп был вниз с левой стороны куба
        bool DownLeftSwipe(Vector2 swipe){
            return currentSwipe.y < 0 && currentSwipe.x < 0f;
        }
     // возвращает 1, если свайп был вниз с правой стороны куба
        bool DownRightSwipe(Vector2 swipe){
             return currentSwipe.y < 0 && currentSwipe.x > 0f;
        }



}
