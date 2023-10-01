using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public SettingVars inputActions;

    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int bulletsPerTap;
<<<<<<< Updated upstream
    public int bulletsLeft, bulletsShot;
=======
    public int bulletsLeft;
>>>>>>> Stashed changes
    public bool isRecoil;
    
    // Bools 
    bool readyToShoot, reloading;

    // Reference
    public Camera fpsCam;
    public Transform shootingPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    Rigidbody playerRB;
    public PlayerScript playerRef;

    [SerializeField] float recoilForce;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeStrength;
    [SerializeField] float impactForce;
    public bool isShooting = false;

    private float lastShotTime;

    public enum InputMethod
    {
        None,
        LeftClick,
        RightClick
    }

    [SerializeField] InputMethod inputMethod;
     void Awake()
    {
        readyToShoot = true;
<<<<<<< Updated upstream

        inputActions = GameObject.Find("Settings").GetComponent<SettingVars>();
        fpsCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
=======
>>>>>>> Stashed changes

        if (inputMethod == InputMethod.LeftClick)
        {
            inputActions.input.Gameplay.LeftHandPressed.performed += ctx => Shoot(InputMethod.LeftClick);
            inputActions.input.Gameplay.LeftHandReleased.performed += ctx => isShooting = false;
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        }
        else if (inputMethod == InputMethod.RightClick)
        {
            inputActions.input.Gameplay.RightHandPressed.performed += ctx => Shoot(InputMethod.RightClick);
            inputActions.input.Gameplay.RightHandReleased.performed += ctx => isShooting = false;

        }
    }

<<<<<<< Updated upstream
    protected virtual void Start()
=======

    void Start()
