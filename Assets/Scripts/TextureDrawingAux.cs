using UnityEngine;
using System.IO;

/// <summary>
/// Class for creating and drawing on a texture
/// </summary>
public class TextureDrawingAux
{
    internal string fileName;
    /// <summary>
    /// Width of the texture
    /// </summary>
    public int Width
    {
        get; private set;
    }

    /// <summary>
    /// Height of the texture
    /// </summary>
    public int Height
    {
        get; private set;
    }

    Texture2D texture;
    /// <summary>
    /// Reference to the texture we draw on
    /// </summary>
    public Texture2D Texture
    {
        get
        {
            return texture;
        }
    }

    /// <summary>
    /// Scale of the texture.
    /// So you can have a texture e.g. 2x bigger than the container size
    /// </summary>
    public int Scale
    {
        get; private set;
    }
    public bool skipAlphaPixels;

    //we cache all the pixels in the texture for easy read-write
    //note: using Color32 (and SetPixel32) is much faster than Color (or SetPixel)
    Color32[] pixels;

    /// <summary>
    /// Create a new texture to draw on
    /// </summary>
    /// <param name="width">width of the texture container</param>
    /// <param name="height">height of the texture container</param>
    public TextureDrawingAux(int width, int height, int scale = 1)
    {
        this.Width = width * scale;
        this.Height = height * scale;
        this.Scale = scale;

        texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
        pixels = texture.GetPixels32();
        Clear();
    }
    /// <summary>
    /// Copy texture
    /// </summary>
    /// <param name="_texture"></param>
    public TextureDrawingAux(Texture2D _texture, bool _skipAlphaPixels)
    {
        skipAlphaPixels = _skipAlphaPixels;
        this.Width = _texture.width;
        this.Height = _texture.height;
        this.Scale = 1;

        texture =  _texture;
        pixels = texture.GetPixels32();
    }

    /// <summary>
    /// Set all the pixels in the texture to white.
    /// </summary>
    public void Clear()
    {
        Clear(Color.white);
    }

