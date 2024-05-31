using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using iText.Commons.Bouncycastle.Cert.Ocsp;
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
        private TourReservationService tourReservationService { get; set; }
        private TourRealisationService tourRealisationService { get; set; }
        private TourService tourService { get; set; }

        public ComplexTourRequestService()
        {
            _repository = Injector.CreateInstance<IComplexTourRequestRepository>();
            complexSimpleRequestPairService = new ComplexSimpleRequestPairService();
            tourRequestService = new TourRequestService();
            tourReservationService = new TourReservationService();
            tourRealisationService = new TourRealisationService();
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

        public void Validate()
        {
            GetAll().ForEach(req =>
            {
                if (!req.IsAcceptable())
                {
                    InvalidateRequest(req);
                }
                else if(req.Requests.Any(sim => sim.Status == STATE.PENDING))
                {
                    InvalidateIfFirstAcceptedIsInvalid(req);
                }
            });
        }

        public void InvalidateIfFirstAcceptedIsInvalid(ComplexTourRequest req)
        {
            foreach (TourRequest simple in req.Requests)
            {
                if (simple.Status == STATE.ACCEPTED)
                {
                    if (!tourRequestService.ShouldAcceptedRequestStillBeValid(simple))
                    {
                        InvalidateRequest(req);
                    }
                }
            }
        }

        public void InvalidateRequest(ComplexTourRequest request)
        {
            foreach(TourRequest req in request.Requests)
            {
                if(req.Status == STATE.ACCEPTED)
                {
                    TourReservation reservation = tourReservationService.GetById(req.TourReservationId);
                    TourRealisation realisation = tourRealisationService.GetTourRealisationById(reservation.TourRealisationId);
                    Tour tour = tourService.GetById(realisation.TourId);

                    tourService.DeleteTour(tour);
                    tourRealisationService.DeleteTourRealisation(realisation);

                    reservation.TourRealisationId = -1;
                    tourReservationService.Update(reservation);
                }
                req.Status = STATE.INVALID;
                tourRequestService.Update(req);
            }
            request.Status = STATE.INVALID;
            Update(request);
        }


    }
}
