using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatterns.Mediator
{
    internal static class Program
    {
        internal static void Main()
        {
            var istanbulTower = new IstanbulControl();
            
            var oh011 = new OzHawaii
            {
                Airport = istanbulTower, 
                FlightNumber = "OH011", 
                From = "Hawaii"
            };
            istanbulTower.Register(oh011);
            
            var oh132 = new OzHawaii { Airport = istanbulTower, FlightNumber = "oh132", From="Roma" };
            istanbulTower.Register(oh132);
            var zy99 = new ToughWay { Airport = istanbulTower, FlightNumber = "zy99", From = "Antarktika" };
            istanbulTower.Register(zy99);
            
            // Uçaklar yeni rotalarını talep ederler.
            zy99.RequestNewWay("34:43E;41:41W");
 
            oh011.RequestNewWay("34:43E;41:41W");
        }
    }

    // Colleague:
    internal class Airline
    {
        public IAirportControl Airport { get; set; }
        public string FlightNumber { get; set; }
        public string From { get; set; }

        public void RequestNewWay(string myWay)
        {
            Airport.SuggestWay(FlightNumber, myWay);
        }

        public virtual void GetWay(string messageFromAirport)
        {
            Console.WriteLine($"You need to head to {messageFromAirport}");
        }
    }

    // Concrete Colleague
    internal class OzHawaii : Airline
    {
        public override void GetWay(string messageFromAirport)
        {
            Console.WriteLine($" Oz Hawai Flight Number: {messageFromAirport}");
            base.GetWay(messageFromAirport);
        }
    }
    
    // Concrete Colleague
    internal class ToughWay : Airline
    {
        public override void GetWay(string messageFromAirport)
        {
            Console.WriteLine($"ToughWay Fligh Number {messageFromAirport}");
            base.GetWay(messageFromAirport);
        }
    }

    internal interface IAirportControl
    {
        void Register(Airline airline);
        void SuggestWay(string flightNumber, string way);
    }

    internal class IstanbulControl : IAirportControl
    {
        private readonly Dictionary<string, Airline> _planes;

        public IstanbulControl()
        {
            _planes = new Dictionary<string, Airline>();
        }

        public void Register(Airline airline)
        {
            if (!_planes.ContainsValue(airline))
            {
                _planes[airline.FlightNumber] = airline;
            }

            airline.Airport = this;
        }

        public void SuggestWay(string flightNumber, string way)
        {
            // Searching for a new route:
            Thread.Sleep(250);
            var random = new Random();
            _planes[flightNumber].GetWay(
                $"{random.Next(1, 100).ToString()}:{random.Next(1, 100).ToString()}E;{random.Next(1, 100).ToString()}:{random.Next(1, 100).ToString()}W");
            
        }
    }
}