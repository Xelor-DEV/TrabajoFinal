using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AdjustCurtainPosition : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        AjustarCortina();
    }

    void AjustarCortina()
    {
        if (canvas == null) return;

        // 1. Obtener la resolución actual de la pantalla
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 2. Obtener el factor de escala del canvas
        float scaleFactor = canvas.scaleFactor;

        // 3. Convertir a unidades de canvas
        float canvasWidth = screenWidth / scaleFactor;
        float canvasHeight = screenHeight / scaleFactor;

        // 4. Ajustar el tamaño de la imagen
        rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight);

        // 5. Calcular la posición Y: altura del canvas + (mitad de la altura * ajuste de pivote)
        // El pivote está en (0.5, 0) - centro horizontal, borde inferior
        float posY = canvasHeight * 0.5f; // Como el pivote está en el borde inferior

        // 6. Posicionar en la parte superior del canvas
        rectTransform.anchoredPosition = new Vector2(
            rectTransform.anchoredPosition.x, // Mantener posición X actual
            posY
        );
    }
}