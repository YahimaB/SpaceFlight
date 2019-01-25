using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using UnityEngine.SceneManagement;

public class ObjectMover: MonoBehaviour {

    public PressGesture gesture;
    public TransformGesture transformGest;
    public float rotationSpeed;
    private Vector3 lastPos;
    private Vector3 preLastPos;
    private Vector3 oldVelocity;
    private float lastDiff;
    private bool moved=false;
    private bool fly = false;

    private void OnEnable()
    {
        transformGest.StateChanged += TransformHandler;
        gesture.Pressed += GesturePressedHandler;
    }

    void OnDisable()
    {
        transformGest.StateChanged -= TransformHandler;
        gesture.Pressed -= GesturePressedHandler;
    }

    //При нажатии - остановить объект
    void GesturePressedHandler(object sender, System.EventArgs eventArgs){
        oldVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        fly = false;
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (gameObject.tag == "Asteroid")
        {
            rb.velocity = new Vector3(0, 0, -5);
        } else {
            rb.velocity = new Vector3(-4, 0, 0);
        }
        rb.angularVelocity = Random.insideUnitSphere * rotationSpeed;
    }
    //При движении
    private void TransformHandler(object sender, System.EventArgs eventArgs)
    {
        //При начале движения
        if (transformGest.State == Gesture.GestureState.Began)
        {
            moved = true;
            fly = false;
        }
        //При отжатии курсора
        if (transformGest.State == Gesture.GestureState.Changed){
            //Если повели пальцем вверх, то отменить движение
            if(transformGest.NormalizedScreenPosition.y>transformGest.PreviousNormalizedScreenPosition.y){
                gameObject.transform.position = lastPos;
                GetComponent<TransformGesture>().Cancel();
                //Продолжить прежнее движение, если не двинулось по горизонтали
                if(Mathf.Abs(lastPos.x - preLastPos.x)<0.1){
                    gameObject.GetComponent<Rigidbody>().velocity = oldVelocity;
                    moved = false;
                    fly = false;  
                }
            }
        }
        //В конце движения
        if(transformGest.State == Gesture.GestureState.Ended && moved){
            fly = true;
            moved = false;
        }

    }

     void Update()
    {
        //Длина вектора разницы настоящего положениия с прежним
        float Diff = (gameObject.transform.position - lastPos).magnitude;
        if (moved)
        {   //Если разница меньше предыдущей, то запустить в полет (либо меняется направление пальца либо падает его скорость)
            if (Diff < lastDiff)
            {
                //Насильно отжать клик на предмете
                //GetComponent<TransformGesture>().Cancel();
                GetComponent<TransformGesture>().enabled = false;
                GetComponent<PressGesture>().enabled = false;
                fly = true;
                moved = false;
            }
        }
        if (fly)
        {
            //придать предмету скорость
            gameObject.GetComponent<Rigidbody>().AddForce((lastPos - preLastPos) * 1000);
            fly = false;
        }
        preLastPos = lastPos;
        lastPos = gameObject.transform.position;
        lastDiff = Diff;
        //add speed if toooooooooo slow
        if(gameObject.GetComponent<Rigidbody>().velocity.magnitude<5.0f){
            gameObject.GetComponent<Rigidbody>().velocity *= 2;
        }

        //ограничить движение по оси Y
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;
    }

}

