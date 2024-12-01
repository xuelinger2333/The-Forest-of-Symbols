using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public ParticleSystem particleEffect_down; // 从编辑器拖拽粒子系统到这个变量
    public ParticleSystem particleEffect_up;
    GameObject down;
    void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 将屏幕点击位置转换为世界坐标
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10; // 设置z轴的深度
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // 在点击位置播放粒子特效
            down = PlayParticleSystemAtPosition(worldPos, particleEffect_down);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (down != null)
            {
                Destroy(down);
            }
            // 将屏幕点击位置转换为世界坐标
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10; // 设置z轴的深度
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            GameObject up = PlayParticleSystemAtPosition(worldPos, particleEffect_up);
            Destroy(up, up.GetComponent<ParticleSystem>().main.duration);
        }
    }

    GameObject PlayParticleSystemAtPosition(Vector3 position, ParticleSystem particle)
    {
        // 克隆粒子系统预制体并设置其位置
        ParticleSystem newParticleSystem = Instantiate(particle, position, particle.transform.rotation);

        // 播放粒子系统
        newParticleSystem.Play();
        return newParticleSystem.gameObject;
        // 可选：播放完成后销毁粒子系统实例
        //Destroy(newParticleSystem.gameObject, newParticleSystem.main.duration);
    }


}
