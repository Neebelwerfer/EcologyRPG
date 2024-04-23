using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Scripting;
using MoonSharp.Interpreter;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.AbilityScripting
{
    public class Targets
    {
        static readonly RaycastHit[] rayCastHits = new RaycastHit[5];

        public static void AddToGlobal(Script script)
        {
            script.Globals["CreateLineIndicator"] = (System.Action<CastContext, float, float, float>)CreateLineIndicator;
            script.Globals["CreateConeIndicator"] = (System.Action<CastContext, float, float, float>)CreateConeIndicator;
            script.Globals["CreateCircleIndicator"] = (System.Action<CastContext, float, float>)CreateCircleIndicator;
            script.Globals["GetTargetsInLine"] = (System.Func<CastContext, float, float, List<BaseCharacter>>)GetTargetsInLine;
            script.Globals["GetTargetsInRadius"] = (System.Func<CastContext, float, List<BaseCharacter>>)GetTargetsInRadius;
            script.Globals["GetTargetsInCone"] = (System.Func<CastContext, float, float, LayerMask, List<BaseCharacter>>)GetTargetsInCone;
            script.Globals["GetMousePoint"] = (System.Func<Vector3>)GetMousePoint;
        }

        public static void CreateLineIndicator(CastContext context, float width, float range, float duration)
        {
            if(Physics.Raycast(context.castPos.Vector, Vector3.down, out RaycastHit hit, 1000, Core.Abilities.AbilityManager.WalkableGroundLayer))
            {
                var meshPrefab = AbilityManager.IndicatorMesh;
                var mesh = Object.Instantiate(meshPrefab);
                mesh.transform.position = hit.point;
                mesh.SetOwner(context.GetOwner());
                mesh.SetColor(context.GetOwner().Faction == Faction.player ? Color.black : Color.red);
                var direction = context.dir.Vector;

                mesh.Clear();
                mesh.TriangulateBox(direction, range, width);
                mesh.Apply();
                Object.Destroy(mesh.gameObject, duration);
            }
            else
            {
                Debug.Log("CreateLineIndicator failed");
            }
        }

        public static void CreateConeIndicator(CastContext context, float angle, float range, float duration)
        {
            if (Physics.Raycast(context.castPos.Vector, Vector3.down, out RaycastHit hit, 1000, Core.Abilities.AbilityManager.WalkableGroundLayer))
            {
                var meshPrefab = AbilityManager.IndicatorMesh;
                var mesh = Object.Instantiate(meshPrefab);
                mesh.transform.position = hit.point;
                mesh.SetOwner(context.GetOwner());
                mesh.SetColor(context.GetOwner().Faction == Faction.player ? Color.black : Color.red);
                var direction = context.dir.Vector;

                mesh.Clear();
                mesh.TriangulateCone(direction, range, angle);
                mesh.Apply();
                Object.Destroy(mesh.gameObject, duration);
            }
            else
            {
                Debug.Log("CreateConeIndicator failed");
            }
        }

        public static void CreateCircleIndicator(CastContext context, float radius, float duration)
        {
            if (Physics.Raycast(context.castPos.Vector, Vector3.down, out RaycastHit hit, 1000, Core.Abilities.AbilityManager.WalkableGroundLayer))
            {
                var meshPrefab = AbilityManager.IndicatorMesh;
                var mesh = Object.Instantiate(meshPrefab);
                mesh.transform.position = hit.point;
                mesh.SetOwner(context.GetOwner());
                mesh.SetColor(context.GetOwner().Faction == Faction.player ? Color.black : Color.red);
                var direction = context.dir.Vector;

                mesh.Clear();
                mesh.TriangulateCircle(direction, radius);
                mesh.Apply();
                Object.Destroy(mesh.gameObject, duration);
            }
            else
            {
                Debug.Log("CreateCircleIndicator failed");
            }
        }

        public static List<BaseCharacter> GetTargetsInLine(CastContext context, float width, float range)
        {
            Vector3 Size = new Vector3(width, 20, range);
            var origin = context.castPos.Vector;
            var direction = context.dir.Vector;
            Collider[] colliderHits = new Collider[5];


            var halfExtents = Size / 2;
            var numHits = Physics.OverlapBoxNonAlloc(origin + (direction * halfExtents.z), halfExtents, colliderHits, Quaternion.LookRotation(direction, Vector3.up), Core.Abilities.AbilityManager.TargetMask, QueryTriggerInteraction.Ignore);
            List<BaseCharacter> targets = new();
            Debug.Log($"Hits: {numHits}");

            for (int i = 0; i < numHits; i++)
            {
                Debug.Log($"Hit {colliderHits[i].gameObject.name}");
                if (colliderHits[i].TryGetComponent<CharacterBinding>(out var binding))
                {
                    if (binding.Character.Faction == context.GetOwner().Faction) continue;
                    Debug.Log("Hit");
                    var character = binding.Character;
                    targets.Add(character);
                }
            }
            return targets;
        }

        public static List<BaseCharacter> GetTargetsInRadius(CastContext context, float radius)
        {
            var origin = context.castPos.Vector;
            Collider[] colliderHits = new Collider[5];
            var numHits = Physics.OverlapSphereNonAlloc(origin, radius, colliderHits, AbilityManager.TargetMask);
            List<BaseCharacter> targets = new();

            for (int i = 0; i < numHits; i++)
            {
                if (colliderHits[i].TryGetComponent<CharacterBinding>(out var binding))
                {
                    if (binding.Character.Faction == context.GetOwner().Faction) continue;
                    var character = binding.Character;
                    targets.Add(character);
                }
            }
            return targets;
        }

        public static List<BaseCharacter> GetTargetsInCone(CastContext context, float angle, float radius, LayerMask mask)
        {
            var origin = context.castPos.Vector;
            var forward = context.dir.Vector;
            Collider[] colliderHits = new Collider[5];
            var numhits = Physics.OverlapSphereNonAlloc(origin, radius, colliderHits, mask);
            List<BaseCharacter> targets = new();

            for (int i = 0; i < numhits; i++)
            {
                if (Vector3.Angle(forward, colliderHits[i].transform.position - origin) < angle)
                {
                    if (colliderHits[i].TryGetComponent<CharacterBinding>(out var binding))
                    {
                        if (binding.Character.Faction == context.GetOwner().Faction) continue;
                        var character = binding.Character;
                        targets[i] = character;
                    }
                }
            }
            return targets;
        }

        //public static Vector3 GetMouseDirection(Vector3 origin, Camera camera)
        //{
        //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out RaycastHit hit, 100, AbilityManager.GroundMask))
        //    {
        //        return (hit.point - origin).normalized;
        //    }
        //    return Vector3.zero;
        //}

        //public static Vector3 GetDirection(Vector3 origin, Vector3 target)
        //{
        //    return (target - origin).normalized;
        //}

        public static Vector3 GetMousePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, AbilityManager.GroundMask))
            {
                return hit.point;
            }
            return Vector3.zero;
        }
    }
}