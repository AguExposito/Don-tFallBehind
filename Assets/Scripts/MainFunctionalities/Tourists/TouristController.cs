using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TouristController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] public ActivateRagdoll ragdollController;

    [Header("Target")]
    [SerializeField] private Transform playerTarget;

    [Header("Movement Settings")]
    [SerializeField] private float gravity = 9.18f;
    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float maxFollowDistance = 10f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;

    [Header("Resurrection Settings")]
    [SerializeField] private float resurrectionTime = 5f;  // Tiempo en segundos para esperar antes de resucitar
    [SerializeField] private float resurrectionTimer = 0f; // Temporizador para contar el tiempo
    [SerializeField] private bool resurrectionPending = false;  // Si estamos esperando para resucitar
    [SerializeField] private Transform ragdollRootBone;  // Ej. Hips o Pelvis

    [Header("Animation Parameters")]
    [SerializeField] private string walkParameter = "Walk";
    [SerializeField] private string idleParameter = "Idle";
    [SerializeField] private string lookingParameter = "Looking";

    [Header("Terrain Settings")]
    [SerializeField] private LayerMask groundLayer = 1;
    [SerializeField] private float maxSlopeAngle = 45f;
    [SerializeField] public bool isInWinZone;

    [Header("TouristHUD")]
    [SerializeField] GameObject questionMarks;
    [SerializeField] GameObject dead;

    private bool isFollowing = false;
    [SerializeField] private bool isFalling = false;  // Estado del ragdoll, si está cayendo o no
    [SerializeField] private bool hasFallen = false;  // Estado del ragdoll, si se cayó y está esperando resurrección
    [SerializeField] public bool isBeingGrabbed = false;  // Estado del ragdoll, si se cayó y está esperando resurrección
    [SerializeField] private bool isRagStopped;  // Si la caída está completa y está esperando la resurrección
    [SerializeField] private List<Rigidbody> ragdollRigidbodies;

    private Vector3 fallenPosition;  // Almacena la posición del ragdoll cuando cae.
    private Coroutine activeCoroutine;

    void Start()
    {
        // Obtener todos los rigidbodies del ragdoll
        ragdollRigidbodies = transform.GetComponentsInChildren<Rigidbody>().ToList();

        // Get components if not assigned
        if (animator == null)
            animator = GetComponent<Animator>();

        if (navAgent == null)
            navAgent = GetComponent<NavMeshAgent>();

        if (ragdollController == null)
            ragdollController = GetComponent<ActivateRagdoll>();

        // Find player if not assigned
        if (playerTarget == null)
            playerTarget = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Setup NavMeshAgent
        if (navAgent != null)
        {
            navAgent.speed = walkSpeed;
            navAgent.stoppingDistance = followDistance;
            navAgent.angularSpeed = 120f;
            navAgent.acceleration = 8f;
        }

        // Start idle animation
        SetIdleAnimation(true);
    }

    void Update()
    {
        isRagStopped = IsRagdollStopped();

        if (isFalling || playerTarget == null || dead.activeInHierarchy) return;

        if(gameObject.transform.position.y<-200 || ragdollRootBone.transform.position.y<-200) Death();

        // Verificar si el ragdoll ha caído y está detenido
        if (hasFallen && IsRagdollStopped() && !isBeingGrabbed)
        {
            if (activeCoroutine == null)
            {
                
                resurrectionPending = true;  // Indicamos que la resurrección está pendiente
                
                activeCoroutine = StartCoroutine(StartResurrection());
            }

        }
        else 
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                hasFallen=true;
                activeCoroutine = null;
                resurrectionPending = false;
                Debug.Log("Coroutine Stopped");
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // State Machine Logic
        if (distanceToPlayer <= followDistance)
        {
            // Player is close - IDLE state
            SetIdleAnimation(true);
            isFollowing = false;

            // Stop NavMeshAgent
            if (navAgent != null)
            {
                navAgent.isStopped = true;
            }
        }
        else if (distanceToPlayer > followDistance && distanceToPlayer < maxFollowDistance)
        {
            // Player is in follow range - WALK state
            FollowPlayer();
        }
        else
        {
            // Player is too far - LOOKING state
            SetLookingAnimation(true);
            isFollowing = false;

            // Stop NavMeshAgent
            if (navAgent != null)
            {
                navAgent.isStopped = true;
            }

            // Check if player comes back into range while looking
            if (distanceToPlayer <= maxFollowDistance)
            {
                // Player detected while looking - transition to WALK
                FollowPlayer();
            }
        }

        navAgent.isStopped = GetComponent<ActivateRagdoll>().isRagdoll;

        // Check for dangerous slopes
        CheckSlopeSafety();
        ApplyCustomGravity();
    }

    private IEnumerator StartResurrection()
    {
        Debug.Log("Starrted Resurrection Timer");
        float timer = resurrectionTime;
        // Almacenar la posición de la caída y preparar para la resurrección
        ragdollRootBone = GetComponent<ActivateRagdoll>().rigidbodies[0].transform;
        fallenPosition = ragdollRootBone.position;

        while (timer > 0f)
        {
            // Puede que en el futuro quieras salir del while si `isBeingGrabbed` vuelve a true
            resurrectionTimer = timer;
            timer -= Time.deltaTime;
            yield return null;
        }

        Resurrect();

        activeCoroutine = null; // Marcar que terminó
    }

    bool IsRagdollStopped()
    {
        // Variable que indica si todos los rigidbodies han dejado de moverse
        bool allRigidbodiesStopped = true;

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            // Comprobar si la velocidad lineal es significativa
            if (rb.linearVelocity.magnitude > 0.5f || rb.angularVelocity.magnitude > 0.5f)
            {
                allRigidbodiesStopped = false;
                break;  // Si encontramos un rigidbody en movimiento, no está detenido
            }
        }
        isFalling=!allRigidbodiesStopped;
        return allRigidbodiesStopped;  // Solo se devuelve true si todos los rigidbodies están detenidos
    }


    void FollowPlayer()
    {
        if (navAgent == null) return;

        questionMarks.SetActive(false);
        dead.SetActive(false);  

        // Resume NavMeshAgent
        navAgent.isStopped = false;

        // Set destination to player
        navAgent.SetDestination(playerTarget.position);

        // Check if moving
        if (navAgent.velocity.magnitude > 0.1f)
        {
            SetWalkAnimation(true);
            isFollowing = true;
        }
        else
        {
            // Has path but not moving - stay in walk state but idle animation
            SetWalkAnimation(false);
            isFollowing = false;
        }
    }

    void CheckSlopeSafety()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            // If slope is too steep, stop following
            if (slopeAngle > maxSlopeAngle)
            {
                navAgent.isStopped = true;
                SetIdleAnimation(true);
            }
            else
            {
                navAgent.isStopped = false;
            }
        }
    }

    void SetWalkAnimation(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool(walkParameter, isWalking);
            animator.SetBool(idleParameter, !isWalking);
            animator.SetBool(lookingParameter, false);
        }
    }

    void SetIdleAnimation(bool isIdle)
    {
        questionMarks.SetActive(false);
        dead.SetActive(false);
        if (animator != null)
        {
            animator.SetBool(idleParameter, isIdle);
            animator.SetBool(walkParameter, !isIdle);
            animator.SetBool(lookingParameter, false);
        }
    }

    void SetLookingAnimation(bool isLooking)
    {
        if (!dead.activeInHierarchy)
        {
            questionMarks.SetActive(true);
        }

        if (animator != null)
        {
            animator.SetBool(lookingParameter, isLooking);
            animator.SetBool(walkParameter, false);
            animator.SetBool(idleParameter, false);
        }
    }

    public void Fall()
    {
        isFalling = true;  // Marcar que el personaje ha caído
        hasFallen = true;  // Marcar que el personaje ha caído
        isBeingGrabbed = false;  // Marcar que el personaje ha caído
        SetLayerRecursively(gameObject, 7);

        // Activar el ragdoll
        if (ragdollController != null)
        {
            ragdollController.SetEnabled(true);
        }


        // Detener el NavMeshAgent
        if (navAgent != null)
        {
            navAgent.isStopped = true;
        }

        // Detener las animaciones
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    void ApplyCustomGravity()
    {
        if (isFalling && ragdollRigidbodies != null)
        {
            foreach (Rigidbody rb in ragdollRigidbodies)
            {
                Vector3 gravityForce = Vector3.down * gravity;  // Gravedad personalizada
                rb.AddForce(gravityForce, ForceMode.Acceleration);
            }
        }
    }
    public void Death() {
        if (dead.activeInHierarchy) return;

        SetLayerRecursively(gameObject, 10);
        ragdollController.SetEnabled(true);
        dead.SetActive(true);

        // Detener el NavMeshAgent
        if (navAgent != null)
        {
            navAgent.isStopped = true;
        }

        // Detener las animaciones
        if (animator != null)
        {
            animator.enabled = false;
        }

        playerTarget.GetComponent<Grabber>().DestroyJoint();
    }

    public void Resurrect()
    {
        Debug.Log("Resurrected!");
        // Desactivar el ragdoll
        if (ragdollController != null)
        {
            ragdollController.SetEnabled(false);
        }


        if (navAgent != null)
        {
            navAgent.enabled = false;

            NavMesh.SamplePosition(fallenPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas);
            transform.position = hit.position;

            navAgent.velocity = Vector3.zero;

            navAgent.enabled = true;

            navAgent.ResetPath();

            StartCoroutine(DelayedResetPath());
        }

        if (animator != null)
        {
            animator.enabled = true;
            animator.applyRootMotion = false;
            animator.Rebind();      // Limpia estados internos
            animator.Update(0f);    // Aplica rebind inmediatamente
            SetIdleAnimation(true);  // Restaurar animación a Idle.
        }

        hasFallen = false;  // Reiniciar el estado de caída
        // Resetear la bandera de "ha caído" para que no se repita el proceso
        isFalling = false;
        resurrectionPending = false;  // Finalizamos el temporizador

        if (isInWinZone)
        {
            Debug.Log("WIN: Resucitado dentro de zona");
        }
    }
    private IEnumerator DelayedResetPath()
    {
        yield return null; // Esperar un frame para que el agente se "asiente" en el NavMesh
        if (navAgent != null && navAgent.isOnNavMesh)
        {
            navAgent.ResetPath();
            Debug.Log("ResetPath safely called.");
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is not on the NavMesh yet.");
        }
    }
    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public void Won(Transform pos)
    {
        playerTarget = pos;
        navAgent.stoppingDistance = 0f;
        navAgent.SetDestination(pos.position);

        // Desactivamos la rotación automática para poder rotar manualmente luego
        navAgent.updateRotation = false;

        StartCoroutine(WaitAndRotateCoroutine(new Vector3(0f, -90f, 0f), 1.0f));
    }


    private bool hasRotated = false;

    private IEnumerator WaitAndRotateCoroutine(Vector3 targetEuler, float rotateDuration)
    {
        while (navAgent.pathPending)
            yield return null;

        while (navAgent.velocity.sqrMagnitude > 0.01f ||
               navAgent.remainingDistance > navAgent.stoppingDistance + 0.05f)
        {
            yield return null;
        }

        if (!hasRotated)
        {
            hasRotated = true;

            gameObject.GetComponent<DanceController>().ActivateDance(true);
            // Rotar manualmente sin conflicto
            RotateToAngle(targetEuler, rotateDuration);

        }
    }


    private Coroutine currentRotationCoroutine;

    public void RotateToAngle(Vector3 eulerAngles, float duration = 1.0f)
    {
        // Si ya hay una rotación activa, la detenemos
        if (currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
            currentRotationCoroutine = null;
        }

        Quaternion targetRotation = Quaternion.Euler(eulerAngles);
        currentRotationCoroutine = StartCoroutine(SmoothRotate(targetRotation, duration));
    }



    private IEnumerator SmoothRotate(Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;

        currentRotationCoroutine = null;
    }


}
