using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModels.TourViewModels.TourGuideViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class ComplexTourRequestService
    {
        private IComplexTourRequestRepository _repository;
        private ComplexSimpleRequestPairService complexSimpleRequestPairService;
        private TourRequestService tourRequestService { get; set; }
        private TourRealisationService tourRealisationService { get; set; }
        private TourReservationService tourReservationService { get; set; }
        private TourService tourService { get; set; }

        public ComplexTourRequestService()
        {
            _repository = Injector.CreateInstance<IComplexTourRequestRepository>();
            complexSimpleRequestPairService = new ComplexSimpleRequestPairService();
            tourRequestService = new TourRequestService();
            tourRealisationService = new TourRealisationService();
            tourReservationService = new TourReservationService();
            tourService = new TourService();    
        }

        public void Delete(ComplexTourRequest request)
        {
            _repository.Delete(request);
            complexSimpleRequestPairService.GetAll().ForEach(x =>
            {
                if (x.ComplexRequestId == request.Id)
                {
                    complexSimpleRequestPairService.Delete(x);
                }
            });

        }

        public List<ComplexTourRequest> GetAll(User tourist)
        {            
            List<ComplexTourRequest> allComplexRequests = _repository.GetAll(tourist);
            List<ComplexSimpleRequestPair> allPairs = complexSimpleRequestPairService.GetAll();

            allComplexRequests.ForEach(complexReq =>
            {
                allPairs.ForEach(pair =>
                {
                    if (pair.ComplexRequestId == complexReq.Id)
                    {
                        TourRequest simpleRequest = tourRequestService.GetById(pair.SimpleRequestId);
                        if (simpleRequest != null)
                        {
                            complexReq.Requests.Add(simpleRequest);
                        }
                    }
                });
            });

            return allComplexRequests;
        }

        public List<ComplexTourRequest> GetAll()
        {
            List<ComplexTourRequest> allComplexRequests = _repository.GetAll();
            List<ComplexSimpleRequestPair> allPairs = complexSimpleRequestPairService.GetAll();

            allComplexRequests.ForEach(complexReq =>
            {
                allPairs.ForEach(pair =>
                {
                    if (pair.ComplexRequestId == complexReq.Id)
                    {
                        TourRequest simpleRequest = tourRequestService.GetById(pair.SimpleRequestId);
                        if (simpleRequest != null)
                        {
                            complexReq.Requests.Add(simpleRequest);
                        }
                    }
                });
            });

            return allComplexRequests;
        }

        public ComplexTourRequest GetById(int id)
        {
            ComplexTourRequest request = _repository.GetById(id);
            List<ComplexSimpleRequestPair> allPairs = complexSimpleRequestPairService.GetAll();

            
            allPairs.ForEach(pair =>
            {
                if (pair.ComplexRequestId == id)
                {
                    TourRequest simpleRequest = tourRequestService.GetById(pair.SimpleRequestId);
                    if (simpleRequest != null)
                    {
                        request.Requests.Add(simpleRequest);
                    }
                }
            });

            return request;
        }

        public int NextId()
        {
            return _repository.NextId();
        }

        public ComplexTourRequest Save(ComplexTourRequest request)
        {
            return _repository.Save(request);
        }

        public ComplexTourRequest Update(ComplexTourRequest request)
        {
            return _repository.Update(request);
        }

        public void UpdateStatus(int reqId)
        {
            ComplexTourRequest request = GetById(reqId);
            int numberOfAccepptedRequests = 0;

            request.Requests.ForEach(req =>
            {
                if (req.Status == STATE.ACCEPTED)
                {
                    numberOfAccepptedRequests++;
                }
            });
            
            if(numberOfAccepptedRequests == request.Requests.Count)
            {
                request.Status = STATE.ACCEPTED;
                Update(request);
            }

        }

        public void FixPotentialDateTimeOverLaping(RequestViewModel simpleRequest)
        {
            List<TourRequest> requestList = new List<TourRequest>();
            ComplexTourRequest complex = new ComplexTourRequest();
            List<DateTime> notPossibleSuggestions = new List<DateTime>();
            foreach (var pair in complexSimpleRequestPairService.GetAll())
            {
                if(pair.SimpleRequestId == simpleRequest.Id)
                {
                    complex = _repository.GetById(pair.ComplexRequestId);
                }
            }
            foreach (var pair in complexSimpleRequestPairService.GetAll())
            {
                if(pair.ComplexRequestId == complex.Id)
                {
                    requestList.Add(tourRequestService.GetById(pair.SimpleRequestId));
                }
            }
            foreach(var simple in requestList)
            {
                if(simple.Status == STATE.ACCEPTED)
                {

                    notPossibleSuggestions.Add(tourRealisationService.GetById(tourReservationService.GetById(simple.TourReservationId).TourRealisationId).StartTime);
                }
            }
            foreach(var not in notPossibleSuggestions)
            {
                foreach(var dates in simpleRequest.AvailableDates)
                {
                    if(not.DayOfYear == simpleRequest.SelectedDate.DayOfYear && not.Hour < simpleRequest.SelectedTime.Hour && not.AddHours(2).Hour > simpleRequest.SelectedTime.Hour)
                    {
                        simpleRequest.AvailableDates.Remove(dates);
                    }
                }
            }
            
        }


    }
}
