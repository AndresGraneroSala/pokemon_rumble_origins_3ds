using UnityEngine;
using UnityEngine.Serialization;

public class Opponent : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToAttack = 1;
    [SerializeField] private float distanceToMove = 10;
    [SerializeField] private Attack attack;
    private RotateBone[] _bones;
    [SerializeField] float rotationSpeedDelay = 10;
    private float _distanceToPlayer = 0;
    private bool _isAttacking;
    private float _upChecker = 0.1f;
    [SerializeField] private Attack.TypeAttack typePokemon1;
    [SerializeField] private Attack.TypeAttack typePokemon2;
    private PlayAttack _playAttack;
    private DirectMovement _directMovement;
    [SerializeField] private float detectionDistance = 1.0f;
    [SerializeField] private LayerMask obstacleLayer;
    public float RotationSpeedDelay { get { return rotationSpeedDelay; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }
    public Attack.TypeAttack TypePokemon1 { get { return typePokemon1; } }
    public Attack.TypeAttack TypePokemon2 { get { return typePokemon2; } }
    private float _raycastCooldown = 0.2f;

    [SerializeField] private bool randomize = true;
    [SerializeField] private float randomDelayAttack=0.5f;
    [SerializeField] private float randomDelayMove=0.5f;

    [SerializeField] private int cp=100;

    public int CP
    {
        get { return cp; }
    }
    
    private float _delayMove = 0;
    private float _delayAttack = 0;
    
    private float _timerMove = 0;
    private float _timerAttack;
    
    void Start() {
        
        _playAttack = GetComponent<PlayAttack>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        RotateBone[] allBones = GetComponentsInChildren<RotateBone>();
        _bones = new RotateBone[allBones.Length];
        int count = 0;
        foreach (RotateBone bone in allBones) {
            if (!bone.IsAttack) {
                _bones[count++] = bone;
            }
        }
        System.Array.Resize(ref _bones, count);
        _playAttack.InitPool(attack);
        _directMovement = GetComponent<DirectMovement>();

        if (randomize)
        {
            _delayMove = Random.Range(0, randomDelayMove);
            _delayAttack = Random.Range(0, randomDelayAttack);
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
        _playAttack.ChangeTarget(newTarget);
    }
    
    public void Update()
    {
        if (_isAttacking) return;

        _distanceToPlayer = ((transform.position - target.position).sqrMagnitude)/4;

        if (_distanceToPlayer <= distanceToMove)
        {
            _timerMove += Time.deltaTime;

            if (_timerMove < _delayMove)
            {
                return;
            }
            
            
            _directMovement.enabled = (_distanceToPlayer > distanceToAttack);
            if (!_directMovement.enabled && !_isAttacking)
            {
                _timerAttack += Time.deltaTime;

                if (_timerAttack  < _delayAttack)
                {
                    return;
                }

                _timerMove = 0;
                _timerAttack = 0;
                
                StartCoroutine(_playAttack.Play(attack, 1,_delayAttack));
            }
        }
        else
        {
            _directMovement.enabled = false;
        }
    }


    public void ChangeSpeedBones(float speed) {
        foreach (RotateBone bone in _bones) {
            bone.SetSpeedState(speed);
        }
    }

    public bool IsColliderInAnyDirection() {
        // --- Cambio Clave 5: Eliminar cooldown y verificar todas las direcciones ---
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        foreach (Vector3 dir in directions) {
            if (IsColliderInDirection(dir)) return true;
        }
        return false;
    }

    public bool IsColliderInDirection(Vector3 direction) {
        Vector3 worldDir = transform.TransformDirection(direction);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * _upChecker, worldDir, out hit, detectionDistance, obstacleLayer)) {
            return hit.collider != null && !hit.collider.isTrigger;
        }
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        if (!enabled) return;
        
        Gizmos.color = Color.blue;
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        foreach (Vector3 direction in directions) {
            Gizmos.DrawRay(transform.position + Vector3.up * _upChecker, transform.TransformDirection(direction) * detectionDistance);
        }
    }
#endif
    
}