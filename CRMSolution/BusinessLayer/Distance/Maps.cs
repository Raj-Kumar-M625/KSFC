using DomainEntities;

namespace BusinessLayer
{
    internal abstract class Maps : ICalculateDistance
    {
        public abstract void CalculateDistanceInMeters(TrackingRecordForDistance record);
    }
}
