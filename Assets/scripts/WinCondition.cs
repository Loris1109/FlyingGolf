/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                   Flying Golf                //////////////
/////////////////                  WinCondition                //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////

// Dieses Script sitzt auf dem Ziel
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{

    //Sobald der Ball oder die Kopie das Ziel berühren, wird die Victory Method ausgeführ und der Ball verschwindet.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            FindObjectOfType<GameManager>().Victory();
        }
        else if (collision.gameObject.layer == 8)
        {
            FindObjectOfType<PredictionManager>().GetendPos(collision.transform.position);
            Destroy(collision);
        }
    }
}
