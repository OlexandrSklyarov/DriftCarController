using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace SA.Game
{
    public sealed class CarCalculateDriftPointSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<CarEngineComponent> _enginePool;
        private EcsPool<CarDriftComponent> _driftPool;
        private EcsFilter _filter;
        private TimeService _time;
        private Hud _hud;

        public void Init(IEcsSystems systems)
        {
            _time = systems.GetShared<SharedData>().TimeService;
            _hud = systems.GetShared<SharedData>().HUD;
            
            var world = systems.GetWorld();

            _enginePool = world.GetPool<CarEngineComponent>();  
            _driftPool = world.GetPool<CarDriftComponent>();  

            _filter = world.Filter<CarEngineComponent>()
                .Inc<CarDriftComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var engine = ref _enginePool.Get(ent);
                ref var drift = ref _driftPool.Get(ent);

                CalcDriftAngle(ref engine, ref drift);

                var config = engine.EngineRef.Config.Drift;
                
                if (drift.Angle > config.MaxDriftAngle)
                {
                    drift.Angle = 0f;
                }

                if (IsCanDrift(ref engine, ref drift))
                {
                    TryStartDrift(ref drift);

                    drift.DisableDriftTime = _time.Time + config.DisableDriftDelay;
                }
                else
                {
                    TryStopDrift(ref drift);
                }

                TryCalculateDriftPoints(ref drift);
                DrawResult(ref drift);
            }
        }

        private void TryStopDrift(ref CarDriftComponent drift)
        {
            if (drift.IsProcess && drift.DisableDriftTime <= _time.Time)
            {
                drift.IsProcess = false;
                drift.TotalPoints += drift.CurrentPoints;
                drift.CurrentPoints = 0f;
            }
        }

        private void TryStartDrift(ref CarDriftComponent drift)
        {
            if (!drift.IsProcess)
            {
                drift.IsProcess = true;
                drift.Factor = 1f;
            }
        }

        private void DrawResult(ref CarDriftComponent drift)
        {
            _hud.DriftPanel.SetDriftResult
            (
                drift.TotalPoints,
                drift.Angle,
                drift.Factor,
                drift.CurrentPoints,
                drift.DisableDriftTime > _time.Time
            );
        }        

        private void TryCalculateDriftPoints(ref CarDriftComponent drift)
        {
            if (!drift.IsProcess) return;

            drift.CurrentPoints += drift.Angle * drift.Factor * _time.DeltaTime;
            drift.Factor += _time.DeltaTime;
        }

        private void CalcDriftAngle(ref CarEngineComponent engine, ref CarDriftComponent drift)
        {
            var forward = engine.EngineRef.RB.transform.forward;
            var vel = new Vector3(engine.EngineRef.RB.velocity.x, 0f, engine.EngineRef.RB.velocity.z).normalized;
            drift.Angle = Vector3.Angle(forward, vel);
        }

        private bool IsCanDrift(ref CarEngineComponent engine, ref CarDriftComponent drift)
        {
            var config = engine.EngineRef.Config.Drift;
            return drift.Angle > config.MinDriftAngle && engine.RealSpeed > config.SpeedThreshold;
        }
    }
}