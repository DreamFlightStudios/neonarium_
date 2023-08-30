using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayer : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] private Animator _playerAnimator;

    public void walk()
    {
        _playerAnimator.SetBool("walk", true);
    }
    public void stopwalk()
    {
        _playerAnimator.SetBool("walk", false);
    }
    public void run()
    {
        _playerAnimator.SetBool("run", true);
    }
    public void stoprun()
    {
        _playerAnimator.SetBool("run", true);
    }
}
