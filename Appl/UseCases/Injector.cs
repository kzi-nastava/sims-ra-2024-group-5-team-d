using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
        { typeof(IUserRepository), new UserRepository() },
        { typeof(ICommentReportRepository), new CommentReportRepository() },
        { typeof(INotificationRepository), new NotificationRepository() },
        { typeof(IAccommodationRenovationRepository), new AccommodationRenovationRepository() },
        { typeof(ITourRealisationRepository), new TourRealisationRepository() },
        { typeof(IAccommodationRepository), new AccommodationRepository() },
        { typeof(ICheckPointRepository), new CheckPointRepository() },
        { typeof(IAccommodationRatingRepository), new AccommodationRatingRepository() },
        { typeof(ICommentRepository), new CommentRepository() },
        { typeof(ISuperUserRepository), new SuperUserRepository() },
        { typeof(IGuestRatingRepository), new GuestRatingRepository() },
        { typeof(ILocationRepository), new LocationRepository() },
        { typeof(ITourGuestRepository), new TourGuestRepository() },
        { typeof(ITourRepository), new TourRepository() },
        { typeof(ITourRatingRepository), new TourRatingRepository() },
        { typeof(IForumCommentRepository), new ForumCommentRepository() },
        { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
        { typeof(ITourReservationRepository), new TourReservationRepository() },
        { typeof(IGuestRequestRepository), new GuestRequestRepository() },
        { typeof(IVoucherRepository), new VoucherRepository() },
        { typeof(IForumRepository), new ForumRepository() },
        { typeof(ITourRequestRepository), new TourRequestRepository() },
        { typeof(IComplexTourRequestRepository), new ComplexTourRequestRepository() },
        { typeof(IComplexSimpleRequestPairRepository), new ComplexSimpleRequestPairRepository()}
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
           {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
