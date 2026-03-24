using UnityEngine;

namespace Saus
{
    public class CastSpell : MonoBehaviour
    {
        [Header("=== CON TRỎ CHỌN PHÉP ===")]
        [SerializeField] private Transform selectionCursor;

        [Header("=== 4 LOẠI PHÉP & HIỆU ỨNG COOLDOWN RIÊNG ===")]
        [SerializeField] private GameObject waterSpellPrefab;
        [SerializeField] private Renderer waterCooldownRenderer;   // Icon/Staff/Orb của Water
        [SerializeField] private float waterCooldownTime = 3f;

        [SerializeField] private GameObject earthSpellPrefab;
        [SerializeField] private Renderer earthCooldownRenderer;
        [SerializeField] private float earthCooldownTime = 4f;

        [SerializeField] private GameObject windSpellPrefab;
        [SerializeField] private Renderer windCooldownRenderer;
        [SerializeField] private float windCooldownTime = 2.5f;

        [SerializeField] private GameObject fireSpellPrefab;
        [SerializeField] private Renderer fireCooldownRenderer;
        [SerializeField] private float fireCooldownTime = 5f;

        [Header("=== VỊ TRÍ BẮN ===")]
        [SerializeField] private Transform firePoint;

        [Header("=== CÀI ĐẶT KHÁC ===")]
        [SerializeField] private bool showDebugLogs = true;

        // Trạng thái riêng cho từng phép
        private float[] cooldownTimers = new float[4];
        private bool[] isOnCooldown = new bool[4];
        private Color[] originalColors = new Color[4];

        private void Awake()
        {
            if (firePoint == null) firePoint = transform;

            // Lưu màu gốc cho từng renderer
            SaveOriginalColor(waterCooldownRenderer, 0);
            SaveOriginalColor(earthCooldownRenderer, 1);
            SaveOriginalColor(windCooldownRenderer, 2);
            SaveOriginalColor(fireCooldownRenderer, 3);
        }

        private void SaveOriginalColor(Renderer renderer, int index)
        {
            if (renderer != null && renderer.material != null)
            {
                originalColors[index] = renderer.material.color;
            }
            else
            {
                originalColors[index] = Color.white;
            }
        }

        private void Start()
        {
            // Ban đầu: tất cả màu bình thường
            ResetAllColors();
        }

        private void Update()
        {
            // Cập nhật hiệu ứng đổi màu cho từng phép đang cooldown
            UpdateCooldown(0, waterCooldownRenderer, waterCooldownTime);
            UpdateCooldown(1, earthCooldownRenderer, earthCooldownTime);
            UpdateCooldown(2, windCooldownRenderer, windCooldownTime);
            UpdateCooldown(3, fireCooldownRenderer, fireCooldownTime);

            if (Input.GetKeyDown(KeyCode.F))
            {
                TryCastCurrentSpell();
            }
        }

        private void TryCastCurrentSpell()
        {
            if (selectionCursor == null) return;

            Transform nearestSlot = FindNearestSlot();
            if (nearestSlot == null) return;

            int index = GetSlotIndex(nearestSlot.name);
            if (index == -1) return;

            // KIỂM TRA COOLDOWN CỦA RIÊNG PHÉP ĐÓ
            if (isOnCooldown[index])
            {
                float remaining = GetCooldownTime(index) - cooldownTimers[index];
                if (showDebugLogs)
                    Debug.Log($"<color=red>{GetSpellName(index)} đang hồi chiêu! Còn {remaining:F1}s</color>");
                return;
            }

            // CHO PHÉP BẮN
            GameObject prefab = GetSpellPrefab(index);
            if (prefab != null)
            {
                Instantiate(prefab, firePoint.position, firePoint.rotation);

                // Bắt đầu cooldown + đổi màu đen ngay lập tức
                isOnCooldown[index] = true;
                cooldownTimers[index] = 0f;
                SetRendererColor(index, Color.black);

                if (showDebugLogs)
                    Debug.Log($"<color=magenta>ĐÃ BẮN: {GetSpellName(index)}!</color>");
            }
        }

        private void UpdateCooldown(int index, Renderer renderer, float duration)
        {
            if (!isOnCooldown[index]) return;

            cooldownTimers[index] += Time.deltaTime;
            float t = Mathf.Clamp01(cooldownTimers[index] / duration);

            // Đổi màu từ đen → màu gốc
            if (renderer != null)
            {
                renderer.material.color = Color.Lerp(Color.black, originalColors[index], t);
            }

            // Hồi chiêu xong
            if (t >= 1f)
            {
                isOnCooldown[index] = false;
                SetRendererColor(index, originalColors[index]);

                if (showDebugLogs)
                    Debug.Log($"<color=cyan>{GetSpellName(index)} đã hồi xong!</color>");
            }
        }

        // === HÀM HỖ TRỢ ===
        private Transform FindNearestSlot()
        {
            Transform[] slots = {
                GameObject.Find("Water_Slot")?.transform,
                GameObject.Find("Earth_Slot")?.transform,
                GameObject.Find("Wind_Slot")?.transform,
                GameObject.Find("Fire_Slot")?.transform
            };

            Transform nearest = null;
            float closest = Mathf.Infinity;

            foreach (var slot in slots)
            {
                if (slot != null)
                {
                    float dist = Vector3.Distance(selectionCursor.position, slot.position);
                    if (dist < closest)
                    {
                        closest = dist;
                        nearest = slot;
                    }
                }
            }
            return nearest;
        }

        private int GetSlotIndex(string name)
        {
            if (name.Contains("Water")) return 0;
            if (name.Contains("Earth")) return 1;
            if (name.Contains("Wind")) return 2;
            if (name.Contains("Fire")) return 3;
            return -1;
        }

        private GameObject GetSpellPrefab(int i) => i switch
        {
            0 => waterSpellPrefab,
            1 => earthSpellPrefab,
            2 => windSpellPrefab,
            3 => fireSpellPrefab,
            _ => null
        };

        private float GetCooldownTime(int i) => i switch
        {
            0 => waterCooldownTime,
            1 => earthCooldownTime,
            2 => windCooldownTime,
            3 => fireCooldownTime,
            _ => 3f
        };

        private string GetSpellName(int i) => i switch
        {
            0 => "Nước",
            1 => "Đất",
            2 => "Gió",
            3 => "Lửa",
            _ => "Phép"
        };

        private void SetRendererColor(int index, Color color)
        {
            Renderer r = index switch
            {
                0 => waterCooldownRenderer,
                1 => earthCooldownRenderer,
                2 => windCooldownRenderer,
                3 => fireCooldownRenderer,
                _ => null
            };
            if (r != null) r.material.color = color;
        }

        private void ResetAllColors()
        {
            for (int i = 0; i < 4; i++)
            {
                SetRendererColor(i, originalColors[i]);
            }
        }
    }
}