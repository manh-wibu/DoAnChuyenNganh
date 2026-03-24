using UnityEngine;

namespace Saus
{
    public class Cast : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform leftSlot;        
        [SerializeField] private Transform leftCenterSlot;  
        [SerializeField] private Transform rightCenterSlot; 
        [SerializeField] private Transform rightSlot;       

        [SerializeField] private float offsetX = -0.01f;    

        [Header("=== DI CHUYỂN & HIỆU ỨNG ===")]
        [SerializeField] private float smoothTime = 0.1f;

        private Transform[] slots;           
        private int currentIndex = 0;        
        private Vector3 velocity = Vector3.zero;

        private void Awake()
        {
            if (target == null) target = transform;

            
            slots = new Transform[4] { leftSlot, leftCenterSlot, rightCenterSlot, rightSlot };

            
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    Debug.LogError($"[Cast] Chưa gán Slot thứ {i + 1}/4! (0=Water, 1=Earth, 2=Wind, 3=Fire)");
            }
        }

        private void Start()
        {
            currentIndex = 0; 
            SnapToCurrentSlot();
        }

        private void Update()
        {
            float scroll = Input.mouseScrollDelta.y;

            if (scroll > 0.1f) 
            {
                currentIndex++;
                if (currentIndex >= slots.Length) currentIndex = slots.Length - 1; 
                SnapToCurrentSlot();
            }
            else if (scroll < -0.1f) 
            {
                currentIndex--;
                if (currentIndex < 0) currentIndex = 0; 
                SnapToCurrentSlot();
            }
            KeepTargetAtCurrentSlot();
        }

        private void SnapToCurrentSlot()
        {
            if (slots[currentIndex] == null) return;

            Vector3 targetPos = slots[currentIndex].position;
            targetPos.x += offsetX;
            targetPos.y = target.position.y;
            targetPos.z = target.position.z;

            if (smoothTime > 0f)
            {
                target.position = Vector3.SmoothDamp(target.position, targetPos, ref velocity, smoothTime);
            }
            else
            {
                target.position = targetPos;
            }
        }

        private void KeepTargetAtCurrentSlot()
        {
            if (slots[currentIndex] == null) return;

            Vector3 fixedPos = slots[currentIndex].position;
            fixedPos.x += offsetX;
            target.position = new Vector3(fixedPos.x, target.position.y, target.position.z);
        }
    }
}