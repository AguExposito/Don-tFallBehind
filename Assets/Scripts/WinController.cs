using UnityEngine;
using TMPro;
public class WinController : MonoBehaviour
{
    public Transform[] spots;
    public TextMeshProUGUI remainingTourists;

    public int npcCount;

    private void OnTriggerEnter(Collider other)
    {
        TouristController tourist = other.transform.root.GetComponent<TouristController>();

        if (tourist != null && tourist.gameObject.layer == 7)
        {
            if (tourist.isInWinZone) return;

            tourist.isInWinZone = true;

            // Caso inmediato: ya está resucitado
            if (!tourist.ragdollController.isRagdoll)
            {
                Debug.Log("WIN: Entró resucitado");
                tourist.gameObject.layer = 10;

                SetLayerRecursively(tourist.gameObject,10);

                tourist.Won(spots[npcCount]);

                npcCount++;
                if (npcCount <= 3)
                {
                    remainingTourists.text = $"Remaining Tourists\r\n{npcCount}/3";
                }
            }
        }
    }
    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        TouristController tourist = other.transform.root.GetComponent<TouristController>();
        if (tourist != null)
        {
            tourist.isInWinZone = false;
        }
    }


}
