﻿
using System.Text.Json.Serialization;

namespace BEngine
{
	public struct PhysicsEntryData
	{
		public Vector3 Position;
		public Quaternion Rotation;
	}

	public class Rigidbody : Script
	{
		public bool Kinematic
		{
			get
			{
				return internal_kinematic;
			}
			set
			{
				if (_collider != null)
					InternalCalls.PhysicsChangeKinematic(_collider.PhysicsID, value);
				internal_kinematic = value;
			}
		}

		[EditorIgnore]
		public Vector3 Velocity
		{
			get
			{
				if (_collider != null)
					return InternalCalls.PhysicsGetVelocity(_collider.PhysicsID);
				else
					return Vector3.zero;
			}
			set
			{
				if (_collider != null)
					InternalCalls.PhysicsSetVelocity(_collider.PhysicsID, value);
			}
		}

		[EditorIgnore]
		public Vector3 AngularVelocity
		{
			get
			{
				if (_collider != null)
					return InternalCalls.PhysicsGetAngularVelocity(_collider.PhysicsID);
				else
					return Vector3.zero;
			}
			set
			{
				InternalCalls.PhysicsSetAngularVelocity(_collider.PhysicsID, value);
			}
		}

		private Collider _collider;
		// temp solution just to serialize properly
		[EditorIgnore] public bool internal_kinematic;

		public override void OnStart()
		{
			Setup();
		}

		public override void OnFixedUpdate()
		{
			if (_collider == null)
			{
				Setup();
				return;
			}
		}

		private void Setup()
		{
			_collider = GetScript<Collider>();
			if (_collider == null || _collider.Prepared == false)
			{
				_collider = null;
				return;
			}

			InternalCalls.PhysicsChangeDynamic(_collider.PhysicsID, true);
			InternalCalls.PhysicsChangeKinematic(_collider.PhysicsID, internal_kinematic);
		}

		public override void OnDestroy()
		{
			if (_collider != null)
				InternalCalls.PhysicsChangeDynamic(_collider.PhysicsID, false);
		}
	}
}
