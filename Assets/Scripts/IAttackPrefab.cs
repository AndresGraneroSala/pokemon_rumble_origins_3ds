using System;

public interface IAttackPrefab
{
    void DoAttack(PlayAttack.MoveBulletDelegate moveCallback);
}