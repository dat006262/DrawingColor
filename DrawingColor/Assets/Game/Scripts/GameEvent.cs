using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public void OnPointerEnterDrawArea(){
        GameManager.Instance. pointerInDrawArea = true;
    }

    public void OnPointerExitDrawArea(){
        GameManager.Instance. pointerInDrawArea = false;
    }
    
}
