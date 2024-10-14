using UnityEngine;

public class PartnerController : MonoBehaviour
{
    public Transform leader;  // 主角的Transform
    public Vector2 offset;    // 相对于主角的偏移
    public float followSpeed = 5f;  // 跟随速度
    private Vector2 targetPosition;  // 目标位置

    void Update()
    {
        // 计算伙伴的目标位置，基于主角的位置和偏移
        targetPosition = (Vector2)leader.position + offset;

        // 平滑移动到目标位置
        transform.position = Vector2.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    // 根据主角的朝向调整偏移
    public void UpdateOffset(bool facingRight)
    {
        if (!facingRight) 
        {
            offset.x = Mathf.Abs(offset.x) * -1;  // 朝左时反转X方向
        }
        else
        {
            offset.x = Mathf.Abs(offset.x);  // 朝右时恢复X方向
        }
    }
}