    /// <summary>
    /// Sets all the pixels of the texture to the given color
    /// </summary>
    /// <param name="clearColor">the color to initialize the texture with</param>
    public void Clear(Color clearColor)
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                SetPixel(i, j, clearColor, true);
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Draw a filled circle
    /// </summary>
    /// <param name="x">X coordinate on the texture of the circle's center, in pixels</param>
    /// <param name="y">Y coordinate on the texture of the circle's center, in pixels</param>
    /// <param name="diameter">the diameter of the circle, in pixels</param>
    /// <param name="color">the color to draw the circle in</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    public void DrawCircleFill(int x, int y, int diameter, Color32 color, bool eraserMode)
    {
        DrawCircleFillInternal(x * Scale, y * Scale, diameter * Scale, color, eraserMode);
        texture.SetPixels32(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Draw the outline of a circle
    /// </summary>
    /// <param name="x">X coordinate on the texture of the circle's center, in pixels</param>
    /// <param name="y">Y coordinate on the texture of the circle's center, in pixels</param>
    /// <param name="diameter">the diameter of the circle, in pixels</param>
    /// <param name="color">the color to draw the circle in</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    public void DrawCircle(int x, int y, int diameter, Color32 color, bool eraserMode)
    {
        for (float angle = 0; angle <= 2 * Mathf.PI; angle += Mathf.PI / (diameter * 4 * Scale))
        {
            SetPixel(x * Scale + Mathf.RoundToInt(diameter * Scale * Mathf.Cos(angle)), y * Scale + Mathf.RoundToInt(diameter * Scale * Mathf.Sin(angle)), color, eraserMode);
        }
        texture.SetPixels32(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Draw a filled rectangle
    /// </summary>
    /// <param name="x">lower left X coordinate of the rectangle, in pixels</param>
    /// <param name="y">lower left Y coordinate of the rectangle, in pixels</param>
    /// <param name="width">width of the rectangle, in pixels</param>
    /// <param name="height">height of the rectangle, in pixels</param>
    /// <param name="color">the color to draw the rectangle in</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    public void DrawRectangle(int x, int y, int width, int height, Color32 color, bool eraserMode)
    {
        for (int i = x * Scale; i < x * Scale + width * Scale; i++)
        {
            for (int j = y * Scale; j < y * Scale + height * Scale; j++)
            {
                SetPixel(i, j, color, eraserMode);
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Draw a filled rectangle at an angle
    /// </summary>
    /// <param name="x">lower left X coordinate of the rectangle, in pixels</param>
    /// <param name="y">lower left Y coordinate of the rectangle, in pixels</param>
    /// <param name="width">width of the rectangle, in pixels</param>
    /// <param name="height">height of the rectangle, in pixels</param>
    /// <param name="angle">the angle at which to rotate the rectangle, in radians</param>
    /// <param name="color">the color to draw the rectangle in</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    public void DrawRectangle(int x, int y, int width, int height, float angle, Color32 color, bool eraserMode)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        int origX = x * Scale;
        int origY = y * Scale;

        for (int i = -width * Scale / 2; i < width * Scale / 2; i++)
        {
            for (int j = -height * Scale / 2; j < height * Scale / 2; j++)
            {
                float rotX = i * cos - j * sin;
                float rotY = i * sin + j * cos;
                int drawX = Mathf.FloorToInt(origX + rotX);
                int drawY = Mathf.FloorToInt(origY + rotY);


                SetPixel(drawX, drawY, color, eraserMode);
                SetPixel(drawX + 1, drawY, color, eraserMode);
                SetPixel(drawX, drawY + 1, color, eraserMode);
            }
        }

        texture.SetPixels32(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Trace a line between the 2 points, "pencil style".
    /// 
    /// This basically draws a series of filled circles between the given points
    /// </summary>
    /// <param name="from">Texture coordinates to draw from, in pixels</param>
    /// <param name="to">Texture coordinates to draw to, in pixels</param>
    /// <param name="radius">The diameter of the pencil, in pixels</param>
    /// <param name="color">The color to trace the line in</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    public void DrawPencil(Vector2 from, Vector2 to, int radius, Color32 color, bool eraserMode)
    {

        //cache the pixels forming the circle in an array, so we don't have to compute it every time
        Color32[] circlePixels = GetCirclePixels(radius * Scale, color);

        DrawCircleFillInternal((int)from.x * Scale, (int)from.y * Scale, radius * Scale, circlePixels, color, eraserMode);
        DrawCircleFillInternal((int)to.x * Scale, (int)to.y * Scale, radius * Scale, circlePixels, color, eraserMode);


        int centerX = Mathf.RoundToInt((from.x + to.x) / 2f);
        int centerY = Mathf.RoundToInt((from.y + to.y) / 2f);
        Vector2 dir = from - to;
        if (to.y < from.y)
        {
            dir = to - from;
        }
        else
        {
            dir = from - to;
        }
        int width = Mathf.RoundToInt(dir.magnitude);
        float angle = Vector2.Angle(dir, Vector2.left) * Mathf.Deg2Rad + Mathf.PI / 2;

        Vector2[] shape =
        {
            new Vector2 (from.x* Scale+radius* Scale*Mathf.Cos(angle),from.y* Scale+radius* Scale*Mathf.Sin(angle)),
            new Vector2 (from.x* Scale-radius* Scale*Mathf.Cos(angle),from.y* Scale-radius* Scale*Mathf.Sin(angle)),
            new Vector2 (to.x* Scale-radius* Scale*Mathf.Cos(angle),to.y* Scale-radius* Scale*Mathf.Sin(angle)),
            new Vector2 (to.x* Scale+radius* Scale*Mathf.Cos(angle),to.y* Scale+radius* Scale*Mathf.Sin(angle)),
        };
        DrawPolygonInternal(shape, color, eraserMode);

        //set the pixels in the texture and apply
        texture.SetPixels32(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Save the file with the given file name in Application.persistentDataPath
    /// 
    /// Note: the file will be automatically overwritten if it exists
    /// </summary>
    /// <param name="file">The file name to save at. Note: Application.persistentDataPath will be appended</param>
    public void SaveToFile(string file)
    {
        fileName = file;
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/" + file, bytes);
    }

    public void SaveToFile()
    {
        if (string.IsNullOrEmpty(fileName))
            Debug.LogError("fileName is null or empty");
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, bytes);
    }
    /// <summary>
    /// Draw a different texture on the drawing canvas
    /// </summary>
    /// <param name="x">where to start drawing the lower left corner of the new texture</param>
    /// <param name="y">where to start drawing the lower left corner of the new texture</param>
    /// <param name="texture">the texture to draw at x,y</param>
    public void DrawTexture(int x, int y, Texture2D drawTexture)
    {
        int scaledX = x * Scale;
        int scaledY = y * Scale;
        for (int i = 0; i < drawTexture.width; i++)
        {
            if (i + scaledX >= Width)
                continue;
            for (int j = 0; j < drawTexture.height; j++)
            {
                if (j + scaledY >= Height)
                    continue;
                Color c = drawTexture.GetPixel(i, j);
                for (int sx = 0; sx < Scale; sx++)
                {
                    if (i + scaledX + sx >= Width)
                        continue;
                    for (int sy = 0; sy < Scale; sy++)
                    {
                        if (j + scaledY + sy >= Height)
                            continue;
                        int textureIndex = (j + scaledY + sy) * Width + i + scaledX + sx;
                        pixels[textureIndex] = Color32.Lerp(pixels[textureIndex], c, c.a);
                    }
                }

            }
        }

        //set the pixels in the texture and apply
        texture.SetPixels32(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Draw a polygon.
    /// </summary>
    /// <param name="points">array of points for the polygon, in pixels</param>
    /// <param name="color">the color in circlePixels that we need to draw. Everything else gets ignored</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    public void DrawPolygon(Vector2[] points, Color32 color, bool eraserMode)
    {
        Vector2[] scaledPoints = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            scaledPoints[i] = new Vector2(points[i].x * Scale, points[i].y * Scale);
        }
        DrawPolygonInternal(scaledPoints, color, eraserMode);
        texture.SetPixels32(pixels);
        texture.Apply();
    }

    #region Private methods
    /// <summary>
    /// Draw a filled circle. Only updates the pixels array, not the texture.
    /// </summary>
    /// <param name="x">X coordinate on the texture of the circle's center, in pixels</param>
    /// <param name="y">Y coordinate on the texture of the circle's center, in pixels</param>
    /// <param name="radius">the diameter of the circle, in pixels</param>
    /// <param name="color">the color to draw the circle in</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    private void DrawCircleFillInternal(int x, int y, int radius, Color color, bool eraserMode)
    {
        Color32[] circlePixels = GetCirclePixels(radius, color);
        DrawCircleFillInternal(x, y, radius, circlePixels, color, eraserMode);
    }

    /// <summary>
    /// Draw a filled circle. Only updates the pixels array, not the texture.
    /// </summary>
    /// <param name="x">X coordinate on the texture of the circle's center, in pixels</param>
    /// <param name="y">Y coordinate on the texture of the circle's center, in pixels</param>
    /// <param name="radius">the diameter of the circle, in pixels</param>
    /// <param name="circlePixels">a cache of the pixel array forming a circle in the given diameter, from <see cref="GetCirclePixels"/>
    /// <param name="color">the color in circlePixels that we need to draw. Everything else gets ignored</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    private void DrawCircleFillInternal(int x, int y, int radius, Color32[] circlePixels, Color32 color, bool eraserMode)
    {
        for (int i = -radius; i <= radius; i++)
        {
            if (x + i < 0 || x + i >= Width)
                continue;
            for (int j = -radius; j <= radius; j++)
            {
                if (y + j < 0 || y + j >= Height)
                    continue;
                int circleIndex = (j + radius) * (2 * radius + 1) + i + radius;
                if (!color.Equals(circlePixels[circleIndex]))
                    continue;
                SetPixel(i + x, j + y, color, eraserMode);
            }
        }
    }

    /// <summary>
    /// Draw a polygon. Only updates the pixels array, not the texture.
    /// </summary>
    /// <param name="points">array of points for the polygon, in pixels</param>
    /// <param name="color">the color in circlePixels that we need to draw. Everything else gets ignored</param>
    /// <param name="eraserMode">If the color should alawys be overwritten instead of merged</param>
    private void DrawPolygonInternal(Vector2[] points, Color32 color, bool eraserMode)
    {
        float minx = points[0].x;
        float miny = points[0].y;
        float maxx = points[0].x;
        float maxy = points[0].y;

        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].x < minx)
            {
                minx = points[i].x;
            }
            if (points[i].y < miny)
            {
                miny = points[i].y;
            }
            if (points[i].x > maxx)
            {
                maxx = points[i].x;
            }
            if (points[i].y > maxy)
            {
                maxy = points[i].y;
            }
        }

        for (int x = Mathf.FloorToInt(minx); x <= maxx; x++)
        {
            for (int y = Mathf.FloorToInt(miny); y <= maxy; y++)
            {
                bool draw = false;
                for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
                {
                    if ((points[i].y > y) != (points[j].y > y) &&
                        x < (points[j].x - points[i].x) * (y - points[i].y) / (points[j].y - points[i].y) + points[i].x)
                    {
                        draw = !draw;
                    }
                }
                if (draw)
                {
                    SetPixel(x, y, color, eraserMode);
                }
            }
        }


    }

    /// <summary>
    /// Get an array containing the pixels for a filled circle with the given parameters
    /// </summary>
    /// <param name="diameter">the diameter of the circle</param>
    /// <param name="color">the color of the circle</param>
    /// <returns></returns>
    private Color32[] GetCirclePixels(int diameter, Color32 color)
    {
        int size = (diameter * 2 + 1) * (diameter * 2 + 1);
        Color32[] circlePixels = new Color32[size];
        byte alpha = (byte)(color.a == 0 ? 1 : 0);
        for (int i = 0; i < size; i++)
        {
            circlePixels[i] = new Color32(0, 0, 0, alpha);
        }
        for (int i = -diameter; i <= diameter; i++)
        {
            int d = Mathf.CeilToInt(Mathf.Sqrt(diameter * diameter - i * i));
            for (int j = -d; j <= d; j++)
            {
                circlePixels[(j + diameter) * (2 * diameter + 1) + i + diameter] = color;
            }
        }
        return circlePixels;
    }

    /// <summary>
    /// Set pixel in pixels array to given color
    /// </summary>
    /// <param name="x">x coordinate of the pixel</param>
    /// <param name="y">y coordinate of the pixel</param>
    /// <param name="color">the color to set for the pixel</param>
    private void SetPixel(int x, int y, Color32 color, bool eraserMode)
    {
        if (x < 0 || x >= Width)
            return;
        if (y < 0 || y >= Height)
            return;
        int textureIndex = y * Width + x;
        Color32 c;
        if (eraserMode)
        {
            c = color;
        }
        else
        {
            Color32 origColor = pixels[textureIndex];
            Color32 newColor = color;
            c = new Color32();
            c.r = (byte)Mathf.Lerp(origColor.r, newColor.r, newColor.a / 255f);
            c.g = (byte)Mathf.Lerp(origColor.g, newColor.g, newColor.a / 255f);
            c.b = (byte)Mathf.Lerp(origColor.b, newColor.b, newColor.a / 255f);
            c.a = (byte)Mathf.Max(origColor.a, newColor.a);
        }

        if(skipAlphaPixels)
        {
            c.a = pixels[textureIndex].a;
        }
            pixels[textureIndex] = c;
    }
    #endregion
}