>>>>>>> Stashed changes
    {
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
       
    }
    private void Shoot(InputMethod inputMethod)
    {
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {

            // Calculate the time since the last shot
            float timeSinceLastShot = Time.time - lastShotTime;

            if (timeSinceLastShot >= timeBetweenShooting)
            {

                bulletsShot = bulletsPerTap;
                readyToShoot = false;
                Debug.Log(inputMethod);

                // Update the last shot time
                lastShotTime = Time.time;

                if (inputMethod == InputMethod.LeftClick ||
           (inputMethod == InputMethod.RightClick))
                {
                    float x = Random.Range(-spread, spread);
                    float y = Random.Range(-spread, spread);
                    Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

                    if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
                    {
                        Debug.Log(rayHit.collider.name);

                        /* if (rayHit.collider.CompareTag("Ground") || rayHit.collider.CompareTag("Wall"))
                         {
                             Quaternion impactRotation = Quaternion.LookRotation(rayHit.normal);
                             // GameObject impact = Instantiate(bulletHoleGraphic, rayHit.point, impactRotation);
                             // impact.transform.parent = rayHit.transform;
                         }*/

                        if (rayHit.rigidbody != null)
                        {
                            rayHit.rigidbody.AddForce(-rayHit.normal * impactForce);
                        }
                    }
                    bulletsLeft--;
                    bulletsShot--;

                    Invoke("ResetShot", timeBetweenShooting);
                    if (isRecoil)
                    {
                        if (!playerRef.onGround)
                        {
                            // Calculate the current downward velocity due to gravity
                            float currentGravityEffect = Vector3.Dot(playerRB.velocity, Vector3.up);

                            // Neutralize the gravity effect for the recoil duration (we subtract it from the recoilForce)
                            Vector3 effectiveRecoilForce = -direction.normalized * (recoilForce - currentGravityEffect);

                            playerRB.velocity += effectiveRecoilForce;
                        }
                        else
                        {
                            // When grounded, just apply the recoil as usual
                            playerRB.velocity += -direction.normalized * recoilForce;
                        }

                        Invoke("ResetShot", reloadTime);
                    }
<<<<<<< Updated upstream

                    if (bulletsShot > 0 && bulletsLeft > 0)
                        Invoke("Shoot", timeBetweenShots);
                }
            }
               
        }      
        
=======
                    if (bulletsLeft > 0 && !reloading)
                    {
                        Reload();
                    }
                }
            }

        }

    }
    void RecoilStateHandler()
    {
        Vector3 direction = fpsCam.transform.forward;

        // Regular recoil
        Vector3 regularRecoil = -direction.normalized * recoilForce;

        // Enhanced recoil when moving or hopping in air
        float recoilMultiplier = IsPlayerMoving() ? 1.5f : 1.0f;
        Vector3 enhancedRecoil = -direction.normalized * recoilForce * recoilMultiplier;
        bool hitSomething = Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range);

        // Determine if the shot hits a wall or the ground
       
            if (rayHit.collider != null && hitSomething)
            {
                if (rayHit.collider.CompareTag("Ground"))
                {
                    Debug.Log("[Tag 101] Shot the ground.");
                    playerRef.currentState = PlayerScript.PlayerState.OnAir;

                }
                else if (rayHit.collider.CompareTag("Wall"))
                {
                    Debug.Log("[Tag 102] Shot a wall.");
                }
                // Check Player's State
                switch (playerRef.currentState)
                {
                    case PlayerScript.PlayerState.OnGround:
                        if (rayHit.collider.CompareTag("Wall"))
                        {
                            Debug.Log("[Tag 001] Applying enhanced recoil on ground after shooting a wall.");
                            playerRB.AddForce(enhancedRecoil, ForceMode.Impulse);
                            playerRB.AddForce(enhancedRecoil, ForceMode.VelocityChange);

                        }
                        else
                        {
                            Debug.Log("[Tag 002] Applying regular recoil on ground after shooting non-wall.");
                            playerRB.AddForce(regularRecoil, ForceMode.Impulse);
                            playerRB.AddForce(enhancedRecoil, ForceMode.VelocityChange);

                        }
                        break;

                    case PlayerScript.PlayerState.Hopping:
                        if (!playerRef.onGround) // Hopping and not on ground
                        {
                            if (rayHit.collider.CompareTag("Wall"))
                            {
                                Debug.Log("[Tag 003] Applying enhanced recoil while hopping and shooting a wall.");
                                playerRB.AddForce(enhancedRecoil, ForceMode.VelocityChange);

                                Debug.Log("[Tag 004] Applying regular recoil while hopping and shooting non-wall.");
                                playerRB.AddForce(regularRecoil * 0.25f, ForceMode.Impulse);
                            }
                            else
                            {
                                Debug.Log("[Tag 004] Applying regular recoil while hopping and shooting non-wall.");
                                float currentGravityEffect = Vector3.Dot(playerRB.velocity, Vector3.up);
                                Vector3 effectiveRecoilForce = -direction.normalized * (recoilForce - currentGravityEffect);
                                playerRB.AddForce(effectiveRecoilForce, ForceMode.Impulse);
                                playerRB.AddForce(enhancedRecoil * 0.15f, ForceMode.VelocityChange);
                            }
                        }
                        else
                        {
                            Debug.Log("[Tag 005] Applying regular recoil while hopping but on ground.");
                            playerRB.AddForce(regularRecoil, ForceMode.Impulse);
                            playerRB.AddForce(enhancedRecoil, ForceMode.VelocityChange);

                        }
                        break;

                    case PlayerScript.PlayerState.OnAir:
                        if (rayHit.collider.CompareTag("Wall"))
                        {
                            Debug.Log("[Tag 006] Applying enhanced recoil on air after shooting a wall.");
                            playerRB.AddForce(enhancedRecoil, ForceMode.Impulse);
                            playerRB.AddForce(enhancedRecoil, ForceMode.VelocityChange);

                        }
                        else
                        {
                            Debug.Log("[Tag 007] Applying regular recoil on air after shooting non-wall.");
                            float currentGravityEffect = Vector3.Dot(playerRB.velocity, Vector3.up);
                            Vector3 effectiveRecoilForce = -direction.normalized * (recoilForce - currentGravityEffect);
                            playerRB.AddForce(effectiveRecoilForce, ForceMode.Impulse);
                            playerRB.AddForce(enhancedRecoil * 0.1f, ForceMode.VelocityChange);

                        }
                        break;

                    default:
                        Debug.Log("[Tag 008] Applying regular recoil in default case.");
                        playerRB.AddForce(regularRecoil, ForceMode.Impulse);
                        break;
                }
            }
            else
            {
                // Here, you can handle the recoil behavior when you shoot the sky or other non-collider objects.
                // For instance, just apply the regular recoil.
                playerRB.AddForce(regularRecoil, ForceMode.Impulse);
               // playerRB.AddForce(enhancedRecoil * 0.15f, ForceMode.VelocityChange);

            }
        
        playerRef.DisableMovementForces();

       
        StartCoroutine(EnableMovementAfterDelay(0.3f));

>>>>>>> Stashed changes
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    public void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        reloading = false;
    }
<<<<<<< Updated upstream
}
=======
    public void Reload()
    {
        if (!reloading)  // Prevent multiple reload triggers
        {
            reloading = true;
            Invoke("ResetShot", reloadTime);
        }
    }
    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);

    }
}
>>>>>>> Stashed changes
