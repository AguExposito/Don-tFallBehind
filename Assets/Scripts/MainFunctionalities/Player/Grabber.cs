using UnityEngine;
using UnityEngine.AI;

public class Grabber : MonoBehaviour
{
    public Transform grabPoint;  // Un empty frente a la cámara
    public float grabRange = 5f;
    [SerializeField] private float maxThrowSpeed = 5f;
    [SerializeField] private float maxAngularSpeed = 5f;
    [SerializeField] private float throwBoostMultiplier = 1.2f;

    public LayerMask touristLayer;

    // Audio references
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabSound;
    [SerializeField] private AudioClip releaseSound;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip grabFailSound;

    private Rigidbody grabbedRigidbody;
    private ConfigurableJoint grabJoint;
    private TouristController touristController;

    void Start()
    {
        // Auto-assign AudioSource if not set
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedRigidbody == null)
                TryGrab();
            else
                ReleaseGrabWithInertia(false);
                
        }

        if (Input.GetMouseButtonDown(1) && grabbedRigidbody != null)
        {
            ReleaseGrabWithInertia(true);
            if (touristController != null) touristController.Fall();
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange, touristLayer))
        {
            Rigidbody rb = hit.rigidbody;
            Debug.Log("Raycast hit: " + hit.collider.name);
            if (rb != null)
            {
                grabbedRigidbody = rb;
                
                grabbedRigidbody.transform.root.GetComponent<ActivateRagdoll>().SetEnabled(true);
                touristController = grabbedRigidbody.transform.root.GetComponent<TouristController>();
                grabbedRigidbody.transform.root.GetComponent<NavMeshAgent>().isStopped = true;  // Detener el NavMeshAgent del ragdoll
                touristController.isBeingGrabbed = true;  // Detener el NavMeshAgent del ragdoll
                touristController.SetLayerRecursively(touristController.gameObject,9);  // Detener el NavMeshAgent del ragdoll

                // Asegúrate de que el grabPoint tenga un Rigidbody y ConfigurableJoint
                Rigidbody grabPointRb = grabPoint.GetComponent<Rigidbody>();
                if (grabPointRb == null)
                {
                    grabPointRb = grabPoint.gameObject.AddComponent<Rigidbody>();
                    grabPointRb.isKinematic = true;
                }

                // Aquí agregamos el ConfigurableJoint al grabPoint si no lo tiene
                grabJoint = grabPoint.GetComponent<ConfigurableJoint>();

                if (grabJoint == null)
                {
                    grabJoint = grabPoint.gameObject.AddComponent<ConfigurableJoint>();
                }

                // Conectar el grabPoint con el ragdoll
                grabJoint.connectedBody = grabbedRigidbody;

                // Configuración de movimiento
                grabJoint.xMotion = ConfigurableJointMotion.Limited;
                grabJoint.yMotion = ConfigurableJointMotion.Limited;
                grabJoint.zMotion = ConfigurableJointMotion.Limited;

                grabJoint.angularXMotion = ConfigurableJointMotion.Limited;
                grabJoint.angularYMotion = ConfigurableJointMotion.Limited;
                grabJoint.angularZMotion = ConfigurableJointMotion.Limited;

                // Ajuste de las restricciones (como la distancia máxima)
                SoftJointLimit limit = new SoftJointLimit();
                limit.limit = 0.5f;  // Distancia máxima de movimiento
                grabJoint.linearLimit = limit;
                //Invoke("ApplyBreakForce", 0.1f);  // Aplicar fuerza de ruptura después de un breve retraso

                // Opcional: Debug
                Debug.Log("ConfigurableJoint creado y conectado entre grabPoint y ragdoll.");
                
                // Play grab sound
                PlaySound(grabSound);
            }
        }
        else
        {
            // Play grab fail sound when trying to grab but nothing is in range
            PlaySound(grabFailSound);
        }
    }

    void ApplyBreakForce() {
        // Ajustar las fuerzas de ruptura
        grabJoint.breakForce = 10000f;  // Fuerza alta para evitar ruptura instantánea
        grabJoint.breakTorque = 10000f;
    }

    void ReleaseGrabWithInertia(bool applyThrowBoost)
    {
        DestroyJoint();

        if (grabbedRigidbody != null)
        {
            grabbedRigidbody.isKinematic = false;

            // Obtener velocidad acumulada (inercia)
            Vector3 releaseVelocity = grabbedRigidbody.linearVelocity;

            // Si fue un "throw" explícito, amplifica la fuerza
            if (applyThrowBoost)
            {
                float boostMultiplier = 1.5f;
                releaseVelocity *= boostMultiplier;
                
                // Play throw sound
                PlaySound(throwSound);
            }
            else
            {
                // Play release sound
                PlaySound(releaseSound);
            }

            // Limitar la magnitud final
            float maxThrowSpeed = 10f;
            releaseVelocity = Vector3.ClampMagnitude(releaseVelocity, maxThrowSpeed);
            grabbedRigidbody.linearVelocity = releaseVelocity;

            // Limitar rotación también
            float maxAngularSpeed = 10f;
            grabbedRigidbody.angularVelocity = Vector3.ClampMagnitude(grabbedRigidbody.angularVelocity, maxAngularSpeed);

            // Damping si quieres frenar un poco después de soltar
            grabbedRigidbody.linearDamping = 0.5f;
            grabbedRigidbody.angularDamping = 0.5f;

            grabbedRigidbody = null;

            if (touristController != null)
            {
                touristController.Fall();
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void DestroyJoint() {
        if (grabJoint != null)
        {
            Destroy(grabJoint);
            grabJoint = null;
            Debug.Log("Joint destruido.");
        }
    }
}
