using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    GameObject activeHealthBar = null;
    void Awake()
    {
        activeHealthBar = transform.Find("health bar filled").gameObject;
    }

    public void UpdateHealthBar(float healthFill)
    {
        activeHealthBar.GetComponent<Image>().fillAmount = healthFill;
    }
}
