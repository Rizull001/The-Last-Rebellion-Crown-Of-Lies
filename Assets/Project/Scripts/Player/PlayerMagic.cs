using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    [Header("Mana Settings")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float currentMana = 100f;
    [SerializeField] private float manaRegen = 5f;

    [Header("Fireball Settings")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballCost = 20f;
    [SerializeField] private float fireballCooldown = 1.5f;
    [SerializeField] private float fireballSpawnDistance = 0.7f;

    [Header("Arcane Infusion")]
    [SerializeField] private float infusionCost = 30f;
    [SerializeField] private float infusionDuration = 5f;

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerCombat playerCombat;

    private float nextFireballTime;
    private float infusionEndTime;

    public float CurrentMana => currentMana;
    public float MaxMana => maxMana;

    public bool IsInfused => Time.time < infusionEndTime;

    private void Update()
    {
        RegenerateMana();

        if (Input.GetKeyDown(KeyCode.F))
        {
            CastFireball();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ActivateInfusion();
        }
    }

    private void RegenerateMana()
    {
        currentMana = Mathf.Min(
            currentMana + manaRegen * Time.deltaTime,
            maxMana
        );
    }

    private bool UseMana(float amount)
    {
        if (currentMana < amount)
        {
            Debug.Log("Not enough mana!");
            return false;
        }

        currentMana -= amount;
        return true;
    }

    private void CastFireball()
    {
        if (Time.time < nextFireballTime)
            return;

        if (fireballPrefab == null ||
            playerController == null)
            return;

        if (!UseMana(fireballCost))
            return;

        nextFireballTime = Time.time + fireballCooldown;

        Vector2 direction =
            playerController.FacingDirection;

        Vector3 spawnPosition =
            transform.position +
            (Vector3)(direction * fireballSpawnDistance);

        GameObject projectile = Instantiate(
            fireballPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Fireball fireball =
            projectile.GetComponent<Fireball>();

        if (fireball != null)
        {
            fireball.Initialize(direction);
        }
    }

    private void ActivateInfusion()
    {
        if (IsInfused)
            return;

        if (playerCombat == null)
            return;

        if (!UseMana(infusionCost))
            return;

        infusionEndTime =
            Time.time + infusionDuration;

        playerCombat.SetArcaneInfusion(true);

        Debug.Log("Arcane Infusion activated!");
    }

    private void LateUpdate()
    {
        if (!IsInfused && playerCombat != null)
        {
            playerCombat.SetArcaneInfusion(false);
        }
    }
}