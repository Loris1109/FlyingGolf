/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                   Flying Golf                //////////////
/////////////////                   ActivateWind               //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////

//Dieses Script sitzt auf einem Ventialtor
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWind : MonoBehaviour
{
   
    AreaEffector2D areaEffector2;
    BoxCollider2D boxCollider2;
    GameObject predictionCopie; //Variable für die Kopie des Ventilators in der Prediction Scene
    public Sprite imageVertical;
    public Sprite imageHorizontal;

    GameObject manager;

    private float maxForce;
    private float curForce; //currentForce
    private float minForce;
    string placementDirection;
    string facing; //beschreibt die Seite, die zum Hindernis hinzeigt
    bool hasplaced = false;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        areaEffector2 = GetComponentInChildren<AreaEffector2D>();
        boxCollider2 = GetComponentInChildren<BoxCollider2D>();
        areaEffector2.enabled = true;
       
        
    }
    // Update is called once per frame
    public void Update() //die Ventilatoren werden am Anfang so platziert, dass sie vom Hinderniss weg pusten.
    {
        Debug.Log(curForce);
        if (placementDirection == "Wall" && facing == "right" && !hasplaced)
        {
            boxCollider2.offset = new Vector2(-0.42f, 0);
            boxCollider2.size = new Vector2(1, 0.34f);
            areaEffector2.forceMagnitude = curForce;
            GetComponent<SpriteRenderer>().sprite = imageHorizontal;
            GetComponent<SpriteRenderer>().flipX = true;
            hasplaced = true;
            manager.GetComponent<PredictionManager>().CopyEffector(transform);
        }
        else if (placementDirection == "Wall" && facing == "left" && !hasplaced)
        {
            boxCollider2.offset = new Vector2(0.42f, 0);
            boxCollider2.size = new Vector2(1, 0.34f);
            areaEffector2.forceMagnitude = curForce;
            GetComponent<SpriteRenderer>().sprite = imageHorizontal;
            GetComponent<SpriteRenderer>().flipX = false;
            hasplaced = true;
            manager.GetComponent<PredictionManager>().CopyEffector(transform);
        }
        else if (placementDirection == "Ground" && facing == "up" && !hasplaced)
        {
            boxCollider2.offset = new Vector2(0, -0.34f);
            boxCollider2.size = new Vector2(0.34f, 1);
            areaEffector2.forceAngle = 90;
            areaEffector2.forceMagnitude = curForce;
            GetComponent<SpriteRenderer>().sprite = imageVertical;
            GetComponent<SpriteRenderer>().flipY = true;
            hasplaced = true;
            manager.GetComponent<PredictionManager>().CopyEffector(transform);
        }
        else if (placementDirection == "Ground" && facing == "down" && !hasplaced)
        {
            boxCollider2.offset = new Vector2(0, 0.34f);
            boxCollider2.size = new Vector2(0.34f, 1);
            areaEffector2.forceAngle = 90;
            areaEffector2.forceMagnitude = curForce;
            GetComponent<SpriteRenderer>().sprite = imageVertical;
            GetComponent<SpriteRenderer>().flipY = false;
            hasplaced = true;
            manager.GetComponent<PredictionManager>().CopyEffector(transform);
        }

        //Wenn es eine Veränderung gegeben hat, 
        //wird immer einmal die Methode predict ausgeführt, damit die Flugbahn immer korreckt überträgt.
        //Die Position der Kopie
        //in der Predictionscene wird mit der Position in der "richtigen Szene" gleichgesetzt.
        if (predictionCopie.transform.position != transform.position) 
        {          
            predictionCopie.transform.position = transform.position;
            manager.GetComponent<GameManager>().Predict();
        }// Die Kraft, die der Schieberegler angibt, wird auf die Kraft der Kopie und die des Originalventilators übertragen.
        else if (curForce != areaEffector2.forceMagnitude)  
        {
            areaEffector2.forceMagnitude = curForce;
            predictionCopie.GetComponentInChildren<AreaEffector2D>().forceMagnitude = areaEffector2.forceMagnitude;
            manager.GetComponent<GameManager>().Predict();
        }//Die Richtung des "Windes" wird zwischen Kopie und Original synchronisiert.
        else if (predictionCopie.GetComponentInChildren<BoxCollider2D>().offset != boxCollider2.offset && 
                 predictionCopie.GetComponentInChildren<BoxCollider2D>().size != boxCollider2.size) 
        {
            predictionCopie.GetComponentInChildren<BoxCollider2D>().offset = boxCollider2.offset;
            predictionCopie.GetComponentInChildren<BoxCollider2D>().size = boxCollider2.size;
            
            manager.GetComponent<GameManager>().Predict();
        }//Der Winkel des areaEffectors wird zwischen beiden Objekten synchronisiert
        else if (predictionCopie.GetComponentInChildren<AreaEffector2D>().forceAngle != areaEffector2.forceAngle)
        {
            predictionCopie.GetComponentInChildren<AreaEffector2D>().forceAngle = areaEffector2.forceAngle;
            manager.GetComponent<GameManager>().Predict();
        }






    }
    //Hier bekommt das Script die nötigen Informationen her, wie der Ventilator ausgerichtet werden soll.
    public void Activate(float p_maxForce,float p_curForce ,float p_minForce,string p_placementDirection, string p_facing)
    {

        //Kontrolliert, dass auch wirklich nur diese zwei Möglichkeiten weitergegeben werden
        if (p_placementDirection == "Ground" || p_placementDirection == "Wall")
        {
            placementDirection = p_placementDirection;
        }
        //areaeffectoren sind immer nach rechts oder nach unten ausgerichtet. 
        //Daher müssen die Werte invertiert werden, wenn dieser nach links oder oben ausgerichtet werden soll.
        if (p_facing == "right"|| p_facing == "up" ) 
        {
            facing = p_facing;
            curForce = -p_curForce;
            maxForce = p_minForce;
            minForce = -p_maxForce;
        }
        else if (p_facing == "left" || p_facing == "down") //Wenn facing gleich links ist, bedeutet dies, 
        {//dass der Ventilator mit der linken Seite an einer Wand ist 
         //und er daher standardmässing nach rechts pusten soll. das gleiche gillt für "down".
            facing = p_facing;
            curForce = p_curForce;
            maxForce = p_maxForce;
            minForce = p_minForce;
        }
        //Debug.Log(p_maxForce);
        //Debug.Log(p_curForce);
        //Debug.Log(p_minForce);
        //Debug.Log(p_placementDirection);
        //Debug.Log(p_facing);

    }
    //hier wird die Verknüpfung zur Ventilator-Kopie in der predictionScene gemacht.
    public void SetPredictionCopie(GameObject newCopie)
    {
        if (newCopie != null)
        {
            predictionCopie = newCopie;
        }
    }

    public GameObject GetPredictionCopie()//Diese Methode ermöglicht es anderen Scripts auf die Kopie zuzugreifen.
    {
        return predictionCopie;
    }

    public string GetFacing()//Anderen Scripts wird es ermöglicht auf die Variable "facing" zuzugreifen.
    {
        return facing;
    }

    public float GetMaxForce()//
    {
        return maxForce;
    }

    public float GetMinForce()//
    {
        return minForce;
    }

    public void SetMaxForce(float newForce)//
    {
        maxForce = newForce;
    }
    public void SetMinForce(float newForce)//
    {
        minForce = newForce;
    }
    public float GetCurForce()//
    {
        return curForce;
        
    }
    //Es wird nur die neue Kraft übernommen, wenn diese wirklich zwischen der maximalen und minimalen Kraft liegt.
    public void SetCurForce(float newForce) 
    {
        if (newForce < maxForce && newForce > minForce)
        {
            curForce = newForce;
        }
    }
}
