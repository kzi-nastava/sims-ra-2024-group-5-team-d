using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Appl.UseCases
{
    public class CompleteComplexTourRequestService
    {
        private ComplexSimpleRequestPairService complexSimpleRequestPairService { get; set; }

        private ComplexTourRequestService complexTourRequestService { get; set; }

        private TourRequestService tourRequestService { get; set; }


        private CompleteComplexTourRequestService()
        {
            complexTourRequestService = new ComplexTourRequestService();
            complexSimpleRequestPairService = new ComplexSimpleRequestPairService();
            tourRequestService = new TourRequestService();
        }

        public void GetAll(User tourist)
        {
            List<Tuple<ComplexTourRequest, List<TourRequest>>> requests = new List<Tuple<ComplexTourRequest, List<TourRequest>>>();
            List<ComplexTourRequest> allComplexRequests = complexTourRequestService.GetAll(tourist);
            List<ComplexSimpleRequestPair> allPairs = complexSimpleRequestPairService.GetAll();

            allComplexRequests.ForEach(complexReq =>
            {
                List<TourRequest> associatedTourRequests = new List<TourRequest>();

                allPairs.ForEach(pair =>
                {
                    if (pair.ComplexRequestId == complexReq.Id)
                    {
                        TourRequest simpleRequest = tourRequestService.GetById(pair.SimpleRequestId);
                        if (simpleRequest != null)
                        {
                            associatedTourRequests.Add(simpleRequest);
                        }
                    }
                });

                requests.Add(new Tuple<ComplexTourRequest, List<TourRequest>>(complexReq, associatedTourRequests));
            });
        }

        public List<Tuple<ComplexTourRequest, List<TourRequest>>> GetAll()
        {
            List<Tuple<ComplexTourRequest, List<TourRequest>>> requests = new List<Tuple<ComplexTourRequest, List<TourRequest>>>();

            List<ComplexTourRequest> allComplexRequests = complexTourRequestService.GetAll();
            List<ComplexSimpleRequestPair> allPairs = complexSimpleRequestPairService.GetAll();

            allComplexRequests.ForEach(complexReq =>
            {
                List<TourRequest> associatedTourRequests = new List<TourRequest>();

                allPairs.ForEach(pair =>
                {
                    if (pair.ComplexRequestId == complexReq.Id)
                    {
                        TourRequest simpleRequest = tourRequestService.GetById(pair.SimpleRequestId);
                        if (simpleRequest != null)
                        {
                            associatedTourRequests.Add(simpleRequest);
                        }
                    }
                });

                requests.Add(new Tuple<ComplexTourRequest, List<TourRequest>>(complexReq, associatedTourRequests));
            });

            return requests;
        }


    }
}
