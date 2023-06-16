using UnityEngine;

namespace Mtd.Utils {
  public static class UnitSpeed {
    public const float MinSpeed = 1f;
    public const float MaxTimeBetweenActions = 2f;
    public const float MaxSpeed = 200f;
    public const float MinTimeBetweenActions = 1f / 20f;
    public const float SpeedRange = MaxSpeed - MinSpeed;
    public const float TimeBetweenActionsRange = MaxTimeBetweenActions - MinTimeBetweenActions;
    
    public static float ToTimeBetweenActions(float speed) {
      if (speed < MinSpeed || speed > MaxSpeed) {
        Debug.LogWarning("The given speed is out of range. (" + speed + " is not between " + MinSpeed + " and " + MaxSpeed + ".)");
      }

      float pointInSpeedRange = (speed - MinSpeed) / SpeedRange;
      float pointInTimeRange = Mathf.Pow(1f - pointInSpeedRange, 2f);

      return pointInTimeRange * TimeBetweenActionsRange + MinTimeBetweenActions;
    }
    
    public static float FromTimeBetweenActions(float timeBetweenActions) {
      if (timeBetweenActions < MinTimeBetweenActions || timeBetweenActions > MaxTimeBetweenActions) {
        Debug.LogWarning("The given timeBetweenActions is out of range. (" + timeBetweenActions + "s is not between " + MinTimeBetweenActions + "s and " + MaxTimeBetweenActions + "s.)");
      }
      
      float pointInTimeRange = (timeBetweenActions - MinTimeBetweenActions) / TimeBetweenActionsRange;
      float pointInSpeedRange = 1f - Mathf.Sqrt(pointInTimeRange);
      
      return (pointInSpeedRange * SpeedRange) + MinSpeed;
    }
  }
}
