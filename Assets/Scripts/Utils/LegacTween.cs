using System.Collections;
using UnityEngine;

public class LegacyTween
{
    public static IEnumerator TweenToTarget(Transform target, Transform objectToMove, float duration)
    {
        float elapsedTime = 0;
        Vector3 startPosition = objectToMove.transform.position;
        // The target position, keeping the camera's z-axis at -10
        Vector3 endPosition = new Vector3(target.position.x, target.position.y, -10);

        while (elapsedTime < duration)  
        {
            // Calculate how far along the tween we are (0 to 1)
            float progress = elapsedTime / duration;
            // Apply an easing function for a smoother feel (optional but nice)
            float easedProgress = Mathf.SmoothStep(0, 1, progress);
            
           objectToMove.transform.position = Vector3.Lerp(startPosition, endPosition, easedProgress);
            
            // Wait for the next frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // After the loop, snap to the final position to guarantee it's correct
       objectToMove.transform.position = endPosition;
    }
}