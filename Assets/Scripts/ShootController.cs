using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{

    public Player player;
    public Transform aimPip;

    public Element[] elements;
    public Style[] styles;
    public Mutation mutation;

    //Almost but this is still shit, look up constructing prefabs/with prefabs
    //Maybe have the prefab'd spell contain the non-mono spell as a field?
    public Spell selectedSpell;

    int elementIdx;
    public Element selectedElement;
    int styleIdx;
    public Style selectedStyle;
    public Mutation selectedMutation;

    bool allowfire = true;

    Vector3 pipDirection;
    Vector3 smoothPipDir;
    float distance = 1;

    //Sling trajectory
    int resolution = 10;

    //Components
    public SpriteRenderer sr;
    public LineRenderer lr;

    void Start() {
        selectedElement = elements[0];
        selectedStyle = styles[0];

        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //Axes seem wrong
        Vector3 aimDirection = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0).normalized;
        pipDirection = Vector3.SmoothDamp(pipDirection, aimDirection, ref smoothPipDir, 0.1f);

        aimPip.position = player.transform.position + (pipDirection * distance);
        Color color = sr.color;
        color.a = pipDirection.magnitude;
        sr.color = color;

        if (aimDirection.magnitude > 0 && Input.GetAxis("Trigger Fire") != 0 && allowfire) {
            StartCoroutine(Shoot());
        }

        if (aimDirection.magnitude > 0 && selectedStyle.GetType() == StyleType.SLING) {
            lr.enabled = true;
            //Draw trajectory
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.white;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.white;
            colorKey[1].time = 1.0f;
            GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 0.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;
            gradient.SetKeys(colorKey, alphaKey);
            lr.colorGradient = gradient;
            RenderArc(aimDirection);
        } else if (lr.enabled) {
            lr.enabled = false;
        }

        if (Input.GetButtonDown("Fire1")) {
            styleIdx = styleIdx + 1 == styles.Length ? 0 : styleIdx + 1;
            selectedStyle = styles[styleIdx];
            Debug.Log(selectedElement + " " + selectedStyle);
        }

        if (Input.GetButtonDown("Fire2")) {
            elementIdx = elementIdx + 1 == elements.Length ? 0 : elementIdx + 1;
            selectedElement = elements[elementIdx];
            Debug.Log(selectedElement + " " + selectedStyle);
        }
    }

    IEnumerator Shoot() {
        allowfire = false;
        Spell spell = Instantiate(selectedSpell, player.transform.position, Quaternion.identity);
        spell.Fire(selectedElement, selectedStyle, mutation, pipDirection);
        yield return new WaitForSeconds(spell.Element.GetFireRate() * spell.Style.GetFireRateModifier());
        allowfire = true;
    }

    private void RenderArc(Vector3 aimDirection) {
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray(aimDirection));
    }

    private Vector3[] CalculateArcArray(Vector3 aimDirection)
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        float radianAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.right, aimDirection);
        float velocity = selectedElement.GetSpeed();
        float gravity = -Sling.Gravity;

        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance, radianAngle, gravity, velocity);
        }

        return arcArray;
    }

    private Vector3 CalculateArcPoint(float t, float maxDistance, float radianAngle, float gravity, float velocity)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }
}
