using UnityEngine;

public class FinishSignController : MonoBehaviour
{

    public Transform target; // El objetivo a mirar (ej: c�mara del jugador)

    void Update()
    {
        if (target == null) return;

        // Calculamos la posici�n del objetivo pero con misma Y que el cartel
        Vector3 lookAtPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

        // Rotamos el cartel hacia esa posici�n (solo yaw)
        transform.LookAt(lookAtPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            transform.GetChild(0).gameObject.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
