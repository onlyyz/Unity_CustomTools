using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GamepadRumbleWithCurve : MonoBehaviour
{
    // 震动持续时间
    public float duration = 1.0f;

    // 震动强度的曲线
    public AnimationCurve intensityCurve = new AnimationCurve(
        new Keyframe(0f, 0f),        // 开始时强度为 0
        new Keyframe(0.5f, 1f),      // 中间时强度为 1
        new Keyframe(1f, 0f));       // 结束时强度为 0

    // 调用这个方法来开始震动
    public void Awake()
    {
        StartCoroutine(RumbleCoroutine());
    }

    private IEnumerator RumbleCoroutine()
    {
        float elapsed = 0f;

        // 获取当前连接的所有游戏手柄
        var gamepads = Gamepad.all;
        if (gamepads.Count == 0)
        {
            Debug.LogWarning("No gamepads connected.");
            yield break;
        }

        while (elapsed < duration)
        {
            // 根据曲线和已经过时间计算强度
            float currentIntensity = intensityCurve.Evaluate(elapsed / duration);

            foreach (var gamepad in gamepads)
            {
                // 设置震动强度
                gamepad.SetMotorSpeeds(currentIntensity, currentIntensity);
            }

            // 等待下一帧
            yield return null;
            elapsed += Time.deltaTime;
        }

        // 震动结束，停止所有手柄的震动
        foreach (var gamepad in gamepads)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}