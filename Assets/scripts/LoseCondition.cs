/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                   Flying Golf                //////////////
/////////////////                  LoseCondition               //////////////
/////////////////           Seminarfacharbeit ÖG 19.01.21      //////////////
/////////////////////////////////////////////////////////////////////////////

// Dieses Script sitzt auf dem Spieler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    Vector3 storedpos;
    bool isChecking = false;
    
    private void Update()
    {
        if (FindObjectOfType<GameManager>().hasShot && !isChecking) //Wenn der start Knopf gedrückt wurde und die Position nicht überprüft wird 
        {
            isChecking = true;
            Debug.Log(storedpos);
            StartCoroutine(Restart());
        }
        else if (transform.position.y < -7f)//das Spiel ist zu Ende, sobald der Ball unter eine Höhe von -5 fällt.
        {
            FindObjectOfType<GameManager>().GameOver();

        }
    }
    IEnumerator Restart()
    {
        storedpos = transform.position;//die momentane Position wird gespeichert
        yield return new WaitForSeconds(.2f);//das Programm wartet 0.2s 
        if (transform.position == storedpos && !FindObjectOfType<GameManager>().hasWon)
        {//Wenn der Ball (grünes Viereck) immer noch an der gleichen Position ist und das Spiel nicht gewonnen wurde, wird das Spiel beendet. 
            FindObjectOfType<GameManager>().GameOver();
        }
        else
        {
            isChecking = false;
        }
    }



}
