using UnityEngine;
using System.Collections;

public class LogCollisionHandler : MonoBehaviour
{
    public float knockbackForce = 10f;
    [Range(0, 1)] public float forwardSpeedLoss = 0.2f;
    public float fadeDuration = 1f; 

    private Animator logAnimator;
    private SpriteRenderer spriteRenderer;
    private bool isDestroying = false;

    void Awake() 
    {
        logAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        logAnimator.SetFloat("AnimSpeed", 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDestroying)
        {
            CarController car = collision.gameObject.GetComponent<CarController>();
            if (car != null)
            {
                Vector2 bounceDirection = (collision.transform.position - transform.position).normalized;
                car.ApplyKnockBack(bounceDirection * knockbackForce, forwardSpeedLoss);

                logAnimator.SetFloat("AnimSpeed", 1f);

                StartCoroutine(WaitAndFade());
            }
        }
    }

    IEnumerator WaitAndFade()
    {
        isDestroying = true;

        yield return new WaitUntil(() => logAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        float counter = 0;
        Color startColor = spriteRenderer.color;

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}