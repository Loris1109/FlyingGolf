/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                  Flying Golf                 //////////////
/////////////////                PredictionManager             //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////

// Dieses Script sitzt auf einem Empty GameObjekt (dem GameManager)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PredictionManager : MonoBehaviour
{
    
    public GameObject obstacles;
    public GameObject effectors;
    public int maxIterations;

    Scene currentScene;
    Scene predictionScene;

    PhysicsScene2D currentPhysicsScene;
    PhysicsScene2D predictionPhysicsScene;
    private List<GameObject> dummyObstacles = new List<GameObject>();
    private List<GameObject> dummyEffectors = new List<GameObject>();

    LineRenderer lineRenderer;//rote Line in der Scene
    public Vector3 endPos;
    GameObject dummy;

    private void Start()
    {
        Physics2D.autoSimulation = false; //Deactiviert Autosimulation, damit wir per Hand steurn können, wann simuliert werden soll.

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters); // erzeugt eine zweite Scene, prediction Scene.
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D(); // erzeugt die physics scene von der prediction scene 

        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene2D(); // Generiert physics scene von der Ausgangsscene. 

        lineRenderer = GetComponent<LineRenderer>();// deklariert den LineRenderer
    }

    private void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
        {
            currentPhysicsScene.Simulate(Time.fixedDeltaTime); // Physics scene von Ausgansszene simuliert duchgehend, sobald diese aktiv ist. 
        }
    }
      
        
    public void CopyAllObstacles() // Kopiert alle Objekte mit einem BoxCollider in die Physcsscene, die Kinder von dem Object Obstacles sind 
    {
        foreach (Transform t in obstacles.transform) // sucht für alle Transfom Komponenten im Ordner Obstacles und declariert diese t
        {
            if (t.gameObject.GetComponent<BoxCollider2D>() != null || t.gameObject.GetComponentInChildren<BoxCollider2D>() != null)
            {// guckt ob t nicht keinen (also einen oder mehrere) box collider hat 
                GameObject fakeT = Instantiate(t.gameObject); // dann wird ein neues Objekt erzeugt, fakeT
                fakeT.transform.position = t.position; //die Position wird mit dem Objekt t gleichgesetzt 
                fakeT.transform.rotation = t.rotation; //sowie die Rotation
                if (fakeT.GetComponentInChildren<ClampUI>() != null)
                {
                    fakeT.GetComponentInChildren<ClampUI>().enabled = false;
                }
                SpriteRenderer fakeR = fakeT.GetComponent<SpriteRenderer>(); // Render Komponente
                TilemapRenderer fakeTr = fakeT.GetComponent<TilemapRenderer>();
                if (fakeR)
                {
                    fakeR.enabled = false; // fakeR, also die Render Komponente von neuen Objekten wird deaktiviert
                }
                else if (fakeTr)
                {
                    fakeTr.enabled = false;
                }
                SceneManager.MoveGameObjectToScene(fakeT, predictionScene); // das Objekt wird in die prediction scene bewegt 
                dummyObstacles.Add(fakeT);  // Objekt wird zu der Liste von kopierten Hindernissen hinzugefügt 
            }
        }
    }

    public void CopyEffector(Transform newTransform) // kopiert alle Objekte mit einem BoxCollider, die Kinder von dem Objekt Obstacles sind, in die Physcsscene
    {
        foreach (Transform t in effectors.transform) //geht alle Transformkomponenten unter dem GameObject effector durch (inclusive des effector GameObjektes) 
        {// guckt ob t oder die Kinder von t nicht keinen (also einen oder mehrere) box collider hat 
            if (t.gameObject.GetComponent<BoxCollider2D>() != null || t.gameObject.GetComponentInChildren<BoxCollider2D>() != null) 
            {
                if (t == newTransform || t == newTransform.parent.transform)
                {
                    GameObject fakeT = Instantiate(t.gameObject); //ein neues GameObjekt wird erzeugt, mit den gleichen Komponenten von t; 
                    fakeT.tag = "Untagged";//ein Tag erlaubt es ein Objekt oder mehrer Objekte besser zufinden 
                    fakeT.transform.position = t.position; //die Position wird mit dem Objekt t gleichgesetzt 
                    fakeT.transform.rotation = t.rotation; //sowie die Rotation
                    if (fakeT.GetComponentInChildren<ClampUI>() != null) //sicherheit gegen Nullpointer 
                    {//die Copie soll keine UI objekte an sich geklemmt haben, daher wird dieses Script deaktiviert
                        fakeT.GetComponentInChildren<ClampUI>().enabled = false; 
                    }
                    if (fakeT.GetComponent<ActivateWind>() != null)
                    {//Die Kopie soll außerdem nicht selbst seinen Wind einstellen. Dieser wird durch das Original gesteuert
                        fakeT.GetComponent<ActivateWind>().enabled = false; 
                    }
                    SpriteRenderer fakeR = fakeT.GetComponent<SpriteRenderer>();
                    if (fakeR)
                    {
                        fakeR.enabled = false; //Die Kopie soll nicht sichbar sein.
                    }
                    SceneManager.MoveGameObjectToScene(fakeT, predictionScene); // das Objekt wird in die prediction scene bewegt 
                    dummyEffectors.Add(fakeT);  // Objekt wird zu der Liste von kopierten Hindernissen hinzugefügt 
                    if (newTransform.GetComponent<ActivateWind>() != null)
                    {//die Referenz zum kopierten Objekt wird zum Ventilator weitergegeben, damit dieser auch die Kopie verendern kann.
                        newTransform.GetComponent<ActivateWind>().SetPredictionCopie(fakeT);
                        
                    }
                }
            }
        }
    }

    public void KillEffector(GameObject newEffector)// Ein bestimmter Ventilator (Effector) wird aus der physics Scene gelöscht
    {
        foreach (GameObject t in dummyEffectors)
        {
            Debug.Log(t);
            Debug.Log(newEffector);
            if (newEffector == t || newEffector.transform.parent == t)
            {
                Destroy(t);
                dummyEffectors.Remove(t);
            }
            
        }
    }
    public void KillAllObstacles() //zertört alle Objekte in der physics Scene, die in der Liste dummyObstacles sind
    {
        foreach (var o in dummyObstacles)
        {
            Destroy(o); // löscht alle Hindernisse 
        }
        dummyObstacles.Clear();
    }
    public void Predict(GameObject subject, Vector3 currentPosition, Vector3 force)
    // eine Simulation, der physik Scene wird durchgeführt und die Flugbahn des dummys wird als Flugbahn (rote Line) im Spiel angezeigt.
    {
        if (currentPhysicsScene.IsValid() && predictionPhysicsScene.IsValid())
        {

            if (dummy == null) //wenn kein Dummy vorhanden ist 
            {
                dummy = Instantiate(subject); //spawnt das weitergegebene Game Objekt unter dem Namen Dummy
                SceneManager.MoveGameObjectToScene(dummy, predictionScene); // bewegt es in die prediction scene
            }
            dummy.layer = 8;
            dummy.transform.position = currentPosition; // dummy wird der weitergegebenen position gleichgesetzt 
            dummy.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse); // fügt dem Objekt die gleiche Kraft zu, wie dem eigentlichen Player
            lineRenderer.positionCount = 0; 
            lineRenderer.positionCount = maxIterations;


            for (int i = 0; i < maxIterations; i++) //Simuliert die physics Scene einmal für jedes i
            {
                predictionPhysicsScene.Simulate(Time.fixedDeltaTime); 
                if (dummy.transform.position == endPos)//wenn der Dummy das Ende (Ziel) erreicht hat, wird die letzt Position gesetzt und die Länge der Line wird gekürtzt 
                {
                    lineRenderer.SetPosition(i, endPos);
                    lineRenderer.positionCount = i;
                    break;
                }
                else
                {

                    lineRenderer.SetPosition(i, dummy.transform.position); //Setzt für jedes i einen neuen Punkt im Linerenderer, welcher die Position vom Dummy hat.
                }
            }
            if (dummy != null)// Zum Schluss wird der Dummy wieder gelöscht, damit ein neuer erzeugt erden kann sobald die Methode erneut aufgerufen wird
            {
                Destroy(dummy);
            }
             
        } 
    }
    public void GetendPos(Vector3 newPos)//die Endposition wird vom WinCondition Script übertragen
    {
        endPos = newPos;
    } 

    void OnDestroy() //Wenn das Script zerstört wird werden auch alle Kopien zerstört
    {
       KillAllObstacles();
    }
}
