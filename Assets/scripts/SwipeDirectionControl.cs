/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                  Flying Golf                 //////////////
/////////////////                SwipeDirectionControl         //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////

// Dieses Script sitzt auf einem Empty GameObjekt (dem GameManager)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDirectionControl : MonoBehaviour
{
    
    public float maxSwipeTime;
    public float minSwipeDistance;

    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;

    private Vector2 startSwipePos;
    private Vector2 endSwipePos;
    private float swipeLength;

    private Vector2 maxZoom;
    private Vector2 minZoom;

    ActivateWind activate;
    float maxForce;
    float minForce;
    float curForce;

    GameObject effector;
    


    

    void Update()
    {
        ZoomTest();
        if (effector != null)// sobald ein Effector(ein Ventilator) platziert wurde, wird die Methode SwipeTest ausgeführt.
        {
            SwipeTest();
        }
    }

    void ZoomTest()
    {
        if (Input.touchCount >= 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if(touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                ZoomControl(touch1.position, touch2.position);
            }
        }
    }
    void ZoomControl(Vector2 touch1Pos, Vector2 touch2Pos)//need to be tested
    {
        Vector2 Distanze = touch1Pos - touch2Pos;
        Debug.Log(Distanze);
    }
    void SwipeTest() // ein Swipe ist ein Strich über den Bildschirm mit einer bestimmten Länge und Dauer. 
        //Hier wird geguckt, ob es einen Swipe gegeben hat.
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); //Konvertiert den Touch in einen Inputwert.
            if (touch.phase == TouchPhase.Began)
            {
                swipeStartTime = Time.time; 
                startSwipePos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEndTime = Time.time;
                endSwipePos = touch.position;
                swipeTime = swipeEndTime - swipeStartTime; // Berechnung der Dauer des Swipes
                swipeLength = (endSwipePos - startSwipePos).magnitude; // Berechnug der Länge des Swipes
                if (swipeTime < maxSwipeTime && swipeLength > minSwipeDistance)// es wird geguckt ob der Touch ein Swipe war
                {
                    SwipeControl();
                }
            }
        }
    }

    void SwipeControl() //verändert die Richtung, die Kraft und das Sprite, damit der Ventilator in die Swipe-Richtung zeigt und pustet.
    {
        Vector2 Distance = endSwipePos - startSwipePos;
        float xDistance = Mathf.Abs(Distance.x); // Absolute Zahlen müssen verwendet werden, damit es keine negativen Werte gibt.
        float yDistance = Mathf.Abs(Distance.y);
        if (xDistance > yDistance) 
        {
            //swipe ist horizontal
            maxForce = activate.GetMaxForce();
            minForce = activate.GetMinForce();
            curForce = activate.GetCurForce();
            
            if (Distance.x > 0 && activate.GetFacing() != "right")
            {
                //swipe nach rechts
                Debug.Log("rechts");
                activate.GetComponentInChildren<BoxCollider2D>().offset = new Vector2(0.42f, 0);
                activate.GetComponentInChildren<BoxCollider2D>().size = new Vector2(1, 0.34f);
                activate.GetComponent<SpriteRenderer>().sprite = activate.imageHorizontal;
                activate.GetComponent<SpriteRenderer>().flipX = false;
                if (minForce < 0)
                {
                    activate.SetMaxForce(Mathf.Abs(minForce));
                    activate.SetMinForce(maxForce);
                    activate.SetCurForce(Mathf.Abs(curForce));
                }
                if (activate.GetComponentInChildren<AreaEffector2D>().forceAngle == 90)
                {
                    activate.GetComponentInChildren<AreaEffector2D>().forceAngle = 0;
                }

            }
            else if(Distance.x < 0 && activate.GetFacing() != "left")
            {
                //swipe nach links
                Debug.Log("links");
                activate.GetComponentInChildren<BoxCollider2D>().offset = new Vector2(-0.42f, 0);
                activate.GetComponentInChildren<BoxCollider2D>().size = new Vector2(1, 0.34f);
                activate.GetComponent<SpriteRenderer>().sprite = activate.imageHorizontal;
                activate.GetComponent<SpriteRenderer>().flipX = true;
                if (minForce >= 0)
                {
                   
                    Debug.Log(activate.GetMaxForce());
                    activate.SetMaxForce(minForce);
                    activate.SetMinForce(-maxForce);
                    activate.SetCurForce(-curForce);
                }
                if (activate.GetComponentInChildren<AreaEffector2D>().forceAngle == 90)
                {
                    activate.GetComponentInChildren<AreaEffector2D>().forceAngle = 0;
                }
            }
        }
        else if (yDistance > xDistance)
        {
            //swipe vertikal
            maxForce = activate.GetMaxForce();
            minForce = activate.GetMinForce();
            curForce = activate.GetCurForce();
            
            if (Distance.y > 0 && activate.GetFacing() != "up")
            {
                //swipe hoch
                Debug.Log("hoch");
                activate.GetComponentInChildren<BoxCollider2D>().offset = new Vector2(0, 0.34f);
                activate.GetComponentInChildren<BoxCollider2D>().size = new Vector2(0.34f, 1);
                activate.GetComponent<SpriteRenderer>().sprite = activate.imageVertical;
                activate.GetComponent<SpriteRenderer>().flipY = false;
                if (minForce < 0)
                {
                    activate.SetMaxForce(Mathf.Abs(minForce));
                    activate.SetMinForce(maxForce);
                    activate.SetCurForce(Mathf.Abs(curForce));
                }
                if (activate.GetComponentInChildren<AreaEffector2D>().forceAngle == 0)
                {
                    activate.GetComponentInChildren<AreaEffector2D>().forceAngle = 90;
                }
            }
            else if(Distance.y < 0 && activate.GetFacing() != "down")
            {
                //swipe runter
                Debug.Log("runter");
                activate.GetComponentInChildren<BoxCollider2D>().offset = new Vector2(0, -0.34f);
                activate.GetComponentInChildren<BoxCollider2D>().size = new Vector2(0.34f, 1);
                activate.GetComponent<SpriteRenderer>().sprite = activate.imageVertical;
                activate.GetComponent<SpriteRenderer>().flipY = true;
                if (minForce >= 0)
                {
                    Debug.Log(activate.GetMaxForce());
                    activate.SetMaxForce(minForce);
                    activate.SetMinForce(-maxForce);
                    activate.SetCurForce(-curForce);
                }
                if (activate.GetComponentInChildren<AreaEffector2D>().forceAngle == 0)
                {
                    activate.GetComponentInChildren<AreaEffector2D>().forceAngle = 90;
                }
            }
        }
    }

    public void SetEffector(GameObject newEffector) 
    {
        effector = newEffector;
        activate = effector.GetComponent<ActivateWind>();
    }
}
