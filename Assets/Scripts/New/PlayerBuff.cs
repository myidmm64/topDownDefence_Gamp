using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    private int _buff = 0;

    public void AddBuff(PlayerBuffType buffType)
    {
        _buff |= (int)buffType; // 무조건 추가
    }

    public void DeleteBuff(PlayerBuffType buffType)
    {
        _buff |= (int)buffType; // 무조건 추가
        _buff ^= (int)buffType; // 둘 다 1로 같으니 0으로 변환
    }

    public bool BuffCheck(PlayerBuffType buffType)
    {
        return (_buff & (int)buffType) > 0;
    }

    public void DisableReset()
    {
        _buff = 0;
    }
}
