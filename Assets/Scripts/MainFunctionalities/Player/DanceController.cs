using UnityEngine;

public class DanceController : MonoBehaviour
{
    public int danceInt;
    public Animator animator;
    public bool rotates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.applyRootMotion = false;

        animator.SetInteger("DanceInt",danceInt);

        if (rotates)
        {
            animator.SetBool("Rotate", true);
        }
    }

    public void ActivateDance(bool state) {
        if (state)
        {
            animator.SetLayerWeight(0, 0);
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(0, 1);
            animator.SetLayerWeight(1, 0);
        }
    }



}
