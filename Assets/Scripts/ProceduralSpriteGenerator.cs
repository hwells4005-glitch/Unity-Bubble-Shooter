using UnityEngine;

public static class ProceduralSpriteGenerator
{
    public static Sprite CreateCircleSprite(int resolution = 128)
    {
        Texture2D tex = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        float center = resolution / 2f;
        float radius = (resolution / 2f) - 2f;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                if (dist <= radius)
                {
                    float highlight = Mathf.Clamp01(1f - (Vector2.Distance(new Vector2(x, y), new Vector2(center * 0.7f, center * 1.3f)) / (radius * 0.8f)));
                    float alpha = Mathf.Clamp01((radius - dist) * 1.5f);
                    Color baseCol = Color.Lerp(new Color(0.85f, 0.85f, 0.85f, alpha), new Color(1f, 1f, 1f, alpha), highlight * 0.7f);
                    tex.SetPixel(x, y, baseCol);
                }
                else
                {
                    tex.SetPixel(x, y, Color.clear);
                }
            }
        }
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f), resolution);
    }

    public static Sprite CreateSquareSprite()
    {
        Texture2D tex = new Texture2D(4, 4);
        Color[] cols = new Color[16];
        for (int i = 0; i < 16; i++) cols[i] = Color.white;
        tex.SetPixels(cols);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4);
    }
}
