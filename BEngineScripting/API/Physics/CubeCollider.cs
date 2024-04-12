﻿
namespace BEngine
{
	public class CubeCollider : Collider
	{
		public Vector3 Size = Vector3.one;
		private Vector3 _lastSize = Vector3.zero;

		public override object[] GetAdditionalData()
		{
			return [Size];
		}

		public override void OnRescale()
		{
			_lastSize = Size;
		}

		public override bool RequiresRescale()
		{
			return Size != _lastSize;
		}

		public override void Setup()
		{
			transform = GetScript<Transform>();
			if (transform != null)
			{
				physicsID = InternalCalls.PhysicsCreateCube(transform.Position, transform.Rotation, Size);
				Prepared = true;
			}
		}
	}
}
