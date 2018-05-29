using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class PerpendicularMotion : MonoBehaviour
{
    /*
    [Header("Input")]
    public float amplitude = 5f;
    public float omega = 3f;
    public float K = 16;
    public float mass = 1f;
    public float phi = 0f;

    [Header("Tweaks")]
    public float yScaleFactor = 4f;
    public float xScaleFactor = 2f;
    public float zscaleFactor = 2f;
    */

    [Header("Input Fields")]
    public InputField[] amplitudeInputFields;
    public InputField[] omegaInputFields;
    public InputField[] phiInputFields;

    [Header("Button Text")]
    public Text timescaleButtonText;

    [Header("Output Text")]
    public Text[] xTexts;
    public Text[] vTexts;
    public Text[] aTexts;

    [Header("System")]
    public GameObject system;
    public Transform cube;
    public Transform[] markers;

    private float[] amplitudes = new float[3];
    private float[] omegas = new float[3];
    private float[] phis = new float[3];

    private float[] x = new float[4];
    private float[] v = new float[4];
    private float[] a = new float[4];

    private float currentTime = 0f;

    private float[] sinus = new float[3];
    private float[] cosinus = new float[3];

    void Start()
    {
        GetInput();

        CalculateAndOutputValues();

        SetCubePos();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

        for (int i = 0; i < 3; i++)
        {
            sinus[i] = Mathf.Sin((currentTime * omegas[i] + phis[i]) * Mathf.PI);
            cosinus[i] = Mathf.Cos((currentTime * omegas[i] + phis[i]) * Mathf.PI);
        }

        CalculateAndOutputValues();
        ManageMarkers();

    }

    void SetCubePos()
    {
        cube.localPosition = new Vector3(x[0], x[1], x[2]);
    }

    public void GetInput()
    {
        for (int i = 0; i < 3; i++)
        {
            GetFloatFromInputField(ref amplitudes[i], amplitudeInputFields[i]);
            GetFloatFromInputField(ref phis[i], phiInputFields[i]);
            GetFloatFromInputField(ref omegas[i], omegaInputFields[i]);
        }

        ResetTimeAndDisplay();
    }

    void ResetTime()
    {
        currentTime = 0;
    }

    void UpdateTime()
    {
        currentTime += Time.deltaTime;
    }

    public void ToggleTimeScale()
    {
        if (Time.timeScale > 0)
        { Time.timeScale = 0f; timescaleButtonText.text = "Start"; }
        else { Time.timeScale = 1f; timescaleButtonText.text = "Stop"; }

    }

    void GetFloatFromInputField(ref float value, InputField text)
    {
        value = float.Parse(text.text);
    }

    void SetInputFieldTextFromFloat(InputField text, float value)
    {
        text.text = value.ToString("F1");
    }

    void SetTextFromFloat (Text text, float value, string description = "")
    {
        text.text = description + value.ToString("F3");
    }

    void CalculateAndOutputValues()
    {
        for (int i = 0; i < 3; i++)
        {
            x[i] = amplitudes[i] * sinus[i];
            v[i] = omegas[i] * amplitudes[i] * cosinus[i];
            a[i] = -omegas[i] * omegas[i] * x[i];
        }

        SetCubePos();

        x[3] = cube.position.magnitude;

        Vector3 speed = new Vector3(v[0], v[1], v[2]);
        v[3] = speed.magnitude;

        Vector3 acceleration = new Vector3(a[0], a[1], a[2]);
        a[3] = acceleration.magnitude;

        for (int i = 0; i < 4; i++)
        {
            SetTextFromFloat(xTexts[i], x[i], "x = ");
            SetTextFromFloat(vTexts[i], v[i], "v = ");
            SetTextFromFloat(aTexts[i], a[i], "a = ");
        }

    }

    void ResetDisplays()
    {
        ResetTime();

        SetCubePos();

    }

    public void ResetTimeAndDisplay()
    {
        ResetTime();
        ResetDisplays();

        cube.GetComponent<TrailRenderer>().Clear();
    }

    void AddIntoLastFloat(float[] values)
    {
        values[values.Length-1] = 0;

        for (int i = 0; i < values.Length-1; i++)
        {
            values[values.Length-1] += values[i];
        }
    }

    void AddIntoLastTransformPos(Transform[] values)
    {
        values[values.Length-1].position = Vector3.zero;

        for (int i = 0; i < values.Length - 1; i++)
        {
            values[values.Length-1].position += values[i].position;
        }
    }

    void ManageMarkers()
    {
        if (markers[0].parent.gameObject.activeSelf == false) return;
        else 
        {
            markers[0].localPosition = Vector3.right * x[0];
            markers[1].localPosition = Vector3.up * x[1];
            markers[2].localPosition = Vector3.forward * x[2];
        }
    }

}
