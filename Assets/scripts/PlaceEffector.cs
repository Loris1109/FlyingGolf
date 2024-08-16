/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                    Flying Golf               //////////////
/////////////////                   PlaceEffector              //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////

// Dieses Script sitzt auf den Blaupausen (blaues oder pinkes Quadrat)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Diese Klasse hat alle Methoden von DragableObjekt geerbt
public class PlaceEffector : DragableObject
{
    GameObject ventilator_copie;
    RaycastHit2D hit; // Ray deklariert hit
    public EffectorKonstrukt effectorKonstrukt;
    [HideInInspector]public bool hasPlaced = false;
    float offset;
    string facing;
    GameObject manager;
    [HideInInspector] public Vector3 startPos;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        startPos = transform.position;
    }

    private void Update()
    {
        FindObstacle();
    }
    
    void FindObstacle()
    {
        Physics2D.queriesStartInColliders = false; // Ray ignoriert den ersten collider den er trifft, da dieser der eigene ist.

        // Wenn ein Wandventilator platziert werden soll, wird nach Hindernissen links oder rechts von der Blaupause gesucht.
        if (effectorKonstrukt.placementDirection == "Wall" && transform.position != startPos)
        {
            hit = Physics2D.Raycast(transform.position, transform.right, 5f); // erstellt ein ray nach rechts
            if (hit.collider != null && hit.collider.CompareTag("Obstacle") == true)
            {
                facing = "right";
                Placeing(hit.point, hit.collider.gameObject.transform.rotation, "right");
                
                if (stopedDraging == true)//wird true sobald der Finger die Blaupase nicht mehr berührt
                {
                    ventilator_copie.tag = "placed"; // Gibt der Kopie den tag (Namensschield) "placed", um sie leichter zu finden.
                    gameObject.SetActive(false);
                    
                }
            }
            else 
            {
                hit = Physics2D.Raycast(transform.position, Vector2.left, 5f); // erstellt ein ray nach links 
                if (hit.collider != null && hit.collider.CompareTag("Obstacle") == true)
                {
                    facing = "left";
                    Placeing(hit.point, hit.collider.gameObject.transform.rotation, "left");
                    
                    if (stopedDraging == true)
                    {
                        ventilator_copie.tag = "placed"; // gibt der Kopie den tag "placed", um sie leichter zu finden.
                        gameObject.SetActive(false);
                        
                    }
                }
                else
                {//Wenn kein Hindernis gefunden wurde, guckt er ob ein Ventilator bereits platziert wurde und löscht diesen dann.
                    if (GameObject.FindGameObjectsWithTag("projection").Length != 0) 
                    {
                       ResetProjection();
                    }
                    if (stopedDraging == true)//Die Blaupause springt an ihre Anfangsposition.
                    {
                        transform.position = startPos;
                    }
                }
                
                
            }
        } //Wenn ein Bodenventilator platziert werden soll, wird erst nach Hindernissen unter der Blaupause und dann über der Blaupause gesucht.
        else if (effectorKonstrukt.placementDirection == "Ground" && transform.position != startPos)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down, 5f); // erstellt ein Ray nach unten
            if (hit.collider != null && hit.collider.CompareTag("Obstacle") == true)
            {
                facing = "down";
                Placeing(hit.point,hit.collider.gameObject.transform.rotation ,"down");
                
                if (stopedDraging == true)
                {
                    ventilator_copie.tag = "placed"; // gibt der Kopie den tag "placed", um sie leichter zu finden.
                    gameObject.SetActive(false);
                    
                }
            }
            else
            {
                hit = Physics2D.Raycast(transform.position, Vector2.up, 5f); // Erstellt ein ray nach oben. 
                if (hit.collider != null && hit.collider.CompareTag("Obstacle") == true)
                {
                    facing = "up";
                    Placeing(hit.point, hit.collider.gameObject.transform.rotation, "up");
                   
                    if (stopedDraging == true)
                    {

                        ventilator_copie.tag = "placed"; // Gibt der Kopie den Tag "placed", um sie leichter zu finden.
                        gameObject.SetActive(false);
                        
                    }
                }
                else
                {
                    if (GameObject.FindGameObjectsWithTag("projection").Length != 0)
                    {
                       ResetProjection();
                    }
                    if (stopedDraging == true)
                    {
                        transform.position = startPos;
                    }
                }
               
            }
        } 


    }
    void Placeing(Vector2 hitPos, Quaternion hitRot ,string shootDirection)//Der Ventilator wird platziert.
    {//Da ein Objekt mit dem Mittelpunkt an der ausgewählten Stelle platziert wird, 
     //muss die Länge / Breite des Objektes gespeichert werden, damit es mit der Kante an der Wand platziert wird.
          offset = GetComponent<SpriteRenderer>().bounds.size.x / 2 - 0.55f; 
        if (shootDirection == "right")                         
        {
            hitPos = new Vector2(hitPos.x - offset, hitPos.y);  
        }
        else if (shootDirection == "left")
        {
            hitPos = new Vector2(hitPos.x + offset, hitPos.y);
        }
        else if (shootDirection == "down")
        {
            hitPos = new Vector2(hitPos.x, hitPos.y + offset);
        }
        else
        {
            hitPos = new Vector2(hitPos.x, hitPos.y - offset);
        }         
        
        if (GameObject.FindGameObjectsWithTag("projection").Length < 1)//Wenn noch kein Ventilator platziert wurde, dann ...
        {
            ventilator_copie = Instantiate(effectorKonstrukt.placingObject, GameObject.Find("Effectors").transform, true); // neuer Ventilator wird erstellt
            manager.GetComponent<SwipeDirectionControl>().SetEffector(ventilator_copie);//gibt dem Script SwipeDirektionControl Zugriff auf den Ventilator
            ventilator_copie.GetComponentInChildren<ActivateWind>().Activate(effectorKonstrukt.maxForce, effectorKonstrukt.curForce,effectorKonstrukt.minForce,effectorKonstrukt.placementDirection, facing);
            ventilator_copie.GetComponentInChildren<ClampUI>().GetBluePrint(gameObject);
            ventilator_copie.transform.localScale = gameObject.transform.localScale;//die Größe wird mit der Blaupause (diesem Game Objekt) gleichgesetzt.
            ventilator_copie.transform.rotation = hitRot; // setzt die Position von der Kopie auf die durchgerreichte Variable hitRot
            ventilator_copie.transform.position = hitPos; // setzt die Position von der Kopie auf die oben errechnete hitPos
            ventilator_copie.tag = "projection"; // gibt der Kopie den tag "placed", um sie leichter zu finden.
        }
        else
        {
            ventilator_copie.transform.position = hitPos;//die Position, des bereits platzierten Ventilators wird verändert
        }
        
    }

    private void ResetProjection()
    {// Zerstört alle Objekte, die nicht platziert sondern nur an die Wand projeziert wurden (isDraging war noch nicht false).
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("projection").Length; i++) 
        {
            GameObject projection = GameObject.FindGameObjectsWithTag("projection")[i];
            Destroy(projection);
            Destroy(ventilator_copie.GetComponent<ActivateWind>().GetPredictionCopie());
            manager.GetComponent<GameManager>().Predict();
        }


    }
    
}
