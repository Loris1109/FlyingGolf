/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                  Flying Golf                 //////////////
/////////////////                  EffectorKonstrukt           //////////////
/////////////////           Seminarfacharbeit ÖG 19.01.21      //////////////
/////////////////////////////////////////////////////////////////////////////

using UnityEngine;

[CreateAssetMenu(fileName = "new Card", menuName = "Interface/Effectors")]
//Scriptable Objekt 
//Dieses Objekt ist ein großer Datencontainer, der auch Methoden aufbewaren kann.
//es können mehrere Objekte mit unterschiedlichen Daten erzeugt werden.
//Scriptable Objekte können nicht auf einem Game Objekt platziert werden.
// Stattdessen werden sie wie Datentypen weiter gegeben.
public class EffectorKonstrukt : ScriptableObject
{
    public new string name;//Name des Ventilators
    public float maxForce;//MaxKraft die der Ventilator pusten kann
    public float curForce;//Start Kraft
    public float minForce;//Min Kraft, die der Ventilator pusten kann
    public GameObject placingObject;//Das Objekt welches platziert wird (der Ventilator)
    public GameObject closeButton;// der dazugehörige close Knopf
    public string placementDirection;// Definiert ob es ein Wand- oder Boden-Ventilator sein soll.
    
   

    
}
 