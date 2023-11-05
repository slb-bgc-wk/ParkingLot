﻿using ParkingLot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParkingLotTest
{
    public class ParkingBoyTest
    {
        [Fact]
        public void Should_return_ticket_When_parkingboy_park_Given_car()
        {
            var car = "Benze";
            var expect_ticket = "T_Benze";
            StandardParkingBoy parkingBoy = new StandardParkingBoy(new ParkingLotPlace());

            var actual_ticket = parkingBoy.ParkCar(car);

            Assert.Equal(expect_ticket, actual_ticket);
        }

        [Fact]
        public void Should_return_car_When_parkingboy_fetch_Given_ticket()
        {
            var expect_car = "Benze";
            var ticket = "T_Benze";
            ParkingLotPlace parkingLot = new ParkingLotPlace();
            parkingLot.ParkCar(expect_car);
            StandardParkingBoy parkingboy = new StandardParkingBoy(parkingLot);

            var actual_car = parkingboy.FetchCar(ticket);

            Assert.Equal(expect_car, actual_car);
        }

        [Fact]
        public void Should_return_right_car_When_parkingboy_fetch_car_Given_right_ticket()
        {
            var expect_car1 = "Benze";
            var expect_car2 = "BMW";
            var ticket1 = "T_Benze";
            var ticket2 = "T_BMW";
            ParkingLotPlace parkingLot = new ParkingLotPlace();
            parkingLot.ParkCar(expect_car1);
            parkingLot.ParkCar(expect_car2);
            StandardParkingBoy parkingboy = new StandardParkingBoy(parkingLot);

            var actual_car1 = parkingboy.FetchCar(ticket1);
            var actual_car2 = parkingboy.FetchCar(ticket2);

            Assert.Equal(expect_car1, actual_car1);
            Assert.Equal(expect_car2, actual_car2);
        }

        [Fact]
        public void Should_throw_exception_When_parkingboy_fetch_Given_wrong_or_no_ticket()
        {
            var expect_msg = "Unrecognized parking ticket.";
            var wrong_ticket = "WRONG Ticket";
            var empty_ticket = string.Empty;
            ParkingLotPlace parkingLot = new ParkingLotPlace();
            StandardParkingBoy parkingBoy = new StandardParkingBoy(parkingLot);

            WrongException acturl_wrong_ticket = Assert.Throws<WrongException>(() => parkingBoy.FetchCar(wrong_ticket));
            WrongException acturl_empty_ticket = Assert.Throws<WrongException>(() => parkingBoy.FetchCar(empty_ticket));

            Assert.Equal(expect_msg, acturl_wrong_ticket.Message);
            Assert.Equal(expect_msg, acturl_empty_ticket.Message);
        }

        [Fact]
        public void Should_throw_exception_When_parkingboy_fetch_Given_used_ticket()
        {
            var car = "Benze";
            ParkingLotPlace parkingLot = new ParkingLotPlace();
            var ticket = parkingLot.ParkCar(car);
            parkingLot.FetchCar(ticket);
            StandardParkingBoy parkingBoy = new StandardParkingBoy(parkingLot);
            var expect_msg = "Unrecognized parking ticket.";

            WrongException acturl_wrong_ticket = Assert.Throws<WrongException>(() => parkingBoy.FetchCar(car));

            Assert.Equal(expect_msg, acturl_wrong_ticket.Message);
        }

        [Fact]
        public void Should_throw_exception_When_parkingBoy_fetch_Given_no_parkingSpot()
        {
            string[] cars = { "Benze", "BMW", "Rolls-Royce", "Tesla", "Lamborghini", "Porsche" };
            ParkingLotPlace parkingLot = new ParkingLotPlace();
            foreach (var car in cars)
            {
                parkingLot.ParkCar(car);
            }

            StandardParkingBoy parkingBoy = new StandardParkingBoy(parkingLot);

            var expect_msg = "No available position.";

            WrongException acturl_wrong_msg = Assert.Throws<WrongException>(() => parkingBoy.ParkCar("Mazda"));

            Assert.Equal(expect_msg, acturl_wrong_msg.Message);
        }

        [Fact]
        public void Should_return_ticket_from_first_parkinglot_When_parkingboyPlus_park_Given_first_parkingLot_available()
        {
            var car = "Benze";
            var expect_ticket = "T_Benze";
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            ParkingBoyBase parkingBoy = new StandardParkingBoyPlus(parkingLotPlaces);

            var actual_ticket = parkingBoy.ParkCar(car);

            Assert.Equal(expect_ticket, actual_ticket);
        }

        [Fact]
        public void Should_return_ticket_from_second_parkinglot_When_parkingboyPlus_park_Given_second_parkingLot_available_first_not()
        {
            var mazda = "Mazda";
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            string[] cars = { "Benze", "BMW", "Rolls-Royce", "Tesla", "Lamborghini", "Porsche" };
            foreach (var car in cars)
            {
                parkingLotOne.ParkCar(car);
            }

            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            StandardParkingBoyPlus parkingBoy = new StandardParkingBoyPlus(parkingLotPlaces);

            var ticket = parkingBoy.ParkCar(mazda);

            Assert.Equal(1, parkingBoy.TellParkingLotIndexCarParked(ticket));
        }

        [Fact]
        public void Should_return_right_cars_from_different_parkingLot_When_parkingboyPlus_fetch_car_Given_right_ticket_of_different_parkingLot()
        {
            var expect_car1 = "Benze";
            var expect_car2 = "BMW";
            var ticket1 = "T_Benze";
            var ticket2 = "T_BMW";
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            parkingLotOne.ParkCar(expect_car1);
            parkingLotTwo.ParkCar(expect_car2);
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            ParkingBoyBase parkingboy = new StandardParkingBoyPlus(parkingLotPlaces);

            var actual_car1 = parkingboy.FetchCar(ticket1);
            var actual_car2 = parkingboy.FetchCar(ticket2);

            Assert.Equal(expect_car1, actual_car1);
            Assert.Equal(expect_car2, actual_car2);
        }

        [Fact]
        public void Should_throw_unrecognized_exception_When_parkingboyPlus_fetch_Given_wrong_ticket()
        {
            var expect_msg = "Unrecognized parking ticket.";
            var mazda_ticket = "T_Mazda";
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            string[] cars = { "Benze", "BMW", "Rolls-Royce", "Tesla", "Lamborghini", "Porsche" };
            foreach (var car in cars)
            {
                parkingLotOne.ParkCar(car);
            }

            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            StandardParkingBoyPlus parkingBoy = new StandardParkingBoyPlus(parkingLotPlaces);

            WrongException acturl_wrong_msg = Assert.Throws<WrongException>(() => parkingBoy.FetchCar(mazda_ticket));

            Assert.Equal(expect_msg, acturl_wrong_msg.Message);
        }

        [Fact]
        public void Should_throw_unrecognized_exception_When_parkingboyPlus_fetch_Given_used_ticket()
        {
            var expect_msg = "Unrecognized parking ticket.";
            var benze_ticket = "T_Benze";
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            StandardParkingBoyPlus parkingBoy = new StandardParkingBoyPlus(parkingLotPlaces);
            string[] cars = { "Benze", "BMW", "Rolls-Royce", "Tesla", "Lamborghini", "Porsche", "Mazda" };
            foreach (var car in cars)
            {
                parkingBoy.ParkCar(car);
            }

            parkingBoy.FetchCar(benze_ticket);

            WrongException acturl_wrong_msg = Assert.Throws<WrongException>(() => parkingBoy.FetchCar(benze_ticket));

            Assert.Equal(expect_msg, acturl_wrong_msg.Message);
        }

        [Fact]
        public void Should_throw_noPosition_exception_When_parkingboyPlus_fetch_Given_no_parkingSpot_available()
        {
            var expect_msg = "No available position.";
            var carToPark = "WuLing";
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            StandardParkingBoyPlus parkingBoy = new StandardParkingBoyPlus(parkingLotPlaces);
            string[] cars = { "Benze", "BMW", "Rolls-Royce", "Tesla", "Lamborghini", "Porsche", "Mazda", "Honda", "Bentley", "GTR", "911", "LiXiang" };
            foreach (var car in cars)
            {
                parkingBoy.ParkCar(car);
            }

            WrongException acturl_wrong_msg = Assert.Throws<WrongException>(() => parkingBoy.ParkCar(carToPark));

            Assert.Equal(expect_msg, acturl_wrong_msg.Message);
        }

        [Fact]
        public void Should_return_ticket_from_parkinglot_When_smartParkingBoy_park_Given_all_three_parkingLot_empty()
        {
            var car = "Benze";
            var expect_ticket = "T_Benze";
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            ParkingLotPlace parkingLotThree = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            parkingLotPlaces.Add(parkingLotThree);
            ParkingBoyBase parkingBoy = new SmartParkingBoy(parkingLotPlaces);

            var actual_ticket = parkingBoy.ParkCar(car);

            Assert.Equal(expect_ticket, actual_ticket);
        }

        [Fact]
        public void Should_return_ticket_from_parkingLot_has_most_freeParkingSpot_When_smartParkingBoy_park_Given_three_parkingLot()
        {
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            string[] carsOne = { "Benze", "BMW" };
            foreach (var car in carsOne)
            {
                parkingLotOne.ParkCar(car);
            }

            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            string[] carsTwo = { "Mazda" };
            foreach (var car in carsTwo)
            {
                parkingLotTwo.ParkCar(car);
            }

            ParkingLotPlace parkingLotThree = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            parkingLotPlaces.Add(parkingLotThree);
            ParkingBoyBase parkingBoy = new SmartParkingBoy(parkingLotPlaces);

            var actual_ticket = parkingBoy.ParkCar("GTR");

            Assert.True(parkingLotThree.TicketToCar.ContainsKey("T_GTR"));
        }

        [Fact]
        public void Should_return_ticket_from_parkingLot_has_most_freeParkingSpot_When_smartParkingBoy_park_Given_three_parkingLot_with_multiple_parkCar_fetchCar_operations()
        {
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            string[] carsOne = { "Benze", "BMW", "GTR", "WuLing", "Porsche", "Mazda" };
            foreach (var car in carsOne)
            {
                parkingLotOne.ParkCar(car);
            }

            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            ParkingLotPlace parkingLotThree = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            parkingLotPlaces.Add(parkingLotThree);
            ParkingBoyBase parkingBoy = new SmartParkingBoy(parkingLotPlaces);

            parkingBoy.FetchCar("T_Benze");
            parkingBoy.ParkCar("Benze");
            parkingBoy.FetchCar("T_BMW");
            parkingBoy.ParkCar("BMW");
            parkingBoy.FetchCar("T_GTR");
            parkingBoy.ParkCar("GTR");
            parkingBoy.FetchCar("T_Porsche");
            parkingBoy.ParkCar("Porsche");

            Assert.True(parkingLotOne.TicketToCar.Count == 2);
            Assert.True(parkingLotTwo.TicketToCar.Count == 2);
            Assert.True(parkingLotThree.TicketToCar.Count == 2);
        }

        [Fact]
        public void Should_return_right_cars_from_different_parkingLot_When_smartParkingBot_fetch_car_Given_right_ticket_of_different_parkingLot()
        {
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            string[] carsOne = { "Benze", "BMW", "Jieda" };
            foreach (var car in carsOne)
            {
                parkingLotOne.ParkCar(car);
            }

            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            string[] carsTwo = { "Mazda" };
            foreach (var car in carsTwo)
            {
                parkingLotTwo.ParkCar(car);
            }

            ParkingLotPlace parkingLotThree = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            parkingLotPlaces.Add(parkingLotThree);
            ParkingBoyBase parkingBoy = new SmartParkingBoy(parkingLotPlaces);
            var expect_car = "GTR";
            var actual_ticket = parkingBoy.ParkCar(expect_car);
            var actual_car = parkingBoy.FetchCar(actual_ticket);

            Assert.Equal(expect_car, actual_car);
        }

       [Fact]
        public void Should_throw_unrecognized_exception_When_smartparkingboyPlus_fetch_Given_wrong_ticket()
        {
            var expect_msg = "Unrecognized parking ticket.";
            var mazda_ticket = "T_Mazda";
            string[] cars = { "Benze", "BMW", "Rolls-Royce", "Tesla", "Lamborghini", "Porsche", "Honda", "Bentley", "GTR", "911", "LiXiang" };
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            ParkingLotPlace parkingLotThree = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            parkingLotPlaces.Add(parkingLotThree);
            SmartParkingBoy parkingBoy = new SmartParkingBoy(parkingLotPlaces);
            foreach (var car in cars)
            {
                parkingBoy.ParkCar(car);
            }

            WrongException acturl_wrong_msg = Assert.Throws<WrongException>(() => parkingBoy.FetchCar(mazda_ticket));

            Assert.Equal(expect_msg, acturl_wrong_msg.Message);
        }

        [Fact]
        public void Should_throw_unrecognized_exception_When_smartparkingboy_fetch_Given_used_ticket()
        {
            var expect_msg = "Unrecognized parking ticket.";
            var benze_ticket = "T_Benze";
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            SmartParkingBoy parkingBoy = new SmartParkingBoy(parkingLotPlaces);
            string[] cars = { "Benze", "BMW", "Rolls-Royce", "Tesla", "Lamborghini", "Porsche", "Mazda" };
            foreach (var car in cars)
            {
                parkingBoy.ParkCar(car);
            }

            parkingBoy.FetchCar(benze_ticket);

            WrongException acturl_wrong_msg = Assert.Throws<WrongException>(() => parkingBoy.FetchCar(benze_ticket));

            Assert.Equal(expect_msg, acturl_wrong_msg.Message);
        }

        [Fact]
        public void Should_throw_noPosition_exception_When_smartParkingboy_fetch_Given_no_parkingSpot_available()
        {
            var expect_msg = "No available position.";
            var carToPark = "WuLing";
            List<ParkingLotPlace> parkingLotPlaces = new List<ParkingLotPlace>();
            ParkingLotPlace parkingLotOne = new ParkingLotPlace();
            ParkingLotPlace parkingLotTwo = new ParkingLotPlace();
            parkingLotPlaces.Add(parkingLotOne);
            parkingLotPlaces.Add(parkingLotTwo);
            SmartParkingBoy parkingBoy = new SmartParkingBoy(parkingLotPlaces);
            string[] cars = { "Benze", "BMW", "Rolls-Royce", "Tesla", "Lamborghini", "Porsche", "Mazda", "Honda", "Bentley", "GTR", "911", "LiXiang" };
            foreach (var car in cars)
            {
                parkingBoy.ParkCar(car);
            }

            WrongException acturl_wrong_msg = Assert.Throws<WrongException>(() => parkingBoy.ParkCar(carToPark));

            Assert.Equal(expect_msg, acturl_wrong_msg.Message);
        }
    }
}
