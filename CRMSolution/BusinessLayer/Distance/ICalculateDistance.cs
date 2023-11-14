using DomainEntities;

namespace BusinessLayer
{
    interface ICalculateDistance
    {
        void CalculateDistanceInMeters(TrackingRecordForDistance record);
    }
}
