/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                  Flying Golf                 //////////////
/////////////////                   CloseButton                //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////

//Dieses Skript sitzt auf dem close Knopf
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButton : MonoBehaviour
{
    GameObject effector;
    GameObject item;
    GameObject manager;


    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
    }
    public void OnClicked()//wird durch Klicken des close Knopfes ausgeführt
    {
        //Der Effector wird gelöscht und das Item wird wieder an seine Startposition zurück gesetzt
        if (effector.scene == SceneManager.GetActiveScene())
        {
            Destroy(effector.transform.parent.gameObject);
            Destroy(effector.transform.parent.GetComponent<ActivateWind>().GetPredictionCopie());
            manager.GetComponent<GameManager>().Predict();
            item.transform.position = item.GetComponent<PlaceEffector>().startPos;
            item.SetActive(true);
        }
    }

    public void SetEffector(GameObject newEffector) // kommt von ClampUIScript
    {
        effector = newEffector;
    }

    public void SetItem(GameObject newItem) // kommt von ClampUIScript
    {
        if (newItem != null)
        {
            item = newItem;
        }
    }
}
