using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] RotateBone rotateBone;
    [SerializeField] private Attack attack1;
    [SerializeField] private Attack attack2;
    [SerializeField] private Transform model;
    [SerializeField] private Transform spawnBullets;
    private PlayAttack _playAttack;
    private PlayerMove _playerMove;
    private bool isAttacking = false;
    private float timer = 0f;

    public float Timer { set { timer = value; } }

    void Start() {
        _playerMove = GetComponent<PlayerMove>();
        _playAttack = GetComponent<PlayAttack>();
        _playAttack.InitPool(attack1);
        _playAttack.InitPool(attack2, 2);
    }

    void Update() {
        if (isAttacking) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                isAttacking = false;
                // Eliminar la línea que modifica _playerMove.block aquí
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) || UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A)) {
            StartAttack(attack1);
        } else if (Input.GetKeyDown(KeyCode.Mouse1) || UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.B)) {
            StartAttack(attack2, 2);
        }
    }

    private void StartAttack(Attack attack, int posAtt = 1) {
        if (!isAttacking) { // Evita ataques superpuestos
            isAttacking = true;
            StartCoroutine(_playAttack.Play(attack, posAtt));
        }
    }
}