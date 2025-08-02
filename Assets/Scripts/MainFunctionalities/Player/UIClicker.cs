using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIClicker : MonoBehaviour
{
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // o usá otro input si preferís
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2); // centro de la pantalla

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                Button btn = result.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.Invoke(); // ejecuta el click
                    Debug.Log("Botón clickeado: " + btn.name);
                    break;
                }
            }
        }
    }
}
