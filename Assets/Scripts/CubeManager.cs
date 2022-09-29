using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{

    public List<GameObject> lowerLayer;
    public List<GameObject> upperLayer;
    public List<GameObject> middleLayer;
    public GameObject CubeCenrtalPiece;
    public bool canRotate = false, canShuffle = true;
    public Timer timer;
    bool isRotate = false;
    

    List<GameObject> AllCubePieces = new List<GameObject>(); // лист всех элементов куба

    // Добавляет все элементы листа FirstList в SecondList.
    void WritePieces(List<GameObject> FirstList, List<GameObject> SecondList){
        foreach (var piece in FirstList)
        {
            SecondList.Add(piece);
        }
    }

    // Определяет расположение грани вращения в мировых координатах.
   Vector3[] RotationVectors = {
     new Vector3(0, 1, 0), new Vector3(0, -1, 0),
     new Vector3(0, 0, 1), new Vector3(0, 0, -1),
     new Vector3(-1, 0, 0), new Vector3(1, 0, 0)
   };

    // Записывает элементы, которые пренадлежат грани UP.
   List<GameObject> UpPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.y) == 1);
        }
    }

    // Записывает элементы, которые пренадлежат грани Down.
    List<GameObject> DownPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.y) == -1);
        }
    }

    // Записывает элементы, которые пренадлежат грани Right.
    List<GameObject> RightPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.z) == -1);
        }
    }

    // Записывает элементы, которые пренадлежат грани Left.
    List<GameObject> LeftPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.z) == 1);
        }
    }

    // Записывает элементы, которые пренадлежат грани Front.
    List<GameObject> FrontPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.x) == -1);
        }
    }

    // Записывает элементы, которые пренадлежат грани Back.
    List<GameObject> BackPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.x) == 1);
        }
    }

    // Записывает элементы, которые пренадлежат среднему слою M (слой между гранями front и down).
    List<GameObject> MPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.z) == 0);
        }
    }

    // Записывает элементы, которые пренадлежат грани Down и ближайшему к ней параллельному слою (первый и второй слои снизу).
    List<GameObject> DwPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.y) == 0 || Mathf.Round(x.transform.position.y) == -1);
        }
    }

    // Записывает элементы, которые пренадлежат грани Up и ближайшему к ней параллельному слою (Третий и второй слои снизу).
    List<GameObject> UwPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.y) == 1 || Mathf.Round(x.transform.position.y) == 0);
        }
    }

    // Записывает элементы, которые пренадлежат грани Right и ближайшему к ней параллельному слою (Сторона Right и средний слой M).
    List<GameObject> RwPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.z) == 0 || Mathf.Round(x.transform.position.z) == -1);
        }
    }

    // Записывает элементы, которые пренадлежат грани Left и ближайшему к ней параллельному слою (Сторона Left и средний слой M).
    List<GameObject> LwPieces {
        get{
            return AllCubePieces.FindAll(x => Mathf.Round(x.transform.position.z) == 0 || Mathf.Round(x.transform.position.z) == 1);
        }
    }

    // Добавляет все элементы куба в AllCubePieces
    void InputPieces(){
        WritePieces(lowerLayer, AllCubePieces);
        WritePieces(middleLayer, AllCubePieces);
        WritePieces(upperLayer, AllCubePieces);
    }

    // Start is called before the first frame update
    void Start()
    {
        InputPieces();
    }

    // Update is called once per frame
    void Update()
    {
        ShuffleWhenPressed();

        if (canRotate){
            ChechInput();
        }
    }

    // Перемешивает кубик.
   IEnumerator Shuffle_(){
       canShuffle = false;

       for (int moveCount = Random.Range(15, 30); moveCount >= 0; moveCount--){
           int face = Random.Range(0,6);
           List<GameObject> facePieces = new List<GameObject>();
           
           switch(face){
               case 0: facePieces = UpPieces; break;
               case 1: facePieces = DownPieces; break;
               case 2: facePieces = RightPieces; break;
               case 3: facePieces = LeftPieces; break;
               case 4: facePieces = FrontPieces; break;
               case 5: facePieces = BackPieces; break;
           }

           StartCoroutine(Rotate(facePieces, RotationVectors[face], 90));
           // в этой функции я использую корутину для того, чтобы кубик разбирался не мнгновенно, но быстро.
           // Это сделано для того, чтобы пользователь смог посмотреть, как быстро крутятся грани во время разборки кубика(как у настоящего спидкубера).
           yield return new WaitForSeconds(0.001f);
       }
       canShuffle = true;
   }

    void ShuffleWhenPressed()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Shuffle();
    }

    public void Shuffle()
    {
        if (canShuffle)
        {

            StartCoroutine(Shuffle_());
            timer.timerRunning = false;
            timer.Seconds = 0;
            timer.Minutes = 0;

        }
    }

    // Проверяет, какая клавиша была нажата. При правильном нажатии крутятся определенные грани или слои в кубике.
    void ChechInput(){

        // Вращение граней
        if (Input.GetKeyDown(KeyCode.J))
            StartCoroutine(Rotate(UpPieces, new Vector3(0, 1, 0))); // U
        else if(Input.GetKeyDown(KeyCode.F))
            StartCoroutine(Rotate(UpPieces, new Vector3(0, -1, 0))); // U'
        else if(Input.GetKeyDown(KeyCode.I))
            StartCoroutine(Rotate(RightPieces, new Vector3(0, 0, -1))); // R
        else if(Input.GetKeyDown(KeyCode.K))
            StartCoroutine(Rotate(RightPieces, new Vector3(0, 0, 1))); // R'
        else if(Input.GetKeyDown(KeyCode.H))
            StartCoroutine(Rotate(FrontPieces, new Vector3(-1, 0, 0))); // F
        else if(Input.GetKeyDown(KeyCode.G))
            StartCoroutine(Rotate(FrontPieces, new Vector3(1, 0, 0))); // F'
        else if(Input.GetKeyDown(KeyCode.W))
            StartCoroutine(Rotate(BackPieces, new Vector3(1, 0, 0))); //B
        else if(Input.GetKeyDown(KeyCode.O))
            StartCoroutine(Rotate(BackPieces, new Vector3(-1, 0, 0))); //B'
        else if(Input.GetKeyDown(KeyCode.D))
            StartCoroutine(Rotate(LeftPieces, new Vector3(0, 0, 1))); // L
        else if(Input.GetKeyDown(KeyCode.E))
            StartCoroutine(Rotate(LeftPieces, new Vector3(0, 0, -1))); // L'
        else if(Input.GetKeyDown(KeyCode.S))
            StartCoroutine(Rotate(DownPieces, new Vector3(0, -1, 0))); // D
        else if(Input.GetKeyDown(KeyCode.L))
            StartCoroutine(Rotate(DownPieces, new Vector3(0, 1, 0))); // D'

        // вращение всего куба
        // y
        else if (Input.GetKeyDown(KeyCode.Semicolon)) // клавиша ;
            StartCoroutine(Rotate(AllCubePieces, new Vector3(0, 1, 0), 30, true));
        // y'
        else if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(Rotate(AllCubePieces, new Vector3(0, -1, 0), 30, true));
        //x
        else if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Y))
            StartCoroutine(Rotate(AllCubePieces, new Vector3(0, 0, -1), 30, true));
        //x'
        else if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.N))
            StartCoroutine(Rotate(AllCubePieces, new Vector3(0, 0, 1), 30, true));
        // z
        else if (Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(Rotate(AllCubePieces, new Vector3(1, 0, 0), 30, true));
        //z'
        else if (Input.GetKeyDown(KeyCode.P))
            StartCoroutine(Rotate(AllCubePieces, new Vector3(-1, 0, 0), 30, true)); 

        // вращение среднего слоя между правой и левой сторонами
        else if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period))
            StartCoroutine(Rotate(MPieces, new Vector3(0, 0, -1))); // M'
        else if(Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Alpha5))
            StartCoroutine(Rotate(MPieces, new Vector3(0, 0, 1))); // M

        // двойные вращения, то есть вращение стороны и ближайшему к ней среднему слою
        else if(Input.GetKeyDown(KeyCode.Z))
            StartCoroutine(Rotate(DwPieces, new Vector3(0, -1, 0))); // Dw
        else if(Input.GetKeyDown(KeyCode.Slash))
            StartCoroutine(Rotate(DwPieces, new Vector3(0, 1, 0))); // Dw'
        else if(Input.GetKeyDown(KeyCode.Comma))
            StartCoroutine(Rotate(UwPieces, new Vector3(0, 1, 0))); // Uw
        else if(Input.GetKeyDown(KeyCode.C))
            StartCoroutine(Rotate(UwPieces, new Vector3(0, -1, 0))); // Uw'
        else if(Input.GetKeyDown(KeyCode.U))
            StartCoroutine(Rotate(RwPieces, new Vector3(0, 0, -1))); // Rw
        else if(Input.GetKeyDown(KeyCode.M))
            StartCoroutine(Rotate(RwPieces, new Vector3(0, 0, 1))); // Rw'
        else if(Input.GetKeyDown(KeyCode.V))
            StartCoroutine(Rotate(LwPieces, new Vector3(0, 0, 1))); // Lw
        else if(Input.GetKeyDown(KeyCode.R))
            StartCoroutine(Rotate(LwPieces, new Vector3(0, 0, -1))); // Lw'
    }


    // Описывает и осуществляет вращение граней.
    IEnumerator Rotate(List<GameObject> pieces, Vector3 rotationVec, int speed = 30, bool isTheRotationOfTheWholeCube = false)
    {
        canRotate = false;
        canShuffle = false;
        if (!isTheRotationOfTheWholeCube)
            isRotate = true;
        else
            isRotate = false;
        int angle = 0;

        while(angle < 90){
            foreach(GameObject go in pieces){
                go.transform.RotateAround(CubeCenrtalPiece.transform.position, rotationVec, speed);
            }
            angle += speed;
            yield return null;
        }
        canShuffle = true;
        if (isTheRotationOfTheWholeCube || !isComplete())
            canRotate = true;
        isRotate = false;
    }

    // Проверяет, собран ли кубик. Если да, то выводит соответствующее сообщение в консоль.
   public bool isComplete(){
        
        if (isRotate && isSideComplete(UpPieces) &&
        isSideComplete(DownPieces) &&
        isSideComplete(RightPieces) &&
        isSideComplete(LeftPieces) &&
        isSideComplete(FrontPieces) &&
        isSideComplete(BackPieces))
        {
            //Debug.Log("Yes");
            return true;
        }
        //Debug.Log("No");
        return false;
        //Debug.Log("Кубик собран!");
    }

    // индексы центров
    // u 7
    // d 7
    // b 7
    // f 7
    // r 7
    // l 7
    // Возвращает значение "истина", если на стороне все наклейки одинакового цвета, иначе "ложь".
    bool isSideComplete(List<GameObject> pieces)
    {
        // индекс центральных элементов всех сторон.
        

        int mainPlaneIndex = pieces[7].GetComponent<CubePieceScr>().Planes.FindIndex(x => x.activeInHierarchy);
        // иногда вылетала ошибка, таким костылем она пока фикцится. Но на функциональность приложения она никак не влияла.
        /*if (mainPlaneIndex < 0 || mainPlaneIndex > 5)
        {
            Debug.Log(mainPlaneIndex);
            return false;
        }*/
        //Debug.Log(mainPlaneIndex);
        for (int i = 0; i < pieces.Count; i++)
        {
            if (!pieces[i].GetComponent<CubePieceScr>().Planes[mainPlaneIndex].activeInHierarchy ||
                pieces[i].GetComponent<CubePieceScr>().Planes[mainPlaneIndex].GetComponent<Renderer>().material.color !=
                pieces[7].GetComponent<CubePieceScr>().Planes[mainPlaneIndex].GetComponent<Renderer>().material.color)
                    return false;
        }

        return true;
    }
}
