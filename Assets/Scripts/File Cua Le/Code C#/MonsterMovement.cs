/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    // Start is called before the first frame update

	[Header("Điểm Di Chuyển")]
	[Tooltip("Các điểm mà kẻ địch sẽ di chuyển giữa chúng")]
	public Transform[] waypoints;

	[Header("Cài Đặt Di Chuyển")]
	[Tooltip("Tốc độ di chuyển của kẻ địch")]
	public float moveSpeed = 2.0f;

	[Tooltip("Khoảng thời gian kẻ địch đứng yên tại mỗi điểm")]
	public float waitTime = 1.0f;

	[Tooltip("Bật/tắt chế độ tuần tra")]
	public bool isPatrolling = true;

	[Header("Phát Hiện Người Chơi")]
	[Tooltip("Transform của người chơi")]
	public Transform player;

	[Tooltip("Khoảng cách phát hiện người chơi")]
	public float detectionRadius = 5.0f;

	[Tooltip("Khoảng cách tấn công người chơi")]
	public float attackRange = 1.5f;

	[Tooltip("Tốc độ khi đuổi theo người chơi")]
	public float chaseSpeed = 3.0f;

	[Header("Tấn Công")]
	[Tooltip("Sát thương mỗi đòn tấn công")]
	public int attackDamage = 10;

	[Tooltip("Thời gian hồi chiêu tấn công")]
	public float attackCooldown = 1.0f;

	[Tooltip("Layer của người chơi để phát hiện")]
	public LayerMask playerLayer;

	// Biến private
	private int currentWaypointIndex = 0;
	private bool isWaiting = false;
	private bool isChasing = false;
	private bool isAttacking = false;
	private bool canAttack = true;
	private Vector2 originalPosition;
	private SpriteRenderer spriteRenderer;
	private Animator animator; // Thêm Animator nếu có animation

	// Enum trạng thái kẻ địch
	private enum EnemyState { Patrolling, Chasing, Attacking, Returning }
	private EnemyState currentState = EnemyState.Patrolling;

	void Start()
	{
		// Kiểm tra nếu không có waypoints nào được thiết lập
		if (waypoints.Length == 0)
		{
			Debug.LogWarning("Không có điểm di chuyển nào được thiết lập cho kẻ địch!");
			isPatrolling = false;
		}

		// Lưu vị trí ban đầu
		originalPosition = transform.position;

		// Lấy component SpriteRenderer nếu có
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Lấy component Animator nếu có
		animator = GetComponent<Animator>();

		// Tìm player nếu chưa được gán
		if (player == null)
		{
			GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
			if (playerObject != null)
				player = playerObject.transform;
		}
	}

	void Update()
	{
		// Kiểm tra trạng thái và thực hiện hành động tương ứng
		switch (currentState)
		{
			case EnemyState.Patrolling:
				Patrol();
				DetectPlayer();
				break;

			case EnemyState.Chasing:
				ChasePlayer();
				CheckAttackRange();
				break;

			case EnemyState.Attacking:
				AttackPlayer();
				break;

			case EnemyState.Returning:
				ReturnToPatrol();
				break;
		}
	}

	// Chức năng tuần tra
	void Patrol()
	{
		if (!isPatrolling || waypoints.Length == 0 || isWaiting)
			return;

		// Tính toán hướng và khoảng cách đến điểm tiếp theo
		Vector2 targetPosition = waypoints[currentWaypointIndex].position;
		Vector2 currentPosition = transform.position;
		Vector2 direction = (targetPosition - currentPosition).normalized;
		float distance = Vector2.Distance(currentPosition, targetPosition);

		// Lật sprite theo hướng di chuyển
		UpdateFacingDirection(direction);

		// Di chuyển kẻ địch
		transform.Translate(direction * moveSpeed * Time.deltaTime);

		// Cập nhật animation (nếu có)
		if (animator != null)
			animator.SetBool("isWalking", true);

		// Kiểm tra xem kẻ địch đã đến điểm tiếp theo chưa
		if (distance < 0.1f)
		{
			// Đã đến điểm, đợi một khoảng thời gian
			StartCoroutine(WaitAtWaypoint());

			// Chuyển sang điểm tiếp theo
			currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
		}
	}

	// Phát hiện người chơi
	void DetectPlayer()
	{
		if (player == null)
			return;

		float distanceToPlayer = Vector2.Distance(transform.position, player.position);

		// Nếu người chơi trong tầm phát hiện
		if (distanceToPlayer <= detectionRadius)
		{
			// Kiểm tra tầm nhìn (Line of Sight)
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized,
												detectionRadius, playerLayer);

			if (hit.collider != null && hit.collider.CompareTag("Player"))
			{
				// Chuyển sang trạng thái đuổi theo
				currentState = EnemyState.Chasing;
				isChasing = true;

				// Cập nhật animation (nếu có)
				if (animator != null)
				{
					animator.SetBool("isWalking", true);
					animator.SetBool("isChasing", true);
				}

				Debug.Log("Kẻ địch phát hiện người chơi!");
			}
		}
	}

	// Đuổi theo người chơi
	void ChasePlayer()
	{
		if (player == null)
		{
			currentState = EnemyState.Returning;
			return;
		}

		float distanceToPlayer = Vector2.Distance(transform.position, player.position);

		// Nếu người chơi ra khỏi tầm phát hiện
		if (distanceToPlayer > detectionRadius * 1.5f)
		{
			// Trở về tuần tra
			currentState = EnemyState.Returning;
			isChasing = false;

			// Cập nhật animation (nếu có)
			if (animator != null)
			{
				animator.SetBool("isChasing", false);
			}

			Debug.Log("Kẻ địch mất dấu người chơi!");
			return;
		}

		// Tính toán hướng và di chuyển đến người chơi
		Vector2 direction = (player.position - transform.position).normalized;
		transform.Translate(direction * chaseSpeed * Time.deltaTime);

		// Lật sprite theo hướng di chuyển
		UpdateFacingDirection(direction);
	}

	// Kiểm tra có thể tấn công không
	void CheckAttackRange()
	{
		if (player == null)
			return;

		float distanceToPlayer = Vector2.Distance(transform.position, player.position);

		// Nếu trong tầm tấn công
		if (distanceToPlayer <= attackRange && canAttack)
		{
			currentState = EnemyState.Attacking;
			isAttacking = true;

			// Cập nhật animation (nếu có)
			if (animator != null)
			{
				animator.SetBool("isWalking", false);
				animator.SetBool("isAttacking", true);
			}
		}
	}

	// Tấn công người chơi
	void AttackPlayer()
	{
		if (!isAttacking || player == null)
			return;

		if (canAttack)
		{
			// Thực hiện tấn công
			StartCoroutine(PerformAttack());

			// Hướng mặt về phía người chơi khi tấn công
			Vector2 direction = (player.position - transform.position).normalized;
			UpdateFacingDirection(direction);
		}

		// Kiểm tra khoảng cách sau khi tấn công
		float distanceToPlayer = Vector2.Distance(transform.position, player.position);

		if (distanceToPlayer > attackRange)
		{
			// Quay lại đuổi theo nếu người chơi ra khỏi tầm tấn công
			currentState = EnemyState.Chasing;
			isAttacking = false;

			// Cập nhật animation (nếu có)
			if (animator != null)
			{
				animator.SetBool("isAttacking", false);
				animator.SetBool("isWalking", true);
			}
		}
	}

	// Trở về tuần tra
	void ReturnToPatrol()
	{
		if (waypoints.Length == 0)
		{
			// Nếu không có waypoints, trở về vị trí ban đầu
			Vector2 direction = (originalPosition - (Vector2)transform.position).normalized;
			transform.Translate(direction * moveSpeed * Time.deltaTime);

			// Lật sprite theo hướng di chuyển
			UpdateFacingDirection(direction);

			float distanceToOriginal = Vector2.Distance(transform.position, originalPosition);

			if (distanceToOriginal < 0.1f)
			{
				currentState = EnemyState.Patrolling;

				// Cập nhật animation (nếu có)
				if (animator != null)
				{
					animator.SetBool("isWalking", false);
				}
			}
		}
		else
		{
			// Tìm waypoint gần nhất
			float minDistance = float.MaxValue;
			int nearestWaypointIndex = 0;

			for (int i = 0; i < waypoints.Length; i++)
			{
				float distance = Vector2.Distance(transform.position, waypoints[i].position);
				if (distance < minDistance)
				{
					minDistance = distance;
					nearestWaypointIndex = i;
				}
			}

			// Đi đến waypoint gần nhất
			Vector2 direction = (waypoints[nearestWaypointIndex].position - transform.position).normalized;
			transform.Translate(direction * moveSpeed * Time.deltaTime);

			// Lật sprite theo hướng di chuyển
			UpdateFacingDirection(direction);

			// Nếu đã đến waypoint gần nhất
			if (minDistance < 0.1f)
			{
				currentWaypointIndex = nearestWaypointIndex;
				currentState = EnemyState.Patrolling;
			}
		}
	}

	// Cập nhật hướng nhìn
	void UpdateFacingDirection(Vector2 direction)
	{
		if (spriteRenderer != null && Mathf.Abs(direction.x) > 0.1f)
		{
			spriteRenderer.flipX = direction.x < 0;
		}
	}

	// Thực hiện tấn công
	IEnumerator PerformAttack()
	{
		canAttack = false;

		// Gây sát thương cho người chơi nếu trong tầm
		if (player != null)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, player.position);

			if (distanceToPlayer <= attackRange)
			{
				// Tìm component Health của người chơi và gây sát thương
				PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
				if (playerHealth != null)
				{
					playerHealth.TakeDamage(attackDamage);
					Debug.Log("Kẻ địch gây " + attackDamage + " sát thương cho người chơi!");
				}
				else
				{
					Debug.Log("Kẻ địch tấn công người chơi!");
				}
			}
		}

		// Đợi hết thời gian hồi chiêu
		yield return new WaitForSeconds(attackCooldown);

		canAttack = true;

		// Kiểm tra lại trạng thái sau khi tấn công
		if (player != null)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, player.position);

			if (distanceToPlayer <= attackRange)
			{
				// Vẫn trong tầm tấn công, tiếp tục tấn công
				currentState = EnemyState.Attacking;
			}
			else if (distanceToPlayer <= detectionRadius)
			{
				// Ngoài tầm tấn công nhưng vẫn trong tầm phát hiện, đuổi theo
				currentState = EnemyState.Chasing;
				isAttacking = false;

				// Cập nhật animation (nếu có)
				if (animator != null)
				{
					animator.SetBool("isAttacking", false);
					animator.SetBool("isWalking", true);
				}
			}
			else
			{
				// Ngoài tầm phát hiện, trở về tuần tra
				currentState = EnemyState.Returning;
				isAttacking = false;
				isChasing = false;

				// Cập nhật animation (nếu có)
				if (animator != null)
				{
					animator.SetBool("isAttacking", false);
					animator.SetBool("isChasing", false);
				}
			}
		}
		else
		{
			// Không tìm thấy người chơi, trở về tuần tra
			currentState = EnemyState.Returning;
			isAttacking = false;
			isChasing = false;

			// Cập nhật animation (nếu có)
			if (animator != null)
			{
				animator.SetBool("isAttacking", false);
				animator.SetBool("isChasing", false);
			}
		}
	}

	// Đợi tại waypoint
	IEnumerator WaitAtWaypoint()
	{
		isWaiting = true;

		// Cập nhật animation (nếu có)
		if (animator != null)
			animator.SetBool("isWalking", false);

		yield return new WaitForSeconds(waitTime);
		isWaiting = false;
	}

	// Hàm để thiết lập tốc độ mới trong runtime
	public void SetSpeed(float newSpeed)
	{
		moveSpeed = newSpeed;
	}

	// Hàm để thiết lập tốc độ đuổi theo mới trong runtime
	public void SetChaseSpeed(float newSpeed)
	{
		chaseSpeed = newSpeed;
	}

	// Hàm để thiết lập sát thương mới trong runtime
	public void SetAttackDamage(int newDamage)
	{
		attackDamage = newDamage;
	}

	// Hàm để thêm điểm di chuyển mới
	public void AddWaypoint(Transform newWaypoint)
	{
		// Tạo mảng mới với kích thước tăng thêm 1
		Transform[] newWaypoints = new Transform[waypoints.Length + 1];

		// Sao chép các điểm cũ
		for (int i = 0; i < waypoints.Length; i++)
		{
			newWaypoints[i] = waypoints[i];
		}

		// Thêm điểm mới
		newWaypoints[waypoints.Length] = newWaypoint;

		// Gán lại mảng
		waypoints = newWaypoints;
	}

	// Hiển thị phạm vi phát hiện và tấn công trong Scene view
	void OnDrawGizmosSelected()
	{
		// Vẽ đường đi giữa các waypoints
		if (waypoints != null && waypoints.Length >= 2)
		{
			// Màu của đường đi
			Gizmos.color = Color.yellow;

			// Vẽ đường đi giữa các điểm
			for (int i = 0; i < waypoints.Length - 1; i++)
			{
				if (waypoints[i] != null && waypoints[i + 1] != null)
					Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
			}

			// Vẽ đường từ điểm cuối về điểm đầu để hoàn thành vòng lặp
			if (waypoints[0] != null && waypoints[waypoints.Length - 1] != null)
				Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);

			// Vẽ hình cầu tại các điểm
			Gizmos.color = Color.red;
			foreach (Transform wp in waypoints)
			{
				if (wp != null)
					Gizmos.DrawSphere(wp.position, 0.2f);
			}
		}

		// Vẽ phạm vi phát hiện người chơi
		Gizmos.color = new Color(1, 0, 0, 0.3f); // Đỏ nhạt
		Gizmos.DrawWireSphere(transform.position, detectionRadius);

		// Vẽ phạm vi tấn công
		Gizmos.color = new Color(1, 0, 0, 0.5f); // Đỏ đậm hơn
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}

// Script cho PlayerHealth, kẻ địch sẽ gọi đến để gây sát thương
// Thêm script này vào GameObject của người chơi


*/