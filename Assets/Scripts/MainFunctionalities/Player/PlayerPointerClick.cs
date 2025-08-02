using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerPointerClick : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Detecta el clic izquierdo del ratón
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Crea un rayo desde la cámara hacia la posición del ratón
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Si el rayo golpea algo
            if (Physics.Raycast(ray, out hit))
            {
                // Intenta obtener el componente ClickableObject del objeto golpeado
                ObjClick objClick = hit.collider.GetComponent<ObjClick>();

                // Si el objeto tiene el componente ClickableObject, llama al método OnPointerClick
                if (objClick != null)
                {
                    // Simula los datos del evento
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    objClick.OnPointerClick(pointerEventData);
                }
            }
        }
    }
}
