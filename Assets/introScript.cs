using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introScript : MonoBehaviour
{
    [SerializeField]
    GameObject spirit;

    [SerializeField]
    GameObject shadeIllustration;

    // Start is called before the first frame update
    void Start()
    {
        shadeIllustration.SetActive(false);
        spirit.SetActive(false);
    }

    public void SpiritAppear()
    {
        StartCoroutine(LightFlicker());
    }

    IEnumerator LightFlicker()
    {
        yield return new WaitForSeconds(1);
        shadeIllustration.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        shadeIllustration.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        shadeIllustration.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        shadeIllustration.SetActive(false);

        yield return new WaitForSeconds(0.4f);
        shadeIllustration.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        shadeIllustration.SetActive(false);

        yield return new WaitForSeconds(0.8f);
        shadeIllustration.SetActive(true);
        spirit.SetActive(true);
    }
}
