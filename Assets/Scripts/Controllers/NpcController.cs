using UnityEngine;

public class NpcController : MonoBehaviour {

    [Range(2f, 4f)]
    public float maxVelocity;
    public AudioClip hitSound;
    public AudioClip[] deathSounds;
    public Sprite[] bloodSprites;

    private bool isDead = false;
    [HideInInspector]
    public bool isPickable = true;

    // Components
    Animator anim;
    Rigidbody2D body;
    Collider2D charCollider;
    SpriteRenderer render;
    AudioSource audioSource;

    // Movement components
    MovementFlock flock;
    MovementRunAway runAway;

    void Start() {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        charCollider = GetComponent<CircleCollider2D>();
        render = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        flock = GetComponent<MovementFlock>();
        runAway = GetComponent<MovementRunAway>();

        maxVelocity = Random.Range(2f, 4f);
    }
    
    void Update() {
        if (!isDead) {
            // Walking animation
            if (Mathf.Abs(body.velocity.magnitude) > 0.5) {
                anim.SetBool("isWalking", true);
            } else {
                anim.SetBool("isWalking", false);
            }

            // Sprite flip
            if (body.velocity.x >= 0.5f) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            } else if (body.velocity.x <= -0.5f) {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
        }
    }

    void FixedUpdate() {
        body.velocity = Vector2.ClampMagnitude(body.velocity, maxVelocity);
    }

    public void Drown() {
        KillCharacter();
        anim.SetTrigger("Drown");
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

    public void Hit() {
        if (!isDead) {
            audioSource.PlayOneShot(hitSound);
            KillCharacter();
            GetComponent<ParticleSystem>().Play();
            CreateBloodStain();
            anim.SetTrigger("Kill");
        }
    }

    public void KillCharacter() {
        isDead = true;
        flock.enabled = false;
        runAway.enabled = false;
        charCollider.isTrigger = true;
        gameObject.layer = 9;
        audioSource.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
    }

    public bool IsDead() {
        return isDead;
    }

    private void CreateBloodStain() {
        GameObject go = new GameObject();
        go.name = "blood_stain";
        go.transform.position = transform.position;
        //go.transform.parent = FindObjectOfType<NpcSpawner>().transform;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = bloodSprites[Random.Range(0, bloodSprites.Length - 1)];
        sr.sortingLayerName = "Background";
    }
}
