/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////             Lorenz Steiniger-Brach           //////////////
/////////////////                 Flying Golf                  //////////////
/////////////////                   ClampUI                    //////////////
/////////////////          Seminarfacharbeit ÖG 19.01.21       //////////////
/////////////////////////////////////////////////////////////////////////////


//Das Script sitzt auf einem Kind (child) der Ventilatoren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClampUI : MonoBehaviour
{
    public Vector2 test = new Vector2(80f, 40f);
    public Button directionButton;
    public Button closeButton;
    Button copie_CloseButton;
    public Slider powerSlider;
    Slider copie_PowerSlider;
    
    bool hasAwakened = false;
    GameObject item;
    ActivateWind activateWind;

    List<GameObject> placeholderList = new List<GameObject>();

     
    private void Awake()
    {
        foreach (Transform t in gameObject.transform)//alle Kinder dieses GameObjektes werden einer Liste hinzugefügt  
        {
            placeholderList.Add(t.gameObject);
        }
        activateWind = gameObject.transform.parent.GetComponent<ActivateWind>();
        

        hasAwakened = true;
    }
    private void Update() // spawned den CloseButton, wenn dises Objekt gespawned wird.
    {
        if (hasAwakened)
        {
            copie_CloseButton = Instantiate(closeButton, GameObject.FindGameObjectWithTag("Storage").transform, true);//UI CloseButton wird erstellt
            copie_CloseButton.GetComponent<CloseButton>().SetEffector(gameObject);//dieses GameObjekt wird als Effector weitergegeben
            copie_CloseButton.GetComponent<CloseButton>().SetItem(item);//das GameObjekt Item wird weitergegeben

            copie_PowerSlider = Instantiate(powerSlider, GameObject.FindGameObjectWithTag("Storage").transform, true);// der Schieberegler wird erstellt
            copie_PowerSlider.GetComponent<UIWindEinstellungen>().SetEffector(gameObject);
            hasAwakened = false;
        }
        CalculatePlaceholder(copie_CloseButton.transform); //die Position der Platzhalter für den close Knopf wird berechnet, 
                                                           //damit dort das UI platziert werden kann
        CalculatePlaceholder(copie_PowerSlider.transform);//die Position für den Schieberegler wird berechnet
    }

    private void OnDestroy()//Wenn der Ventilator zerstört wird, wird das UI ebenfalls zerstört
    {
        if (copie_CloseButton != null)
        {
            Destroy(copie_CloseButton.gameObject);
        }
        if (copie_PowerSlider != null)
        {
            Destroy(copie_PowerSlider.gameObject);
        }
        
        hasAwakened = false;
    }
    public void GetBluePrint(GameObject newItem) // kommt vom PlaceEffector Script
    {
        item = newItem;
    }

    private Vector3 UIPosition(Transform placeholderTransform)//die Welt-Koordinate der Position wird in Screen-Koordinaten umgewandelt, 
                                                             //um die Position für das UI zu bekommen
    {
        Vector3 UIPosition = Camera.main.WorldToScreenPoint(placeholderTransform.position);
        return UIPosition;
    }

    private void CalculatePlaceholder(Transform copie_X)//Die korrekten Platzhalter für den close Knopf und den Schieberegler werden ausgesucht.
    {
        if (copie_X == copie_CloseButton.transform)// close Knopf wird immer oben rechts vom Ventilator platziert
        {
            copie_X.position = UIPosition(placeholderList[0].transform);
            copie_X.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 0);
            copie_X.tag = "closeButton";
        }
        else if (copie_X == copie_PowerSlider.transform)// Der Schieberegler wird immer auf der Seite des Hindernisses platziert.
        {
            for (int i = 0; i < placeholderList.Count; i++)
            {//jedes placeholder Gameobjekt hat ein tag, der seine Position beschreibt ("left, right, up, down"). 
             //Wenn dies mit der Orientierung des Ventilators übereinstimmt, wird dieser Platzhalter ausgewählt.
                if (placeholderList[i].CompareTag(activateWind.GetFacing()))
                {
                    copie_X.position = UIPosition(placeholderList[i].transform);
                    if (copie_X.GetComponent<RectTransform>() != null)//die Größe des Schiebereglers wird angepasst
                    {
                        copie_X.GetComponent<RectTransform>().sizeDelta = new Vector2(80f, copie_X.GetComponent<RectTransform>().sizeDelta.y);
                        copie_X.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 0);
                    }// falls der Schieberegler an den Seiten angebracht ist, wird er noch um 90 Grad gedreht
                    if (activateWind.GetFacing() == "left" || activateWind.GetFacing() == "right")
                    {
                        copie_X.rotation = Quaternion.Euler(0,0,90);
                    }
                }
            }
        }
        
    }
}
