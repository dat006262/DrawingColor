using System;
using MoreMountains.Tools;
using TempleCode.DrawMesh;
using TempleCode.FolderRoot.Attributes;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace TempleCode.Emmiter
{
    public abstract class DAT_EmmiterBase : MMSingleton<DAT_EmmiterBase>
    {
        public ProjectilePrefab projectilePrefab;
        public int              maxProjectiles = 1000;
        public bool             isAutofire;
        public bool             IsFixedTimestep;
        public bool             BounceOffSurfaces                  = true;
        public bool             CullProjectilesOutsideCameraBounds = true;

        [SerializeField] protected Camera Camera;

        protected ContactFilter2D      ContactFilter;
        protected ProjectTileManager   projecTileManager;
        protected Pool<ProjectileData> Projectiles;
        protected int[]                ArrayProjectTilesBranch;
        protected int[]                ArrayProjectTileMain;
        protected int                  branchIndex;


        protected Plane[]        Planes           = new Plane[6];
        protected RaycastHit2D[] RaycastHitBuffer = new RaycastHit2D[1];
        private   float          Timer;
        private   int            _totalProjectTile;
        protected Action         onTotalProjectTileSet;
        public int TotalProjectTile
        {
            get { return _totalProjectTile;}
            protected set
            {
                _totalProjectTile = value;
    
            }
        }

        [ConditionalField(nameof(IsFixedTimestep)), Range(0.01f, 0.2f)]
        public float FixedTimestepRate = 0.01f;

        public virtual void Awake()
        {
            Planes = new Plane[6];
            ContactFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask    = 1,
                useTriggers  = true,
            };
            projecTileManager = ProjectTileManager.Instance;
        }

        void OnDisable()
        {
            ClearAllProjectiles();
        }


        public void Initialize()
        {
            Projectiles = new Pool<ProjectileData>(maxProjectiles);


            ArrayProjectTilesBranch = new int[maxProjectiles];
            ArrayProjectTileMain    = new int[maxProjectiles];
        }

        public void UpdateEmitter(float tick)
        {
            {
            }
            if (isAutofire)
            {
                //Spawn somthing
            }
            else
            {
            }

            if (IsFixedTimestep)
            {
                Timer += tick;
                while (Timer > FixedTimestepRate)
                {
                    Timer -= FixedTimestepRate;
                    UpdateProjectiles(FixedTimestepRate);
                }
            }
            else
            {
                UpdateProjectiles(tick);
            }

            UpdateBuffers(0);
            
            if (onTotalProjectTileSet != null)
            {
                onTotalProjectTileSet.Invoke();
            }
        }

        public void UpdateProjectiles(float tick)
        {
            TotalProjectTile = 0;

            //Update camera planes if needed
            if (CullProjectilesOutsideCameraBounds)
            {
                GeometryUtility.CalculateFrustumPlanes(Camera, Planes);
            }

            int mainIndex = branchIndex;
            branchIndex = 0;
            // Only loop through currently active projectiles
            for (int i = 0; i < ArrayProjectTileMain.Length - 1; i++)
            {
                // End of array is set to -1
                if (ArrayProjectTileMain[i] == -1)
                    break;

                Pool<ProjectileData>.Node node = Projectiles.GetNode(ArrayProjectTileMain[i]);
                UpdateProjectile(ref node, tick);

                // If still active store in our active projectile collection
                if (node.Active)
                {
                    ArrayProjectTilesBranch[branchIndex] = node.NodeIndex;
                    branchIndex++;
                }
            }

            ArrayProjectTilesBranch[branchIndex] = -1;
            System.Array.Copy(ArrayProjectTilesBranch, ArrayProjectTileMain,
                Mathf.Max(branchIndex, mainIndex));
        }

        protected void UpdateBuffers(float tick)
        {
            TotalProjectTile = 0;

            for (int i = 0; i < ArrayProjectTilesBranch.Length; i++)
            {
                // End of array is set to -1
                if (ArrayProjectTilesBranch[i] == -1)
                    break;

                Pool<ProjectileData>.Node node = Projectiles.GetNode(ArrayProjectTilesBranch[i]);

                node.Item.TimeToLive -= tick;

                projecTileManager.UpdateBufferData(projectilePrefab, node.Item);
                TotalProjectTile++;
            }

            // faster to do two loops than to update the outlines at the same time due to the renderer swapping
            for (int i = 0; i < ArrayProjectTilesBranch.Length; i++)
            {
                // End of array is set to -1
                if (ArrayProjectTilesBranch[i] == -1)
                    break;

                Pool<ProjectileData>.Node node = Projectiles.GetNode(ArrayProjectTilesBranch[i]);

                //handle outline
            }
        }

        protected abstract void SpawnOneProjectile();
        protected abstract void UpdateProjectile(ref Pool<ProjectileData>.Node node, float tick);

        public void ClearAllProjectiles()
        {
            for (int i = 0; i < ArrayProjectTilesBranch.Length; i++)
            {
                // End of array is set to -1
                if (ArrayProjectTilesBranch[i] == -1)
                    break;

                Pool<ProjectileData>.Node node = Projectiles.GetNode(ArrayProjectTilesBranch[i]);
                ReturnNode(node);

                ArrayProjectTilesBranch[i] = -1;
            }

            branchIndex = 0;
        }


        protected virtual void ReturnNode(Pool<ProjectileData>.Node node)
        {
            if (node.Active)
            {
                
                node.Item.TimeToLive   = -1;
                node.Item.isMoveReturn = false;
                Projectiles.Return(node.NodeIndex);
                node.Active = false;
             
            }
            
        }
    }
}