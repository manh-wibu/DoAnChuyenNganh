using Saus.CoreSystem;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Boost", menuName = "Items/Damage Boost")]
public class SO_Item_DamageBoost : SO_Item
{
    [Header("Buff Damage Info")]
    [Range(0f, 10f)]
    public float damagePercent = 0.5f;  // 50% tăng damage
    public float duration = 5f;          // tồn tại bao lâu (giây)

    public override void Use(GameObject player)
    {
        base.Use(player);

        // ✅ Lấy đúng Combat component (chỉ trên player)
        KnockBackReceiver combat = player.GetComponentInChildren<KnockBackReceiver>();
if (combat == null)
{
    Debug.LogWarning($"⚠ Player {player.name} không có Combat component trên root hoặc children!");
    return;
}

        //// ✅ Chỉ cho phép player nhận buff
        //if (!combat.isPlayer)
        //{
        //    Debug.LogWarning($"❌ {player.name} không phải player, không được buff!");
        //    return;
        //}

        // ✅ Bắt đầu coroutine từ InventoryManager
        //InventoryManager.Instance.ApplyCoroutine(ApplyDamageBoost(combat));
        //Debug.Log($"💥 {itemName}: Damage +{damagePercent * 100}% trong {duration} giây cho {combat.gameObject.name}");
    }

    //private IEnumerator ApplyDamageBoost(Combat combat)
    //{
    //    // ✅ Tăng damage
    //    combat.AddDamageMultiplier(damagePercent);
    //    Debug.Log($"🔥 Player {combat.gameObject.name} nhận buff damage +{damagePercent * 100}%!");

    //    yield return new WaitForSeconds(duration);

    //    // ✅ Hết thời gian thì giảm lại
    //    combat.RemoveDamageMultiplier(damagePercent);
    //    Debug.Log($"⏳ Buff damage hết hiệu lực cho {combat.gameObject.name}.");
    //}
}
