/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                  Flying Golf                 //////////////
/////////////////                   GameManager                //////////////
/////////////////           Seminarfacharbeit ÖG 19.01.21      //////////////
/////////////////////////////////////////////////////////////////////////////

// Dieses Script sitzt auf einem Empty GameObjekt (dem GameManager)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PredictionManager predictionManager;
    [SerializeField]Vector2 force = new Vector2(1, 1);//Kraft für den "Abschlag".

    public GameObject competeLVLUI;
    public GameObject GameOverUI;

    [SerializeField] private GameObject dummy;
    [SerializeField] private GameObject launchStation;
    bool hasCopied = false;
    public bool hasShot = false;

    public bool hasWon = false;

    void Start()
    {
        predictionManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<PredictionManager>(); // Verknüpfung zum Script PredictionManager
    }


    void Update()
    {
        if (!hasCopied) //führt einmal relativ am Anfang des Spiels die Methode CopyAllObstacles und Predict aus. 
            //Siehe im Script PredictionManager die Methode CopyAllObstacles und in diesem Script die Methode Predict. 
        {
            predictionManager.CopyAllObstacles();
            Predict();
            hasCopied = true;
        }
    }

    public void Shoot() //Shoot wird ebenfalls nur einmal ausgeführt. Allerdings nur, wenn der Start Knopf gedrückt wird.
    {
        if (!hasShot)
        {
            hasShot = true;
            launchStation.transform.GetChild(0).gameObject.layer = 8;
            launchStation.GetComponentInChildren<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            // fügt dem Ball eine start Kraft hinzu, damit der Ball immer ohne einen weiteren Einfluss eine Parabel fliegt.
        }


    }// führt die Methode Predict im PredictionManager aus. Allerdings brauch dieser bestimmte Parameter, welche dieses Script besitzt. 
    //Wenn aus anderen Skripts die Methode Predict ausgeführt werden soll, läuft es daher immer über den GameManager.
    public void Predict() 
    {
        predictionManager.Predict(dummy, launchStation.transform.position, force);
    }

    public void Victory() //Startet den Win-Screen, einmalig.
    {
        if (!hasWon )
        {
            Debug.Log("Victory");
            competeLVLUI.SetActive(true);
            hasWon = true;
        }
        
    }
    public void GameOver() // startet den GameOver-Screen  
    {

        GameOverUI.SetActive(true);

    }

    public void LoadNextLvl() // lädt das nächste Level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLvl() // lädt das aktive Level nochmal 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
