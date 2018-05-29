using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ParallelMotion : MonoBehaviour
{
    /*
    [Header("Input")]
    public float amplitude = 5f;
    public float omega = 3f;
    public float K = 16;
    public float mass = 1f;
    public float phi = 0f;
    */

    [Header("Tweaks")]
    public float yScaleFactor = 4f;
    public float xScaleFactor = 2f;

    [Header("Input Fields")]
    public InputField[] amplitudeInputFields;
    public InputField[] omegaInputFields;
    public InputField[] phiInputFields;

    [Header("Dropdowns")]
    public Dropdown graphDropdown;
    public Dropdown valuesDropdown;

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

    [Header("Phasor Graph")]
    public GameObject phasorGraph;
    public Transform[] phasorParents;
    public Transform[] phasorTracers;

    [Header("Graphable Values")]
    public GameObject graphableValuesGraph;
    public Transform[] tracers;

    public enum VisibleObjects
    {
        System,
        PhasorGraph,
        GraphableGraph
    };

    [Header("Visible Objects")]
    public VisibleObjects displayOption;

    private float[] amplitudes = new float[3];
    private float[] omegas = new float[2];
    private float[] phis = new float[2];

    private float[] x = new float[3];
    private float[] v = new float[3];
    private float[] a = new float[3];

    private float currentTime = 0f;

    private float[] sinus = new float[2];
    private float[] cosinus = new float[2];

    private float maxV;
    private float maxA;

    private int oscillationsNumber;


    void Start()
    {
        oscillationsNumber = amplitudes.Length;

        GetInput();

        CalculateAndOutputValues();

        SetCubePos();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();

        for (int i = 0; i < oscillationsNumber-1; i++)
        {
            sinus[i] = Mathf.Sin((currentTime * omegas[i] + phis[i]) * Mathf.PI);
            cosinus[i] = Mathf.Cos((currentTime * omegas[i] + phis[i]) * Mathf.PI);
        }
    

        CalculateAndOutputValues();

        switch (displayOption)
        {
            case VisibleObjects.System:
                SetCubePos();
                ManageMarkers();
                break;

            case VisibleObjects.PhasorGraph:
                ManagePhasorGraph();
                break;

            case VisibleObjects.GraphableGraph:
                ManageGraphableValues();
                break;
        }

    }

    void SetCubePos()
    {
        cube.position = new Vector3(x[oscillationsNumber-1], 0f, 0f);
    }

    public void GetInput()
    {
        maxV = 0;
        maxA = 0;

        for (int i = 0; i < oscillationsNumber-1; i++)
        {
            GetFloatFromInputField(ref amplitudes[i], amplitudeInputFields[i]);
            GetFloatFromInputField(ref phis[i], phiInputFields[i]);
            GetFloatFromInputField(ref omegas[i], omegaInputFields[i]);

            maxV += amplitudes[i] * omegas[i];
            maxA += amplitudes[i] * omegas[i] * omegas[i];
        }

        AddIntoLastFloat(amplitudes);

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
        for (int i = 0; i < oscillationsNumber-1; i++)
        {
            x[i] = amplitudes[i] * sinus[i];
            v[i] = omegas[i] * amplitudes[i] * cosinus[i];
            a[i] = -omegas[i] * omegas[i] * x[i];
        }

        AddIntoLastFloat(x);
        AddIntoLastFloat(v);
        AddIntoLastFloat(a);

        for (int i = 0; i < oscillationsNumber; i++)
        {
            SetTextFromFloat(xTexts[i], x[i], "x = ");
            SetTextFromFloat(vTexts[i], v[i], "v = ");
            SetTextFromFloat(aTexts[i], a[i], "a = ");
        }

    }

    void ManagePhasorGraph()
    {
        for (int i = 0; i < oscillationsNumber-1; i++)
        {
            phasorTracers[i].localPosition = new Vector3(amplitudes[i], 0f, 0f);
            phasorParents[i].Rotate(new Vector3(0f, 0f, omegas[i] * Mathf.PI * Mathf.Rad2Deg * Time.deltaTime));
        }

        AddIntoLastTransformPos(phasorTracers);

    }

    void ResetDisplays()
    {
        ResetTime();

        switch (displayOption)
        {
            case VisibleObjects.System:
                SetCubePos();
                break;

            case VisibleObjects.PhasorGraph:
                for (int i = 0; i < oscillationsNumber-1; i++)
                {
                    phasorParents[i].rotation = Quaternion.identity;
                    phasorParents[i].Rotate(new Vector3(0f, 0f, Mathf.PI * phis[i] * Mathf.Rad2Deg));
                }
                AddIntoLastTransformPos(phasorTracers);
                break;

            case VisibleObjects.GraphableGraph:
                foreach(Transform tracer in tracers)
                    tracer.GetComponent<TrailRenderer>().Clear();
                break;
        }

    }

    public void DropdownSelectGraph()
    {
        switch(graphDropdown.value)
        {
            case 0:
                displayOption = VisibleObjects.System;

                system.SetActive(true);
                phasorGraph.SetActive(false);
                graphableValuesGraph.SetActive(false);

                break;

            case 1:
                displayOption = VisibleObjects.PhasorGraph;

                system.SetActive(false);
                phasorGraph.SetActive(true);
                graphableValuesGraph.SetActive(false);

                break;   

            case 2:
                displayOption = VisibleObjects.GraphableGraph;

                system.SetActive(false);
                phasorGraph.SetActive(false);
                graphableValuesGraph.SetActive(true);

                break;
        }

        valuesDropdown.interactable = displayOption.Equals(VisibleObjects.GraphableGraph);

        ResetDisplays();
    }

    void ManageGraphableValues()
    {

        float yParam = currentTime;

        for (int i = 0; i < oscillationsNumber; i++)
        {
            SetTracerPos(tracers[i], yParam, SetParamFromDropdown(i));
        }



    }

    void SetTracerPos(Transform tracer, float xParam, float yParam)
    {
        tracer.position = new Vector3(xScaleFactor * xParam, yScaleFactor * yParam, 0f);
    }

    float SetParamFromDropdown(int i)
    {
        switch(valuesDropdown.value)
        {
            case 0:
                return x[i]/amplitudes[oscillationsNumber - 1];

            case 1:
                return v[i]/maxV ;

            case 2:
                return a[i]/maxA;

            default:
                return 0;
        }

    }

    public void ResetTimeAndDisplay()
    {
        ResetTime();
        ResetDisplays();
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
        for (int i = 0; i < oscillationsNumber-1; i++)
        {
            markers[i].position = new Vector3(x[i], -2*i-2, 0f);
        }
    }
}
