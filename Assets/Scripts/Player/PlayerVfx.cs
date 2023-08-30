using UnityEngine;

public class PlayerVfx
{
    private readonly Animator _animator;

    public PlayerVfx(Animator animator) => _animator = animator;

    public void Move(float speed) => _animator.SetFloat("MoveSpeed", speed);
    public void Fall(float speed) => _animator.SetFloat("FallSpeed", speed);
}
