using System;
using UnityEngine;
using System.Collections;

public class PlayAttack : MonoBehaviour
{
    [SerializeField] RotateBone rotateBone;
    [SerializeField] private Transform model;
    [SerializeField] private Transform spawnBullets;
    private PlayerMove _playerMove;
    private PlayerAttack _playerAttack;
    private Opponent _opponent;
    private Transform target;
    private GameObject _prefAttack1, _prefAttack2;
    private bool isPlayer;
    [SerializeField] private SpriteRenderer hitDetector;

    void Start()
    {
        _playerMove = GetComponent<PlayerMove>();
        _opponent = GetComponent<Opponent>();
        _playerAttack = GetComponent<PlayerAttack>();
        isPlayer = gameObject.CompareTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (!isPlayer)
        {
            hitDetector.sortingOrder = -19;
        }
    }

    private void OnDisable()
    {
        Destroy(hitDetector);
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void InitPool(Attack attack, int posAtt = 1)
    {
        if (!attack || attack.Bullet == null) return;

        GameObject prefab = Instantiate(attack.Bullet, spawnBullets);
        StartCoroutine(MoveBullet(prefab, 0, 0, false));
        if (posAtt == 1) _prefAttack1 = prefab;
        else _prefAttack2 = prefab;
    }

    public IEnumerator Play(Attack attack, int posAtt = 1, float delayAttack = 0)
    {
        if (!attack || (isPlayer && (GameManager.instance.PlayerSpeed <= 0|| Time.timeScale <= 0))) yield break;
        if (isPlayer)
        {
            _playerMove.block = true;
            _playerAttack.Timer = attack.FireRate;
        }
        else
        {
            _opponent.IsAttacking = true;
            _opponent.ChangeSpeedBones(0.25f);
            hitDetector.sortingOrder = 19;
        }

        rotateBone.Configure(attack.Rotate);
        Coroutine rotateCoroutine = null;
        if (!isPlayer) rotateCoroutine = StartCoroutine(RotateTowardsCoroutine());

        yield return new WaitForSeconds(isPlayer ? attack.Delay : attack.DelayOpponent + delayAttack);

        if (!isPlayer)
        {
            hitDetector.sortingOrder = -19;
        }

        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
        rotateBone.PlayAttack();

        if (attack.MovePlayer > 0)
        {
            if (!attack.IsBulletBefore)
            {
                yield return StartCoroutine(MoveCoroutine(attack.MovePlayer, attack.MovePlayerSpeed));
            }
            else
            {
                StartCoroutine(MoveCoroutine(attack.MovePlayer, attack.MovePlayerSpeed));
            }
        }

        if (!isPlayer)
        {
            _opponent.IsAttacking = false;
            _opponent.ChangeSpeedBones(1);
        }

        if (attack.Bullet != null)
        {
            GameObject bullet = posAtt == 1 ? _prefAttack1 : _prefAttack2;
            StartCoroutine(MoveCoroutine(attack.MovePlayer, attack.MovePlayerSpeed));
            AttackCollision ac = bullet.GetComponent<AttackCollision>();
            if (ac) ac.SetDamage(attack.Damage, attack.Type, isPlayer);

            Billboard bb = bullet.GetComponent<Billboard>();
            if (bb) bb.SetBillboard(model.eulerAngles.y);

            StartCoroutine(MoveBullet(bullet, -1, 1, true));

            IAttackPrefab iAttack = bullet.GetComponent<IAttackPrefab>();
            if (iAttack == null)
            {
                StartCoroutine(MoveBullet(bullet, attack.MovePlayer, attack.MovePlayerSpeed, false));
            }
            else
            {
                iAttack.DoAttack(MoveBullet);
            }

        }
    }

    public delegate IEnumerator MoveBulletDelegate(GameObject bullet, float distance, float speed, bool enable);


    public IEnumerator MoveBullet(GameObject bullet, float distance, float speed, bool enable)
    {
        if (enable)
        {
            bullet.transform.position -= new Vector3(0, -1, 0); // Mover fuera de pantalla
            bullet.GetComponent<SpriteRenderer>().sortingOrder = 20;

        }
        else
        {
            if (distance > 0)
            {
                yield return new WaitForSeconds(distance / speed);
            }

            bullet.transform.position += new Vector3(0, -1, 0); // Mover fuera de pantalla
            bullet.GetComponent<SpriteRenderer>().sortingOrder = -20;
        }
    }

    private IEnumerator MoveCoroutine(float distance, float speed)
    {
        if (isPlayer) _playerMove.block = true;

        Vector3 startPosition = transform.position;
        Vector3 direction = model.forward.normalized;
        Vector3 targetPosition = startPosition + direction * distance;

        float totalTime = distance / speed;
        float elapsedTime = 0f;

        Vector3 lastSafePosition = transform.position;

        while (elapsedTime < totalTime)
        {
            // Verificar colisión según el tipo
            bool hasObstacle = (isPlayer && _playerMove.IsColliderInFront()) ||
                               (!isPlayer && _opponent.IsColliderInAnyDirection());

            if (hasObstacle)
            {
                // Si se detecta colisión, volver al último punto válido
                transform.position = lastSafePosition;
                break;
            }

            // Avanzar
            float progress = (elapsedTime * speed) / distance;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            // Guardar posición segura
            lastSafePosition = transform.position;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Solo mover al final si NO hubo colisión
        if (elapsedTime >= totalTime)
        {
            transform.position = targetPosition;
        }

        if (isPlayer) _playerMove.block = false;
    }

    private IEnumerator RotateTowardsCoroutine()
    {
        Transform rotateTarget = model;
        while (true)
        {
            Vector3 direction = target.position - transform.position;
            if (direction.magnitude < 0.01f) yield break;

            rotateTarget.rotation = Quaternion.Lerp(
                rotateTarget.rotation,
                Quaternion.LookRotation(direction),
                _opponent.RotationSpeedDelay * Time.deltaTime
            );
            yield return null;
        }
    }

    private void OnDestroy()
    {
        Destroy(_prefAttack1);
        Destroy(_prefAttack2);
    }
}