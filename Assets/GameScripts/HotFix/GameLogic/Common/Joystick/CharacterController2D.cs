using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 movement;
    private bool facingRight = true;

    public PartnerController[] partners;  // 伙伴数组，长度为8

   void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        transform.Translate(movement * moveSpeed * Time.deltaTime);

        if (movement.x < 0 && facingRight)
        {
            Flip();
        }
        else if (movement.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // 通知所有伙伴更新偏移
        foreach (PartnerController partner in partners)
        {
            partner.UpdateOffset(facingRight);
        }
    }
}