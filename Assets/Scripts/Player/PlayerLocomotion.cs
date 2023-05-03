using UnityEngine;
using System.Collections.Generic;

public class PlayerLocomotion : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField] public float collisionOffset;
    [SerializeField] private ContactFilter2D contactFilter;

    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsuleCollider;
    private PlayerManager _player;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private void Awake()
    {
        _player = GetComponent<PlayerManager>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }
    public void HandelMovment(Vector2 moveVector)
    {
        if(moveVector == Vector2.zero)
        {
            _player.playerAnimatorManager.SetAnimatorMove(false);
            return;
        }    
        else
        {
            bool isMove = TryMove(moveVector);
            if (!isMove)
            {
                isMove = TryMove(new Vector2(moveVector.x, 0));
            }
            if(!isMove)
            {
                isMove = TryMove(new Vector2(0, moveVector.y));
            }    
            _player.playerAnimatorManager.SetAnimatorMove(isMove);
        }

        if(moveVector.x < 0)
        {
            _player.spriteRenderer.flipX = true;
        }
        else if(moveVector.x > 0)
        {
            _player.spriteRenderer.flipX = false;
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if(direction == Vector2.zero)
        {
            return false;
        }
        int count = _capsuleCollider.Cast(direction,
                        contactFilter,
                        castCollisions,
                        moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            _rb.MovePosition(_rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }
}
