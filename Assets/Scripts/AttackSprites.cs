using UnityEngine;

public class AttackSprites : MonoBehaviour, IAttackPrefab
{
    private SpriteRenderer spriteRenderer;

    public Sprite[] sprites;
    private int currentSpriteIndex = 0;

    [SerializeField] private float timeBetweenSprites = 0.1f;
    private float timer = 0f;

    [SerializeField] private int repeatTimes = 1;
    private int repeatCounter;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();


        if (sprites.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
        else
        {
            Debug.LogWarning("Asegúrate de asignar sprites en el inspector y que el objeto tenga un SpriteRenderer.");
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenSprites)
        {
            timer = 0f;
            NextSprite();
        }
    }

    private void NextSprite()
    {
        if (sprites.Length == 0 || spriteRenderer == null) return;

        currentSpriteIndex++;

        if (currentSpriteIndex >= sprites.Length)
        {
            repeatCounter--;
            currentSpriteIndex = 0;

            if (repeatCounter <= 0)
            {
                return;
            }
        }

        spriteRenderer.sprite = sprites[currentSpriteIndex];
    }

    public void DoAttack(PlayAttack.MoveBulletDelegate moveCallback)
    {
        repeatCounter = repeatTimes;
        currentSpriteIndex = 0;
        
        StartCoroutine(moveCallback(gameObject, (timeBetweenSprites*sprites.Length)*repeatTimes, 1, false));
    }
}
