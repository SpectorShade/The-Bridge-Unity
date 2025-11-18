using UnityEngine;

public class HighlightEmission : MonoBehaviour
{
    private static readonly int EmissivePropertyName = Shader.PropertyToID("_FresnelGlow");
    private Renderer[] renderers;

    // For each renderer we store an array of materials and an array of base emission colors
    private Material[][] matsPerRenderer;
    private Color[][] baseEmissionColorsPerRenderer;

    private bool isHighlighted = false;

    [Header("Highlight Settings")]
    [ColorUsage(false, true)]
    public Color highlightColor = Color.white;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>(includeInactive: true);

        int rCount = renderers.Length;
        matsPerRenderer = new Material[rCount][];
        baseEmissionColorsPerRenderer = new Color[rCount][];

        for (int i = 0; i < rCount; i++)
        {
            Renderer rend = renderers[i];
            if (!rend)
            {
                matsPerRenderer[i] = new Material[0];
                baseEmissionColorsPerRenderer[i] = new Color[0];
                continue;
            }

            Material[] originalMats = rend.materials; // returns material instances, but we'll duplicate to be safe
            int mCount = originalMats.Length;

            Material[] newMats = new Material[mCount];
            Color[] baseColors = new Color[mCount];

            for (int j = 0; j < mCount; j++)
            {
                // Duplicate each material so we don't modify shared material assets
                Material dup = new Material(originalMats[j]);
                newMats[j] = dup;

                if (dup.HasProperty(EmissivePropertyName))
                {
                    dup.EnableKeyword("_EMISSION");
                    baseColors[j] = dup.GetColor(EmissivePropertyName);
                }
                else
                {
                    baseColors[j] = Color.black;
                }
            }

            // Reassign the renderer's materials to our duplicated materials array
            rend.materials = newMats;

            matsPerRenderer[i] = newMats;
            baseEmissionColorsPerRenderer[i] = baseColors;
        }
    }

    public void SetHighlight(bool active)
    {
        if (isHighlighted == active) return;
        isHighlighted = active;

        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer rend = renderers[i];
            Material[] mats = matsPerRenderer[i];
            Color[] baseColors = baseEmissionColorsPerRenderer[i];

            for (int j = 0; j < mats.Length; j++)
            {
                Material mat = mats[j];
                if (!mat || !mat.HasProperty(EmissivePropertyName)) continue;

                Color targetColor = active ? highlightColor.linear : baseColors[j];
                mat.SetColor(EmissivePropertyName, targetColor);

                // DynamicGI.SetEmissive accepts a renderer and color; calling it per material is fine.
                DynamicGI.SetEmissive(rend, targetColor);
            }
        }

        Debug.Log($"<color={(active ? "yellow" : "grey")}>[HighlightEmission]</color> Highlight {(active ? "ON" : "OFF")} for {gameObject.name}");
    }
}
