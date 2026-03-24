namespace Saus.Weapons.Components
{
	public class ManaCostData : ComponentData<AttackMana>
	{
		protected override void SetComponentDependency()
		{
			ComponentDependency = typeof(KnockBackOnParry);
		}
	}
}