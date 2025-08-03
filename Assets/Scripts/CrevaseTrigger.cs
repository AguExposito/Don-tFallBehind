using UnityEngine;

public class CrevaseTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Crevasse Trigger Entered by: " + other.gameObject.name);
        if (other.gameObject.layer==7 || other.gameObject.layer == 9)
        {
            // Assuming the player has a PlayerController script with a method to handle falling into a crevasse
            TouristController touristController = other.transform.root.GetComponent<TouristController>();
            if (touristController != null)
            {
                touristController.Death();
            }
        }
    }
}
