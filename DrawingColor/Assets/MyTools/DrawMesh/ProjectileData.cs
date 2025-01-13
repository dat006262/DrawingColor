using MoreMountains.Feedbacks;
//using MoreMountains.TopDownEngine;
using UnityEngine;

namespace TempleCode.DrawMesh
{
    public enum ProjectileSpeed
    {
        ConstantChange,
        MonotonicChange,
        SinChange
        
    }
    
    public enum ProjectileMovement
    {
        Normal,
        Sin,
        Circle,
        FlyToTarget
        
    }

    public enum SizeOnMove
    {
        NotChange,
        Bigger
    }
    public class ProjectileData
    {
        public  bool    Enable;
        private Vector2 _position;
        #region Data
        public ProjectileMovement  MovementType = ProjectileMovement.Normal;
        public ProjectileSpeed     SpeedType    = ProjectileSpeed.ConstantChange;
        public SizeOnMove          SizeOnMove   = SizeOnMove.NotChange;
        public float               DelayDestroy;
    //    public GPUBullet           Render;
        public GameObject          Owner;
        public GameObject          Target;
   //     public ProjectileGPUWeapon weapon;
        public Vector2             Velocity;
        public Vector2             DirecNormalize;
        public float               DelayToMove;
        public int                 projectileIndex;
        public float               scaleRadiusSpeed;
        public float               LinearAcceleration  ;
        public float               AgngularSpeed;
        public float               TimeLive;
        public float               AngularAcceleration ;
        public Vector2             TargetFlyToPosition;
        public float DistanceToTargetFlyToPosition;
        public float               initialY;
        public float               MoveYSmooth;
        public Vector2 Position
        {
            get => _position;
            set
            {
                // if (Render != null)
                // {
                //     SetPosition();
                // }
                _position = value;
            }
        }
        
        public float               Rotation;
        public float               Speed;
        public int                 Headlth;
        public float               Damage;
        public bool                isFlip;
        public Color               Color;
        public float               Scale;
       
        public float               TimeToLive;
        public bool                isSpawnedBullet = false;
        public int                 IDLastHit       = 0;
        
        public bool    isMoveReturn = false;
        public float   timeElapsed  =0;
        public float   RadiusCircleMove ;
        public Vector2 CenterCircleMove ;
        public float   currentAngleCircleMove;

        #endregion

        public void ClearProjectile()
        {
            MovementType           = ProjectileMovement.Normal;
            SpeedType              = ProjectileSpeed.ConstantChange;
            SizeOnMove             = SizeOnMove.NotChange;
        //   Render                  = null;
           Enable                  = false;
           Owner                   = null;
     //      weapon                  = null;
           Target                  = null;
           DelayToMove             = 0;
           projectileIndex         = 0;
           TimeLive                = 0;
          TimeToLive               = -1;
           isMoveReturn            = false;
           IDLastHit               = 0;
           Scale                   = 0;
           isFlip                  = false;
           scaleRadiusSpeed        = 0;
            RadiusCircleMove       = 0 ;
            CenterCircleMove       = Vector2.zero;
            currentAngleCircleMove = 0;
        }
        // public void ShowRender()
        // {
        //    SetPosition();
        //     if (Render != null)
        //     {
        //         Render.gameObject.SetActive(true);
        //         Render.Modul.SetActive(true);
        //     }
        // }
        // public void SetIDHit(GameObject newOwner)
        // {
        //     IDLastHit = newOwner.GetInstanceID();
        // }
        // public void SetOwner(GameObject newOwner)
        // {
        //     Owner = newOwner;
        // }
        // public void SetTarget(GameObject newTaget)
        // {
        //     Target              = newTaget;
        //     TargetFlyToPosition = newTaget.transform.position;
        //     if (MovementType == ProjectileMovement.FlyToTarget)
        //     {
        //         DistanceToTargetFlyToPosition = Vector2.Distance(Position, TargetFlyToPosition);
        //     }
        // }
        // public void SetRender(GPUBullet render)
        // {
        //     Render = render;
        // }
        // public void SetWeapon(ProjectileGPUWeapon newWeapon)
        // {
        //     weapon = newWeapon;
        // }
        // public void SetDamage(int damage)
        // {
        //     Damage = damage;
        // }
        //
        // public void SetProjectileIndex(int newProjectileIndex) => projectileIndex = newProjectileIndex;
        //
        // public void SetDirection(Vector3 newDirection, Quaternion newRotation)
        // {
        //     DirecNormalize = newDirection.normalized;
        //     Velocity = Speed                * ((newDirection.normalized));
        //     Rotation = Quaternion.LookRotation(Vector3.forward, newDirection).eulerAngles.z / 180f * 3.14f; /* newRotation.z / 180f * 3.14f;*/;
        //     if (Render != null)
        //     {
        //         Render.transform.rotation = Quaternion.LookRotation(Vector3.forward, newDirection);
        //     }
        // }
        //
        // public void SetCenterCicleMove()
        // {
        //     if (MovementType == ProjectileMovement.Circle)
        //     {
        //
        //         CenterCircleMove = Position + DirecNormalize * RadiusCircleMove;
        //     }
        //
        //     currentAngleCircleMove = Vector2.Angle( Vector2.up,DirecNormalize);
        //
        //     if (DirecNormalize.x < 0)
        //     {
        //         currentAngleCircleMove = 360-currentAngleCircleMove;
        //     }
        //
        //     currentAngleCircleMove += 90;
        // }
        //
        // public Vector2 PositionMoveCircle( )
        // {
        //  
        //     float   pointX = CenterCircleMove.x + RadiusCircleMove * Mathf.Cos((360- currentAngleCircleMove) * Mathf.Deg2Rad);
        //     float   pointY = CenterCircleMove.y + RadiusCircleMove * Mathf.Sin((360-currentAngleCircleMove) * Mathf.Deg2Rad);
        //     Vector2 newPos = new Vector2(pointX,pointY);
        //     
        //     return newPos;
        // }
        // public  void SetPosition( bool relative = false)
        // {
        //     Render.transform.position = Position + Vector2.up * initialY;
        // }
    }
    
 


    public class DrawSpriteData
    {
        #region Data
        public Vector2 Position;
        public float Rotation;
        public Color Color;
        public float Scale;

        #endregion
    }
}