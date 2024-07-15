using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDirectionalEmitter : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float particleSpeed = 5.0f;

    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        EmitParticles();
    }

    void EmitParticles()
    {
        Vector3 direction = (endPoint - startPoint).normalized;
        float distance = Vector3.Distance(startPoint, endPoint);
        float lifetime = distance / particleSpeed;

        // 确保粒子系统不会自动发射粒子
        var mainModule = particleSystem.main;
        mainModule.startSpeed = particleSpeed;
        mainModule.startLifetime = lifetime;
        mainModule.startSize = 1.0f; // 根据需要调整大小
        mainModule.maxParticles = 1; // 可以根据需要设置更多的粒子

        // 创建一个粒子
        particles = new ParticleSystem.Particle[1];
        particles[0].position = startPoint;
        particles[0].velocity = direction * particleSpeed;
        particles[0].startLifetime = lifetime;
        particles[0].remainingLifetime = lifetime; // 确保粒子不会立即消失
        particles[0].startSize = 1.0f; // 根据需要调整大小

        // 发射粒子
        particleSystem.SetParticles(particles, 1);
    }
}