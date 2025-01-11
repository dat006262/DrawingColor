using System.Collections.Generic;
using MoreMountains.Tools;
using TempleCode.DrawMesh;
using TempleCode.Popup_SoundManager.Generic;
using UnityEngine;

namespace TempleCode.Emmiter
{
    public class ProjectTileManager : MMSingleton<ProjectTileManager>
    {
        [SerializeField] private int MaxEmitters = 100;

        [SerializeField] private List<ProjectilePrefab> ProjectilePrefabs;

        [SerializeField] private DAT_EmmiterBase[] EmittersArray;

        // Each projectile type has its own material, therefore, own IndirectRenderer
        private Dictionary<int, IndirectRenderer> dicIndirectRenderers;
        private Dictionary<int, ProjectileTotal>  dicProjectileTotals;
        private int                               EmitterCount;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
            RegisterEmitters();
        }

        private void Update()
        {
            DrawEmitters();
            UpdateEmitters();
        }

        void OnGUI()
        {
            int total = 0;
            for (int n = 0; n < dicProjectileTotals.Count; n++)
            {
                total += dicProjectileTotals[n].ActiveProjectiles;
            }

        //    GUI.skin.label.fontSize = 70;
         //   GUI.Label(new Rect(5, 5, 1080, 100), "Projectiles: " + total.ToString());
        }

        private void Initialize()
        {
            //Set Dictionary, ID,...

            // Grab a list of Projectile Types founds in assets folder "ProjectilePrefabs"
            var projectileTypes = Resources.LoadAll<GameObject>("ProjectilePrefabs");
            ProjectilePrefabs    = new List<ProjectilePrefab>();
            dicIndirectRenderers = new Dictionary<int, IndirectRenderer>();
            dicProjectileTotals  = new Dictionary<int, ProjectileTotal>();
            var ID_proPref = 0;
            for (var n = 0; n < projectileTypes.Length; n++)
            {
                var prefabs = projectileTypes[n].GetComponent<ProjectilePrefab>();

                // if script is not attached, then we skip as it is invalid.
                if (prefabs == null)
                    continue;

                prefabs.Initialize(ID_proPref);
                ProjectilePrefabs.Add(prefabs);

                // If material is set to be a static color ensure we do not send color data to shader
                dicIndirectRenderers.Add(ID_proPref,
                    new IndirectRenderer(prefabs.GetMaxProjectileCount(), prefabs.Material, prefabs.Mesh,
                        prefabs.IsStaticColor));
                dicProjectileTotals.Add(ID_proPref, new ProjectileTotal());
                ID_proPref++;
            }

            EmittersArray = new DAT_EmmiterBase[MaxEmitters];
        }

        private void RegisterEmitters()
        {
            EmitterCount = 0;
            var emittersTemp
                = FindObjectsOfType<DAT_EmmiterBase>();
            for (var n = 0; n < emittersTemp.Length; n++)
            {
                EmittersArray[n] = emittersTemp[n];
            }

            for (var n = 0; n < EmittersArray.Length; n++)
            {
                if (EmittersArray[n] != null)
                {
                    EmittersArray[n].Initialize();
                    EmitterCount++;
                }
            }
        }

        private void RefreshEmitters()
        {
        }

        private void DrawEmitters()
        {
            // We draw all emitters at the same time based on their Projectile Type.  1 draw call per projectile type.
            for (var n = 0; n < ProjectilePrefabs.Count; n++)
            {
                var id               = ProjectilePrefabs[n].Index;
                var totalProjectTile = dicProjectileTotals[id].ActiveProjectiles;
                dicIndirectRenderers[ProjectilePrefabs[n].Index].Draw2(totalProjectTile);
            }
        }


        private int              LastAccessedProjectileTypeIndex = -1;
        private IndirectRenderer LastAccessedRenderer;

        public void UpdateEmitters()
        {
            //reset
            for (var n = 0; n < ProjectilePrefabs.Count; n++)
            {
                dicProjectileTotals[n].ActiveProjectiles = 0;
                ProjectilePrefabs[n].ResetBufferIndex();
            }

            for (var n = 0; n < EmittersArray.Length; n++)
            {
                if (EmittersArray[n] != null)
                {
                    if (EmittersArray[n].gameObject.activeSelf && EmittersArray[n].enabled)
                    {
                        EmittersArray[n].UpdateEmitter(Time.deltaTime);

                        dicProjectileTotals[EmittersArray[n].projectilePrefab.Index].ActiveProjectiles +=
                            EmittersArray[n].TotalProjectTile;
                    }
                    else
                    {
                        // if the gameobject was disabled then clear all projectiles from this emitter
                        //EmittersArray[n].ClearAllProjectiles();
                    }
                }
            }
        }

        public void UpdateBufferData(ProjectilePrefab projectileType, ProjectileData data)
        {
            if (projectileType.Index != LastAccessedProjectileTypeIndex)
            {
                LastAccessedProjectileTypeIndex = projectileType.Index;
                LastAccessedRenderer            = dicIndirectRenderers[LastAccessedProjectileTypeIndex];
            }

            LastAccessedRenderer.UpdateBufferData(projectileType.BufferIndex, data);
            projectileType.IncrementBufferIndex();
        }
    }

    public class ProjectileTotal
    {
        public int ActiveProjectiles = 0;
    }
}