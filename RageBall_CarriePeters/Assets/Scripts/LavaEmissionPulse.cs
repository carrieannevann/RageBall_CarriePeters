using UnityEngine;

public class LavaEmissionPulse : MonoBehaviour
{
    public Renderer targetRenderer;      // drag your cube's MeshRenderer here
    public Color emissionColor = Color.yellow;
    public float baseIntensity = 2f;     // minimum brightness
    public float pulseIntensity = 2f;    // how much extra to add
    public float pulseSpeed = 2f;        // how fast it pulses

    Material _mat;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // make an instance of the material so we don't edit it for every object
        _mat = targetRenderer.material;
        _mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // 0..1->0 sin curve
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float intensity = baseIntensity + t * pulseIntensity;

        // URP/Lit still uses _EmissionColor for the emission color
        _mat.SetColor("_EmissionColor", emissionColor * intensity);
    }
}