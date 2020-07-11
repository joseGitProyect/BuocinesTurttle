using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] Transform turtleTransform;
    [SerializeField] Movement movement;
    [SerializeField] ParticleSystem particles;
    [SerializeField] TMP_InputField IFrotateSpeedH;
    [SerializeField] TMP_InputField IFrotateSpeedV;
    [SerializeField] TMP_InputField IFwaterSpeed;
    [SerializeField] TMP_InputField IFwaterTime;
    [SerializeField] TMP_InputField IFspeed;
    [SerializeField] TMP_InputField IFCameraDistance;
    [SerializeField] float playerLife;
    [SerializeField] float minTurnAngle, maxTurnAngle;
    [SerializeField] GameObject damagePanel;
    [SerializeField] TMP_Text life;
    public float rotateSpeedH = 5;
    public float rotateSpeedV = 5;
    public float waterSpeed = 10;
    public float waterTime = 3;
    public float speed;
    public Vector3 cameraOffSet;
    Vector3 offset;
    GameObject player;
    Transform cameraTransform;
    bool isOnWaterShoot;
    bool canAttack = true;
    float startTimeTimer;
    float actualSpeed;
    float lastSpeed;
    float rotX;

    bool isDamaged;
   
    void Start()
    {
        cameraTransform = Camera.main.transform;
        player = movement.gameObject;
        offset = cameraOffSet;
        Cursor.lockState = CursorLockMode.Locked;
        actualSpeed = speed;
        SetInputText();
    }

    private void Update()
    {        
        if(isOnWaterShoot)
        {
            if(Timer(startTimeTimer,waterTime,ref isOnWaterShoot))
            {
                UpdateSpeed(lastSpeed);
                particles.Stop();
                canAttack = true;
                WaterShoot(false);
            }
            else
            {
                UpdateSpeed(waterSpeed);
            }
        }
        else if (canAttack && !isOnWaterShoot && Input.GetMouseButtonDown(0))
        {
            WaterShoot(true);
        }

        if (isDamaged)
        {
            if(Timer(startTimeTimer,1,ref isDamaged))
            {
                damagePanel.SetActive(false);
            }
        }
        
        movement.ControlMyVelocity(actualSpeed);

        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
    }

    void LateUpdate()
    {
        MouseAiming();
    }

    void WaterShoot(bool isStart)
    {
        if (isStart)
        {
            LeanTween.value(0, 180, .8f).setOnUpdate(WaterShootAnim).setOnComplete(WaterShootAction).setEaseOutElastic();
            canAttack = false;
        }
        else LeanTween.value(180, 0, .8f).setOnUpdate(WaterShootAnim).setEaseOutElastic();
    }

    void WaterShootAnim(float _val)
    {
        Vector3 newRot = turtleTransform.localRotation.eulerAngles;
        newRot.y = _val;
        turtleTransform.localRotation = Quaternion.Euler(newRot);
    }

    void WaterShootAction()
    {
        movement.Dash(cameraTransform.forward, waterSpeed);
        startTimeTimer = Time.time;
        particles.Play();
        isOnWaterShoot = true;
    }

    void MouseAiming()
    {
        float y = Input.GetAxis("Mouse X") * rotateSpeedH;
        rotX += Input.GetAxis("Mouse Y") * rotateSpeedV;

        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

        // rotate the camera
        cameraTransform.eulerAngles = new Vector3(-rotX, cameraTransform.eulerAngles.y + y, 0);
        player.transform.eulerAngles = new Vector3(-rotX, player.transform.eulerAngles.y + y, 0);

        // move the camera position
        cameraTransform.position = player.transform.position - (cameraTransform.rotation * offset);
    }

    public bool Timer(float _startTime, float duration, ref bool isTimerActive)
    {
        float time = Time.time - _startTime;
        if(time >= duration)
        {
            isTimerActive = false;
            return true;
        }
        return false;
    }

    public void ChangeVal(string _string)
    {
        print(_string);
        switch (_string)
        {
            case "RHorizontal":
                rotateSpeedH = float.Parse(IFrotateSpeedH.text);
                break;
            case "RVertical":
                rotateSpeedV = float.Parse(IFrotateSpeedV.text);
                break;
            case "Speed":
                speed = float.Parse(IFspeed.text);
                break;
            case "WaterSpeed":
                waterSpeed = float.Parse(IFwaterSpeed.text);
                break;
            case "WaterTime":
                waterTime = float.Parse(IFwaterTime.text);
                break;
            case "CameraDis":
                offset.z = float.Parse(IFCameraDistance.text);
                break;
        }
    }

    public void SetInputText()
    {
        IFrotateSpeedH.text = rotateSpeedH.ToString();
        IFrotateSpeedV.text = rotateSpeedV.ToString();
        IFspeed.text = speed.ToString();
        IFwaterSpeed.text = waterSpeed.ToString();
        IFwaterTime.text = waterTime.ToString();
        IFCameraDistance.text = (cameraOffSet.z).ToString();
    }

    public void TakeDamage(float _damage)
    {
        //feedback
        float newLife = playerLife - _damage;
        if(newLife <= 0)
        {
            playerLife = 0;
            PlayerDead();
        }
        else
        {
            playerLife = newLife;
            //cambiar canvas
        }

        life.text = "Life = " + playerLife;

        isDamaged = true;
        damagePanel.SetActive(true);
        startTimeTimer = Time.time;
    }

    public void PlayerDead()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        InteractableObject interactable = collision.gameObject.GetComponent<InteractableObject>();
        print(collision.gameObject.name);
        if(interactable != null)
        {
            UpdateSpeed(interactable.bouncinesPower);
            interactable.DoEffect(this);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    InteractableEnemy enemy = other.gameObject.GetComponent<InteractableEnemy>();
    //    if(enemy!= null)
    //    {
    //        enemy.Hunt(player.transform);
    //    }
    //}

    public void UpdateSpeed(float _newSpeed)
    {
        float newSpeed = _newSpeed;
        lastSpeed = actualSpeed;
        actualSpeed = newSpeed;
    }
}
