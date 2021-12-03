using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*Author: Ly Duong Huy
 * Class: DamagePopup
 * Summary:
 * Make damage pops up.
 * Bimzy Dev, 2021, How to make DAMAGE POPUPS in 5 Minutes! - Unity [online], https://www.youtube.com/watch?v=I2j6mQpCrWE&t=56s [accessed 12/03/2021]
 */

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageDisplay;
    [SerializeField] float lifetime = 0.6f;
    [SerializeField] float minDist = 2f;
    [SerializeField] float maxDist = 3f;

    private Vector3 initialPos;
    private Vector3 targetPos;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        //Making the popup look at the camera
        transform.LookAt(2 * transform.position - Camera.main.transform.position);

        //Store the initial position
        initialPos = this.transform.position;

        //Generate randomized direction and distance for the popup
        float direction = Random.rotation.eulerAngles.z;
        float distance = Random.Range(minDist, maxDist);

        //Calculation for the target position of the popup
        targetPos = initialPos + (Quaternion.Euler(0, 0, direction) * new Vector3(distance, distance, 0f));

        //Reset the scale to zero
        transform.localScale = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        Timer();

    }

    private void Timer()
    {
        //increase time based on delta time
        timer += Time.deltaTime;

        //half of the time
        float fraction = lifetime / 2f;

        //if the time limit is reached then destroy the popup
        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
        //if there is half of the time left for the damage popup
        else if (timer > fraction)
        {
            //Lerp the color to transparent
            damageDisplay.color = Color.Lerp(damageDisplay.color, Color.clear, (timer - fraction) / (lifetime - fraction));
        }

        //Transform the location and scale of the popup using lerp
        transform.localPosition = Vector3.Lerp(initialPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
    }

    //Display value
    public void SetDamageText(int damage)
    {
        damageDisplay.text = damage.ToString();
    }
}
