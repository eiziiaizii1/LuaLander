using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;
    private State state;
    public static Lander Instance {get; private set;}
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnUpForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public float speed;
        public float angle;
        public int multiplier;
        public int point;
    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastSpeed
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }

    private Rigidbody2D landerRigidbody2D;

    private float maxFuel = 10f;
    private float fuelAmount;


    private void Awake()
    {
        Instance = this;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        fuelAmount = maxFuel;

        landerRigidbody2D.gravityScale = 0f;
        state = State.WaitingToStart;
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Keyboard.current.upArrowKey.isPressed ||
                    Keyboard.current.leftArrowKey.isPressed ||
                    Keyboard.current.rightArrowKey.isPressed)
                {                   
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }

                break;

            case State.Normal:
                if (Keyboard.current.upArrowKey.isPressed ||
                    Keyboard.current.leftArrowKey.isPressed ||
                    Keyboard.current.rightArrowKey.isPressed)
                {
                    ConsumeFuel();
                }

                if (fuelAmount <=0)
                {
                    return; 
                }

                
                if (Keyboard.current.upArrowKey.isPressed)
                {
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                    float force = 700f; 
                    landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                }
                if (Keyboard.current.leftArrowKey.isPressed)
                {
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                    float turnSpeed = +100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                }
                if (Keyboard.current.rightArrowKey.isPressed)
                {
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                    float turnSpeed = -100f;
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision2d)
    {
        if (!collision2d.gameObject.TryGetComponent<LandingPad>(out LandingPad landingPad))
        {
            Debug.Log("Crashed on terrain!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                speed = 0,
                angle = 0,
                multiplier = 0,
                point = 0,
            });
            SetState(State.GameOver);
                return;
        }

        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2d.relativeVelocity.magnitude;
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            // Landed hard
            Debug.Log("Landed hard");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastSpeed,
                speed = relativeVelocityMagnitude,
                angle = 0,
                multiplier = 0,
                point = 0,
            });
            SetState(State.GameOver); 
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDot =.9f;

        if (dotVector < minDot)
        {
            // too steep angle
            Debug.Log("too steep angle");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                speed = relativeVelocityMagnitude,
                angle = dotVector,
                multiplier = 0,
                point = 0,
            });
            SetState(State.GameOver);
            return;
        }

        Debug.Log("succesful landing");

        float maxLandingAngleScore = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxLandingAngleScore - Mathf.Abs(dotVector - 1) * scoreDotVectorMultiplier * maxLandingAngleScore;

        float maxLandingSpeedScore = 100f;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxLandingSpeedScore;

        Debug.Log("landingAngleScore: " + landingAngleScore);
        Debug.Log("landingSpeedScore: " + landingSpeedScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.getScoreMultiplier());
        Debug.Log("score: " + score);
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            speed = relativeVelocityMagnitude,
            angle = dotVector,
            multiplier = landingPad.getScoreMultiplier(),
            point = score,
        });
        
        SetState(State.GameOver);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<FuelPickup>(out FuelPickup fuelPickup))
        {
            float fuelPickupAmount = 10f;
            fuelAmount += fuelPickupAmount;
            if (fuelAmount >= maxFuel)
            {
                fuelAmount = maxFuel;
            }
            fuelPickup.DestroySelf();
        }
        if (collision.TryGetComponent<CoinPickup>(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

        private void ConsumeFuel()
    {
        float consumptionAmount= 1f;
        fuelAmount -= consumptionAmount * Time.deltaTime;
    }

    public void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }
    public float GetVelocityX()
    {
        return landerRigidbody2D.linearVelocityX;
    }

    public float GetVelocityY()
    {
        return landerRigidbody2D.linearVelocityY;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetFuelNormalized()
    {
        return fuelAmount / maxFuel;
    }
}
